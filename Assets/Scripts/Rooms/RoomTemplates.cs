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
  public GameObject closedHorizontalWall;
  public GameObject closedVerticalWall;
  [HideInInspector] public GameObject bossRoom; // Maybe create a list of boss rooms and replace the one here with one of those

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
