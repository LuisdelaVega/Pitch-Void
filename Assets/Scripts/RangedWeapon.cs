using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedWeapon : Weapon
{

  public Projectile projectilePrefab;

  public override void Attack(List<Transform> visibleTargets)
  {
    if (visibleTargets.Count == 0 || cooldownTimer > 0.01f)
      return; // Don't attack

    var projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity, transform);
    projectile.Target = visibleTargets[Random.Range(0, visibleTargets.Count)];
    cooldownTimer = cooldown;
  }
}
