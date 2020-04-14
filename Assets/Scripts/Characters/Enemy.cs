using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MovingCharacter
{
  /* Movement */
  [SerializeField] private GameManager.Count movementTimes = new GameManager.Count(1, 2);
  private float movementTimer;
  private Vector2 lastDirection;
  private bool foundDirectioThisTurn = false;
  [SerializeField] private GameManager.Count movementCooldownTimes = new GameManager.Count(1, 2);
  private bool movementOnCooldown = false;
  private bool movementCooldownInProcess = false;

  /* Attack */
  [SerializeField] private GameManager.Count attackTimes = new GameManager.Count(1, 2);
  private float attackTimer;
  [SerializeField] private GameManager.Count attackCooldownTimes = new GameManager.Count(1, 2);
  private bool attackOnCooldown = false;
  private bool attackCooldownInProcess = false;

  // Start is called before the first frame update
  void Awake()
  {
    holdAttack = true;
    FindNewDirection();
    movementTimer = GetRandomInRange(movementTimes);
    attackTimer = GetRandomInRange(attackTimes);
  }

  /* Movement */
  protected override void Move()
  {
    if (!movementOnCooldown && canMove)
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
    rb.MovePosition(rb.position + Direction * moveSpeed * Time.fixedDeltaTime);

    movementTimer = AdjustTimer(movementTimer, movementTimes);
    if (movementTimer <= 0)
      movementOnCooldown = true;
  }

  private void AvoidObstacles()
  {
    RaycastHit2D raycastHit = Physics2D.Raycast(transform.position + (Vector3)Direction, Direction, 1f);
    Debug.DrawRay(transform.position, Direction * 1f, Color.red, 0.1f);
    if (raycastHit.collider != null)
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
    if (!attackOnCooldown)
      PerformAttack();
    else if (attackOnCooldown && !attackCooldownInProcess)
      StartCoroutine(AttackCooldown());
  }

  private void PerformAttack()
  {
    List<Transform> visibleTargets = GetComponent<FieldOfView>().visibleTargets;
    if (visibleTargets.Count == 0) return;

    GetComponentInChildren<Weapon>().Attack();

    attackTimer = AdjustTimer(attackTimer, attackTimes);
    if (attackTimer <= 0)
      attackOnCooldown = true;
  }

  public void Alert(Vector3 position)
  {
    FindNewDirection(position);
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

  private IEnumerator AttackCooldown()
  {
    attackCooldownInProcess = true;
    yield return new WaitForSeconds(GetRandomInRange(attackCooldownTimes));
    attackOnCooldown = attackCooldownInProcess = false;
    attackTimer = GetRandomInRange(attackTimes);
  }

  private void OnDestroy()
  {
    //TODO: Remove the code below
    PreviousPositions.Clear();
  }

  /* Helper Methods */
  private int GetRandomInRange(GameManager.Count range) => Random.Range(range.minimum, range.maximum + 1);
  private float AdjustTimer(float timer, GameManager.Count range) => Mathf.Clamp(timer - Time.fixedDeltaTime, 0, range.maximum);

  // TODO: Remove this
  public override void RecruitFollower(Follower follower)
  {
  }
}
