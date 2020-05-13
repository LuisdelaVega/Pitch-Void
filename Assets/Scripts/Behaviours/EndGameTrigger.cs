using System;
using UnityEngine;

public class EndGameTrigger : MonoBehaviour
{
  private bool canEndGame = false;
  public static event Action OnEndGameTrigger;

  public void OpenDoor() => GetComponent<Animator>()?.SetBool("Open", canEndGame = true);

  private void OnTriggerEnter2D(Collider2D other)
  {
    if (other.TryGetComponent<Player>(out var player) && canEndGame)
    {
      player.controls.Disable();
      OnEndGameTrigger?.Invoke();

      // TODO: This will be triggered by a Timeline signal and not here
      // GameManager.instance.GameOver(false);
    }
  }
}
