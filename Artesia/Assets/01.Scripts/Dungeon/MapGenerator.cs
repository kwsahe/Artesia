using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

public class MapGenerator : MonoBehaviour
{
    static MapGenerator m_instance; // 싱글톤
    [SerializeField] Vector2Int mapSize;
    [SerializeField] float minDevideRate;
    [SerializeField] float maxDevideRate;
    [SerializeField] int maxDepth;
    Node StartRoom;
    Vector3Int startPos;
    public int[,] TileInfoArray { get; private set; }
    public Vector3Int stairPos { get; private set; }
    public List<Node> rooms { get; private set; }

    public Vector2Int MapSize
    {
        get { return mapSize; }
    }
    public Vector3Int StartPos
    {
        get { return startPos; }
    }
    public Node startRoom
    {
        get { return StartRoom; }
    }
    public static MapGenerator instance
    {
        get
        {
            return m_instance;
        }
    }
    public enum TileInfo
    { // 타일 배열에 관한 열거
        Out,
        Room,
        Wall,
        Stair,
    }

    Node StairRoom;

    int StartDepth;
    int StairDepth;

    [SerializeField] Vector2Int maxRoomSize;

    void Awake()
    {
        if (m_instance == null)
            m_instance = this;
        else if (m_instance != this)
            Destroy(this.gameObject);
    }

    void InitMapInfoArray()
    {
        for (int i = 0; i < mapSize.y; i++)
            for (int j = 0; j < mapSize.x; j++)
                TileInfoArray[i, j] = (int)TileInfo.Out;
    }

    public void InitMap()
    {
        Node root = null;
        root = initMember(root);

        Divide(root, 0);
        InitRoom(root, 0);
        GenerateRoom(root, 0);
        GenerateRoad(root, 0);
        GenerateWall();

        EnemySpawner.instance.ActiveFromPool();
    }

    Node initMember(Node root)
    {
        root = new Node(new RectInt(1, 1, mapSize.x - 2, mapSize.y - 2));
        rooms = new List<Node>();

        TileInfoArray = new int[mapSize.y, mapSize.x];
        InitMapInfoArray();

        StairDepth = 0;
        StartDepth = 0;

        return root;
    }

    void InitRoom(Node Tree, int n)
    {

        if (Random.Range(0, 2) == 0)
        {
            StartRoom = Tree.leftNode;
            StairRoom = Tree.rightNode;
        }
        else
        {
            StartRoom = Tree.rightNode;
            StairRoom = Tree.leftNode;
        }

        InitStartRoom(StartRoom, n + 1, Random.Range(0, (int)Mathf.Pow(2, maxDepth - 1)));
        InitStairRoom(StairRoom, n + 1, Random.Range(0, (int)Mathf.Pow(2, maxDepth - 1)));
    }

    void InitStartRoom(Node Tree, int n, int maxStartDepth)
    {
        if (n == maxDepth)
        {
            if (StartDepth == maxStartDepth)
            {
                StartRoom = Tree;
            }
            StartDepth++;
            return;
        }
        InitStartRoom(Tree.leftNode, n + 1, maxStartDepth);
        InitStartRoom(Tree.rightNode, n + 1, maxStartDepth);
    }

    void InitStairRoom(Node Tree, int n, int maxStairDepth)
    {
        if (n == maxDepth)
        {
            if (StairDepth == maxStairDepth)
            {
                StairRoom = Tree;
            }
            StairDepth++;
            return;
        }
        InitStairRoom(Tree.leftNode, n + 1, maxStairDepth);
        InitStairRoom(Tree.rightNode, n + 1, maxStairDepth);
    }

    void Divide(Node Tree, int n)
    {
        if (n == maxDepth)
        {
            rooms.Add(Tree);
            return;
        }

        int maxLength = Mathf.Max(Tree.nodeRect.width, Tree.nodeRect.height);
        int split = Mathf.RoundToInt(Random.Range(maxLength * minDevideRate, maxLength * maxDevideRate));
        if (Tree.nodeRect.width >= Tree.nodeRect.height)
        {
            Tree.leftNode = new Node(new RectInt(Tree.nodeRect.x, Tree.nodeRect.y, split, Tree.nodeRect.height));
            Tree.rightNode = new Node(new RectInt(Tree.nodeRect.x + split, Tree.nodeRect.y, Tree.nodeRect.width - split, Tree.nodeRect.height));
        }
        else
        {
            Tree.leftNode = new Node(new RectInt(Tree.nodeRect.x, Tree.nodeRect.y, Tree.nodeRect.width, split));
            Tree.rightNode = new Node(new RectInt(Tree.nodeRect.x, Tree.nodeRect.y + split, Tree.nodeRect.width, Tree.nodeRect.height - split));
        }
        Tree.leftNode.parNode = Tree;
        Tree.rightNode.parNode = Tree;

        Divide(Tree.leftNode, n + 1);
        Divide(Tree.rightNode, n + 1);
    }

