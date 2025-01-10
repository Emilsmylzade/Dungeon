using UnityEngine;
using System.Collections.Generic;

public class RoomGenerator : MonoBehaviour
{
    public GameObject[] roomPrefabs; // Array of room prefabs to spawn
    public GameObject teleportationDoorPrefab; // Prefab for the teleportation door
    public int maxRooms = 10; // Maximum number of rooms to generate
    private int currentRoomCount = 0; // Counter for generated rooms
    private List<GameObject> generatedRooms = new List<GameObject>(); // List to keep track of generated rooms

    // Define the size of each room (adjust based on your prefab dimensions)
    public Vector2 roomSize = new Vector2(10, 10);

    // Start is called before the first frame update
    void Start()
    {
        // Instantiate the first room at origin
        GameObject firstRoom = Instantiate(roomPrefabs[Random.Range(0, roomPrefabs.Length)], Vector3.zero, Quaternion.identity);
        generatedRooms.Add(firstRoom);
        currentRoomCount++;

        // Generate additional rooms
        while (currentRoomCount < maxRooms)
        {
            GenerateRoom();
        }
    }

    // Generates a room connected to an existing room
    void GenerateRoom()
    {
        if (currentRoomCount >= maxRooms)
        {
            Debug.Log("Maximum room limit reached.");
            return;
        }

        // Select a room to connect from
        GameObject baseRoom = generatedRooms[Random.Range(0, generatedRooms.Count)];
        // Determine a connection point on the base room (e.g., a door)
        Transform doorPoint = GetRandomAvailableDoor(baseRoom);

        if (doorPoint == null)
        {
            Debug.Log("No available door points on the base room.");
            return;
        }

        // Choose a new room prefab
        GameObject newRoomPrefab = roomPrefabs[Random.Range(0, roomPrefabs.Length)];

        // Determine the new room's position based on the door's position and orientation
        Vector3 newPosition = CalculateNewRoomPosition(doorPoint, newRoomPrefab);

        // Check for overlap
        if (IsPositionOccupied(newPosition))
        {
            Debug.Log("Position occupied, skipping room placement.");
            return;
        }

        // Instantiate the new room
        GameObject newRoom = Instantiate(newRoomPrefab, newPosition, Quaternion.identity);
        generatedRooms.Add(newRoom);
        currentRoomCount++;

        // Connect doors between rooms
        ConnectDoors(doorPoint, newRoom);
    }

    // Finds a random available door in the given room
    Transform GetRandomAvailableDoor(GameObject room)
    {
        List<Transform> availableDoors = new List<Transform>();
        foreach (Transform child in room.transform)
        {
            if (child.CompareTag("Door") && !child.GetComponent<Door>().isConnected)
            {
                availableDoors.Add(child);
            }
        }

        if (availableDoors.Count == 0)
            return null;

        return availableDoors[Random.Range(0, availableDoors.Count)];
    }

    // Calculates the position for the new room based on the door's position and orientation
    Vector3 CalculateNewRoomPosition(Transform door, GameObject newRoomPrefab)
    {
        Vector3 offset = Vector3.zero;

        // Determine the direction based on door's rotation
        float rotationY = door.eulerAngles.y;
        switch ((int)rotationY)
        {
            case 0: // Facing up
                offset = new Vector3(0, roomSize.y, 0);
                break;
            case 90: // Facing right
                offset = new Vector3(roomSize.x, 0, 0);
                break;
            case 180: // Facing down
                offset = new Vector3(0, -roomSize.y, 0);
                break;
            case 270: // Facing left
                offset = new Vector3(-roomSize.x, 0, 0);
                break;
            default:
                offset = new Vector3(0, roomSize.y, 0);
                break;
        }

        return door.position + offset;
    }

    // Checks if the position is already occupied by another room
    bool IsPositionOccupied(Vector3 position)
    {
        foreach (GameObject room in generatedRooms)
        {
            if (Vector3.Distance(room.transform.position, position) < roomSize.x)
            {
                return true;
            }
        }
        return false;
    }

    // Connects the doors between two rooms
    void ConnectDoors(Transform doorA, GameObject roomB)
    {
        // Find the corresponding door in the new room to connect
        Transform doorB = GetMatchingDoor(roomB, doorA);

        if (doorB != null)
        {
            // Mark doors as connected to avoid reuse
            doorA.GetComponent<Door>().isConnected = true;
            doorB.GetComponent<Door>().isConnected = true;

            // Instantiate teleportation doors or other connection mechanisms
            Vector3 connectionPosition = (doorA.position + doorB.position) / 2;
            Instantiate(teleportationDoorPrefab, connectionPosition, Quaternion.identity);
        }
    }

    // Finds a door in the new room that matches the direction opposite to doorA
    Transform GetMatchingDoor(GameObject newRoom, Transform doorA)
    {
        float oppositeRotation = (doorA.eulerAngles.y + 180) % 360;
        foreach (Transform door in newRoom.transform)
        {
            if (door.CompareTag("Door") && Mathf.Approximately(door.eulerAngles.y, oppositeRotation))
            {
                return door;
            }
        }
        return null;
    }
}

public class Door : MonoBehaviour
{
    public bool isConnected = false;
}
