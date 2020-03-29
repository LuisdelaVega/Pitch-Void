using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
  [SerializeField] private float cooldown = 1f;
  [SerializeField] private float attackPower = 5f;
  [SerializeField] private string attackType = "Ranged";
  [SerializeField] private Projectile projectilePrefab;
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
          var projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
          projectile.Target = visibleTargets[Random.Range(0, visibleTargets.Count)];
          cooldownTimer = cooldown;
          break;
        default:
          break;
      }
  }
}
