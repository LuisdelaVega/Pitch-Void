using UnityEngine;

public class ShadowCameraTargetGroup : MonoBehaviour
{
  [HideInInspector] public GameObject player;
  [HideInInspector] public GameObject closestTarget;
  [SerializeField] private float interpolant = 0.25f;

  private void Start() => GameManager.instance.vcam1.Follow = transform;

  private void Update()
  {
    if (player != null)
      transform.position = Vector3.Lerp(player.transform.position, Camera.main.ScreenToWorldPoint(Input.mousePosition), interpolant);
  }
}
