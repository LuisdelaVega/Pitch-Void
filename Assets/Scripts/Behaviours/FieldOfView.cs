using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
  /* FOV values */
  public float viewRadius = 7f;
  [Range(0, 360)] public float viewAngle = 45f;
  public LayerMask targetMask;
  public LayerMask obstacleMask;
  private float scanSpeed = 0.01f;

  /* Targets */
  [HideInInspector] public List<Transform> visibleTargets = new List<Transform>();
  [HideInInspector] public Transform closestTarget;

  private void Start() => StartCoroutine("FindTargetsWithDelay", scanSpeed);

  IEnumerator FindTargetsWithDelay(float delay)
  {
    while (true)
    {
      yield return new WaitForSeconds(delay);
      FindVisibleTargets();
    }
  }

  void FindVisibleTargets()
  {
    // Clear targets
    visibleTargets.Clear();
    closestTarget = null;

    Collider2D[] targetsInViewRadius = Physics2D.OverlapCircleAll(transform.position, viewRadius, targetMask);

    foreach (Collider2D targetCollider in targetsInViewRadius)
    {
      Transform target = targetCollider.transform;
      Vector2 directionToTarget = (target.position - transform.position).normalized;
      if (TryGetComponent<MovingCharacter>(out var thisMovingCharacter))
      {
        if (Vector2.Angle(thisMovingCharacter.Direction, directionToTarget) < viewAngle / 2)
        {
          float distanceToTarget = Vector2.Distance(transform.position, target.position);

          if (!Physics2D.Raycast(transform.position, directionToTarget, distanceToTarget, obstacleMask))
            visibleTargets.Add(target);
        }
      }
    }

    visibleTargets.ForEach(visibleTarget =>
    {
      if (closestTarget == null)
        closestTarget = visibleTarget;
      else if ((transform.position - visibleTarget.position).sqrMagnitude < (transform.position - closestTarget.position).sqrMagnitude)
        closestTarget = visibleTarget;
    });
  }

  // Used only by the FieldOfViewEditor to draw the FOV
  public Vector2 DirectionFromAngle(float angleInDegrees, bool angleIsGlobal)
  {
    if (TryGetComponent<MovingCharacter>(out var thisMovingCharacter))
    {
      if (!angleIsGlobal)
      {
        Weapon weapon = thisMovingCharacter.GetComponentInChildren<Weapon>();
        if (weapon != null)
        {
          float weaponAngle = weapon.Angle;
          if (weaponAngle > 0 && weaponAngle <= 180)
            angleInDegrees -= weaponAngle - 90;
          else if (weaponAngle <= 0 && weaponAngle >= -180)
            angleInDegrees += -weaponAngle + 90;
        }
      }
    }

    return new Vector2(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
  }
}
