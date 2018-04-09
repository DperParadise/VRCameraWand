using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Puzzle : MonoBehaviour {

    private const int numRows = 4;
    private const int numCols = 4;
    private GameObject[] cubes;
    public GameObject cubePrefab;
    private const float cubeDepth = 0.5f;
    private const float originOffset = 1.5f;
    
    private int numTestPos = 4;
    private Vector2[] testOffsets;
    private List<Vector2> testPosList;

    public List<Material> piecesMaterial;

    // Use this for initialization
    void Start () {

        cubes = new GameObject[numRows * numCols];
        CreateLayout(cubes);
        testOffsets = new Vector2[] { new Vector2(0f, -1f),
                                      new Vector2(0f, 1f),
                                      new Vector2(-1f, 0f),
                                      new Vector2(1f, 0f) };
        testPosList = new List<Vector2>();
   
	}
	
    private void CreateLayout(GameObject[] cubes)
    {
        Vector3 origin = transform.position;
        origin.x -= cubePrefab.transform.lossyScale.x * originOffset;
        origin.y += cubePrefab.transform.lossyScale.x * originOffset;
        origin.z -= cubePrefab.transform.lossyScale.x * cubeDepth;
        Vector3 offset = Vector3.zero;
        
        for (int row = 0; row < numRows; row++)
        {
            for (int col = 0; col < numCols; col++)
            {
                if (row == numRows - 1 && col == numCols - 1)
                {
                    cubes[row * numCols + col] = null;
                    return;
                }

                offset.x = col * cubePrefab.transform.lossyScale.x ;
                offset.y = -row * cubePrefab.transform.lossyScale.x;
                GameObject cube = Instantiate(cubePrefab, origin + offset, Quaternion.identity);
                cube.transform.SetParent(transform);
                cube.GetComponent<PuzzlePieceInfo>().posInBoard = new Vector2(col, row);
                cube.GetComponent<MeshRenderer>().material = piecesMaterial[row * numCols + col];
                cubes[row * numCols + col] = cube;
            }
        }
    }

    private void MovePuzzlePiece(GameObject puzzlePiece)
    {
        Vector2 posInBoard = puzzlePiece.GetComponent<PuzzlePieceInfo>().posInBoard;

        for(int i = 0; i < numTestPos; i++)
        {
            AddTestPosition(testPosList, testOffsets[i], posInBoard);
        }
        
        foreach(Vector2 testPos in testPosList)
        {
            int index = (int)testPos.y * numCols + (int)testPos.x;
            if(index >= 0 && index < numRows * numCols)
            {
                if (cubes[index] == null)
                {
                    cubes[index] = puzzlePiece;
                    cubes[(int)posInBoard.y * numCols + (int)posInBoard.x] = null;
                    puzzlePiece.GetComponent<PuzzlePieceInfo>().posInBoard = testPos;

                    Vector3 origin = transform.position;
                    origin.x -= cubePrefab.transform.lossyScale.x * originOffset;
                    origin.y += cubePrefab.transform.lossyScale.x * originOffset;
                    origin.z -= cubePrefab.transform.lossyScale.x * cubeDepth;
                    Vector3 offset = Vector3.zero;
                    offset.x = testPos.x * cubePrefab.transform.lossyScale.x;
                    offset.y = -testPos.y * cubePrefab.transform.lossyScale.x;

                    puzzlePiece.transform.position = origin + offset;
                    break;
                }
            }
        }
        testPosList.Clear();
    }

    private void AddTestPosition(List<Vector2> testPositions, Vector2 offset, Vector2 centralPos)
    { 
        Vector2 testPos = centralPos + offset;
        
        if ((int)testPos.x >= 0 && (int)testPos.y >= 0)
        {
            testPositions.Add(testPos);
        }
    }
}
