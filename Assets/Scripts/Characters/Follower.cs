using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follower : MovingCharacter
{
  // Start is called before the first frame update
  void Start()
  {
    PreviousPositions = new Queue<Vector2>();
  }

  void OnTriggerEnter2D(Collider2D other)
  {
    if (other.tag == "Player" && Leader == null)
    {
      Player player = other.gameObject.GetComponent<Player>();
      int followerCount = player.GetFollowerCount();

      if (followerCount > 0)
      {
        Follower lastFollower = player.GetLastFollower();
        lastFollower.RecruitFollower(this);
      }
      else
        SwitchLeader(player);

      player.RecruitFollower(this);
    }
  }

  public void SwitchLeader(MovingCharacter newLeader) => Leader = newLeader;

  protected override void Move()
  {
    if (Leader == null) return;

    var nextPosition = Leader.PreviousPositions.Dequeue();
    var heading = nextPosition - (Vector2)transform.position;
    var distance = heading.sqrMagnitude;
    var direction = heading / distance;

    transform.position = Vector2.MoveTowards(transform.position, nextPosition, moveSpeed * Time.fixedDeltaTime);
    Direction = direction;
  }

  // TODO: Implement this
  protected override void Attack() => Debug.Log("Attack");

  public override void RecruitFollower(Follower newFollower)
  {
    // Set my new follower then set me as his Leader
    follower = newFollower;
    newFollower.SwitchLeader(this);
  }
}
