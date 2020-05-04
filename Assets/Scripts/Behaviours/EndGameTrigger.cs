using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class EndGameTrigger : MonoBehaviour
{
  private bool canEndGame = false;

  public void OpenDoor() => GetComponent<Animator>()?.SetBool("Open", canEndGame = true);

  private void OnTriggerEnter2D(Collider2D other)
  {
    if (other.TryGetComponent<Player>(out var player) && canEndGame)
    {
      // TODO: Start End Game Timeline animation
      Debug.Log("End the game");
    }
  }
}
