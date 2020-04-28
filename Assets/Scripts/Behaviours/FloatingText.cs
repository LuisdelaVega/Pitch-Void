using UnityEngine;

public class FloatingText : MonoBehaviour
{
  [HideInInspector] public Transform location;

  // Update is called once per frame
  void Update() => transform.position = Camera.main.WorldToScreenPoint(new Vector2(location.position.x + 1f, location.position.y + 1f));
}
