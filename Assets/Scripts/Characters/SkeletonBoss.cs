using System;
using UnityEngine;

public class SkeletonBoss : Enemy
{
  public GameObject explosionsPrefab;
  /* Event */
  public static event Action OnBossKilled;

  public override void Die(Quaternion rotation)
  {
    alertLightOn = false;
    alertLight.intensity = maxIntensity;
    spriteRenderer.color = Color.white;
    GameManager.instance.vcam1.Follow = transform;
    Instantiate(bloodParticleEffect, transform.position, rotation);
    Instantiate(bloodStain, transform.position, rotation);
    AudioManager.instance.Play("Evil Laugh");
    movementOnCooldown = movementCooldownInProcess = true;
    animator.SetTrigger("Dead");
    Instantiate(explosionsPrefab, transform.position, transform.rotation, transform);
    Invoke("BossKilled", 2f);
    Invoke("ReturnCamera", 3f);
  }

  private void BossKilled() => OnBossKilled?.Invoke();

  private void ReturnCamera()
  {
    GameManager.instance.vcam1.Follow = FindObjectOfType<ShadowCameraTargetGroup>().transform;
    Destroy(gameObject);
  }

  public override void Bleed(Quaternion rotation)
  {
    var healthBehaviour = GetComponent<HealthBehaviour>();
    float otherColors = 1 - (healthBehaviour.MaxHealth - healthBehaviour.Health) / healthBehaviour.MaxHealth;
    Instantiate(bloodStain, transform.position, rotation);
    Instantiate(bloodParticleEffect, transform.position, rotation);

    spriteRenderer.color = new Color(
      spriteRenderer.color.r,
      otherColors,
      otherColors
    );

    moveSpeed += 0.15f;
    transform.localScale += new Vector3(0.03f, 0.03f, 0);
  }
}
