using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
  private float startTime;

  public void StartTimer() => startTime = Time.time;
  public void StopTimer() => startTime = 0;

  public string GetElapsedTime()
  {
    float elapsedTime = Time.time - startTime;
    string minutes = ((int)elapsedTime / 60).ToString();
    string seconds = (elapsedTime % 60).ToString("f2");

    return $"Time: {minutes}:{seconds}";
  }
}
