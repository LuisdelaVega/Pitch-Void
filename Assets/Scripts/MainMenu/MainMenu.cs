using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
  public GameObject LevelLoader;

  public void PlayGame(string sceneName)
  {
    LevelLoader.SetActive(true);
    StartCoroutine(LevelLoader.GetComponent<LevelLoader>().LoadLevel(sceneName));
  }
  public void QuitGame() => Application.Quit();
}
