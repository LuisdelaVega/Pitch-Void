using UnityEngine;

public class RoomSpawner : MonoBehaviour
{
  public int openingDirection;
  // 1 --> need bottom door
  // 2 --> need left door
  // 3 --> need top door
  // 4 --> need right door

  public bool spawned = false;
  [SerializeField] private float lifeSpan = 3f;

  private void Start()
  {
    Destroy(gameObject, lifeSpan);
    Invoke("Spawn", 0.1f);
  }

  private void Spawn()
  {
    if (spawned) return;
    Random.InitState(RoomTemplates.instance.seed++);

    switch (openingDirection)
    {
      case 1:
        // Need to spawn a room with a BOTTOM door
        Instantiate(RoomTemplates.instance.bottomRooms[Random.Range(0, RoomTemplates.instance.bottomRooms.Length)], transform.position, transform.rotation).GetComponent<RoomManager>();
        break;
      case 2:
        // Need to spawn a room with a LEFT door
        Instantiate(RoomTemplates.instance.leftRooms[Random.Range(0, RoomTemplates.instance.leftRooms.Length)], transform.position, transform.rotation).GetComponent<RoomManager>();
        break;
      case 3:
        // Need to spawn a room with a TOP door
        Instantiate(RoomTemplates.instance.topRooms[Random.Range(0, RoomTemplates.instance.topRooms.Length)], transform.position, transform.rotation).GetComponent<RoomManager>();
        break;
      default:
        // Need to spawn a room with a RIGHT door
        Instantiate(RoomTemplates.instance.rightRooms[Random.Range(0, RoomTemplates.instance.rightRooms.Length)], transform.position, transform.rotation).GetComponent<RoomManager>();
        break;
    }

    spawned = true;
  }

  private void OnTriggerEnter2D(Collider2D other)
  {
    if (other.CompareTag(gameObject.tag))
    {
      RoomSpawner otherRoomSpawner = other.GetComponent<RoomSpawner>();
      if (!otherRoomSpawner.spawned && !spawned)
      {
        CloseRoom(this);
        if (otherRoomSpawner.isActiveAndEnabled)
          CloseRoom(otherRoomSpawner);
        Destroy(gameObject);
      }
    }
    spawned = true;
  }

  public void CloseRoom(RoomSpawner spawner)
  {
    GameObject wall;
    Vector2 wallPosition;
    if (spawner.openingDirection == 1 || spawner.openingDirection == 3)
    {
      wall = RoomTemplates.instance.closedHorizontalWall;
      wallPosition = new Vector2(
          spawner.transform.position.x,
          spawner.openingDirection == 1 ? spawner.transform.position.y : spawner.transform.position.y + 22
        );
    }
    else
    {
      wall = RoomTemplates.instance.closedVerticalWall;
      wallPosition = new Vector2(
          spawner.openingDirection == 4 ? spawner.transform.position.x : spawner.transform.position.x - 21,
          spawner.transform.position.y
        );
    }

    Instantiate(wall, wallPosition, spawner.transform.rotation, spawner.transform.parent.transform);
  }
}
