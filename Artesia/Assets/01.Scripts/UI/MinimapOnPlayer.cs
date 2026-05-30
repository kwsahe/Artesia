using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapOnPlayer : MonoBehaviour
{
    GameObject Player;
    Vector3 PlayerPos;
    Vector2Int MapSize;

    private void Start()
    {
        MapSize = MapGenerator.instance.MapSize;
        Player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        PlayerPos = Player.transform.position;
        Vector3 minimapPosition = WorldToMinimapPosition(PlayerPos);
        transform.position = minimapPosition;
    }

    Vector3 WorldToMinimapPosition(Vector3 worldPosition)
    {
        return new Vector3(worldPosition.x + MapSize.x / 2f, worldPosition.y + MapSize.y / 2f);
    }
}