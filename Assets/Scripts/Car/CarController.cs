using UnityEngine;
using System.Collections.Generic;

public class CarController: MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float rotationSpeed = 360f;
    private List<Vector3> path;
    private int currentIndex;
    private Vector3 smoothDir;

    public event System.Action OnPathComplete;
    public event System.Action<float> OnProgressChanged;
    private bool pathCompleted = false;


    public void Init(List<Vector3> pathPoints)
    {
        path = pathPoints;
        currentIndex = 0;
        transform.position = path[0];
    }

   
    public void MoveAlongPath()
    {
        if (path == null || currentIndex >= path.Count) return;

        float moveRemaining = speed * Time.deltaTime;

        while (moveRemaining > 0f && currentIndex < path.Count)
        {
            Vector3 target = path[currentIndex];
            Vector3 toTarget = target - transform.position;
            float dist = toTarget.magnitude;

            if (dist <= moveRemaining)
            {
                // move to point and continue to next
                transform.position = target;
                moveRemaining -= dist;
                currentIndex++;

                if (currentIndex >= path.Count && !pathCompleted)
                {
                    pathCompleted = true;
                    OnPathComplete?.Invoke();
                    return;
                }
            }
            else
            {
                // move partially toward point
                Vector3 dir = toTarget / dist;
                transform.position += dir * moveRemaining;
                moveRemaining = 0f;

                // smooth direction
                smoothDir = Vector3.Lerp(smoothDir, dir, 10f * Time.deltaTime);

                // rotation
                if (smoothDir.sqrMagnitude > 0.001f)
                {
                    Quaternion targetRot = Quaternion.LookRotation(smoothDir, Vector3.up);

                    transform.rotation = Quaternion.RotateTowards(
                        transform.rotation,
                        targetRot,
                        rotationSpeed * Time.deltaTime
                    );
                }
            }
        }

        float progress = (float)currentIndex / path.Count;
        OnProgressChanged?.Invoke(progress);
    }



}
