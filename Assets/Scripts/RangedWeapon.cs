using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedWeapon : Weapon
{

  public Projectile projectilePrefab;
  [SerializeField] private float projectileForce = 20f;

  public override void Attack()
  {
    if (cooldownTimer > 0.01f)
      return;

    Vector2 direction = GetDirection();
    float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

    var projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation, transform.parent);
    Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
    rb.AddForce(firePoint.right * projectileForce, ForceMode2D.Impulse);

    cooldownTimer = cooldown;
  }
}
