using UnityEngine;

public class AddRoom : MonoBehaviour
{
  private void Start()
  {
    RoomTemplates.instance.allRooms.Add(gameObject);
    RoomTemplates.instance.timer = RoomTemplates.instance.waitTime;
  }
}
