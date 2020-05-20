using System;
using UnityEngine;

public class SkeletonBoss : Enemy
{
  /* Event */
  public static event Action OnBossKilled;
  public override void Die(Quaternion rotation)
  {
    GameManager.instance.vcam1.Follow = transform;
    Instantiate(bloodParticleEffect, transform.position, rotation);
    Instantiate(bloodStain, transform.position, rotation);
    AudioManager.instance.Play("Evil Laugh");
    movementOnCooldown = movementCooldownInProcess = true;
    animator.SetTrigger("Dead");
    Invoke("BossKilled", 1f);
    Invoke("ReturnCamera", 2f);
  }

  private void BossKilled() => OnBossKilled?.Invoke();

  private void ReturnCamera()
  {
    GameManager.instance.vcam1.Follow = FindObjectOfType<ShadowCameraTargetGroup>().transform;
    Destroy(gameObject);
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
