using UnityEngine;
using System.Collections.Generic;

public class ItemSpawner : MonoBehaviour
{
    [SerializeField] GameObject itemPrefab;
    [SerializeField] int numRoomsToSpawn = 5;

    void Start()
    {
        SpawnItemsInRandomRooms();
    }

    public void SpawnItemsInRandomRooms()
    {
        ClearSpawnedItems();
        List<Node> rooms = MapGenerator.instance.rooms;
        List<Node> spawnedRooms = new List<Node>();

        for (int i = 0; i < numRoomsToSpawn && i < rooms.Count; i++)
        {
            Node room = rooms[Random.Range(0, rooms.Count)];
            if (!spawnedRooms.Contains(room) && room != MapGenerator.instance.startRoom)
            {
                int numItemsToSpawn = GetNumItemsToSpawn(room.roomRect.size);
                SpawnItemsInRoom(room, numItemsToSpawn);
                spawnedRooms.Add(room);
            }
        }
    }

    int GetNumItemsToSpawn(Vector2 roomSize)
    {
        float roomArea = roomSize.x * roomSize.y;
        int numItems = Mathf.RoundToInt(roomArea / 50f);
        return Mathf.Clamp(numItems, 1, 5);
    }

    void SpawnItemsInRoom(Node room, int numItems)
    {
        for (int i = 0; i < numItems; i++)
        {
            Vector2 roomCenter = room.roomRect.center;
            Vector2 roomSize = room.roomRect.size;

            int randomX = Mathf.RoundToInt(Random.Range(roomCenter.x - roomSize.x / 2f + 0.5f, roomCenter.x + roomSize.x / 2f - 0.5f));
            int randomY = Mathf.RoundToInt(Random.Range(roomCenter.y - roomSize.y / 2f + 0.5f, roomCenter.y + roomSize.y / 2f - 0.5f));

            Vector2Int mapSize = MapGenerator.instance.MapSize;
            Vector3Int spawnPosition = new Vector3Int(randomX - mapSize.x / 2, randomY - mapSize.y / 2, 0);

            GameObject spawnedItem = Instantiate(itemPrefab, spawnPosition, Quaternion.identity, transform);
        }
    }

    void ClearSpawnedItems()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }
}