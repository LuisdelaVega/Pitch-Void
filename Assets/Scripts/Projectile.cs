using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
  public LayerMask targetMask;
  private float damage = 0;

  private void Start()
  {
    // Get these values here in case any of these Components get Destroyed
    targetMask = transform.parent.GetComponent<FieldOfView>().targetMask;
    damage = transform.parent.GetComponentInChildren<Weapon>().GetAttackPower();
  }

  void OnTriggerEnter2D(Collider2D other)
  {
    int layerMask = 1 << other.gameObject.layer;
    if (layerMask == targetMask)
    {
      other.GetComponent<MovingCharacter>().ApplyDamage(damage);
      Destroy(gameObject);
    }
    else if (other.tag == "Wall")
      Destroy(gameObject);
  }
}
