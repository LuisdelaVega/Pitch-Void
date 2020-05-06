using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TimelineManager : MonoBehaviour
{
  // public static TimelineManager instance = null;
  public PlayableDirector playableDirector;

  // void Awake()
  // {
  //   if (instance == null)
  //     instance = this;
  //   else if (instance != this)
  //   {
  //     Destroy(gameObject);
  //     return;
  //   }

  //   DontDestroyOnLoad(gameObject);
  // }

  private void OnEnable() => SkeletonBoss.OnBossKilled += Play;
  private void OnDisable() => SkeletonBoss.OnBossKilled -= Play;
  public void Play() => playableDirector.Play();
}
