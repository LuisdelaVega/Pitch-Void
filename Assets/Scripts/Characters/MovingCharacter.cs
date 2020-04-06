using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MovingCharacter : MonoBehaviour
{
  /* Health */
  [SerializeField] protected float health = 500f;

  /* Attack */
  protected bool holdAttack = false;

  /* Movement */
  [SerializeField] protected float moveSpeed = 5.5f;
  [SerializeField] private int maxPreviousPositions = 14;
  public Queue<Vector2> PreviousPositions { get; protected set; }
  public Vector2 Direction { get; protected set; }

  /* Related Characters */
  public MovingCharacter Leader { get; protected set; }
  protected Follower follower;

  /* Protected variables */
  protected Rigidbody2D rb;

  private void Update()
  {
    if (holdAttack)
      Attack();
  }

  void FixedUpdate()
  {
    if (moveSpeed > 0)
      Move();

    // TODO: Remove this
    Vector2 previousPosition = new Vector2(transform.position.x, transform.position.y);
    PreviousPositions.Enqueue(previousPosition);

    if (PreviousPositions.Count > maxPreviousPositions)
    {
      PreviousPositions.Dequeue();
    }
  }

  private void OnDestroy()
  {
    PreviousPositions.Clear();
  }

  public void RestoreHealth(float amount) => health += health <= 0 ? 0 : health;
  public void ApplyDamage(float damage) => health -= damage <= 0 ? 0 : damage;

  /* Abstract methods */
  protected abstract void Move();
  protected abstract void Attack();
  //TODO: Probably gonna remove the methods below
  public abstract void RecruitFollower(Follower follower);
}
