using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class LightsManager : MonoBehaviour
{
  public static LightsManager instance = null;
  private Light2D[] roomLights;
  public Light2D globalLight;
  private bool dimPointLights = true;
  private bool dimToggle = false;
  [SerializeField] private float dimAmount = 0.05f;
  [SerializeField] private float dimInterval = 0.15f;
  [SerializeField] private float minDimIntensity = 0;
  [SerializeField] private float maxDimIntensity = 0.75f;

  private void Awake()
  {
    if (instance == null)
      instance = this;
    else if (instance != this)
      Destroy(gameObject);
  }

  // Start is called before the first frame update
  void Start() => roomLights = GetComponentsInChildren<Light2D>();

  public IEnumerator ToggleDim(bool toggle)
  {
    dimToggle = toggle;
    while (dimToggle)
    {
      yield return new WaitForSeconds(dimInterval);
      for (int index = 0; index < roomLights.Length; index++)
      {
        Light2D light = roomLights[index];
        if (index == roomLights.Length - 1)
          if (light.intensity == minDimIntensity)
            dimPointLights = false;
          else if (light.intensity == maxDimIntensity)
            dimPointLights = true;

        float interval = dimAmount;
        if (dimPointLights)
          interval *= -1;

        light.intensity = Mathf.Clamp(light.intensity + interval, minDimIntensity, maxDimIntensity);
      }
    }
  }

  public void ToggleGlobalLight(bool toggle)
  {
    if (globalLight != null) globalLight.enabled = toggle;
  }
}
