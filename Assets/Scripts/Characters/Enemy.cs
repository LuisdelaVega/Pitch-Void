using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MovingCharacter
{
  [SerializeField] private float randomTurnInterval = 3f; // Secconds
  [SerializeField] private int randomTurnChance = 100;
  private Vector2[] directions = new Vector2[]
      {
        Vector2.up,
        Vector2.right,
        Vector2.down,
        Vector2.left
      };
  private Vector2 lastDirection;
  private Player player;
  private float diagonalOfRoom;

  // Start is called before the first frame update
  void Start()
  {
    rb = GetComponent<Rigidbody2D>();
    PreviousPositions = new Queue<Vector2>();

    FindNewDirection();
    InvokeRepeating("RandomDirection", randomTurnInterval, randomTurnInterval);
  }

  // Update is called once per frame
  // void Update() => AvoidBorders();

  private void AvoidBorders()
  {
    RaycastHit2D raycastHit = Physics2D.Raycast(transform.position + (Vector3)Direction, Direction, 0.3f);
    // Debug.DrawRay(transform.position + (Vector3) Direction, Direction, Color.red, 0.1f);
    if (raycastHit.collider != null)
    {
      switch (raycastHit.collider.tag)
      {
        case "Enemy":
        case "Wall":
          FindNewDirection();
          break;
        default:
          break;
      }
    }
  }

  protected override void Move()
  {
    AvoidBorders();
    rb.MovePosition(rb.position + Direction * moveSpeed * Time.fixedDeltaTime);
  }

  protected override void Attack() => Debug.Log("Enemy Attack");

  public override void RecruitFollower(Follower follower)
  {
    // TODO: Implement this to be used when generating enemies so we have enemy trains
  }

  private void FindNewDirection()
  {
    Vector2 newDirection;
    int index;
    do
    {
      index = Random.Range(0, 4);
      newDirection = directions[index];
    } while (
        newDirection == Direction || // Makes sure to change the direction
        newDirection == lastDirection || // Avoids running into the previous border
        Direction == directions[(index + 2) % directions.Length] // Avoids making a 180 turn
    );

    lastDirection = Direction;
    Direction = newDirection;
  }

  private void RandomDirection()
  {
    if (player == null) return;

    var distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);
    var distanceToPlayerPercentage = Mathf.Abs(distanceToPlayer) / diagonalOfRoom;
    var probabilityOfTurn = randomTurnChance * distanceToPlayerPercentage;
    if (Random.Range(0, 100) < probabilityOfTurn) FindNewDirection();
  }

  private void OnDestroy()
  {
    //TODO: Remove the code below
    PreviousPositions.Clear();
  }

  /* Getters and setters */
  public void SetPlayer(Player playerCharacter) => player = playerCharacter;
  public void SetDiagonalOfRoom(Vector2 roomSize) => diagonalOfRoom = Mathf.Sqrt(Mathf.Pow(roomSize.x, 2) + Mathf.Pow(roomSize.y, 2));
}
