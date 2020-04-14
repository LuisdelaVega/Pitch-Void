using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowCameraTargetGroup : MonoBehaviour
{
  //   [SerializeField] private Vector3 offset;
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
