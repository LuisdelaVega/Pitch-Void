using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBehaviour : MonoBehaviour, IDamageable
{
  [SerializeField] private float maxHealth = 100;
  //   public event EventHandler<HealthChangedEventArgs> OnHealthChanged;
  public float MaxHealth => maxHealth;
  private float health;
  public float Health
  {
    get => health;
    private set
    {
      health = Mathf.Clamp(value, 0, maxHealth);
      //   OnHealthChanged?.Invoke(this, new HealthChangedEventArgs
      //   {
      //     Health = health,
      //     MaxHealth = maxHealth
      //   });
    }
  }

  private void Start() => Health = maxHealth;

  private void Add(float value)
  {
    value = Mathf.Max(value, 0f);
    Health += value;
  }

  private void Remove(float value)
  {
    value = Mathf.Max(value, 0f);
    Health -= value;

    if (Health == 0)
      Destroy(gameObject);
  }

  public void DealDamage(float damageValue) => Remove(damageValue);

  // TODO: Figure out why events are throwing errors
  //   public class HealthChangedEventArgs : EventArgs
  //   {
  //     public float Health { get; set; }
  //     public float MaxHealth { get; set; }
  //   }
}
