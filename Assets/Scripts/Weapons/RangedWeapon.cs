using UnityEngine;

public class RangedWeapon : Weapon
{

  /* Projectile */
  public Projectile projectilePrefab;
  [SerializeField] private float projectileForce = 20f;

  /* Fire Point */
  public Transform firePoint;

  public override void Attack()
  {
    if (onCooldown) return;

    var projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation, transform.parent);
    projectile.GetComponent<Rigidbody2D>()?.AddForce(firePoint.right * projectileForce, ForceMode2D.Impulse);
    GetComponent<AudioSource>()?.Play();
    GetComponent<Recoil>()?.AddRecoil();
    GetComponent<ScreenShake>()?.Shake();

    onCooldown = true;
  }
}
