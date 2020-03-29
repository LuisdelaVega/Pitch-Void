using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    /* Public variables */
    [Range(0, 360)] public float viewAngle = 45f;
    public float viewRadius = 7f;
    public LayerMask targetMask;
    public LayerMask obstacleMask;
    public float attackSpeed = 0.1f;

    // Placeholder stuff
    [HideInInspector] public List<Transform> visibleTargets = new List<Transform>();

    /* Private variables */
    private MovingObject thisMovingObject;

    private void Start()
    {
        thisMovingObject = transform.gameObject.GetComponent<MovingObject>();
        StartCoroutine("FindTargetsWithDelay", attackSpeed);
    }

    IEnumerator FindTargetsWithDelay(float delay)
    {
        while(true)
        {
            yield return new WaitForSeconds(delay);
            FindVisibleTargets();
        }
    }

    void FindVisibleTargets()
    {
        if (thisMovingObject == null) return;
        if (thisMovingObject.Direction == null || thisMovingObject.Direction.sqrMagnitude == 0) return;
        
        visibleTargets.Clear();
        Collider2D[] targetsInViewRadius = Physics2D.OverlapCircleAll(transform.position, viewRadius, targetMask);

        for (int i = 0; i < targetsInViewRadius.Length; i++)
        {
            Transform target = targetsInViewRadius[i].transform;
            Vector2 targetMovement = target.GetComponent<MovingObject>().Direction;

            if (targetMovement == null || targetMovement.sqrMagnitude == 0) continue;

            Vector2 directionToTarget = (target.position - transform.position).normalized;
            if (Vector2.Angle(thisMovingObject.Direction, directionToTarget) < viewAngle/2)
            {
                float distanceToTarget = Vector2.Distance(transform.position, target.position);

                if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, obstacleMask))
                {
                    visibleTargets.Add(target);
                }
            }
        }

        var weapon = thisMovingObject.GetComponent<Weapon>();
        if (weapon != null)
            weapon.Attack(visibleTargets);
    }

    // Used only by the FieldOfViewEditor to draw the FOV
    public Vector2 DirectionFromAngle(float angleInDegrees, bool angleIsGlobal)
    {
        if (!angleIsGlobal && thisMovingObject != null)
        {
            if (thisMovingObject.Direction != null)
            {
                if (thisMovingObject.Direction.y == 0)
                    angleInDegrees += thisMovingObject.Direction.x > 0 ? 90 : -90;
                else if (thisMovingObject.Direction.x == 0)
                    angleInDegrees += thisMovingObject.Direction.y > 0 ? 0 : 180;
            }
        }
        
        return new Vector2(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }
}
