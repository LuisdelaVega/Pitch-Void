using System.Collections;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class LightsManager : MonoBehaviour
{
  // private Light2D[] roomLights;
  private Light2D roomLight;
  private bool dimLights = true;
  // Dimmer values
  [SerializeField] private bool dimToggle = false;
  [SerializeField] private float dimAmount = 0.05f;
  [SerializeField] private float dimInterval = 0.15f;
  [SerializeField] private float waitToTurnBackOn = 3f;
  // Intensity
  [SerializeField] private float minDimIntensity = 0;
  [SerializeField] private float maxDimIntensity = 0.75f;

  // Start is called before the first frame update
  void Start()
  {
    // roomLights = GetComponentsInChildren<Light2D>();
    roomLight = GetComponentInChildren<Light2D>();
    if (!dimToggle)
      roomLight.intensity = 0;
    // foreach (Light2D light in roomLights)
    //   light.intensity = 0;
    else
      StartCoroutine(ToggleDim(dimToggle));
  }

  public IEnumerator ToggleDim(bool toggle)
  {
    dimToggle = toggle;
    if (dimToggle) // TODO: Add some flicker effect when light first turns on
      roomLight.intensity = 1;
    else
      roomLight.intensity = 0;

    while (dimToggle)
    {
      yield return new WaitForSeconds(dimInterval);

      if (roomLight.intensity == minDimIntensity)
      {
        dimLights = false;
        yield return new WaitForSeconds(waitToTurnBackOn);
      }
      else if (roomLight.intensity == maxDimIntensity)
        dimLights = true;

      float interval = dimAmount;
      if (dimLights)
        interval *= -1;

      roomLight.intensity = Mathf.Clamp(roomLight.intensity + interval, minDimIntensity, maxDimIntensity);
    }
  }
}
