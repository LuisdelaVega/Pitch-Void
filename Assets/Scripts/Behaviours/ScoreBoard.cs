using UnityEngine;
using UnityEngine.UI;

public class ScoreBoard : MonoBehaviour
{
  public GameObject scoreText;
  private int score = 0;

  private void OnEnable() => Enemy.OnEnemyKilled += BumpScore;
  private void OnDisable() => Enemy.OnEnemyKilled -= BumpScore;

  private void BumpScore() => scoreText.GetComponent<Text>().text = "" + ++score;
}
