using UnityEngine;
using Random = UnityEngine.Random; //Tells Random to use the Unity Engine random number generator.

public class RoomSpawner : MonoBehaviour
{
  public int openingDirection;
  // 1 --> need bottom door
  // 2 --> need left door
  // 3 --> need top door
  // 4 --> need right door

  private RoomTemplates templates;
  private bool spawned = false;
  [SerializeField] private float lifeSpan = 1f;

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
        Instantiate(templates.bottomRooms[Random.Range(0, templates.bottomRooms.Length)], transform.position, transform.rotation).GetComponent<RoomManager>();//?.SpawnCover();
        break;
      case 2:
        // Need to spawn a room with a LEFT door
        Instantiate(templates.leftRooms[Random.Range(0, templates.leftRooms.Length)], transform.position, transform.rotation).GetComponent<RoomManager>();//?.SpawnCover();
        break;
      case 3:
        // Need to spawn a room with a TOP door
        Instantiate(templates.topRooms[Random.Range(0, templates.topRooms.Length)], transform.position, transform.rotation).GetComponent<RoomManager>();//?.SpawnCover();
        break;
      default:
        // Need to spawn a room with a RIGHT door
        Instantiate(templates.rightRooms[Random.Range(0, templates.rightRooms.Length)], transform.position, transform.rotation).GetComponent<RoomManager>();//?.SpawnCover();
        break;
    }

    spawned = true;
  }

  private void OnTriggerEnter2D(Collider2D other)
  {
    if (other.CompareTag(gameObject.tag))
    {
      if (!other.GetComponent<RoomSpawner>().spawned && !spawned)
      {
        Instantiate(templates.closedRoom, transform.position, transform.rotation);
        Destroy(gameObject);
      }
    }
    spawned = true;
  }
}
