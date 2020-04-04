using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MovingCharacter
{
  /* Private variables */
  private List<Follower> followers = new List<Follower>();
  private Controls controls;

  void Awake() => controls = new Controls();

  void OnEnable()
  {
    controls.Enable();

    controls.Player.Shoot.performed += _ => Shoot();

    controls.Player.Movement.performed += ctx => ChangeDirection(ctx.ReadValue<Vector2>());
  }

  // TODO: This is for testing
  void Shoot()
  {
    Weapon weapon = gameObject.GetComponentInChildren<Weapon>();
    if (weapon != null)
    {
      weapon.Attack();
    }
  }

  private void ChangeDirection(Vector2 newDirection) => Direction = newDirection;

  void OnDisable() => controls.Disable();

  // Start is called before the first frame update
  void Start()
  {
    rb = GetComponent<Rigidbody2D>();
    PreviousPositions = new Queue<Vector2>();
    // Direction = Vector2.up; // Start moving up
  }

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
