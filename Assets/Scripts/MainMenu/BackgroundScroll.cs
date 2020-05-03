using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackgroundScroll : MonoBehaviour
{
  [SerializeField] private int xSeconds = 20;

  void Update()
  {
    Material material = GetComponent<Image>().material;
    Vector2 offset = material.mainTextureOffset;
    offset.x += Time.deltaTime / xSeconds;

    material.mainTextureOffset = offset;
  }
}
