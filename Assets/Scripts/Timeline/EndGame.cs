using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGame : TimelineManager
{
  private void OnEnable() => EndGameTrigger.OnEndGameTrigger += Play;
  private void OnDisable() => EndGameTrigger.OnEndGameTrigger -= Play;
}
