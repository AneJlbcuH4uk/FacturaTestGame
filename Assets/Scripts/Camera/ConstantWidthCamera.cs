using UnityEngine;


[RequireComponent(typeof(Camera))]
public class ConstantWidthCamera : MonoBehaviour
{
    [SerializeField] private float horizontalFOV = 90f;

    private Camera cam;
    private float aspectOld = 0;
    void Awake()
    {
        cam = GetComponent<Camera>();
        UpdateFOV();
    }

    private void Update()
    {
        UpdateFOV();
    }

    void UpdateFOV()
    {
        float aspect = (float)Screen.width / Screen.height;

        if (aspectOld != aspect) 
        {
            float hFOVRad = horizontalFOV * Mathf.Deg2Rad;
            float vFOVRad = 2f * Mathf.Atan(Mathf.Tan(hFOVRad / 2f) / aspect);
            cam.fieldOfView = vFOVRad * Mathf.Rad2Deg;
        }      
    }
}
