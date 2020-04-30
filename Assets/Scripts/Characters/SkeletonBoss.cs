using System;
using UnityEngine;

public class SkeletonBoss : Enemy
{
  /* Event */
  public static event Action OnBossKilled;
  public override void Die(Quaternion rotation)
  {
    AudioManager.instance.Play("Vinyl Rewind");
    Instantiate(bloodParticleEffect, transform.position, rotation);
    Instantiate(bloodStain, transform.position, rotation);
    floatingText.CreateFloatingText(Instantiate(corpse, transform.position, rotation).transform);
    OnBossKilled?.Invoke();
  }
}
