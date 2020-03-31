using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
  /* Attack */
  [SerializeField] protected float attackPower = 10f;
  [SerializeField] protected float splash = 1f;

  /* Cooldown */
  [SerializeField] protected float cooldown = 1f;
  protected float cooldownTimer = 0f;


  void Update()
  {
    if (cooldownTimer > 0)
      cooldownTimer -= Time.deltaTime;
  }

  public abstract void Attack(List<Transform> visibleTargets);

  public float GetAttackPower() => attackPower;
}
