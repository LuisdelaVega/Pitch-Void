using System.Collections;
using UnityEngine;

public class EndGameTrigger : MonoBehaviour
{
  private bool canEndGame = false;
  private void OnEnable() => SkeletonBoss.OnBossKilled += () => StartCoroutine(OpenDoor());

  private IEnumerator OpenDoor() // TODO: Do this as part of the End Game Timeline
  {
    GetComponent<Animator>()?.SetBool("Open", canEndGame = true);
    yield return new WaitForSeconds(0.2f);
    gameObject.SetActive(false);
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
