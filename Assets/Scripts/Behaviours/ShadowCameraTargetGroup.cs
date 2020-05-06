using UnityEngine;

public class ShadowCameraTargetGroup : MonoBehaviour
{
  [HideInInspector] public GameObject player;
  [HideInInspector] public GameObject closestTarget;

  private void Start() => GameManager.instance.vcam1.Follow = transform;

  private void Update()
  {
    if (player != null)
    {
      Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
      transform.position = new Vector3(
          player.transform.position.x - (player.transform.position.x - mousePosition.x) / 4,
          player.transform.position.y - (player.transform.position.y - mousePosition.y) / 4,
            player.transform.position.z
      );
    }
  }
}
