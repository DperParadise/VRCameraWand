using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WandRayCast : MonoBehaviour {

    public float rayLength = 2.0f;
    private GameObject wandGO;
    private LineRenderer lineRenderer;
    private RaycastHit hitInfo;
    int interactionMask;
    private PickDropObject pickDropObject;
    private Vector3 decalPos;
    private GameObject decalGO;
    
    private GameObject cameraGO;
    private Puzzle puzzle;
    private Quaternion decalOrientation = Quaternion.identity;
    private float drawOffset = 0.08f;

    private const string interactionLayer = "Interaction";
    private const string pickableTag = "Pickable";
    private const string cubePlatformTag = "CubePlatform";
    private const string floorWallTag = "FloorWall";
    private const string puzzlePieceTag = "PuzzlePiece";

    private const string rightMouseButton = "Fire2";

    private void Awake()
    {
        wandGO = gameObject;
        
        lineRenderer = transform.Find("Laser").GetComponent<LineRenderer>();
        if (!lineRenderer)
            Debug.Log("No LineRenderer found.");

        pickDropObject = FindObjectOfType<PickDropObject>();
        if (!pickDropObject)
            Debug.Log("No PickDropObject found.");
        
        decalGO = transform.Find("Decal").gameObject;
        if (!decalGO)
            Debug.Log("No Decal gameobject found.");
        
        cameraGO = GameObject.FindGameObjectWithTag("MainCamera");
        if (!cameraGO)
            Debug.Log("No main camera found.");

        puzzle = FindObjectOfType<Puzzle>();
        if (!puzzle)
            Debug.Log("No Puzzle found");

        interactionMask = 1 << LayerMask.NameToLayer(interactionLayer);
    }
    
    private void LateUpdate()
    {
        if (Physics.Raycast(wandGO.transform.position, wandGO.transform.forward, out hitInfo, rayLength, interactionMask))
        {
            decalPos = hitInfo.point + drawOffset * hitInfo.normal;
            decalOrientation = Quaternion.LookRotation(hitInfo.normal);

            if (Input.GetButtonDown(rightMouseButton))
            {
                if (hitInfo.rigidbody.gameObject.CompareTag(pickableTag))
                {
                    pickDropObject.SendMessage("PickObject", hitInfo.rigidbody.gameObject);                   
                }
                else if (hitInfo.rigidbody.gameObject.CompareTag(cubePlatformTag))
                {
                    pickDropObject.SendMessage("DropObjectInPlatform", hitInfo.rigidbody.gameObject);
                }
                else if (hitInfo.rigidbody.gameObject.CompareTag(floorWallTag))
                {
                    pickDropObject.SendMessage("DropObjectInScene");
                }
                else if(hitInfo.rigidbody.gameObject.CompareTag(puzzlePieceTag))
                {
                    puzzle.SendMessage("MovePuzzlePiece", hitInfo.rigidbody.gameObject);
                }
            }
        }
        else
        {
            decalPos = wandGO.transform.position + wandGO.transform.forward * rayLength;
            decalOrientation = cameraGO.transform.rotation;

            if (Input.GetButtonDown(rightMouseButton))
                pickDropObject.SendMessage("DropObjectInScene");
        }

        //DrawDecal(decalPos, cameraGO.transform.rotation);
        DrawDecal(decalPos, decalOrientation);
        DrawLine(wandGO.transform.position, decalPos);
        
    }

    private void DrawLine(Vector3 origin, Vector3 target)
    {
        lineRenderer.SetPosition(0, origin);
        lineRenderer.SetPosition(1, target);
    }

    private void DrawDecal(Vector3 position, Quaternion orientation)
    {
        decalGO.transform.position = position;
        decalGO.transform.rotation = orientation;
    }
}
