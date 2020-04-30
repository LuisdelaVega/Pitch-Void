using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
  /* Player controls */
  private Controls controls;
  public static bool GameIsPaused = false;
  public GameObject pauseMenuUI;

  private void Awake()
  {
    controls = new Controls();
    pauseMenuUI.SetActive(false);
  }

  private void OnEnable()
  {
    controls.Enable();
    controls.PauseMenu.PauseGame.performed += _ => StartPressed();
  }

  private void OnDisable() => controls.Disable();

  private void StartPressed()
  {
    if (GameIsPaused)
      Resume();
    else
      Pause();
  }

  public void Resume()
  {
    pauseMenuUI.SetActive(false);
    Time.timeScale = 1f;
    GameIsPaused = false;
  }

  private void Pause()
  {
    pauseMenuUI.SetActive(true);
    Time.timeScale = 0f;
    GameIsPaused = true;
  }

  public void QuitGame()
  {
    Resume();
    Destroy(GameManager.instance.gameObject);
    SceneManager.LoadScene("Main Menu");
  }
}
