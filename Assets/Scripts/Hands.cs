using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hands : MonoBehaviour
{
  private void FixedUpdate()
  {
    Vector2 direction = GetDirection();
    float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.AngleAxis(angle, Vector3.forward), 12 * Time.fixedDeltaTime);

    Debug.Log(transform.rotation);
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
}
