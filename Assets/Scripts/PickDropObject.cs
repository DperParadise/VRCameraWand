using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickDropObject : MonoBehaviour {

    private GameObject grabPointGO;
    private GameObject holdGO;
    private const string grabPointGOName = "GrabPoint";
    private ExamineObject examineObject;
 
    private void Awake()
    {
        grabPointGO = GameObject.Find(grabPointGOName);
        if (!grabPointGO)
            Debug.Log("No grabPointGO found.");

        examineObject = GameObject.FindObjectOfType<ExamineObject>();
        if (!examineObject)
            Debug.Log("No examineObject found.");
    }

    private void PickObject(GameObject go)
    {
        if (holdGO)
        {
            DropObjectInScene();
            return;
        }
        
        if(!holdGO)
        {
            holdGO = go;
            holdGO.transform.SetParent(grabPointGO.transform);
            holdGO.transform.position = grabPointGO.transform.position;

            if (holdGO.GetComponent<CubeInfo>().platformInfo)
            {
                holdGO.GetComponent<CubeInfo>().platformInfo.empty = true;
                holdGO.GetComponent<CubeInfo>().platformInfo = null;
            }

            examineObject.enabled = true;
            examineObject.gameObjectToExamine = holdGO;
        }
    }

    private void DropObjectInPlatform(GameObject platformGO)
    {
        if (!holdGO)
            return;

        if (platformGO.GetComponent<PlatformInfo>().empty)
        {
            platformGO.GetComponent<PlatformInfo>().empty = false;
            holdGO.GetComponent<CubeInfo>().platformInfo = platformGO.GetComponent<PlatformInfo>();
            holdGO.transform.SetParent(null);
            holdGO.transform.position = holdGO.GetComponent<CubeInfo>().platformInfo.slotPosition;
            holdGO.transform.rotation = holdGO.GetComponent<CubeInfo>().initialOrientation;
            holdGO = null;
        }
        else
        {
            DropObjectInScene();
        }

        examineObject.gameObjectToExamine = null;
        examineObject.enabled = false;
    }

    private void DropObjectInScene()
    {
        if (!holdGO)
            return;

        holdGO.transform.SetParent(null);
        holdGO = null;
        
        examineObject.gameObjectToExamine = null;
        examineObject.enabled = false;
    }
}
