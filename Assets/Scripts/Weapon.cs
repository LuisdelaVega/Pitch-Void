using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
  /* Attack */
  [SerializeField] private float attackPower = 10f;
  [SerializeField] private string attackType = "Ranged";
  [SerializeField] private Projectile projectilePrefab;

  /* Cooldown */
  [SerializeField] private float cooldown = 1f;
  private float cooldownTimer = 0f;


  void Update()
  {
    if (cooldownTimer > 0)
      cooldownTimer -= Time.deltaTime;
  }

  public void Attack(List<Transform> visibleTargets)
  {
    if (visibleTargets.Count > 0 && cooldownTimer <= 0)
      switch (attackType)
      {
        case "Ranged":
          var projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity, transform);
          projectile.Target = visibleTargets[Random.Range(0, visibleTargets.Count)];
          cooldownTimer = cooldown;
          break;
        default:
          break;
      }
  }

  public float GetAttackPower() => attackPower;
}
