using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
  public void PlayGame(string sceneName) => SceneManager.LoadScene(sceneName);

  public void QuitGame() => Application.Quit();
}
