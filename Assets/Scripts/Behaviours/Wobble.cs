using UnityEngine;

public class Wobble : MonoBehaviour
{
  [SerializeField, Range(0.1f, 5f)] private float waitBetweenWobbles = 0.5f;
  [SerializeField, Range(1f, 50f)] private float intensity = 10f;
  private Quaternion targetAngle;

  void Start() => InvokeRepeating("ChangeTarget", 0, waitBetweenWobbles);
  void Update() => transform.rotation = Quaternion.Lerp(transform.rotation, targetAngle, Time.deltaTime);

  private void ChangeTarget()
  {
    float randomIntensity = Random.Range(0.1f, intensity);
    float curve = Mathf.Sin(Random.Range(0, Mathf.PI * 2));
    targetAngle = Quaternion.Euler(Vector3.forward * curve * randomIntensity);
  }
}
