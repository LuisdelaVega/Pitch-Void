using UnityEngine;

public class HealthBehaviour : MonoBehaviour, IDamageable
{
  [SerializeField] private float maxHealth = 100;
  //   public event EventHandler<HealthChangedEventArgs> OnHealthChanged;
  public float MaxHealth => maxHealth;
  [SerializeField] private float health;
  public float Health
  {
    get => health;
    private set => health = Mathf.Clamp(value, 0, maxHealth);
  }

  private void Start() => Health = maxHealth;

  private void Add(float value)
  {
    value = Mathf.Max(value, 0f);
    Health += value;
  }

  private void Remove(float value, Quaternion rotation)
  {
    value = Mathf.Max(value, 0f);
    Health -= value;

    if (Health == 0)
    {
      GetComponent<ScreenShake>()?.Shake();
      if (TryGetComponent<MovingCharacter>(out MovingCharacter character))
      {
        character.Die(rotation);
      }
      Destroy(gameObject);
    }
  }

  public void DealDamage(float damageValue, Quaternion rotation) => Remove(damageValue, rotation);
}
