using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedWeapon : Weapon
{

  /* Projectile */
  public Projectile projectilePrefab;
  [SerializeField] private float projectileForce = 20f;

  /* Recoil */
  protected Recoil recoilScript;

  private void Start() => recoilScript = GetComponent<Recoil>();

  public override void Attack()
  {
    if (onCooldown) return;

    var projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation, transform.parent);
    Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
    rb.AddForce(firePoint.right * projectileForce, ForceMode2D.Impulse);
    recoilScript.AddRecoil();
    onCooldown = true;
  }
}
