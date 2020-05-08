using System;
using UnityEngine;

public class SkeletonBoss : Enemy
{
  /* Event */
  public static event Action OnBossKilled;
  public override void Die(Quaternion rotation)
  {
    Instantiate(bloodParticleEffect, transform.position, rotation);
    Instantiate(bloodStain, transform.position, rotation);
    Instantiate(corpse, transform.position, rotation);
    OnBossKilled?.Invoke();
  }
}
