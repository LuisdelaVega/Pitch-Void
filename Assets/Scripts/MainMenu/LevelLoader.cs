using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
  public Animator transition;

  public void Load(string sceneName) => StartCoroutine(LoadLevel(sceneName));

  public IEnumerator LoadLevel(string sceneName)
  {
    transition.SetTrigger("Start");
    yield return new WaitForSeconds(1f);
    SceneManager.LoadScene(sceneName);
  }
}
