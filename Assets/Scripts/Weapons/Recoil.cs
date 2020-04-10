using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recoil : MonoBehaviour
{

  public float maximumOffsetDistance;
  public float recoilAcceleration;
  public float weaponRecoilStartSpeend;

  private bool recoilInEffect;
  private bool weaponHeadedBackToStartPosition;

  private Vector3 offsetPosiiton;
  private Vector3 recoilSpeed;

  public void AddRecoil()
  {
    recoilInEffect = true;
    weaponHeadedBackToStartPosition = false;

    recoilSpeed = transform.right * weaponRecoilStartSpeend;
  }

  private void Start()
  {
    recoilSpeed = Vector3.zero;
    offsetPosiiton = Vector3.zero;

    recoilInEffect = false;
    weaponHeadedBackToStartPosition = false;
  }

  private void FixedUpdate() => UpdateRecoil();

  private void UpdateRecoil()
  {
    if (!recoilInEffect) return;

    // Set up speed and then position
    recoilSpeed += (-offsetPosiiton.normalized) * recoilAcceleration * Time.fixedDeltaTime;
    Vector3 newOffsetPosition = offsetPosiiton + recoilSpeed * Time.fixedDeltaTime;
    Vector3 newTransformPosition = transform.position - offsetPosiiton;

    if (newOffsetPosition.magnitude > maximumOffsetDistance)
    {
      recoilSpeed = Vector3.zero;
      weaponHeadedBackToStartPosition = true;
      newOffsetPosition = offsetPosiiton.normalized * maximumOffsetDistance;
    }
    else if (weaponHeadedBackToStartPosition && newOffsetPosition.sqrMagnitude > offsetPosiiton.sqrMagnitude)
    {
      transform.position -= offsetPosiiton;
      offsetPosiiton = Vector3.zero;

      recoilInEffect = false;
      weaponHeadedBackToStartPosition = false;
      return;
    }

    transform.position = newTransformPosition + newOffsetPosition;
    offsetPosiiton = newOffsetPosition;
  }
}
