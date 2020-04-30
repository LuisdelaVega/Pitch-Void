using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomCloser : MonoBehaviour
{
  private void Update()
  {
    if (RoomTemplates.instance.spawedBoss)
      Destroy(gameObject);
  }

  private void OnTriggerEnter2D(Collider2D other)
  {
    if (other.CompareTag("SpawnPoint"))
    {
      RoomSpawner roomSpawner = other.GetComponent<RoomSpawner>();
      roomSpawner.CloseRoom(roomSpawner);
      roomSpawner.spawned = true;
    }
  }
}
