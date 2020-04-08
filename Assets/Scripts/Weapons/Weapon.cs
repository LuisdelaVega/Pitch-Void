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
  SpriteRenderer spriteRenderer;

  /* Rotation */
  private float rotationSpeed = 12f;

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
    float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
    spriteRenderer.flipY = angle > 90 || angle < -90;

    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.AngleAxis(angle, Vector3.forward), rotationSpeed * Time.deltaTime);
  }

  private Vector2 GetDirection()
  {
    Transform parent = transform.parent;
    Vector2 distanceVector = Vector2.right;
    if (parent != null)
    {
      Transform closestTarget = transform.parent.GetComponent<FieldOfView>().closestTarget;
      if (closestTarget != null) // Point to the closest target
        distanceVector = closestTarget.position - transform.position;
      else // If no targert in range, point towards the direction the player is moving
        distanceVector = transform.parent.GetComponent<MovingCharacter>().Direction;
    }

    return distanceVector;
  }

  /* Abstract methods */
  public abstract void Attack();
}
