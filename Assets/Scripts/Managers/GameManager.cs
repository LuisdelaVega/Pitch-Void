using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.Collections.Generic;
using Cinemachine;
using Random = UnityEngine.Random; //Tells Random to use the Unity Engine random number generator.
using System.Collections;

public class GameManager : MonoBehaviour
{
  /* Public variables */
  public static GameManager instance = null;


  public CompositeCollider2D borders;

  /* Characters */
  public GameObject playerPrefab;
  private Player player;
  [HideInInspector] public List<String> followers;
  /* Enemies */
  public List<Enemy> enemyPrefabList;
  private Count enemiesCount;
  private int enemiesToSpawn;
  // private int enemiesSpawnedInRoom = 0;

  /* Items */
  public List<GameObject> weapons;

  /* Cinemachine */
  public CinemachineVirtualCamera vcam1;

  /* Lights Manager */
  public LightsManager lightsManager;

  /* Controls */
  Controls controls;

  /* Private variables */
  private int level = 6;
  private bool waitingToSpawnEnemy;

  // Start is called before the first frame update
  void Awake()
  {
    if (instance == null)
      instance = this;
    else if (instance != this)
    {
      Destroy(gameObject);
      return;
    }

    DontDestroyOnLoad(gameObject);

    // Initialize Controls
    controls = new Controls();

    // Get the min and max amount of enemies for this room
    InitializeEnemiesCount();

    //Call the InitGame function to initialize the first level 
    InitGame();
  }

  void OnEnable()
  {
    controls.Enable();
    controls.GameManager.Restart.performed += _ => Restart();
  }

  private void Update()
  {
    if (!waitingToSpawnEnemy)
      StartCoroutine(SpawnEnemies());
  }

  // [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
  // static public void CallbackInitialization()
  // {
  //   // Register the callback to be called everytime the scene is loaded
  //   SceneManager.sceneLoaded += OnSceneLoaded;
  // }

  private void InitializeEnemiesCount()
  {
    instance.enemiesCount = new Count(
        Mathf.FloorToInt(Mathf.Log(instance.level + 2, 2)),
        Mathf.CeilToInt(Mathf.Log(instance.level + 2, 2) * 2)
    );
  }

  //This is called each time a scene is loaded.
  // static private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
  // {
  //   instance.level++;
  //   instance.InitializeEnemiesCount();
  //   instance.InitGame();
  // }

  void InitGame()
  {
    StartCoroutine(lightsManager.ToggleDim(true));

    player = GetComponent<BoardManager>().SetupScene(level, playerPrefab, vcam1).GetComponent<Player>();
    enemiesToSpawn = Random.Range(enemiesCount.minimum, enemiesCount.maximum);
    // InvokeRepeating("SpawnEnemies", 2, 2);
  }

  public IEnumerator SpawnEnemies()
  {
    waitingToSpawnEnemy = true;
    yield return new WaitForSeconds(2);
    if (player != null)
    {
      // if (enemiesSpawnedInRoom < enemiesToSpawn)
      // {
      Vector2 distanceToPlayer;
      Vector2 spawnLocation;

      do
      {
        spawnLocation = new Vector2(
          Random.Range(borders.bounds.min.x + 5, borders.bounds.max.x - 5),
          Random.Range(borders.bounds.min.y + 5, borders.bounds.max.y - 5)
        );
        distanceToPlayer = spawnLocation - (Vector2)player.transform.position;
      } while (distanceToPlayer.sqrMagnitude < 50); // TODO: Revisit this number

      int index = Random.Range(0, enemyPrefabList.Count);
      var enemy = Instantiate(enemyPrefabList[index], spawnLocation, Quaternion.identity);
      // }

      // enemiesSpawnedInRoom++;
    }

    waitingToSpawnEnemy = false;
  }

  private void Restart()
  {
    // Coroutines
    StopAllCoroutines();

    // Player Character
    if (player != null)
    {
      player.enabled = false;
      Destroy(player.gameObject);
    }

    // Enemies
    waitingToSpawnEnemy = false;
    Enemy[] enemies = FindObjectsOfType<Enemy>();
    foreach (Enemy enemy in enemies)
    {
      enemy.enabled = false;
      Destroy(enemy.gameObject);
    }
    // enemiesSpawnedInRoom = 0;

    // Init game
    instance.InitializeEnemiesCount();
    instance.InitGame();
  }
}
