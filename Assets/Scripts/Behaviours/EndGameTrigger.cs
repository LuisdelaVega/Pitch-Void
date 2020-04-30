using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class EndGameTrigger : MonoBehaviour
{
  private bool canEndGame = false;
  public Text goBackText;
  [SerializeField] private float fadeTime = 2f;

  private void Start() => goBackText.gameObject.SetActive(false);
  private void OnEnable() => SkeletonBoss.OnBossKilled += () => StartCoroutine(OpenDoor());
  private void OnDisable() => SkeletonBoss.OnBossKilled -= () => StartCoroutine(OpenDoor());

  private IEnumerator OpenDoor() // TODO: Do this as part of the End Game Timeline
  {
    GetComponent<Animator>()?.SetBool("Open", canEndGame = true);
    goBackText.gameObject.SetActive(canEndGame);
    Destroy(goBackText.gameObject, 1.5f);
    yield return new WaitForSeconds(fadeTime);
  }

  private void OnTriggerEnter2D(Collider2D other)
  {
    if (other.TryGetComponent<Player>(out var player) && canEndGame)
    {
      // Start End Game Timeline animation
      Debug.Log("End the game");
    }
  }
}
