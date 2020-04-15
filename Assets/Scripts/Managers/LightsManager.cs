using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class LightsManager : MonoBehaviour
{
  public static LightsManager instance = null;
  private Light2D[] roomLights;
  private bool turnOff = true;

  private void Awake()
  {
    if (instance == null)
      instance = this;
    else if (instance != this)
      Destroy(gameObject);
  }

  // Start is called before the first frame update
  void Start()
  {
    roomLights = GetComponentsInChildren<Light2D>();
  }

  public IEnumerator Flashing()
  {
    while (true)
    {
      yield return new WaitForSeconds(0.15f);
      foreach (var light in roomLights)
      {
        if (light.intensity == 0)
          turnOff = false;
        else if (light.intensity == 0.75f)
          turnOff = true;

        float interval = 0.05f;
        if (turnOff)
          interval *= -1;

        light.intensity = Mathf.Clamp(light.intensity + interval, 0, 0.75f);
      }
    }
  }
}
