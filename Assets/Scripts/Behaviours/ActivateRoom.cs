using UnityEngine;
using UnityEngine.Tilemaps;

public class ActivateRoom : MonoBehaviour
{

  public LightsManager lightsManager;
  private void OnTriggerEnter2D(Collider2D other)
  {
    if (other.CompareTag("Player"))
    {
      Destroy(GetComponent<TilemapCollider2D>());
      Destroy(GetComponent<CompositeCollider2D>());
      Destroy(GetComponent<Rigidbody2D>());
      Debug.Log("Player Entered");

      StartCoroutine(lightsManager.ToggleDim(true));
    }
  }
}
