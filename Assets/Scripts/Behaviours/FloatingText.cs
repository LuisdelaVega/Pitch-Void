using UnityEngine;

public class FloatingText : MonoBehaviour
{
  [HideInInspector] public Transform location;

  private void Start()
  {
    if (!location)
      gameObject.SetActive(false);
  }
  void Update()
  {
    if (location != null && !gameObject.activeSelf)
      gameObject.SetActive(true);

    if (location != null)
      Translate();
  }

  private void Translate() => transform.position = Camera.main.WorldToScreenPoint(new Vector2(location.position.x + 1f, location.position.y + 1f));
}
