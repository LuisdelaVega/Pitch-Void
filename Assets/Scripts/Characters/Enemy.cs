using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using Random = UnityEngine.Random; //Tells Random to use the Unity Engine random number generator.

public class Enemy : MovingCharacter
{
  /* Movement */
  [SerializeField] private Count movementTimes = new Count(1, 2);
  private float movementTimer;
  private Vector2 lastDirection;
  private bool foundDirectioThisTurn = false;
  [SerializeField] private Count movementCooldownTimes = new Count(1, 2);
  private bool movementOnCooldown = false;
  private bool movementCooldownInProcess = false;

  /* Attacking */
  private bool attacking = false;
  [SerializeField] private float attackDelay = 0.3f;

  /* Event */
  public static event Action OnEnemyKilled;

  /* Alert */
  public Light2D alertLight;
  public float maxIntensity = 0.8f;
  public float alertLightTime = 1f;
  private bool alertLightOn = false;
  public GameObject alertBangPrefab;

  /* Corpse */ // TODO: Move this to Moving Character
  public GameObject corpse;

  /* Popup Text */
  public FloatingTextManager floatingText;

  // Start is called before the first frame update
  void Awake()
  {
    holdAttack = true;
    FindNewDirection();
    alertLight.intensity = 0;
    movementTimer = GetRandomInRange(movementTimes);
    floatingText = GameObject.Find("Floating Text Manager").GetComponent<FloatingTextManager>();
  }

  private void OnEnable()
  {
    RangedWeapon.OnShotFired += Alert;
    EndGameTrigger.OnEndGameTrigger += WaveGoodbye;
  }
  private void OnDisable()
  {
    RangedWeapon.OnShotFired -= Alert;
    EndGameTrigger.OnEndGameTrigger -= WaveGoodbye;
  }

  /* Movement */
  protected override void Move()
  {
    if (!movementOnCooldown)
      PerformMovement();
    else if (movementOnCooldown && !movementCooldownInProcess)
      StartCoroutine(MovementCooldown());
  }

  private void PerformMovement()
  {
    Transform closestTarget = GetComponent<FieldOfView>().closestTarget;
    if (closestTarget != null) FindNewDirection(closestTarget.position);
    else if (!foundDirectioThisTurn) FindNewDirection();

    AvoidObstacles();
    animator.SetFloat("Speed", Direction.sqrMagnitude);
    rb.MovePosition(rb.position + Direction * moveSpeed * Time.fixedDeltaTime);

    movementTimer = AdjustTimer(movementTimer, movementTimes);
    if (movementTimer <= 0)
    {
      animator.SetFloat("Speed", 0);
      movementOnCooldown = true;
    }
  }

  private void AvoidObstacles()
  {
    RaycastHit2D raycastHit = Physics2D.Raycast(transform.position + (Vector3)Direction, Direction, 2f);
    if (raycastHit.collider != null && !raycastHit.collider.gameObject.Equals(gameObject))
    {
      switch (raycastHit.collider.tag)
      {
        case "Enemy":
        case "Wall":
        case "Blocking Object":
          FindNewDirection(true);
          break;
        default:
          break;
      }
    }
  }

  private void FindNewDirection() => FindNewDirection(false);
  private void FindNewDirection(Vector3 position)
  {
    Vector2 newDirection = position - transform.position;
    SetDirection(newDirection);
  }
  private void FindNewDirection(bool avoid)
  {
    Vector2 newDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));

    // Avoid going through walls and other obstacles
    if (avoid)
    {
      if ((newDirection.x <= 0 && lastDirection.x <= 0) || (newDirection.x >= 0 && lastDirection.x >= 0))
        newDirection.x *= -1;
      if ((newDirection.y <= 0 && lastDirection.y <= 0) || (newDirection.y >= 0 && lastDirection.y >= 0))
        newDirection.y *= -1;
    }

    SetDirection(newDirection);
  }

  private void SetDirection(Vector2 newDirection)
  {
    lastDirection = Direction;
    newDirection.Normalize();
    Direction = newDirection;
    foundDirectioThisTurn = true;
  }

  /* Attack */
  protected override void Attack()
  {
    if (!attacking)
      StartCoroutine(PerformAttack());
  }

  private IEnumerator PerformAttack()
  {
    attacking = true;
    yield return new WaitForSeconds(attackDelay);

    List<Transform> visibleTargets = GetComponent<FieldOfView>().visibleTargets;
    if (visibleTargets.Count != 0)
      GetComponentInChildren<Weapon>().Attack();

    attacking = false;
  }

  public void Alert(Vector2 position, float distance)
  {
    Vector2 playerPosition = position;
    float distanceToTarget = ((Vector2)transform.position - playerPosition).sqrMagnitude;

    if (distanceToTarget > distance) return;

    if (alertBangPrefab != null)
      Destroy(Instantiate(alertBangPrefab, new Vector2(transform.position.x, transform.position.y + 1.5f), Quaternion.identity, transform), alertLightTime);

    alertLight.intensity = maxIntensity;
    if (!alertLightOn)
      StartCoroutine(AlertLightTimer());
    FindNewDirection(playerPosition);
    movementOnCooldown = movementCooldownInProcess = false;
    movementTimer = GetRandomInRange(movementTimes);
  }

  /* Cooldowns */
  private IEnumerator MovementCooldown()
  {
    movementCooldownInProcess = true;
    yield return new WaitForSeconds(GetRandomInRange(movementCooldownTimes));
    movementOnCooldown = foundDirectioThisTurn = movementCooldownInProcess = false;
    movementTimer = GetRandomInRange(movementTimes);
  }

  private IEnumerator AlertLightTimer()
  {
    alertLightOn = true;
    yield return new WaitForSeconds(alertLightTime);
    alertLight.intensity = 0;
    alertLightOn = false;
  }

  public override void Bleed(Quaternion rotation)
  {
    Instantiate(bloodStain, transform.position, rotation);
    Instantiate(bloodParticleEffect, transform.position, rotation);
  }

  public override void Die(Quaternion rotation)
  {
    floatingText.CreateFloatingText(Instantiate(corpse, transform.position, rotation).transform);
    OnEnemyKilled?.Invoke();
    Destroy(gameObject);
  }

  public void WaveGoodbye()
  {
    animator.SetTrigger("Wave");
    Destroy(GetComponentInChildren<RangedWeapon>().gameObject);
    enabled = false;
  }

  /* Helper Methods */
  private int GetRandomInRange(Count range) => Random.Range(range.minimum, range.maximum + 1);
  private float AdjustTimer(float timer, Count range) => Mathf.Clamp(timer - Time.fixedDeltaTime, 0, range.maximum);
}
