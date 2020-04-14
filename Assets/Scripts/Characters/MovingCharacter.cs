using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MovingCharacter : MonoBehaviour
{
  /* Attack */
  protected bool holdAttack = false;

  /* Movement */
  [SerializeField] protected float moveSpeed = 5.5f;
  [SerializeField] private int maxPreviousPositions = 14;
  public Queue<Vector2> PreviousPositions { get; protected set; }
  public Vector2 Direction { get; protected set; }
  protected bool canMove = true;

  /* Related Characters */
  public MovingCharacter Leader { get; protected set; }
  protected Follower follower;

  /* Components */
  protected Rigidbody2D rb;
  public Animator animator;
  [HideInInspector] public SpriteRenderer spriteRenderer;

  void Start()
  {
    rb = GetComponent<Rigidbody2D>();
    spriteRenderer = GetComponent<SpriteRenderer>();
    PreviousPositions = new Queue<Vector2>(); // TODO: Remove this
  }

  private void Update()
  {
    if (animator != null) animator.SetFloat("Speed", Direction.sqrMagnitude); // TODO: The conditional will not be needed in the future

    if (holdAttack) Attack();
  }

  void FixedUpdate()
  {
    if (canMove)
      Move();

    // TODO: Remove the code below
    Vector2 previousPosition = new Vector2(transform.position.x, transform.position.y);
    PreviousPositions.Enqueue(previousPosition);

    if (PreviousPositions.Count > maxPreviousPositions)
    {
      PreviousPositions.Dequeue();
    }
  }

  /* Abstract methods */
  protected abstract void Move();
  protected abstract void Attack();
  //TODO: Probably gonna remove the methods below
  public abstract void RecruitFollower(Follower follower);
}
