using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TimelineManager : MonoBehaviour
{
  public PlayableDirector playableDirector;

  private void OnEnable() => SkeletonBoss.OnBossKilled += Play;
  public void Play() => playableDirector.Play();
}
