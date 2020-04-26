using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random; //Tells Random to use the Unity Engine random number generator.

public class RoomManager : MonoBehaviour
{
  [SerializeField] private bool arcadeMode = false;
  /* Room */
  // [SerializeField] private bool roomIsActive = false;
  public CompositeCollider2D walls;
  private bool roomIsActive = false;

  /* Lights */
  public RoomLightsManager roomLightsManager;
  private Coroutine toggleDim;

  /* Enemies */
  public List<Enemy> enemyPrefabList;
  private int enemiesToSpawn = 0;
  private bool waitingToSpawnEnemy = true;
  private int enemiesSpawnedInRoom = 0;
  private int enemiesInRoom = 0;
  [SerializeField] private float spawnEnemyCooldown = 1f;

  /* Door */
  public GameObject[] doors;

  /* Minimap Texture */
  public GameObject minimapTexture;

  private void Awake() => StartCoroutine(ActivateDoors(false, false));

  private void Start()
  {
    if (arcadeMode) SetUpRoom();
  }

  private void OnDisable() => Enemy.OnEnemyKilled -= UpdateEnemyCount;

  private void UpdateEnemyCount()
  {
    if (--enemiesInRoom == 0 && enemiesSpawnedInRoom == enemiesToSpawn)
    {
      StartCoroutine(ActivateDoors(false));
      roomIsActive = false;
      StopCoroutine(toggleDim);
      roomLightsManager.TurnOnLights(true);
      minimapTexture.GetComponent<Tilemap>().color = new Color(0, 255, 0);
      minimapTexture.GetComponent<TilemapRenderer>().sortingOrder += 1;
      enabled = false;
    }
  }

  // Update is called once per frame
  void Update()
  {
    if (roomIsActive && !waitingToSpawnEnemy && (enemiesSpawnedInRoom < enemiesToSpawn || arcadeMode))
      StartCoroutine(SpawnEnemies());
  }

  public void SetUpRoom()
  {
    StartCoroutine(ActivateDoors(true));

    enemiesInRoom = enemiesToSpawn = GetAmountOfEnemiesToSpawn();
    toggleDim = StartCoroutine(roomLightsManager.ToggleDim(true));
    waitingToSpawnEnemy = false;
    roomIsActive = true;
    if (!arcadeMode)
      Enemy.OnEnemyKilled += UpdateEnemyCount;
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
      } while (distanceToPlayer.sqrMagnitude < 50); // TODO: Revisit this number

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

  private IEnumerator ActivateDoors(bool isActive) => ActivateDoors(isActive, true);
  private IEnumerator ActivateDoors(bool isActive, bool playAudio)
  {
    if (playAudio)
      AudioManager.instance.PlayWithRandomPitch(isActive ? "Door Open" : "Door Close", 0.9f, 1.1f);

    foreach (GameObject door in doors)
    {
      if (!isActive)
      {
        door.GetComponent<Animator>()?.SetBool("Open", !isActive);
        yield return new WaitForSeconds(0.2f);
        door.SetActive(isActive);
      }
      else
      {
        door.SetActive(isActive);
        door.GetComponent<Animator>()?.SetBool("Open", !isActive);
      }
    }
  }
}
