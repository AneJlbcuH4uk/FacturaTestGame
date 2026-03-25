using UnityEngine;
using UnityEngine.InputSystem;

public class TurretController : MonoBehaviour 
{
    [SerializeField] private Vector2 angleLimit;
    [SerializeField] private float deadZone = 0.1f;

    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float fireRate = 0.2f;
    [SerializeField] private Transform projectileHolder;

    private float nextFireTime;

    private void Update()
    {
        if (Touchscreen.current == null) return;

        var primaryTouch = Touchscreen.current.primaryTouch;

        if (primaryTouch.press.isPressed)
        {
            Vector3 touchPos = primaryTouch.position.ReadValue();

            float screenWidth = Screen.width;
            float normalizedX = touchPos.x / screenWidth;
            float clamped = Mathf.Clamp01((normalizedX - deadZone) / (1f - deadZone * 2f));
            float curved = Mathf.SmoothStep(0f, 1f, clamped);
            float angle = Mathf.Lerp(angleLimit.x, angleLimit.y, curved);

            Rotate(angle);
            Shoot();
        }
    }

    private void Rotate(float angle) 
    {
        transform.rotation = Quaternion.Euler(0, angle, 0);
    }

    private void Shoot() 
    {
        if (Time.time < nextFireTime) return;

        nextFireTime = Time.time + fireRate;

        Instantiate(
            projectilePrefab,
            firePoint.position,
            firePoint.rotation,
            projectileHolder
        );
    }

}
