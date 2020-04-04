using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
  /* Public variables */
  public float viewRadius = 7f;
  public LayerMask targetMask;
  public LayerMask obstacleMask;
  [HideInInspector] public List<Transform> visibleTargets = new List<Transform>();

  public Transform closestTarget;

  /* Private variables */
  private float scanSpeed = 0.01f;
  private MovingCharacter thisMovingObject;

  private void Start()
  {
    thisMovingObject = transform.gameObject.GetComponent<MovingCharacter>();
    StartCoroutine("FindTargetsWithDelay", scanSpeed);
  }

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
      float distanceToTarget = Vector2.Distance(transform.position, target.position);

      if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, obstacleMask))
      {
        visibleTargets.Add(target);
      }
    }

    visibleTargets.ForEach(visibleTarget =>
    {
      if (closestTarget == null)
        closestTarget = visibleTarget;
      else if (Vector2.Distance(transform.position, visibleTarget.position) < Vector2.Distance(transform.position, closestTarget.position))
        closestTarget = visibleTarget;
    });

    // TODO: This will happen with the shoot trigger for the player and some other trigger for the followers and enemies
    // Weapon weapon = thisMovingObject.GetComponent<Weapon>();
    // if (weapon != null)
    //   weapon.Attack(closestTarget);
  }
}
