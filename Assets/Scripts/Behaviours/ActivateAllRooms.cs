using System;
using UnityEngine;

public class ActivateAllRooms : MonoBehaviour
{
  public static event Action OnActivateAllRooms;

  public void ActivateRooms() => OnActivateAllRooms?.Invoke();
}
