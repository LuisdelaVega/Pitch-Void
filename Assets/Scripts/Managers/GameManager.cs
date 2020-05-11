using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Cinemachine;

public class GameManager : MonoBehaviour
{
  public static GameManager instance = null;

  /* Game mode */
  [SerializeField] private bool arcadeMode = false;
  [SerializeField] private bool repeatSeed = false;

  /* Player */
  public GameObject player;

  /* Cinemachine */
  public CinemachineVirtualCamera vcam1;

  /* Controls */
  private Controls controls;

  /* Level */
  public int level = 1;

  /* UI */
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
    endGameScreen.SetActive(false);

    GetComponent<Timer>().StartTimer();
    RoomTemplates.instance.GameStarted(true);
  }

  public void GameOver()
  {
    Time.timeScale = 0f;
    endGameScreen.SetActive(true);
    timerText.text = GetComponent<Timer>().GetElapsedTime();
  }

  public void RepeatSeed(bool value) => repeatSeed = value;

  public void Restart()
  {
    Time.timeScale = 1f;
    // Handle RoomTemplates
    if (RoomTemplates.instance != null)
    {
      RoomTemplates.instance.GameStarted(false);
      RoomTemplates.instance.timer = RoomTemplates.instance.waitTime;
      RoomTemplates.instance.bossRoomChosen = false;
      if (repeatSeed)
        RoomTemplates.instance.seedTextSet = true;
      else
      {
        RoomTemplates.instance.seedTextSet = false;
        RoomTemplates.instance.NewSeed();
      }
    }

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
