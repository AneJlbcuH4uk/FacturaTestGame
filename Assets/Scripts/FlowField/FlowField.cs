using UnityEngine;
using System.Collections.Generic;

public class FlowField
{
    private int sizeX;
    private int sizeY;

    private WorldGrid grid;

    private Vector2Int origin;

    private int[,] integration;
    private Vector2[,] directions;


    public Vector2[,] Directions => directions;
    public Vector2Int Origin => origin;

    public int SizeX => sizeX;
    public int SizeY => sizeY;


    public FlowField(WorldGrid grid)
    {
        this.grid = grid;
    }

    public void Build(Vector2Int targetTile, Vector2Int origin, int width, int height)
    {
        this.origin = origin;
        this.sizeX = width;
        this.sizeY = height;

        integration = new int[sizeX, sizeY];
        directions = new Vector2[sizeX, sizeY];

        BuildIntegration(targetTile);
        BuildDirections();
    }

    private void BuildIntegration(Vector2Int targetTile)
    {
        for (int x = 0; x < sizeX; x++)
            for (int y = 0; y < sizeY; y++)
                integration[x, y] = int.MaxValue;

        Queue<Vector2Int> queue = new();
        Vector2Int localTarget = targetTile - origin;

        if (!IsInside(localTarget)) return;

        integration[localTarget.x, localTarget.y] = 0;
        queue.Enqueue(localTarget);

        while (queue.Count > 0)
        {
            Vector2Int current = queue.Dequeue();

            foreach (var dir in Directions8)
            {
                Vector2Int next = current + dir;

                if (!IsInside(next)) continue;

                Vector2Int worldPos = origin + next;

                if (!grid.IsWalkable(worldPos)) continue;

                int baseCost = (dir.x != 0 && dir.y != 0) ? 12 : 10;
                int noise = Random.Range(0, 3);

                int newCost = integration[current.x, current.y] + baseCost + noise;

                if (newCost < integration[next.x, next.y])
                {
                    integration[next.x, next.y] = newCost;
                    queue.Enqueue(next);
                }
            }
        }
    }

    private void BuildDirections()
    {
        for (int x = 0; x < sizeX; x++)
            for (int y = 0; y < sizeY; y++)
            {
                Vector2Int current = new Vector2Int(x, y);

                Vector2Int best = current;
                int bestCost = integration[x, y];

                foreach (var d in Directions8)
                {
                    var next = current + d;

                    if (!IsInside(next)) continue;

                    if (integration[next.x, next.y] < bestCost)
                    {
                        bestCost = integration[next.x, next.y];
                        best = next;
                    }
                }

                Vector2 dir = best - current;
                directions[x, y] = dir.normalized;
            }
    }

    public Vector3 GetDirection(Vector3 worldPos)
    {
        Vector2Int gridPos = grid.WorldToGrid(worldPos);
        Vector2Int local = gridPos - origin;

        if (!IsInside(local)) return Vector3.zero;

        Vector2 dir = directions[local.x, local.y];

        return new Vector3(dir.x, 0, dir.y).normalized;
    }

    static readonly Vector2Int[] Directions8 =
    {
    new(1,0), new(-1,0), new(0,1), new(0,-1),
    new(1,1), new(-1,1), new(1,-1), new(-1,-1)
};

    private bool IsInside(Vector2Int p)
    {
        return p.x >= 0 && p.x < sizeX &&
               p.y >= 0 && p.y < sizeY;
    }

}
