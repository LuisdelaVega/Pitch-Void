using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroyer : MonoBehaviour
{
  [SerializeField] private float lifeSpan = 1f;
  private void Start() => Destroy(gameObject, lifeSpan);

  private void OnTriggerEnter2D(Collider2D other)
  {
    if (other.CompareTag("SpawnPoint")) Destroy(other.gameObject);
  }
}
