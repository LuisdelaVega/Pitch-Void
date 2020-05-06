using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Cinemachine;

public class GameManager : MonoBehaviour
{
  public static GameManager instance = null;

  /* Game mode */
  [SerializeField] private bool arcadeMode = false;

  /* Player */
  public GameObject player;

  /* Cinemachine */
  public CinemachineVirtualCamera vcam1;

  /* Controls */
  private Controls controls;

  /* Level */
  public int level = 1;

  /* UI */
  public GameObject resetText;
  public Texture2D cursor;
  public Text timerText;
  public GameObject endGameScreen;

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

    //Call the InitGame function to initialize the first level
    InitGame();
  }

  private void Start() => Cursor.SetCursor(cursor, Vector2.zero, CursorMode.ForceSoftware);

  void OnEnable()
  {
    controls.Enable();
    controls.GameManager.Restart.performed += _ => Restart();
  }

  void OnDisable() => controls.Disable();

  void InitGame()
  {
    if (arcadeMode)
      player.GetComponent<Player>().EnablePlayerControls();
    resetText.SetActive(false);
    endGameScreen.SetActive(false);

    GetComponent<Timer>().StartTimer();
  }

  public void GameOver() => resetText.SetActive(true);

  public void GameEnded()
  {
    Time.timeScale = 0f;
    endGameScreen.SetActive(true);
    timerText.text = GetComponent<Timer>().GetElapsedTime();
  }

  public void Restart()
  {
    Time.timeScale = 1f;
    // Handle RoomTemplates
    RoomTemplates.instance.seedTextSet = false;
    RoomTemplates.instance.NewSeed();
    RoomTemplates.instance.timer = RoomTemplates.instance.waitTime;
    RoomTemplates.instance.spawedBoss = false;

    // Coroutines
    StopAllCoroutines();

    // // Player Character
    if (player != null)
      Destroy(player.gameObject);

    Destroy(gameObject);

    // Init game
    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
  }
}
