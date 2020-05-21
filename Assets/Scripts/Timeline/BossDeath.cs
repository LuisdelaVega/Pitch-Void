using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDeath : TimelineManager
{
  private void OnEnable() => SkeletonBoss.OnBossKilled += Play;
  private void OnDisable() => SkeletonBoss.OnBossKilled -= Play;
}
