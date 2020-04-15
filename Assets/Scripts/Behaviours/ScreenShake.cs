using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class ScreenShake : MonoBehaviour
{

  [SerializeField] private float ShakeDuration = 0.3f;          // Time the Camera Shake effect will last
  [SerializeField] private float ShakeAmplitude = 1.2f;         // Cinemachine Noise Profile Parameter
  [SerializeField] private float ShakeFrequency = 2.0f;         // Cinemachine Noise Profile Parameter
  private bool doShake = false;
  private float ShakeElapsedTime = 0f;
  // Cinemachine Shake
  private CinemachineVirtualCamera VirtualCamera;
  private CinemachineBasicMultiChannelPerlin virtualCameraNoise;
  // Start is called before the first frame update
  void Start()
  {
    VirtualCamera = GameManager.instance.vcam1;
    // Get Virtual Camera Noise Profile
    if (VirtualCamera != null)
      virtualCameraNoise = VirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
  }

  private void Update()
  {
    if (!doShake) return;

    // If the Cinemachine componet is not set, avoid update
    if (VirtualCamera != null && virtualCameraNoise != null)
    {
      // If Camera Shake effect is still playing
      if (ShakeElapsedTime > 0)
      {
        // Set Cinemachine Camera Noise parameters
        virtualCameraNoise.m_AmplitudeGain = ShakeAmplitude;
        virtualCameraNoise.m_FrequencyGain = ShakeFrequency;

        // Update Shake Timer
        ShakeElapsedTime -= Time.deltaTime;
      }
      else
      {
        // If Camera Shake effect is over, reset variables
        virtualCameraNoise.m_AmplitudeGain = 0f;
        ShakeElapsedTime = 0f;
        doShake = false;
      }
    }
  }

  public void Shake()
  {
    ShakeElapsedTime = ShakeDuration;
    doShake = true;
  }
}
