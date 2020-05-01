using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Random = UnityEngine.Random; //Tells Random to use the Unity Engine random number generator.

public class RoomTemplates : MonoBehaviour // TODO: Make this into a Manager (?)
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
  public List<GameObject> allRooms = new List<GameObject>();

  /* Boss */
  [HideInInspector] public GameObject bossRoom; // Maybe create a list of boss rooms and replace the one here with one of those
  public GameObject boss;
  public bool spawedBoss = false;

  /* Timers */
  public float waitTime = 2;
  public float timer = 2;

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

  private void OnEnable() => SkeletonBoss.OnBossKilled += ActivateAllRooms;
  private void OnDisable() => SkeletonBoss.OnBossKilled -= ActivateAllRooms;

  public void NewSeed()
  {
    if (!seedTextSet) // KRIUFYEQ
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

    Debug.Log("SeedText: " + seedText);
    seed = seedText.GetHashCode();
  }

  private void Update()
  {
    if (timer <= 0 && !spawedBoss)
    {
      bossRoom = allRooms[allRooms.Count - 1];
      Instantiate(boss, bossRoom.transform.position, bossRoom.transform.rotation);
      spawedBoss = true;
    }
    else
      timer -= Time.deltaTime;
  }

  private void ActivateAllRooms()
  {
    allRooms.Remove(bossRoom);
    allRooms.ForEach(room =>
    {
      RoomManager manager = room.GetComponent<RoomManager>();
      manager.RemoveGroundCollider();
      manager.SetUpRoom(false);
      manager.roomLightsManager.TurnOnLights(true);
    });
  }
}
