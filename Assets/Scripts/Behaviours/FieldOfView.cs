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

    for (int i = 0; i < targetsInViewRadius.Length; i++)
    {
      Transform target = targetsInViewRadius[i].transform;
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
      else if (Vector2.Distance(transform.position, visibleTarget.position) < Vector2.Distance(transform.position, closestTarget.position))
        closestTarget = visibleTarget;
    });

    AdjustCamera();
  }

  private void AdjustCamera()
  {
    if (TryGetComponent<Player>(out var player))
    {
      if (closestTarget != null)
      {
        player.shadowCameraTargetGroup.closestTarget = closestTarget.gameObject;
        GameManager.instance.vcam1.Follow = player.shadowCameraTargetGroup.transform;
      }
      else if (GameManager.instance.vcam1.m_Follow != player.transform)
      {
        player.shadowCameraTargetGroup.closestTarget = null;
        GameManager.instance.vcam1.Follow = player.transform;
      }
    }
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
          float weaponAngle = thisMovingCharacter.GetComponentInChildren<Weapon>().Angle;
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
