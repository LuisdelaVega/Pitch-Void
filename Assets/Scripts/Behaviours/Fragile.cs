using UnityEngine;

// TODO: Delete this class maybe?
public class Fragile : MonoBehaviour, IDamageable
{
  public void DealDamage(float damage) => Destroy(gameObject);
}
