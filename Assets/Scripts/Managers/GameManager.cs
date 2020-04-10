﻿using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.Collections.Generic;
using System.Collections;
using Cinemachine;
using Random = UnityEngine.Random; //Tells Random to use the Unity Engine random number generator.


public class GameManager : MonoBehaviour
{

  // Using Serializable allows us to embed a class with sub properties in the inspector.
  [Serializable]
  public class Count
  {
    public int minimum; //Minimum value for our Count class.
    public int maximum; //Maximum value for our Count class.


    //Assignment constructor.
    public Count(int min, int max)
    {
      minimum = min;
      maximum = max;
    }
  }

  /* Public variables */
  public static GameManager instance = null;
  public CompositeCollider2D borders;

  /* Characters */
  public List<GameObject> characters;
  private Player player;
  private GameObject playerPrefab;
  [HideInInspector] public List<String> followers;
  public List<Enemy> innsmouthEnemies;
  private Count enemiesCount;
  private int enemiesToSpawn;
  private int enemiesSpawnedInRoom = 0;

  /* Items */
  public List<GameObject> weapons;

  /* Cinemachine */
  public CinemachineVirtualCamera vcam1;

  /* Private variables */
  private BoardManager boardManager;
  [SerializeField] private float levelStartDelay = 0f;
  private GameObject levelImage;
  private int level = 1;
  private string[] levels = new string[]
      {
            "Innsmouth",
            "Dunwich"
      };
  // private Text levelText;
  // private bool doingSetup = true;

  // Start is called before the first frame update
  void Awake()
  {
    if (instance == null)
      instance = this;
    else if (instance != this)
      Destroy(gameObject);

    DontDestroyOnLoad(gameObject);

    // Get a component reference to the attached BoardManager script
    boardManager = GetComponent<BoardManager>();

    // Get the min and max amount of enemies for this room
    InitializeEnemiesCount();

    // TODO: Create a character select screen
    playerPrefab = characters[Random.Range(0, characters.Count)];
    characters.ForEach(character =>
    {
      if (character.name != playerPrefab.name)
        followers.Insert(followers.Count, character.name);
    });

    //Call the InitGame function to initialize the first level 
    InitGame();
  }

  [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
  static public void CallbackInitialization()
  {
    // Register the callback to be called everytime the scene is loaded
    SceneManager.sceneLoaded += OnSceneLoaded;
  }

  private void InitializeEnemiesCount()
  {
    instance.enemiesCount = new Count(
        Mathf.FloorToInt(Mathf.Log(instance.level + 2, 2)),
        Mathf.CeilToInt(Mathf.Log(instance.level + 2, 2) * 2)
    );
  }

  //This is called each time a scene is loaded.
  static private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
  {
    instance.level++;
    instance.InitializeEnemiesCount();
    instance.InitGame();
  }

  void InitGame()
  {
    //While doingSetup is true the player can't move, prevent player from moving while title card is up.
    // doingSetup = true;

    //Get a reference to our image LevelImage by finding it by name.
    levelImage = GameObject.Find("LevelImage");

    //Call the HideLevelImage function with a delay in seconds of levelStartDelay.
    Invoke("HideLevelImage", levelStartDelay);

    player = boardManager.SetupScene(level, playerPrefab, vcam1).GetComponent<Player>();

    enemiesToSpawn = Random.Range(enemiesCount.minimum, enemiesCount.maximum);

    InvokeRepeating("SpawnEnemies", 3, 4);
    // InvokeRepeating("SpawnFollowwers", 3, 4);
  }

  //Hides black image used between levels
  void HideLevelImage()
  {
    //Disable the levelImage gameObject.
    levelImage.SetActive(false);

    //Set doingSetup to false allowing player to move again.
    // doingSetup = false;
  }

  // Update is called once per frame
  // void Update()
  // {
  //     if (doingSetup) return;
  // }

  private void SpawnEnemies()
  {
    if (enemiesSpawnedInRoom < enemiesToSpawn)
    {
      switch (levels[level])
      {
        case "Innsmouth":
        default:
          // We cast the coordinates to int to make sure that the food is always spawned at a position like (1, 2) but never at something like (1.234, 2.74565)
          int x = (int)Random.Range(borders.bounds.min.x + 2, borders.bounds.max.x - 2);
          int y = (int)Random.Range(borders.bounds.min.y + 2, borders.bounds.max.y - 2);

          int index = Random.Range(0, innsmouthEnemies.Count);
          var enemy = Instantiate(innsmouthEnemies[index], new Vector2(x, y), Quaternion.identity);
          break;
      }

      enemiesSpawnedInRoom++;
    }
  }

  // TODO: This will not happen here in the future. Enemies will spawn the followers when they die
  private void SpawnFollowwers()
  {
    if (followers.Count == 0) return;

    // We cast the coordinates to int to make sure that the food is always spawned at a position like (1, 2) but never at something like (1.234, 2.74565)
    int x = (int)Random.Range(borders.bounds.min.x + 2, borders.bounds.max.x - 2);
    int y = (int)Random.Range(borders.bounds.min.y + 2, borders.bounds.max.y - 2);

    int index = Random.Range(0, followers.Count);
    var followerPrefab = characters.Find(character => character.name == followers[index]);
    var follower = Instantiate(followerPrefab, new Vector2(x, y), Quaternion.identity).AddComponent(typeof(Follower));
    follower.tag = "Follower";
    followers.Remove(follower.name.Substring(0, follower.name.Length - "(Clone)".Length));
  }

  public void GameOver()
  {
    //Set levelText to display number of levels passed and game over message
    // levelText.text = "You died after " + level + " day" + (level > 1 ? "s" : "");

    //Enable black background image gameObject.
    levelImage.SetActive(true);
    enabled = false;
  }
}