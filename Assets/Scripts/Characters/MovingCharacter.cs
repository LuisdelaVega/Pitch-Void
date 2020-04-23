using UnityEngine;

public abstract class MovingCharacter : MonoBehaviour
{
  /* Attack */
  protected bool holdAttack = false;

  /* Movement */
  [SerializeField] protected float moveSpeed = 5.5f;
  public Vector2 Direction { get; protected set; }
  protected bool canMove = true;

  /* Components */
  protected Rigidbody2D rb;
  public Animator animator;
  [HideInInspector] public SpriteRenderer spriteRenderer;

  void Start()
  {
    rb = GetComponent<Rigidbody2D>();
    spriteRenderer = GetComponent<SpriteRenderer>();
  }

  private void Update()
  {
    if (holdAttack) Attack();
  }

  void FixedUpdate()
  {
    if (canMove) Move();
  }

  /* Abstract methods */
  protected abstract void Move();
  protected abstract void Attack();
  public abstract void Die();
}
