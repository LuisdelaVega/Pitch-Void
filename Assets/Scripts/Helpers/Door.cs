using UnityEngine;

[System.Serializable]
public class Door
{
  public float x;
  public float y;
  [Range(0, 90)] public int rotation;
}
