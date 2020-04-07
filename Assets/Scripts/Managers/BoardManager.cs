using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class BoardManager : MonoBehaviour
{
  //SetupScene initializes our level and calls the previous functions to lay out the game board
  public GameObject SetupScene(int level, GameObject playerPrefab, CinemachineVirtualCamera vcam1)
  {

    var player = Instantiate(playerPrefab, new Vector2(0, 0), Quaternion.identity);
    // player.AddComponent(typeof(Player)); // TODO: Instead of doing this, add an isAcive boolean to the Player script and add the Player script to all Player Characters
    player.tag = "Player";
    vcam1.m_LookAt = player.transform;
    vcam1.LookAt = player.transform;
    vcam1.m_Follow = player.transform;
    vcam1.Follow = player.transform;

    return player;
  }
}
