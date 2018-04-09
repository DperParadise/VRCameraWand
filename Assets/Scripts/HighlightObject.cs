using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighlightObject : MonoBehaviour {

    private const string decalName = "Decal";
    private const string uniformColorName = "_OutlineColor";
    private Color yellow = Color.yellow;
    private Color transparent;

    private void Start()
    {
        transparent = new Color(0.0f, 0.0f, 0.0f, 0.0f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == decalName)
        {
            GetComponent<Renderer>().material.SetColor(uniformColorName, yellow);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.name == decalName)
        {
            GetComponent<Renderer>().material.SetColor(uniformColorName, transparent);
        }
    }
}
