using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Transform startPosition;
    private GameObject cameraGO;

    private float yaw = 0.0f;
    private float pitch = 0.0f;
    private float maxPitch = 89.0f;
    private float minPitch = -89.0f;
    private Vector3 front;
    public float sensitivity = 10f;
    public float smoothRotation = 10.0f;
    public float smoothTranslation = 5.0f;
    private Vector3 newPos;
    public float speed = 10.0f;
    private Vector3 cancelYComp;
    private bool movingWand = false;
    private WandMovement wandMovement;
    private Quaternion orientation;

    private const string mainCameraName = "MainCamera";
    private const string mouseYAxisName = "Mouse Y";
    private const string mouseXAxisName = "Mouse X";
    private const string horizontalAxisName = "Horizontal";
    private const string verticalAxisName = "Vertical";

    private string leftMouseButton = "Fire1";

    private void Awake()
    {
        cameraGO = GameObject.FindWithTag(mainCameraName);
        if (!cameraGO)
            Debug.Log("No main camera found.");

        wandMovement = GameObject.FindObjectOfType<WandMovement>();
        if (!wandMovement)
            Debug.Log("No WandMovement script found.");

    }

    private void Start()
    {
        cameraGO.transform.position = startPosition.position;
        newPos = cameraGO.transform.position;
        cancelYComp = new Vector3(1.0f, 0.0f, 1.0f);
    }

    void Update()
    {
        if (Input.GetButtonDown(leftMouseButton))
        {
            movingWand = !movingWand;
            wandMovement.enabled = !wandMovement.enabled;
        }

        if (!movingWand)
        {
            pitch += Input.GetAxisRaw(mouseYAxisName) * sensitivity;
            yaw += Input.GetAxisRaw(mouseXAxisName) * sensitivity;
        }

        yaw = yaw % 360.0f;

        if (pitch > maxPitch)
        {
            pitch = maxPitch;
        }
        if (pitch < minPitch)
        {
            pitch = minPitch;
        }

        front.x = Mathf.Cos(Mathf.Deg2Rad * pitch) * Mathf.Sin(Mathf.Deg2Rad * yaw);
        front.y = Mathf.Sin(Mathf.Deg2Rad * pitch);
        front.z = Mathf.Cos(Mathf.Deg2Rad * pitch) * Mathf.Cos(Mathf.Deg2Rad * yaw);

        orientation = Quaternion.LookRotation(front.normalized, Vector3.up);
        cameraGO.transform.rotation = Quaternion.Lerp(cameraGO.transform.rotation, orientation, smoothRotation * Time.deltaTime);

        if (!movingWand && Input.GetAxisRaw(verticalAxisName) > 0.0f)
        {
            newPos += Vector3.Scale(cameraGO.transform.forward, cancelYComp) * speed * Time.deltaTime;           
        }
        if (!movingWand && Input.GetAxisRaw(verticalAxisName) < 0.0f)
        {
            newPos -= Vector3.Scale(cameraGO.transform.forward, cancelYComp) * speed * Time.deltaTime;
        }     
        if (!movingWand && Input.GetAxisRaw(horizontalAxisName) > 0.0f)
        {
            newPos += cameraGO.transform.right * speed * Time.deltaTime;
        }
        if (!movingWand && Input.GetAxisRaw(horizontalAxisName) < 0.0f)
        {
            newPos -= cameraGO.transform.right * speed * Time.deltaTime;
        }

        cameraGO.transform.position = Vector3.Lerp(cameraGO.transform.position, newPos, smoothTranslation * Time.deltaTime);
    }
}
