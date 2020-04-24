using UnityEngine;

public class Projectile : MonoBehaviour
{
  private float damage = 0;

  // Get this value here in case the parent gets destroyed later
  private void Start() => damage = transform.parent.GetComponentInChildren<Weapon>().AttackPower;

  void OnTriggerEnter2D(Collider2D other)
  {
    int otherLayerMask = 1 << other.gameObject.layer;
    int thisLayerMask = 1 << gameObject.layer;
    int parentLayerMask = 1 << transform.parent.gameObject.layer;
    if (
      otherLayerMask != parentLayerMask && // Avoid Fiendly Fire
      otherLayerMask != thisLayerMask && // Avoid triggering on other Projectiles
      other.TryGetComponent<IDamageable>(out var damageable)
    )
    {
      damageable.DealDamage(damage, transform.rotation);
      Destroy(gameObject);
    }
    else if (other.tag == "Wall")
      Destroy(gameObject);
  }
}
