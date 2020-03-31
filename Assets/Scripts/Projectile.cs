using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
  [SerializeField] private float moveSpeed = 12f;
  // [SerializeField] private float damage = 10f;
  [SerializeField] private float splash = 1f;
  private bool targetAccuired = false;
  private Transform target;
  [HideInInspector]
  public Transform Target
  {
    private get => target;
    set
    {
      target = value;
      targetAccuired = true;
    }
  }

  void Update()
  {
    if (targetAccuired && target == null)
      Destroy(gameObject);
  }

  private void FixedUpdate() => MoveToTarget();

  void OnTriggerEnter2D(Collider2D other)
  {
    if (other.tag == target.tag)
    {
      other.GetComponent<MovingCharacter>().ApplyDamage(transform.parent.GetComponent<Weapon>().GetAttackPower());
      Destroy(gameObject);
    }
  }

  private void MoveToTarget()
  {
    if (!targetAccuired || target == null)
      return;

    transform.position = Vector2.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
    Vector2 direction = target.transform.position - transform.position;
    float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
    Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    transform.rotation = Quaternion.Slerp(transform.rotation, rotation, moveSpeed * Time.deltaTime);
  }
}
