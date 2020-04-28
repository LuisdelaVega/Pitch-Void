using UnityEngine;

public class RoomSpawner : MonoBehaviour
{
  public int openingDirection;
  // 1 --> need bottom door
  // 2 --> need left door
  // 3 --> need top door
  // 4 --> need right door

  private RoomTemplates templates;
  private bool spawned = false;
  [SerializeField] private float lifeSpan = 3f;

  private void Start()
  {
    Destroy(gameObject, lifeSpan);
    templates = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTemplates>();
    Invoke("Spawn", 0.1f);
  }

  private void Spawn()
  {
    if (spawned) return;

    switch (openingDirection)
    {
      case 1:
        // Need to spawn a room with a BOTTOM door
        Instantiate(templates.bottomRooms[Random.Range(0, templates.bottomRooms.Length)], transform.position, transform.rotation).GetComponent<RoomManager>();
        break;
      case 2:
        // Need to spawn a room with a LEFT door
        Instantiate(templates.leftRooms[Random.Range(0, templates.leftRooms.Length)], transform.position, transform.rotation).GetComponent<RoomManager>();
        break;
      case 3:
        // Need to spawn a room with a TOP door
        Instantiate(templates.topRooms[Random.Range(0, templates.topRooms.Length)], transform.position, transform.rotation).GetComponent<RoomManager>();
        break;
      default:
        // Need to spawn a room with a RIGHT door
        Instantiate(templates.rightRooms[Random.Range(0, templates.rightRooms.Length)], transform.position, transform.rotation).GetComponent<RoomManager>();
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

        if (openingDirection == 1 || openingDirection == 3)
          Instantiate(templates.closedHorizontalWall, new Vector2(transform.position.x, openingDirection == 1 ? transform.position.y : transform.position.y + 22), transform.rotation);
        else
          Instantiate(templates.closedVerticalWall, new Vector2(openingDirection == 4 ? transform.position.x : transform.position.x - 21, transform.position.y), transform.rotation);

        if (otherRoomSpawner.openingDirection == 1 || otherRoomSpawner.openingDirection == 3)
          Instantiate(templates.closedHorizontalWall, new Vector2(otherRoomSpawner.transform.position.x, otherRoomSpawner.openingDirection == 1 ? otherRoomSpawner.transform.position.y : otherRoomSpawner.transform.position.y + 22), otherRoomSpawner.transform.rotation);
        else
          Instantiate(templates.closedVerticalWall, new Vector2(otherRoomSpawner.openingDirection == 4 ? otherRoomSpawner.transform.position.x : otherRoomSpawner.transform.position.x - 21, otherRoomSpawner.transform.position.y), otherRoomSpawner.transform.rotation);

        Destroy(gameObject);
      }
    }
    spawned = true;
  }
}
