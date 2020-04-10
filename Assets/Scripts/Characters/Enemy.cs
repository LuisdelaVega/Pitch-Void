using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MovingCharacter
{

  /* Probabilities */
  [SerializeField, Range(0, 100)] private int attackProbability = 50;
  [SerializeField, Range(0, 100)] private int moveProbability = 75;

  /* Movement */
  [SerializeField] private float randomMovementInterval = 3f; // Secconds
  private Vector2 lastDirection;

  // Start is called before the first frame update
  void Awake() => InvokeRepeating("RandomDirection", randomMovementInterval, randomMovementInterval);

  private void AvoidObstacles()
  {
    RaycastHit2D raycastHit = Physics2D.Raycast(transform.position + (Vector3)Direction, Direction, 1f);
    Debug.DrawRay(transform.position, Direction * 1f, Color.red, 0.1f);
    if (raycastHit.collider != null)
    {
      switch (raycastHit.collider.tag)
      {
        case "Enemy":
        case "Wall":
        case "Blocking Object":
          FindNewDirection(true);
          break;
        default:
          break;
      }
    }
  }

  protected override void Move()
  {
    AvoidObstacles();
    rb.MovePosition(rb.position + Direction * moveSpeed * Time.fixedDeltaTime);
  }

  private void FindNewDirection() => FindNewDirection(false);
  private void FindNewDirection(bool avoid)
  {
    canMove = true;
    // Debug.Log("FindNewDirection");
    // TODO: Revisit this. Maybe make movement based on player location, so they sort of follow him but not like zombies
    Vector2 newDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));

    // Avoid going through walls and other obstacles
    if (avoid)
    {
      if ((newDirection.x <= 0 && lastDirection.x <= 0) || (newDirection.x >= 0 && lastDirection.x >= 0))
        newDirection.x *= -1;
      if ((newDirection.y <= 0 && lastDirection.y <= 0) || (newDirection.y >= 0 && lastDirection.y >= 0))
        newDirection.y *= -1;
    }
    lastDirection = Direction;
    Direction = newDirection;
  }

  private void RandomDirection()
  {
    if (Random.Range(0, 100) < moveProbability) FindNewDirection();
    else canMove = false;
  }

  private void OnDestroy()
  {
    //TODO: Remove the code below
    PreviousPositions.Clear();
  }

  protected override void Attack() => Debug.Log("Enemy Attack");

  public override void RecruitFollower(Follower follower)
  {
    // TODO: Implement this to be used when generating enemies so we have enemy trains
  }
}
