using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExamineObject : MonoBehaviour {

    [HideInInspector]
    public GameObject gameObjectToExamine;
    private float degrees;
    public float scrollWheelSensitivity = 10;
    private Quaternion rotation;
    private const string mouseScrollWheel = "Mouse ScrollWheel";

	private void Awake () {

        enabled = false;
	}

    private void OnEnable()
    {
        degrees = 0.0f;
    }

    // Update is called once per frame
    void Update () {

		if(Input.GetAxisRaw(mouseScrollWheel) > 0.0f)
        {
            degrees += scrollWheelSensitivity;
        }
        if(Input.GetAxisRaw(mouseScrollWheel) < 0.0f)
        {
            degrees -= scrollWheelSensitivity;
        }

        degrees = degrees % 360.0f;

        gameObjectToExamine.transform.localRotation = Quaternion.AngleAxis(degrees, Vector3.up);
	}
}
