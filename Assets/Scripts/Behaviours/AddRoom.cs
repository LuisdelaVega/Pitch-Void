using UnityEngine;

public class AddRoom : MonoBehaviour
{
  private RoomTemplates templates;

  private void Start()
  {
    templates = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTemplates>();
    templates.bossRoom = this.gameObject;
    templates.timer = templates.waitTime;
  }
}
