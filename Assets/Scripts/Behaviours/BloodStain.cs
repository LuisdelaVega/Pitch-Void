using UnityEngine;
using Random = UnityEngine.Random; //Tells Random to use the Unity Engine random number generator.

public class BloodStain : MonoBehaviour
{
  [SerializeField] private float minScaleX = 1f;
  [SerializeField] private float maxScaleX = 2.5f;
  [SerializeField] private float minScaleY = 1f;
  [SerializeField] private float maxScaleY = 4f;

  // Start is called before the first frame update
  void Start()
  {
    transform.localScale = new Vector3(
        Random.Range(minScaleX, maxScaleX),
        Random.Range(minScaleY, maxScaleY),
        1
    );
  }
}
