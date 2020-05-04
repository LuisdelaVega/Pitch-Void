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

  /* Blood Effects */
  public GameObject bloodParticleEffect;
  public GameObject bloodStain;

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
    if (!canMove) Direction = Vector2.zero;
    Move();
  }

  public void EnableMovement(bool enabled)
  {
    Debug.Log($"canMove: {canMove}");
    canMove = enabled;
  }

  /* Abstract methods */
  protected abstract void Move();
  protected abstract void Attack();
  public abstract void Die(Quaternion rotation);
}
