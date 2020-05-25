using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RoomManager : MonoBehaviour
{
  [SerializeField] private bool arcadeMode = false;
  /* Room */
  // [SerializeField] private bool roomIsActive = false;
  public CompositeCollider2D walls;
  private bool roomIsActive = false;
  public bool isBossRoom = false;

  /* Lights */
  public RoomLightsManager roomLightsManager;
  private Coroutine toggleDim;

  /* Enemies */
  public List<GameObject> enemyPrefabList;
  private int enemiesToSpawn = 0;
  private bool waitingToSpawnEnemy = true;
  private int enemiesSpawnedInRoom = 0;
  private int enemiesInRoom = 0;
  [SerializeField] private float spawnEnemyCooldown = 1f;

  /* Door */
  public GameObject[] doors;

  /* Ground */
  public GameObject ground;

  /* Minimap Texture */
  private Color clearedRoom = new Color(0, 255, 0);

  private void Awake() => StartCoroutine(ActivateDoors(false, false));

  private void Start()
  {
    if (arcadeMode) SetUpRoom(true);
  }

  private void OnEnable() => ActivateAllRooms.OnActivateAllRooms += ActivateRoom;

  private void OnDisable()
  {
    ActivateAllRooms.OnActivateAllRooms -= ActivateRoom;
    if (!arcadeMode && isBossRoom)
      SkeletonBoss.OnBossKilled -= UpdateEnemyCount;
    else
      Enemy.OnEnemyKilled -= UpdateEnemyCount;
  }

  private void UpdateEnemyCount()
  {
    if (--enemiesInRoom == 0 && enemiesSpawnedInRoom == enemiesToSpawn)
    {
      roomIsActive = false;
      if (!arcadeMode)
        Enemy.OnEnemyKilled -= UpdateEnemyCount;
      else
        SkeletonBoss.OnBossKilled -= UpdateEnemyCount;
      Tilemap[] tilemaps = GetComponentsInChildren<Tilemap>();
      foreach (Tilemap tilemap in tilemaps)
        if (tilemap.CompareTag("Minimap Texture"))
        {
          tilemap.color = clearedRoom;
          tilemap.GetComponent<TilemapRenderer>().sortingOrder += 1;
        }
      StartCoroutine(ActivateDoors(false));
      StopCoroutine(toggleDim);
      roomLightsManager.TurnOnLights();
    }
  }

  // Update is called once per frame
  void Update()
  {
    if (roomIsActive && !waitingToSpawnEnemy && (enemiesSpawnedInRoom < enemiesToSpawn || arcadeMode))
      StartCoroutine(SpawnEnemies());
  }

  public void SetUpRoom(bool fullSetUp)
  {
    roomIsActive = true;
    waitingToSpawnEnemy = false;
    enemiesSpawnedInRoom = 0;
    enemiesInRoom = enemiesToSpawn = !isBossRoom ? GetAmountOfEnemiesToSpawn() : 1;

    StartCoroutine(ActivateDoors(fullSetUp));
    toggleDim = StartCoroutine(roomLightsManager.ToggleDim(fullSetUp));

    if (!arcadeMode && fullSetUp)
      if (!isBossRoom)
        Enemy.OnEnemyKilled += UpdateEnemyCount;
      else
        SkeletonBoss.OnBossKilled += UpdateEnemyCount;
  }

  public IEnumerator SpawnEnemies()
  {
    waitingToSpawnEnemy = true;
    yield return new WaitForSeconds(spawnEnemyCooldown);
    if (GameManager.instance.player != null)
    {
      Vector2 distanceToPlayer;
      Vector2 spawnLocation;

      do
      {
        spawnLocation = new Vector2(
          Random.Range(walls.bounds.min.x + 5, walls.bounds.max.x - 5),
          Random.Range(walls.bounds.min.y + 5, walls.bounds.max.y - 5)
        );
        distanceToPlayer = spawnLocation - (Vector2)GameManager.instance.player.transform.position;
      } while (distanceToPlayer.sqrMagnitude < 60);

      int index = Random.Range(0, enemyPrefabList.Count);
      Instantiate(enemyPrefabList[index], spawnLocation, Quaternion.identity);

      if (!arcadeMode)
        ++enemiesSpawnedInRoom;
    }

    waitingToSpawnEnemy = false;
  }

  private int GetAmountOfEnemiesToSpawn() => Random.Range(
        Mathf.FloorToInt(Mathf.Log(GameManager.instance.level + 10, 2)),
        Mathf.CeilToInt(Mathf.Log(GameManager.instance.level + 10, 2) * 2)
    );

  private IEnumerator ActivateDoors(bool shouldClose) => ActivateDoors(shouldClose, true);
  private IEnumerator ActivateDoors(bool shouldClose, bool playAudio)
  {
    if (playAudio)
      AudioManager.instance.PlayWithRandomPitch(shouldClose ? "Door Close" : "Door Open", 0.9f, 1.1f);

    foreach (GameObject door in doors)
    {
      if (!shouldClose) // Should Open
      {
        door.GetComponent<Animator>()?.SetBool("Open", true);
        yield return new WaitForSeconds(0.25f);
        door.SetActive(false);
      }
      else
      {
        door.SetActive(true);
        door.GetComponent<Animator>()?.SetBool("Open", false);
      }
    }
  }

  public void RemoveGroundCollider()
  {
    Destroy(ground.GetComponent<TilemapCollider2D>());
    Destroy(ground.GetComponent<CompositeCollider2D>());
    Destroy(ground.GetComponent<Rigidbody2D>());
  }

  private void ActivateRoom()
  {
    if (!isBossRoom)
    {
      RemoveGroundCollider();
      roomLightsManager.TurnOnLights();
      SetUpRoom(false);
    }
  }
}
