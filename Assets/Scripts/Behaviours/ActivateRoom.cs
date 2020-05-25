using UnityEngine;
using UnityEngine.Tilemaps;

public class ActivateRoom : MonoBehaviour
{
  public RoomManager roomManager;

  private void OnTriggerEnter2D(Collider2D other)
  {
    if (other.CompareTag("Player"))
    {
      roomManager.RemoveGroundCollider();
      Invoke("SetUp", 0.1f);
    }
  }

  private void SetUp() => roomManager?.SetUpRoom(true);
}
