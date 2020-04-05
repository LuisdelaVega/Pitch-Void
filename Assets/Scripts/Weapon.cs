using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
  public Transform firePoint;
  /* Attack */
  [SerializeField] protected float attackPower = 10f;
  [SerializeField] protected float splash = 1f;

  /* Cooldown */
  [SerializeField] protected float cooldown = 0.1f;
  [SerializeField] protected float cooldownTimer = 0f;

  public Animator animator;

  void Update()
  {
    if (cooldownTimer > 0)
      cooldownTimer -= Time.deltaTime;

    animator.SetFloat("cooldownTimer", cooldownTimer);
  }

  public float GetAttackPower() => attackPower;

  /* Abstract methods */
  public abstract void Attack();
}
