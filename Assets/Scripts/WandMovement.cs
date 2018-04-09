using UnityEngine;

public class WandMovement : MonoBehaviour {

    private GameObject cameraGO;
    private GameObject wandGO;

    private float yaw = 0.0f;
    private float pitch = 0.0f;
    private float maxYawPitch = 60.0f;
    private float minYawPitch = -60.0f;
    private Vector3 front;
    public float sensitivity = 10f;
    public float smoothRotation = 10.0f;

    public float speed = 2.0f;
    public float smooth = 10.0f;
    private Quaternion orientation;

    private const string mouseYAxisName = "Mouse Y";
    private const string mouseXAxisName = "Mouse X";
    private const string mainCameraTagName = "MainCamera";

    private void Awake()
    {
        cameraGO = GameObject.FindWithTag(mainCameraTagName);
        if (!cameraGO)
            Debug.Log("No main camera found.");

        wandGO = gameObject;
       
        enabled = false;
    }

    private void OnEnable()
    {
        orientation = wandGO.transform.localRotation;
    }
    private void Update()
    {
        pitch += Input.GetAxisRaw(mouseYAxisName) * sensitivity;
        yaw += Input.GetAxisRaw(mouseXAxisName) * sensitivity;       

        if (yaw > maxYawPitch)
        {
            yaw = maxYawPitch;
        }
        if (yaw < minYawPitch)
        {
            yaw = minYawPitch;
        }
        if (pitch > maxYawPitch)
        {
            pitch = maxYawPitch;
        }
        if (pitch < minYawPitch)
        {
            pitch = minYawPitch;
        }

        front.x = Mathf.Cos(Mathf.Deg2Rad * pitch) * Mathf.Sin(Mathf.Deg2Rad * yaw);
        front.y = Mathf.Sin(Mathf.Deg2Rad * pitch);
        front.z = Mathf.Cos(Mathf.Deg2Rad * pitch) * Mathf.Cos(Mathf.Deg2Rad * yaw);

        orientation = Quaternion.LookRotation(front, Vector3.up);
        wandGO.transform.localRotation = Quaternion.Lerp(wandGO.transform.localRotation, orientation, smoothRotation * Time.deltaTime);
    }

}
