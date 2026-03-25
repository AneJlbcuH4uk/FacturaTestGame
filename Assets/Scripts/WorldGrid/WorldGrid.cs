using UnityEngine;

public class WorldGrid
{
    public int Width;
    public int Height;
    public float CellSize;
    private Vector3 origin;

    public bool[,] Walkable;

    public WorldGrid(Vector3 corn, float s, bool[,] obst) 
    {
        Width = obst.GetLength(0);
        Height = obst.GetLength(1);
        CellSize = s;
        Walkable = obst;
        origin = corn;

        Debug.Log($"generated grid ({Width}x{Height}) with cell size {CellSize} origin is {origin}");
    }

    public Vector3 GridToWorld(Vector2Int pos, float height)
    {
        return new Vector3(pos.x * CellSize,height,pos.y * CellSize) + origin;
    }

    public Vector2Int WorldToGrid(Vector3 pos)
    {
        Vector3 local = (pos - origin) / CellSize;

        return new Vector2Int(
            Mathf.FloorToInt(local.x + 0.5f),
            Mathf.FloorToInt(local.z + 0.5f)
        );
    }

    public bool IsInside(Vector2Int pos)
    {
        return pos.x >= 0 && pos.x < Width &&
               pos.y >= 0 && pos.y < Height;
    }

    public bool IsWalkable(Vector2Int pos)
    {
        if (!IsInside(pos)) return false;
        return Walkable[pos.x, pos.y];
    }
}
