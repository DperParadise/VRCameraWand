using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeInfo : MonoBehaviour {

    public PlatformInfo platformInfo;
    [HideInInspector]
    public Quaternion initialOrientation;

    private void Awake()
    {
        initialOrientation = transform.rotation;
    }

}
