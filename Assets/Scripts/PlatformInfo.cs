using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformInfo : MonoBehaviour {

    [HideInInspector]
    public Vector3 slotPosition;
    [HideInInspector]
    public bool empty = false;
    private const string slot = "Slot";

     private void Awake()
    {
        slotPosition = transform.Find(slot).transform.position;
    }

}
