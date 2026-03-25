using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class WorldGridBuilder : MonoBehaviour
{
    private float gridSize;
    private List<Tile> tiles = new();
    private int cellsInWidth = 0;
    private int cellsInHeight = 0;

    private WorldGrid worldGrid;

    public WorldGrid BuildWorldGrid(List<Tile> t, float gs)
    {
        tiles = t;
        gridSize = gs;

        Rect worldRect = new();
        Vector3 center = Vector3.zero;
        int tilesCount = 0;

        foreach (var tile in tiles)
        {

            worldRect.xMin = tile.Bounds.min.x < worldRect.xMin ? tile.Bounds.min.x : worldRect.xMin;
            worldRect.yMin = tile.Bounds.min.z < worldRect.yMin ? tile.Bounds.min.z : worldRect.yMin;
            worldRect.xMax = tile.Bounds.max.x > worldRect.xMax ? tile.Bounds.max.x : worldRect.xMax;
            worldRect.yMax = tile.Bounds.max.z > worldRect.yMax ? tile.Bounds.max.z : worldRect.yMax;

            center += tile.Bounds.center;
            tilesCount += 1;
        }

        center /= tilesCount;

        cellsInWidth = Mathf.CeilToInt(worldRect.width / gridSize);
        cellsInHeight = Mathf.CeilToInt(worldRect.height / gridSize);

        var obstGrid = GenerateObstacleGrid(cellsInWidth, cellsInHeight);

        float gridMinx = center.x - (cellsInWidth / 2f * gridSize) + gridSize / 2f;
        float gridMinz = center.z - (cellsInHeight / 2f * gridSize) + gridSize / 2f;

        Vector3 origin = new(gridMinx,0, gridMinz);

        worldGrid = new(origin, gridSize, obstGrid);
        return worldGrid;
    }

    private bool[,] GenerateObstacleGrid(int w, int h) 
    {
        bool[,] obst = new bool[w, h];
        
        for(int i = 0; i < w; i++)
            for(int j = 0; j < h; j++)
                obst[i, j] = true;
        
        return obst;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector3 size = new(gridSize, 0f, gridSize);
        for (int x = 0; x < cellsInWidth; x++)
            for (int y = 0; y < cellsInHeight; y++)
            {
                Vector3 worldPos = worldGrid.GridToWorld(new Vector2Int(x, y), 0.5f);
                Gizmos.DrawWireCube(worldPos, size);
            }
    }

    
}
