using UnityEngine;
using UnityEngine.UI;

public class FloatingTextManager : MonoBehaviour
{
  public Canvas canvas;
  public string[] finalWords;
  public Text floatingTextPrefab;

  public void CreateFloatingText(Transform location)
  {
    Text floatingText = Instantiate(floatingTextPrefab).GetComponent<Text>();
    floatingText.transform.SetParent(canvas.transform, false);
    floatingText.GetComponent<FloatingText>().location = location;
    SetText(floatingText);
    Destroy(floatingText, 1);
  }

  public void SetText(Text floatingText) => floatingText.text = finalWords[Random.Range(0, finalWords.Length)];
}
