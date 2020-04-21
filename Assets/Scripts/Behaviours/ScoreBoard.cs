using UnityEngine;
using UnityEngine.UI;

public class ScoreBoard : MonoBehaviour
{
  private GameObject scoreText;
  private int score = 0;

  private void Start() => scoreText = GameObject.Find("ScoreText");
  private void OnEnable() => Enemy.OnEnemyKilled += BumpScore;
  private void OnDisable() => Enemy.OnEnemyKilled -= BumpScore;

  private void BumpScore() => scoreText.GetComponent<Text>().text = "" + ++score;
}
