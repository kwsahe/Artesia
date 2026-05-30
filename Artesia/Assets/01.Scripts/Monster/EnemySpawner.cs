using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    static EnemySpawner m_instance;
    [SerializeField] GameObject enemyPrefab;
    [SerializeField] int poolSize = 20;
    [SerializeField] int FirstSpawn = 10;
    [SerializeField] int RandomSpawnNumber = 3;
    List<GameObject> Enemies;
    List<GameObject> enemyPool;

    Vector3 savedPlayerPos;

    public List<GameObject> enemies
    {
        get
        {
            return Enemies;
        }
    }

    public static EnemySpawner instance
    {
        get
        {
            return m_instance;
        }
    }

    void Awake()
    {
        if (m_instance == null)
            m_instance = this;
        else if (m_instance != this)
            Destroy(this.gameObject);

        Enemies = new List<GameObject>();
        enemyPool = new List<GameObject>();
        AddEnemyToPool(enemyPrefab, poolSize);
    }

    void Start()
    {
        savedPlayerPos = GameObject.FindGameObjectWithTag("Player").transform.position;
    }

    public void updatePath(Vector3 PlayerPos)
    {
        if (savedPlayerPos != PlayerPos)
        {
            savedPlayerPos = PlayerPos;
            foreach (GameObject enemy in enemies)
            {
                enemy.GetComponent<MobController>().setListPath(PlayerPos);
            }
        }
    }

    public void updatePath(GameObject enemy, Vector3 PlayerPos)
    {
        enemy.GetComponent<MobController>().setListPath(PlayerPos);
    }

    void AddEnemyToPool(GameObject Prefab, int EnemyCnt)
    {
        for (int i = 0; i < EnemyCnt; i++)
        {
            GameObject enemy = Instantiate(Prefab, Vector3.zero, Quaternion.identity);
            enemy.SetActive(false);
            enemy.name = (Prefab.name);
            enemy.transform.SetParent(this.transform);
            enemyPool.Add(enemy);
        }
    }

    public void killEnemy(GameObject enemy)
    {
        GameObject killedObject = null;
        foreach (var temp in enemies)
        {
            if (temp.name == enemy.name)
            {
                enemy.GetComponent<MobController>().setStateToIdle();
                enemy.SetActive(false);
                killedObject = enemy;
            }
        }
        if (killedObject != null)
            enemies.Remove(killedObject);
    }

    public void EnemyListClear()
    {
        foreach (GameObject Enemy in Enemies)
        {
            Enemy.GetComponent<MobController>().setStateToIdle();
            Enemy.SetActive(false);
        }
        enemies.Clear();
    }

    public void ActiveFromPool()
    {
        List<Node> rooms = MapGenerator.instance.rooms;
        List<Node> SpawnedRooms = new List<Node>();

        if (FirstSpawn > rooms.Count) FirstSpawn = rooms.Count;

        for (int i = 0; i < FirstSpawn;)
        {
            Node room = rooms[Random.Range(0, rooms.Count)];
            if (!SpawnedRooms.Contains(room) && room != MapGenerator.instance.startRoom)
            {
                SpawnEnemy(room);
                SpawnedRooms.Add(room);
                i++;
            }
        }
    }

    GameObject GetPooledEnemy()
    {
        for (int i = 0; i < enemyPool.Count; i++)
        {
            if (!enemyPool[i].activeInHierarchy)
            {
                return enemyPool[i];
            }
        }
        return null;
    }

    public void RandomSpawnEnemy()
    {
        List<Node> rooms = MapGenerator.instance.rooms;

        int SpawnCnt = Random.Range(1, RandomSpawnNumber + 1);

        for (int i = 0; i < SpawnCnt;)
        {
            Node room = rooms[Random.Range(0, rooms.Count)];
            if (!room.IntersectsOtherObject(Camera.main.GetComponent<CameraController>().screenRect))
            {
                SpawnEnemy(room);
                i++;
            }
        }
    }

    void SpawnEnemy(Node room)
    {
        GameObject enemy = GetPooledEnemy();

        if (enemy != null)
        {
            Vector2 temp1 = room.roomRect.center;
            Vector2Int temp2 = MapGenerator.instance.MapSize;
            Vector3 roomCenter = new Vector3(((int)temp1.x - temp2.x / 2), ((int)temp1.y - temp2.y / 2), 0);

            enemy.GetComponent<AStarPathfinder>().Init();
            enemy.GetComponent<MobController>().setListPath();

            enemy.transform.position = roomCenter;

            enemy.SetActive(true);
            enemies.Add(enemy);
        }
    }

    void SpawnEnemy(Vector3 pos)
    {
        GameObject enemy = GetPooledEnemy();

        if (enemy != null)
        {
            enemy.transform.position = pos;
            enemy.SetActive(true);
            enemies.Add(enemy);
        }
    }
}