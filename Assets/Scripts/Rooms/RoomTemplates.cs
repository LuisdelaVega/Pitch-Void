using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomTemplates : MonoBehaviour
{
  public GameObject[] topRooms;
  public GameObject[] rightRooms;
  public GameObject[] bottomRooms;
  public GameObject[] leftRooms;

  public GameObject closedRoom;
  public GameObject bossRoom; // This doesn't have to be a list. It be updated by every room and set equal to itself. Then the room restarts the wait time. Once wait time is < 0, then this cariable will contain the value of the last room

  public float waitTime = 2;
  public float timer = 2;
  private bool spawedBoss = false;
  public GameObject boss;

  private void Update()
  {
    if (timer <= 0 && !spawedBoss)
    {
      Instantiate(boss, bossRoom.transform.position, bossRoom.transform.rotation);
      spawedBoss = true;
    }
    else
      timer -= Time.deltaTime;
  }
}