    private RectInt GenerateRoom(Node Tree, int n)
    {
        RectInt rect;
        if (n == maxDepth)
        {
            rect = Tree.nodeRect;
            int width = Random.Range(rect.width / 2, Mathf.Min(rect.width - 1, maxRoomSize.x));
            int height = Random.Range(rect.height / 2, Mathf.Min(rect.height - 1, maxRoomSize.y));

            int x = rect.x + Random.Range(1, rect.width - width);
            int y = rect.y + Random.Range(1, rect.height - height);

            rect = new RectInt(x, y, width, height);
            FillRoom(rect, n);
            if (Tree == StartRoom || Tree == StairRoom) FillRoom(Tree, rect);
        }
        else
        {
            Tree.leftNode.roomRect = GenerateRoom(Tree.leftNode, n + 1);
            Tree.rightNode.roomRect = GenerateRoom(Tree.rightNode, n + 1);
            rect = Tree.leftNode.roomRect;
        }
        return rect;
    }

    private void GenerateRoad(Node Tree, int n)
    {
        if (n == maxDepth) return;

        Vector2Int currentCenter = new Vector2Int((Tree.leftNode.center.x + Tree.rightNode.center.x) / 2, (Tree.leftNode.center.y + Tree.rightNode.center.y) / 2);

        ConnectNodes(currentCenter, Tree.leftNode.center);
        ConnectNodes(currentCenter, Tree.rightNode.center);

        GenerateRoad(Tree.leftNode, n + 1);
        GenerateRoad(Tree.rightNode, n + 1);
    }

    private void ConnectNodes(Vector2Int start, Vector2Int end)
    {
        int dx = Math.Sign(end.x - start.x);
        int dy = Math.Sign(end.y - start.y);

        Vector2Int current = start;
        while (current.x != end.x || current.y != end.y)
        {
            addMapInfoArray(current.x, current.y, TileInfo.Room);
            if (current.x != end.x) current.x += dx;
            else current.y += dy;
        }
        addMapInfoArray(end.x, end.y, TileInfo.Room);
    }

    void GenerateWall()
    {
        for (int i = 1; i < mapSize.x - 1; i++)
            for (int j = 1; j < mapSize.y - 1; j++)  // i, j 맵 전체 순회
                if (TileInfoArray[j, i] == (int)TileInfo.Out)// 순회한 위치가 바깥 타일이면
                    if (ShouldPlaceWall(i, j))  // 조건 확인 메서드 사용
                        addMapInfoArray(i, j, TileInfo.Wall);// 벽 생성
    }

    bool ShouldPlaceWall(int i, int j)
    {
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0) continue;
                if (TileInfoArray[j + y, i + x] == (int)TileInfo.Room)
                {
                    return true;
                }
            }
        }
        return false; // 벽을 생성하지 않음
    }

    private void FillRoom(RectInt rect, int n)
    {
        for (int i = rect.x; i < rect.x + rect.width; i++)
            for (int j = rect.y; j < rect.y + rect.height; j++)
            {
                addMapInfoArray(i, j, TileInfo.Room);
            }
    }

    void FillRoom(Node Tree, RectInt rect)
    { // 얘는 왜 이름이 FillRoom ??
        Vector3Int Pos = new Vector3Int((int)rect.center.x - mapSize.x / 2, (int)rect.center.y - mapSize.y / 2, 0);
        if (Tree == StartRoom)
        {
            startPos = Pos;
        }
        else if (Tree == StairRoom)
        {
            stairPos = Pos;
        }
    }

    void addMapInfoArray(int x, int y, TileInfo TypeEnum)
    {
        TileInfoArray[y, x] = (int)TypeEnum;
    }
}