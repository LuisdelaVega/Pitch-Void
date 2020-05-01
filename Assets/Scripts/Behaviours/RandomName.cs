using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomName : MonoBehaviour
{
  /* Player */
  public Transform player;

  /* Popup Text */
  public FloatingTextManager floatingText;

  /* Resources */
  private string[] firstNames;
  private string[] lastNames;

  // Start is called before the first frame update
  void Awake()
  {
    // Get first and last names
    firstNames = Resources.Load<TextAsset>("FirstNames").text.Split("\n"[0]);
    lastNames = Resources.Load<TextAsset>("LastNames").text.Split("\n"[0]);

    string name = $"{firstNames[Random.Range(0, firstNames.Length)]} {lastNames[Random.Range(0, lastNames.Length)]}";

    floatingText = GameObject.Find("Floating Text Manager").GetComponent<FloatingTextManager>();
    floatingText.CreateFloatingText(player, $"{name} reporting for duty!");
  }
}
