using UnityEngine;

public class ShadowCameraTargetGroup : MonoBehaviour
{
  [HideInInspector] public GameObject player;
  [HideInInspector] public GameObject closestTarget;

  private void Update()
  {
    if (player != null && closestTarget != null)
    {
      transform.position = new Vector3(
          player.transform.position.x - (player.transform.position.x - closestTarget.transform.position.x) / 2,
          player.transform.position.y - (player.transform.position.y - closestTarget.transform.position.y) / 2,
            player.transform.position.z
      );
    }
  }
}
