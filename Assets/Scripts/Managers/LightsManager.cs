using System.Collections;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class LightsManager : MonoBehaviour
{
  private Light2D[] roomLights;
  private bool dimPointLights = true;
  [SerializeField] private bool dimToggle = false;
  [SerializeField] private float dimAmount = 0.05f;
  [SerializeField] private float dimInterval = 0.15f;
  [SerializeField] private float minDimIntensity = 0;
  [SerializeField] private float maxDimIntensity = 0.75f;

  // Start is called before the first frame update
  void Start()
  {
    roomLights = GetComponentsInChildren<Light2D>();
    if (!dimToggle)
      foreach (Light2D light in roomLights)
        light.intensity = 0;
    else
      StartCoroutine(ToggleDim(dimToggle));
  }

  public IEnumerator ToggleDim(bool toggle)
  {
    dimToggle = toggle;
    // if (dimToggle)
    //   foreach (Light2D light in roomLights)
    //     light.intensity = 1;
    // else
    //   foreach (Light2D light in roomLights)
    //     light.intensity = 0;

    while (dimToggle)
    {
      yield return new WaitForSeconds(dimInterval);

      for (int index = 0; index < roomLights.Length; index++)
      {
        Light2D light = roomLights[index];
        if (index == roomLights.Length - 1)
          if (light.intensity == minDimIntensity)
          {
            dimPointLights = false;
            yield return new WaitForSeconds(1.75f);
          }
          else if (light.intensity == maxDimIntensity)
            dimPointLights = true;

        float interval = dimAmount;
        if (dimPointLights)
          interval *= -1;

        light.intensity = Mathf.Clamp(light.intensity + interval, minDimIntensity, maxDimIntensity);
      }
    }
  }
}
