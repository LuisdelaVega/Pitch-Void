using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
  /* Fire Point */
  public Transform firePoint;

  /* Attack */
  [SerializeField] protected float attackPower = 10f;
  public float AttackPower { get => attackPower; }
  [SerializeField] protected float splash = 1f;

  /* Cooldown */
  [SerializeField] protected float cooldown = 0.1f;
  protected bool onCooldown = false;
  private bool cooldownCoroutineInProcess = false;

  /* Sprite */
  private SpriteRenderer spriteRenderer;

  /* Rotation */
  private float rotationSpeed = 100f;
  private float angle = 0;
  [HideInInspector] public float Angle { get => angle; private set => angle = value; }

  private void Awake() => spriteRenderer = GetComponent<SpriteRenderer>();

  private void Update()
  {
    Rotate();

    if (onCooldown && !cooldownCoroutineInProcess)
      StartCoroutine(WaitForCooldown());
  }

  private IEnumerator WaitForCooldown()
  {
    cooldownCoroutineInProcess = true;
    yield return new WaitForSeconds(cooldown);
    onCooldown = cooldownCoroutineInProcess = false;
  }

  private void Rotate()
  {
    Vector2 direction = GetDirection();
    if (direction.sqrMagnitude == 0) return;

    Angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
    bool flipStrite = Angle > 90 || Angle < -90;

    // Flip the Weapon
    spriteRenderer.flipY = flipStrite;

    // Hide the Weapon behind the Character
    if (Angle > 67.5 && Angle < 112.5 && spriteRenderer.sortingOrder != 0)
      spriteRenderer.sortingOrder = 0;
    else if ((Angle <= 67.5 || Angle >= 112.5) && spriteRenderer.sortingOrder != 2)
      spriteRenderer.sortingOrder = 2;

    // Set the direction of the Character relative to the Weapon's angle
    if (transform.parent != null && transform.parent.tag == "Player") // TODO: Make this happen for Enemies in the future
    {
      transform.parent.GetComponent<MovingCharacter>().animator.SetFloat("Angle", Angle / 180);
      transform.parent.GetComponent<MovingCharacter>().spriteRenderer.flipX = flipStrite; // TODO: This won't happen in the future. I think...
    }

    // Rotate the Weapon
    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.AngleAxis(Angle, Vector3.forward), rotationSpeed * Time.deltaTime);
  }

  private Vector2 GetDirection()
  {
    Transform parent = transform.parent;
    Vector2 distanceVector = Vector2.zero;
    if (parent != null)
    {
      Transform closestTarget = parent.GetComponent<FieldOfView>().closestTarget;
      if (closestTarget != null) // Point to the closest target
        distanceVector = closestTarget.position - transform.position;
      else // If no targert in range, point towards the direction the player is moving
        distanceVector = parent.GetComponent<MovingCharacter>().Direction;
    }

    return distanceVector;
  }

  /* Abstract methods */
  public abstract void Attack();
}
