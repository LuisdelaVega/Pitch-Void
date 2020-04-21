using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;

public class GameManager : MonoBehaviour
{
  public static GameManager instance = null;

  /* Player */
  public GameObject playerPrefab;
  [HideInInspector] public GameObject player;

  /* Cinemachine */
  public CinemachineVirtualCamera vcam1;

  /* Controls */
  private Controls controls;

  /* Level */
  public int level = 6;

  /* UI */
  private GameObject resetText;

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

    resetText = GameObject.Find("ResetText");

    //Call the InitGame function to initialize the first level 
    InitGame();
  }

  void OnEnable()
  {
    controls.Enable();
    controls.GameManager.Restart.performed += _ => Restart();
  }

  void OnDisable() => controls.Disable();

  void InitGame()
  {
    resetText.SetActive(false);
    InstantiatePlayer();
  }

  public void InstantiatePlayer()
  {
    player = Instantiate(playerPrefab, new Vector2(0, 0), Quaternion.identity);
    vcam1.LookAt = player.transform;
    vcam1.Follow = player.transform;
  }

  public void GameOver() => resetText.SetActive(true);

  private void Restart()
  {
    // Coroutines
    StopAllCoroutines();

    // // Player Character
    if (player != null)
      Destroy(player.gameObject);

    // // Enemies
    // enemiesSpawnedInRoom = 0;
    // waitingToSpawnEnemy = false;
    // Enemy[] enemies = FindObjectsOfType<Enemy>();
    // foreach (Enemy enemy in enemies)
    // {
    //   enemy.enabled = false;
    //   Destroy(enemy.gameObject);
    // }

    // Init game
    instance = null;
    enabled = false;
    Destroy(gameObject);
    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
  }
}
