using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TimelineManager : MonoBehaviour
{
  public PlayableDirector playableDirector;
  public void Play() => playableDirector.Play();
}
