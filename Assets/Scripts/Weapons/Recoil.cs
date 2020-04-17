using UnityEngine;

public class Recoil : MonoBehaviour
{

  [SerializeField] private float maximumOffsetDistance = 0.3f;
  [SerializeField] private float recoilAcceleration = 40;
  [SerializeField] private float weaponRecoilStartSpeed = -8;

  private bool recoilInEffect;
  private bool weaponHeadedBackToStartPosition;

  private Vector3 offsetPosiiton;
  private Vector3 recoilSpeed;

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

  public void AddRecoil()
  {
    recoilInEffect = true;
    weaponHeadedBackToStartPosition = false;

    recoilSpeed = transform.right * weaponRecoilStartSpeed;
  }
}
