using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
  public Transform firePoint;
  /* Attack */
  [SerializeField] protected float attackPower = 10f;
  [SerializeField] protected float splash = 1f;

  /* Cooldown */
  [SerializeField] protected float cooldown = 1f;
  [SerializeField] protected float cooldownTimer = 0f;

  void Update()
  {
    if (cooldownTimer > 0)
      cooldownTimer -= Time.deltaTime;
  }

  private void FixedUpdate()
  {
    Vector2 direction = GetDirection();
    float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.AngleAxis(angle, Vector3.forward), 12 * Time.fixedDeltaTime);
  }

  protected Vector2 GetDirection()
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

  public float GetAttackPower() => attackPower;

  /* Abstract methods */
  public abstract void Attack();
}
