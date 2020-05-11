using UnityEngine;
using UnityEngine.UI;

public class UpdateSeedText : MonoBehaviour
{
  // Start is called before the first frame update
  void Start()
  {
    if (TryGetComponent<Text>(out var seedText))
      seedText.text = $"SEED: {RoomTemplates.instance.seedText}";
  }
}
