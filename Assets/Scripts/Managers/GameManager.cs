using UnityEngine;
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
    if (arcadeMode)
      player.GetComponent<Player>().EnablePlayerControls();
    resetText.SetActive(false);
  }

  public void GameOver() => resetText.SetActive(true);

  private void Restart()
  {
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
