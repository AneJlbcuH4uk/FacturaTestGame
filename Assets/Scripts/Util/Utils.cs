using UnityEngine;
using System.Collections.Generic;

public class Utils
{
    public static List<Vector3> GenerateSpline(List<Vector3> points, int resolution = 10)
    {
        List<Vector3> result = new();

        if (points.Count < 2)
            return result;

        for (int i = 0; i < points.Count - 1; i++)
        {
            Vector3 p0 = i == 0 ? points[i] : points[i - 1];
            Vector3 p1 = points[i];
            Vector3 p2 = points[i + 1];
            Vector3 p3 = i + 2 < points.Count ? points[i + 2] : points[i + 1];

            for (int j = 0; j < resolution; j++)
            {
                float t = j / (float)resolution;
                result.Add(CatmullRom(p0, p1, p2, p3, t));
            }
        }

        // add final point
        result.Add(points[^1]);

        return result;
    }

    public static Vector3 CatmullRom(
    Vector3 p0,
    Vector3 p1,
    Vector3 p2,
    Vector3 p3,
    float t)
    {
        float t2 = t * t;
        float t3 = t2 * t;

        return 0.5f * (
            (2f * p1) +
            (-p0 + p2) * t +
            (2f * p0 - 5f * p1 + 4f * p2 - p3) * t2 +
            (-p0 + 3f * p1 - 3f * p2 + p3) * t3
        );
    }
}
