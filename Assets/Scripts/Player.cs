using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MovingCharacter
{
  /* Private variables */
  private List<Follower> followers = new List<Follower>();

  /* Weapons */
  public List<Weapon> weapons;
  private Weapon activeWeapon;

  /* Player controls */
  private Controls controls;

  void Awake() => controls = new Controls();

  void Start()
  {
    rb = GetComponent<Rigidbody2D>();
    PreviousPositions = new Queue<Vector2>();
    activeWeapon = gameObject.GetComponentInChildren<Weapon>();
  }

  void OnEnable()
  {
    controls.Enable();

    controls.Weapon.Attack.performed += ctx =>
    {
      Attack();
      holdAttack = ctx.ReadValue<float>() >= 0.9f;
    };
    controls.Player.Movement.performed += ctx => ChangeDirection(ctx.ReadValue<Vector2>());
  }

  private void ChangeDirection(Vector2 newDirection) => Direction = newDirection;

  void OnDisable() => controls.Disable();

  // Start is called before the first frame update

  void OnTriggerEnter2D(Collider2D other)
  {
    switch (other.tag)
    {
      case "Border":
        // GameManager.instance.GameOver();
        break;
    }
  }

  protected override void Move() => rb.MovePosition(rb.position + Direction * moveSpeed * Time.fixedDeltaTime);

  protected override void Attack() => activeWeapon.Attack();

  public override void RecruitFollower(Follower newFollower)
  {
    if (followers.Count == 0)
      follower = newFollower;

    followers.Insert(followers.Count, newFollower);
  }

  protected override void Die()
  {
    foreach (var currentFollower in followers)
    {
      Destroy(currentFollower.gameObject);
    }

    Destroy(gameObject);
  }

  public void RemoveFollower(Follower deadFollower)
  {
    followers.Remove(deadFollower);
  }

  /* Getters and Setters */
  public int GetFollowerCount() => followers.Count;
  public Follower GetLastFollower() => followers.Count != 0 ? followers[followers.Count - 1] : null;
}
