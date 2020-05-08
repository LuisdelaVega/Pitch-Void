using UnityEngine;
using UnityEngine.UI;

public class FloatingTextManager : MonoBehaviour
{
  public Canvas canvas;
  public string[] finalWords;
  public Text floatingTextPrefab;

  public void CreateFloatingText(Transform location) => CreateFloatingText(location, finalWords[Random.Range(0, finalWords.Length)]);
  public void CreateFloatingText(Transform location, string message)
  {
    Text floatingText = Instantiate(floatingTextPrefab).GetComponent<Text>();
    floatingText.transform.SetParent(canvas.transform, false);
    floatingText.GetComponent<FloatingText>().location = location;
    floatingText.text = message;
    Destroy(floatingText, 2);
  }
}
