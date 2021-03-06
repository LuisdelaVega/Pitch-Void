﻿using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Random = UnityEngine.Random; //Tells Random to use the Unity Engine random number generator.

public class RoomTemplates : MonoBehaviour
{
  public static RoomTemplates instance = null;

  /* Seed */
  public string seedText;
  [HideInInspector] public int seed = 1;
  public bool seedTextSet = false;

  /* Rooms */
  public GameObject[] topRooms;
  public GameObject[] rightRooms;
  public GameObject[] bottomRooms;
  public GameObject[] leftRooms;
  public GameObject closedHorizontalWall;
  public GameObject closedVerticalWall;
  [HideInInspector] public List<GameObject> allRooms = new List<GameObject>();

  /* Boss */
  [HideInInspector] public GameObject bossRoom;
  public GameObject boss;
  public bool bossRoomChosen = false;

  /* Timers */
  public float waitTime = 2;
  public float timer = 2;

  private bool gameStarted = false;

  private void Awake()
  {
    if (instance == null)
      instance = this;
    else if (instance != this)
    {
      Destroy(gameObject);
      return;
    }

    DontDestroyOnLoad(gameObject);

    NewSeed();
  }

  public void NewSeed() => NewSeed("");
  public void NewSeed(string newSeed)
  {
    if (newSeed.Length == 8)
    {
      seedTextSet = true;
      seedText = newSeed;
    }

    if (!seedTextSet)
    {
      int length = 8;
      StringBuilder str_build = new StringBuilder();
      char letter;

      for (int i = 0; i < length; i++)
      {
        double flt = Random.Range(0f, 1f);
        int shift = Convert.ToInt32(Math.Floor(25 * flt));
        letter = Convert.ToChar(shift + 65);
        str_build.Append(letter);
      }

      seedText = str_build.ToString();
    }

    seed = seedText.GetHashCode();
  }

  public void GameStarted(bool value)
  {
    gameStarted = value;
    bossRoom = null;
    bossRoomChosen = false;
    allRooms.Clear();
    seed = seedText.GetHashCode();
  }

  private void Update()
  {
    if (!gameStarted || GameManager.instance.arcadeMode) return;

    if (timer <= 0 && !bossRoomChosen)
    {
      bossRoom = allRooms[allRooms.Count - 1];
      var bossRoomManager = bossRoom.GetComponent<RoomManager>();
      bossRoomManager.MakeIntoBossRoom();
      bossRoomManager.enemyPrefabList = new List<GameObject>();
      bossRoomManager.enemyPrefabList.Add(boss);
      bossRoomChosen = true;
    }

    timer -= Time.deltaTime;
  }
}
