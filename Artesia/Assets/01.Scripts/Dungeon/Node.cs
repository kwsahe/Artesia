using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.Mathematics;
using UnityEngine;

public class Node
{
    public Node leftNode;
    public Node rightNode;
    public Node parNode;
    public RectInt nodeRect;
    public RectInt roomRect;
    public Vector2Int center
    {
        get
        {
            return new Vector2Int(roomRect.x + roomRect.width / 2, roomRect.y + roomRect.height / 2);
        }
    }
    public Node(RectInt rect)
    {
        this.nodeRect = rect;
    }

    public bool IntersectsOtherObject(RectInt otherRect)
    {
        if (MapGenerator.instance != null)
        {
            Vector2Int m_mapsize = MapGenerator.instance.MapSize;
            otherRect = new RectInt(otherRect.x + m_mapsize.x / 2, otherRect.y + m_mapsize.y / 2, otherRect.width, otherRect.height);
        }
        return roomRect.Overlaps(otherRect);
    }
}
