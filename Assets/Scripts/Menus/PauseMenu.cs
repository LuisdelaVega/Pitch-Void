using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
  /* Player controls */
  private Controls controls;
  public static bool gameIsPaused = false;
  public GameObject pauseMenu;

  /* Player Character */
  public GameObject player;

  private void Awake()
  {
    controls = new Controls();
    pauseMenu.SetActive(false);
  }

  private void OnEnable()
  {
    controls.Enable();
    controls.PauseMenu.PauseGame.performed += _ => PauseButtonPressed();
  }

  private void OnDisable() => controls.Disable();

  private void PauseButtonPressed()
  {
    if (gameIsPaused)
      Resume();
    else
      Pause();
  }

  public void Resume()
  {
    pauseMenu.SetActive(false);
    Time.timeScale = 1f;
    gameIsPaused = false;
    if (player != null && player.TryGetComponent<Player>(out var playerCharacter))
      playerCharacter.controls.Enable();
  }

  private void Pause()
  {
    pauseMenu.SetActive(true);
    Time.timeScale = 0f;
    gameIsPaused = true;
    if (player != null && player.TryGetComponent<Player>(out var playerCharacter))
      playerCharacter.controls.Disable();
  }

  public void QuitGame()
  {
    Resume();
    if (GameManager.instance != null)
      Destroy(GameManager.instance.gameObject);
    if (RoomTemplates.instance != null)
      Destroy(RoomTemplates.instance.gameObject);
    SceneManager.LoadScene("Main Menu");
  }
}
