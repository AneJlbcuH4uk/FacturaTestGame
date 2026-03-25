using UnityEngine;
using System.Collections.Generic;

public class PathManager : MonoBehaviour
{
    public List<Vector3> PathPoints { get; private set; }

    public float PathVariation = 2f;

    public void GeneratePath(List<Tile> tiles)
    {
        List<Vector3> points = new();

        for (int i = 0; i < tiles.Count; i++)
        {
            var pos = tiles[i].Entry.position;

            if (i != 0) // skip first element
            {
                pos.x += Random.Range(-PathVariation, PathVariation);
            }
            points.Add(pos);
        }

        PathPoints = Utils.GenerateSpline(points);
    }

    private void OnDrawGizmos()
    {
        if (PathPoints == null) return;

        Gizmos.color = Color.green;

        for (int i = 0; i < PathPoints.Count - 1; i++)
        {
            Gizmos.DrawLine(PathPoints[i], PathPoints[i + 1]);
        }
    }

    public Vector3 GetLastPoint() => PathPoints[^1];

}
