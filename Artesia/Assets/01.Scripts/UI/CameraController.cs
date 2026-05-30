using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

public class CameraController : MonoBehaviour
{
    public float cameraSpeed = 5.0f;
    [HideInInspector] public RectInt screenRect;
    GameObject player;
    public Tilemap tilemap;

    private Vector3 minBounds;
    private Vector3 maxBounds;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        CalculateTilemapBounds();
    }

    private void LateUpdate()
    {

        Vector3 targetPosition = new Vector3(player.transform.position.x, player.transform.position.y, -5);
        targetPosition.x = Mathf.Clamp(targetPosition.x, minBounds.x, maxBounds.x);
        targetPosition.y = Mathf.Clamp(targetPosition.y, minBounds.y, maxBounds.y);
        transform.position = Vector3.Lerp(transform.position, targetPosition, cameraSpeed * Time.deltaTime);
        CalculateScreenBorderRect();
    }

    void CalculateScreenBorderRect()
    {
        Camera cam = Camera.main;
        if (cam == null)
        {
            Debug.LogError("Camera reference not set!");
            return;
        }

        Vector3 bottomLeft = cam.ScreenToWorldPoint(new Vector3(0, 0, cam.nearClipPlane));
        Vector3 topRight = cam.ScreenToWorldPoint(new Vector3(Screen.width * 0.7f, Screen.height, cam.nearClipPlane));

        int xMin = Mathf.FloorToInt(bottomLeft.x);
        int yMin = Mathf.FloorToInt(bottomLeft.y);
        int width = Mathf.CeilToInt(topRight.x) - xMin;
        int height = Mathf.CeilToInt(topRight.y) - yMin;

        screenRect = new RectInt(xMin, yMin, width, height);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector3 bottomLeft = new Vector3(screenRect.xMin, screenRect.yMin, 0);
        Vector3 bottomRight = new Vector3(screenRect.xMax, screenRect.yMin, 0);
        Vector3 topRight = new Vector3(screenRect.xMax, screenRect.yMax, 0);
        Vector3 topLeft = new Vector3(screenRect.xMin, screenRect.yMax, 0);

        Gizmos.DrawLine(bottomLeft, bottomRight);
        Gizmos.DrawLine(bottomRight, topRight);
        Gizmos.DrawLine(topRight, topLeft);
        Gizmos.DrawLine(topLeft, bottomLeft);
    }

    public void CalculateTilemapBounds()
    {
        Vector3 tileSize = tilemap.layoutGrid.cellSize;
        Vector3Int tilemapSize = tilemap.size;

        Vector3 tileSizeHalf = tileSize / 2f;
        Vector3 tilemapSizeHalf = new Vector3(tilemapSize.x / 2f, tilemapSize.y / 2f, 0f);

        minBounds = tilemap.CellToWorld(-Vector3Int.FloorToInt(tilemapSizeHalf)) + tileSizeHalf;
        maxBounds = tilemap.CellToWorld(Vector3Int.FloorToInt(tilemapSizeHalf)) - tileSizeHalf;

        Vector3 cameraSize = GetCameraSize();
        minBounds += cameraSize / 2f;
        maxBounds -= cameraSize / 2f;
    }

    private Vector3 GetCameraSize()
    {
        float height = Camera.main.orthographicSize * 2f;
        float width = height * Camera.main.aspect;
        return new Vector3(width, height, 0f);
    }
}