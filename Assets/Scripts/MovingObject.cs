using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MovingObject : MonoBehaviour
{
  /* Public variables */
  public Queue<Vector2> previousPositions;

  /* Protected variables */
  [SerializeField] protected float moveSpeed = 8f;
  protected Rigidbody2D rb;
  protected BoxCollider2D bc;
  protected MovingObject leader;
  protected Follower follower;
  public Vector2 Direction { get; protected set; }

  /* Private variables */
  private int maxPreviousPositions = 8;

  // This function is called every fixed framerate frame, if the MonoBehaviour is enabled.
  void FixedUpdate()
  {
    // Remember your previous positions
    Vector2 previousPosition = new Vector2(transform.position.x, transform.position.y);
    previousPositions.Enqueue(previousPosition);

    if (previousPositions.Count > maxPreviousPositions)
    {
      previousPositions.Dequeue();
    }

    Move();
  }

  protected abstract void Move();
  public abstract void RecruitFollower(Follower follower);
}
