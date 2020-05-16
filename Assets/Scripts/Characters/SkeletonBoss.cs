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
    // TODO: Create a disappear animation (?)
    // Instantiate(corpse, transform.position, rotation);
    OnBossKilled?.Invoke();
  }

  public override void Bleed(Quaternion rotation)
  {
    Instantiate(bloodStain, transform.position, rotation);
    Instantiate(bloodParticleEffect, transform.position, rotation);

    spriteRenderer.color = new Color(
      spriteRenderer.color.r,
      spriteRenderer.color.g - 0.5f,
      spriteRenderer.color.b - 0.5f
    );
  }
}
