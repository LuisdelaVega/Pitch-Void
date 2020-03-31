using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follower : MovingCharacter
{
  // Start is called before the first frame update
  void Start()
  {
    bc = GetComponent<BoxCollider2D>();
    bc.size = new Vector2(1.8f, 1.8f);
    PreviousPositions = new Queue<Vector2>();
  }

  void OnTriggerEnter2D(Collider2D other)
  {
    if (other.tag == "Player" && leader == null)
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

      // Make the Box Collider smaller
      bc.size = new Vector2(0.8f, 0.45f);
    }
  }

  public void SwitchLeader(MovingCharacter newLeader) => leader = newLeader;

  protected override void Move()
  {
    if (leader == null) return;

    var nextPosition = leader.PreviousPositions.Dequeue();
    var heading = nextPosition - (Vector2)transform.position;
    var distance = heading.sqrMagnitude;
    var direction = heading / distance;

    transform.position = Vector2.MoveTowards(transform.position, nextPosition, moveSpeed * Time.deltaTime);
    Direction = direction;
  }

  public override void RecruitFollower(Follower newFollower)
  {
    // Set my new follower then set me as his leader
    follower = newFollower;
    newFollower.SwitchLeader(this);
  }

  public void Die()
  {
    GameManager.instance.followers.Insert(GameManager.instance.followers.Count, this.name);
  }
}
