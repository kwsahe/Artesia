using System.Collections.Generic;
using UnityEngine;

public class AStarPathfinder : MonoBehaviour
{
    private int[,] tileInfoArray;
    private Vector2Int mapSize;

    private List<Vector2Int> openList;
    private List<Vector2Int> closedList;

    [SerializeField] int MaxPathSize = 7;

    private void Awake()
    {
        openList = new List<Vector2Int>();
        closedList = new List<Vector2Int>();
    }

    public void Init()
    {
        MapGenerator mapGenerator = MapGenerator.instance;
        tileInfoArray = mapGenerator.TileInfoArray;
        mapSize = mapGenerator.MapSize;
    }

    public List<Vector2Int> StartPathfinding(Vector3 startTransform, Vector3 endTransform)
    {
        Vector2Int startPos = ConvertWorldToMapPosition(startTransform);
        Vector2Int endPos = ConvertWorldToMapPosition(endTransform);

        return FindPath(startPos, endPos);
    }

    public Vector2Int ConvertWorldToMapPosition(Vector3 worldPosition)
    {
        int x = (int)(worldPosition.x + mapSize.x / 2);
        int y = (int)(worldPosition.y + mapSize.y / 2);
        return new Vector2Int(x, y);
    }

    public Vector3 ConvertMapToWorldPosition(Vector2Int mapPosition)
    {
        float x = mapPosition.x - mapSize.x / 2;
        float y = mapPosition.y - mapSize.y / 2;
        return new Vector3(x, y, 0);
    }

    List<Vector2Int> FindPath(Vector2Int startPos, Vector2Int endPos)
    {
        openList.Clear();
        closedList.Clear();

        openList.Add(startPos);

        while (openList.Count > 0)
        {
            Vector2Int currentPos = GetLowestFCostNode(endPos);
            if (currentPos == endPos)
            {
                return ListPath(endPos);
            }

            openList.Remove(currentPos);
            closedList.Add(currentPos);

            ExploreNeighbors(currentPos, endPos);
        }

        return null;
    }

    Vector2Int GetLowestFCostNode(Vector2Int endPos)
    {
        Vector2Int lowestFCostNode = openList[0];
        int lowestFCost = GetFCost(lowestFCostNode, endPos);

        foreach (Vector2Int node in openList)
        {
            int fCost = GetFCost(node, endPos);
            if (fCost < lowestFCost)
            {
                lowestFCost = fCost;
                lowestFCostNode = node;
            }
        }

        return lowestFCostNode;
    }

    void ExploreNeighbors(Vector2Int currentPos, Vector2Int endPos)
    {
        Vector2Int[] neighbors = GetNeighbors(currentPos);

        foreach (Vector2Int neighborPos in neighbors)
        {
            if (closedList.Contains(neighborPos))
                continue;

            int gCost = GetGCost(currentPos, neighborPos);
            int hCost = GetHCost(neighborPos, endPos);
            int fCost = gCost + hCost;

            if (openList.Contains(neighborPos))
            {
                int index = openList.IndexOf(neighborPos);
                if (fCost < GetFCost(openList[index], endPos))
                {
                    openList[index] = neighborPos;
                }
            }
            else
            {
                openList.Add(neighborPos);
            }
        }
    }

    Vector2Int[] GetNeighbors(Vector2Int currentPos)
    {
        Vector2Int[] neighbors = new Vector2Int[]
        {
            new Vector2Int(0, 1),
            new Vector2Int(0, -1),
            new Vector2Int(1, 0),
            new Vector2Int(-1, 0),
            //new Vector2Int(1, 1),
            //new Vector2Int(1, -1),
            //new Vector2Int(-1, 1),
            //new Vector2Int(-1, -1)
        };

        List<Vector2Int> validNeighbors = new List<Vector2Int>();

        foreach (Vector2Int neighbor in neighbors)
        {
            Vector2Int neighborPos = currentPos + neighbor;

            if (neighborPos.x < 0 || neighborPos.x >= mapSize.x || neighborPos.y < 0 || neighborPos.y >= mapSize.y)
                continue;

            if (tileInfoArray[neighborPos.y, neighborPos.x] == (int)MapGenerator.TileInfo.Wall)
                continue;

            if (neighbor.x != 0 && neighbor.y != 0)
            {
                if (tileInfoArray[currentPos.y + neighbor.y, currentPos.x] == (int)MapGenerator.TileInfo.Wall ||
                    tileInfoArray[currentPos.y, currentPos.x + neighbor.x] == (int)MapGenerator.TileInfo.Wall)
                    continue;
            }

            validNeighbors.Add(neighborPos);
        }

        return validNeighbors.ToArray();
    }

    int GetGCost(Vector2Int startPos, Vector2Int endPos)
    {
        int diagonalMoveCost = 14;
        int straightMoveCost = 10;

        int distX = Mathf.Abs(endPos.x - startPos.x);
        int distY = Mathf.Abs(endPos.y - startPos.y);

        if (distX > distY)
            return diagonalMoveCost * distY + straightMoveCost * (distX - distY);
        return diagonalMoveCost * distX + straightMoveCost * (distY - distX);
    }

    int GetHCost(Vector2Int pos, Vector2Int endPos)
    {
        return Mathf.Abs(pos.x - endPos.x) + Mathf.Abs(pos.y - endPos.y);
    }

    int GetFCost(Vector2Int pos, Vector2Int endPos)
    {
        return GetGCost(pos, endPos) + GetHCost(pos, endPos);
    }

    List<Vector2Int> ListPath(Vector2Int endPos)
    {
        List<Vector2Int> path = new List<Vector2Int>();
        Vector2Int currentPos = endPos;

        while (currentPos != ConvertWorldToMapPosition(transform.position))
        {
            path.Add(currentPos);
            if (path.Count > MaxPathSize) return null;

            Vector2Int minPos = Vector2Int.zero;
            int minCost = int.MaxValue;

            Vector2Int[] neighbors = GetNeighbors(currentPos);

            foreach (Vector2Int neighborPos in neighbors)
            {
                if (!closedList.Contains(neighborPos))
                    continue;

                int cost = GetGCost(ConvertWorldToMapPosition(transform.position), neighborPos);
                if (cost < minCost)
                {
                    minCost = cost;
                    minPos = neighborPos;
                }
            }

            if (minPos == Vector2Int.zero)
                break;

            currentPos = minPos;
        }
        path.Reverse();

        return path;
    }
}