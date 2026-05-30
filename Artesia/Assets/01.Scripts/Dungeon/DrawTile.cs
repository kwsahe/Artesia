using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

public class DrawTile : MonoBehaviour
{
    enum TileInfo
    { // 타일 배열에 관한 열거
        Out,
        Room,
        Wall,
        Stair,
    }

    [SerializeField] Tilemap tileMap; // 배경
    [SerializeField] Tilemap stairMap; // 상호작용 타일 이름 바꿀듯?
    Dictionary<TileInfo, RuleTile> dicTile = new Dictionary<TileInfo, RuleTile>();
    [SerializeField] RuleTile RoomTile;
    [SerializeField] RuleTile WallTile;
    [SerializeField] RuleTile stairTile;
    int[,] MapTileInfo;
    Vector2Int m_mapSize;
    Vector3Int m_stairPos;

    //미니맵
    [SerializeField] Tilemap minimapGrid;
    [SerializeField] RuleTile minimapTile;

    void Awake()
    {
        initDic();
    }

    void mySetTile(Tilemap tilemap, int x, int y, TileInfo roomInfo)
    {
        tilemap.SetTile(new Vector3Int(x - m_mapSize.x / 2, y - m_mapSize.y / 2, 0), dicTile[roomInfo]);
    }

    void mySetTile(Tilemap tilemap, Vector3Int pos, TileInfo roomInfo)
    {
        tilemap.SetTile(pos, dicTile[roomInfo]);
    }

    void mySetTile(Tilemap tilemap, Vector3Int pos, RuleTile roomInfo)
    {
        tilemap.SetTile(pos, roomInfo);
    }

    public void InitTile()
    {
        initMember();
        FillMap();
    }

    void initMember()
    {
        MapTileInfo = MapGenerator.instance.TileInfoArray;
        m_mapSize = MapGenerator.instance.MapSize;

        if (m_stairPos != null)
            if (stairMap.GetTile(m_stairPos) == stairTile)
                stairMap.SetTile(m_stairPos, null);
        m_stairPos = MapGenerator.instance.stairPos;
    }

    void initDic()
    {
        dicTile.Add(TileInfo.Room, RoomTile);
        dicTile.Add(TileInfo.Stair, stairTile);
        dicTile.Add(TileInfo.Wall, WallTile);
    }

    void FillMap()
    {
        FillBackGround();
        ClearMiniMap();
        DrawMap();
        DrawStair();
    }

    void DrawStair()
    {
        mySetTile(stairMap, m_stairPos, TileInfo.Stair);
    }

    void DrawMap()
    {
        for (int i = 0; i < m_mapSize.y; i++)
        {
            for (int j = 0; j < m_mapSize.x; j++)
            {
                if ((TileInfo)MapTileInfo[i, j] != TileInfo.Out)
                {
                    if ((TileInfo)MapTileInfo[i, j] == TileInfo.Wall)
                        mySetTile(minimapGrid, new Vector3Int(j, i), minimapTile);
                    mySetTile(tileMap, j, i, (TileInfo)MapTileInfo[i, j]);
                }
            }
        }
    }

    private void FillBackGround()
    {
        for (int i = -10; i < m_mapSize.x + 10; i++)
            for (int j = -10; j < m_mapSize.y + 10; j++)
                mySetTile(tileMap, i, j, TileInfo.Wall);
    }

    void ClearMiniMap()
    {
        for (int i = 0; i < m_mapSize.x; i++)
            for (int j = 0; j < m_mapSize.y; j++)
                minimapGrid.SetTile(new Vector3Int(i, j), null);
    }
}