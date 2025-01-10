using UnityEngine;
using System.Collections.Generic;

public class RoomGenerator : MonoBehaviour
{
    [Header("Room Prefabs")]
    public GameObject[] roomPrefabs; // Array of room prefabs to spawn

    [Header("Start Room")]
    public GameObject startRoomPrefab; // Dedicated prefab for the start room

    [Header("Teleportation Door")]
    public GameObject teleportationDoorPrefab; // Prefab for the teleportation door

    [Header("Generation Settings")]
    public int maxRooms = 10; // Maximum number of rooms to generate
    public float roomDistance = 2f; // Additional distance between rooms (modifiable by game designer)

    private int currentRoomCount = 0; // Counter for generated rooms
    private List<GameObject> generatedRooms = new List<GameObject>(); // List to keep track of generated rooms

    [Header("Room Size")]
    public Vector2 roomSize = new Vector2(10, 10); // Define room size (adjust to prefab dimensions)

    [Header("Placement Settings")]
    public Vector3 startPosition = Vector3.zero; // Starting position for the first room

    // Room offsets based on direction and room size
    private Vector3 roomOffsetRight;
    private Vector3 roomOffsetUp;
    private Vector3 roomOffsetDown;
    private Vector3 roomOffsetLeft;

    // To keep track of available doors for expansion
    private Queue<RoomConnection> connectionQueue = new Queue<RoomConnection>();

    void Start()
    {
        // Calculate room offsets based on roomSize and roomDistance
        // Assuming roomSize represents the width (x) and height (y) of the room
        roomOffsetRight = new Vector3((roomSize.x) + roomDistance, 0, 0); // Right
        roomOffsetLeft = new Vector3(-(roomSize.x) - roomDistance, 0, 0); // Left
        roomOffsetUp = new Vector3(0, (roomSize.y) + roomDistance, 0); // Up
        roomOffsetDown = new Vector3(0, -(roomSize.y) - roomDistance, 0); // Down

        // Instantiate the start room at the starting position
        GameObject firstRoom = Instantiate(
            startRoomPrefab, // Use the dedicated start room prefab
            startPosition,
            Quaternion.identity
        );
        generatedRooms.Add(firstRoom);
        currentRoomCount++;

        // Enqueue available connections from the start room
        EnqueueAvailableDoors(firstRoom);

        // Generate additional rooms until maxRooms is reached or no more connections are available
        while (currentRoomCount < maxRooms && connectionQueue.Count > 0)
        {
            RoomConnection connection = connectionQueue.Dequeue();
            GenerateRoom(connection);
        }

        // After generation, deactivate unlinked doors
        DeactivateUnlinkedDoors();

        Debug.Log($"Room generation complete. Total rooms: {currentRoomCount}");
    }

    void EnqueueAvailableDoors(GameObject room)
    {
        // Check all four doors and enqueue them if they're not connected
        EnqueueDoorConnection(room, "RightDoor", roomOffsetRight, DoorDirection.Right, DoorDirection.Left);
        EnqueueDoorConnection(room, "TopDoor", roomOffsetUp, DoorDirection.Top, DoorDirection.Down);
        EnqueueDoorConnection(room, "LeftDoor", roomOffsetLeft, DoorDirection.Left, DoorDirection.Right);
        EnqueueDoorConnection(room, "DownDoor", roomOffsetDown, DoorDirection.Down, DoorDirection.Top);
    }


    void EnqueueDoorConnection(GameObject room, string doorName, Vector3 offset, DoorDirection direction, DoorDirection oppositeDirection)
    {
        Transform doorTransform = room.transform.Find(doorName);
        if (doorTransform != null)
        {
            Door doorComponent = doorTransform.GetComponent<Door>();
            if (doorComponent != null && !doorComponent.isConnected)
            {
                connectionQueue.Enqueue(new RoomConnection
                {
                    OriginRoom = room,
                    OriginDoor = doorComponent,
                    Offset = offset,
                    Direction = direction,
                    OppositeDirection = oppositeDirection
                });
            }
        }
        else
        {
            Debug.LogWarning($"Room '{room.name}' is missing the '{doorName}'.");
        }
    }

    void GenerateRoom(RoomConnection connection)
    {
        if (currentRoomCount >= maxRooms)
        {
            Debug.Log("Maximum room limit reached.");
            return;
        }

        // Calculate new room position based on the offset
        Vector3 newPosition = connection.OriginRoom.transform.position + connection.Offset;

        if (IsPositionOccupied(newPosition))
        {
            Debug.Log($"Position {newPosition} is occupied. Skipping room placement.");
            return;
        }

        // Instantiate the new room
        GameObject newRoomPrefab = roomPrefabs[Random.Range(0, roomPrefabs.Length)];
        GameObject newRoom = Instantiate(newRoomPrefab, newPosition, Quaternion.identity);
        generatedRooms.Add(newRoom);
        currentRoomCount++;

        // Connect the doors between the origin room and the new room
        ConnectRooms(connection.OriginRoom, newRoom, connection.Direction, connection.OppositeDirection);

        // Enqueue available doors from the new room for further expansion
        EnqueueAvailableDoors(newRoom);
    }


    bool IsPositionOccupied(Vector3 position)
    {
        foreach (GameObject room in generatedRooms)
        {
            if (Vector3.Distance(room.transform.position, position) < (Mathf.Max(roomSize.x, roomSize.y) + roomDistance))
            {
                return true;
            }
        }
        return false;
    }

    void ConnectRooms(GameObject roomA, GameObject roomB, DoorDirection directionA, DoorDirection directionB)
    {
        // Find the corresponding doors based on direction
        Transform doorA = roomA.transform.Find($"{directionA}Door");
        Transform doorB = roomB.transform.Find($"{directionB}Door");

        if (doorA == null || doorB == null)
        {
            Debug.LogWarning("One of the doors is missing. Ensure rooms have the required doors.");
            return;
        }

        // Get Door components
        Door doorComponentA = doorA.GetComponent<Door>();
        Door doorComponentB = doorB.GetComponent<Door>();

        if (doorComponentA != null && doorComponentB != null)
        {
            // Mark doors as connected
            doorComponentA.isConnected = true;
            doorComponentA.linkedDoor = doorComponentB;

            doorComponentB.isConnected = true;
            doorComponentB.linkedDoor = doorComponentA;
        }
        else
        {
            Debug.LogWarning("Door components missing on one of the doors.");
        }
    }

    void DeactivateUnlinkedDoors()
    {
        foreach (GameObject room in generatedRooms)
        {
            foreach (DoorDirection direction in System.Enum.GetValues(typeof(DoorDirection)))
            {
                string doorName = $"{direction}Door";
                Transform doorTransform = room.transform.Find(doorName);
                if (doorTransform != null)
                {
                    Door doorComponent = doorTransform.GetComponent<Door>();
                    if (doorComponent != null && !doorComponent.isConnected)
                    {
                        // Deactivate the door GameObject
                        doorTransform.gameObject.SetActive(false);
                    }
                }
                else
                {
                    Debug.LogWarning($"Room '{room.name}' is missing the '{doorName}'.");
                }
            }
        }
    }

    // Enum to define door directions for clarity
    enum DoorDirection
    {
        Right,
        Left,
        Top,
        Down
    }

    // Struct to hold information about a room connection
    struct RoomConnection
    {
        public GameObject OriginRoom;
        public Door OriginDoor;
        public Vector3 Offset;
        public DoorDirection Direction;
        public DoorDirection OppositeDirection;
    }
}
