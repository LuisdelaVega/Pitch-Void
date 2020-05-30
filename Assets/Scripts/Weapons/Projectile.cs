using UnityEngine;

public class Projectile : MonoBehaviour
{
  [SerializeField] private float damage = 10;
  public LayerMask shootersLayerMask;

  // Get this value here in case the parent gets destroyed later
  void OnTriggerEnter2D(Collider2D other)
  {
    int otherLayerMask = 1 << other.gameObject.layer;
    int thisLayerMask = 1 << gameObject.layer;
    if (
      otherLayerMask != shootersLayerMask && // Avoid Fiendly Fire
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
