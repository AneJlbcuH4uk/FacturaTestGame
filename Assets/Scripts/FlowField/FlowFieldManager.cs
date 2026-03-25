using UnityEngine;


public class FlowFieldManager : MonoBehaviour
{
    private WorldGrid grid;
    private FlowField flowField;

    [SerializeField] private int tilesAhead = 10;
    [SerializeField] private int tilesBehind = 3;
    [SerializeField] private int halfWidth = 6;


    public void Init(WorldGrid worldGrid)
    {
        grid = worldGrid;
        flowField = new FlowField(grid);
    }
    public void UpdateField(Vector3 carPos, Vector3 carForward)
    {
        Vector2Int carTile = grid.WorldToGrid(carPos);

        int height = tilesAhead + tilesBehind;
        Vector2Int origin = carTile - new Vector2Int(halfWidth, tilesBehind);

        flowField.Build(carTile, origin, halfWidth * 2 + 1, height);
    }

    public Vector3 GetDirection(Vector3 worldPos)
    {
        return flowField.GetDirection(worldPos);
    }

    private void OnDrawGizmos()
    {
        if (flowField == null || grid == null) return;

        var dirs = flowField.Directions;
        var origin = flowField.Origin;
        int sizeX = flowField.SizeX;
        int sizeY = flowField.SizeY;

        Gizmos.color = Color.blue;

        for (int x = 0; x < sizeX; x++)
        {
            for (int y = 0; y < sizeY; y++)
            {
                Vector2 dir = dirs[x, y];

                if (dir == Vector2.zero)
                    continue;


                Vector2Int worldCell = origin + new Vector2Int(x, y);
                Vector3 worldPos = grid.GridToWorld(worldCell, 0.5f);

                Vector3 direction = new Vector3(dir.x, 0f, dir.y);

                Gizmos.DrawLine(worldPos, worldPos + direction * 2);
            }
        }
    }

}
