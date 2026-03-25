using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform carTransform;
    [SerializeField] private float smoothTime = 0.15f;

    private Vector3 offset;
    private Vector3 velocity;
    private float fixedX;

    private void Awake()
    {
        offset = transform.position - carTransform.position;
        fixedX = transform.position.x;
    }

    private void LateUpdate()
    {
        Vector3 targetPos = carTransform.position + offset;
        targetPos.x = fixedX;

        transform.position = Vector3.SmoothDamp(
            transform.position,
            targetPos,
            ref velocity,
            smoothTime
        );
    }
}
