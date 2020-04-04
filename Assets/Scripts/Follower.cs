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

  public override void RecruitFollower(Follower newFollower)
  {
    // Set my new follower then set me as his Leader
    follower = newFollower;
    newFollower.SwitchLeader(this);
  }

  protected override void Die()
  {
    // If I have a follower, make him follow my Leader
    if (follower != null)
      follower.SwitchLeader(Leader);

    // Find the Player and remove myself from the list of Characters following him
    MovingCharacter player = Leader;
    while (player.Leader != null)
      player = player.Leader;

    player.GetComponent<Player>().RemoveFollower(this);

    // Subscribe myself to the available Followers
    string name = this.name.Substring(0, this.name.Length - "(Clone)".Length);
    GameManager.instance.followers.Insert(GameManager.instance.followers.Count, name);
  }
}
