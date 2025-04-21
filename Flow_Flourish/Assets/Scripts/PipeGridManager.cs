using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditorInternal;
using UnityEngine;

public class PipeGridMAnager : MonoBehaviour
{
    // 0 - Corner
    // 1 - Straight
    // 2 - 3-Way
    // 3 - 4-Way
    public GameObject[] pipePrefabs; // Assign different pipe prefabs in Inspector
    public float cellSize = 10.0f; // Adjust spacing for 3D grid
    int[,] levelPipeType;
    int[,] levelCorrectPath;
    int[,] levelCorrectPipeAngle;

    [SerializeField] TMP_Text attemptScore;
    public void Start()
    {
        levelPipeType = new int[,]{
            { 0, 3,  1,  3,  0 },
            { 1, 2,  1,  0,  2 },
            { 2, 0,  0,  0,  3 },
            { 0, 3,  0,  0,  2 },
            { 0, 1,  2,  2,  1 },
            { 0, 1,  0,  1,  1 }
        };

        levelCorrectPath = new int [,]{
            { 0, 0,  1,  0,  0 },
            { 0, 0,  1,  0,  0 },
            { 0, 0,  1,  1,  0 },
            { 0, 0,  1,  1,  0 },
            { 1, 1,  1,  0,  0 },
            { 1, 1,  1,  0,  0 }
        };

        levelCorrectPipeAngle = new int[,] {
            { 0, 0,  90,  0,  0 },
            { 0, 0,  90,  0,  0 },
            { 0, 0,  180,  0,  0 },
            { 0, 0,  90,  270,  0 },
            { 90, 0,  270,  0,  0 },
            { 180, 0,  0,  0,  0 }
        };
        GenerateGrid();
    }

// Update is called once per frame
    public void Update()
    {
        
    }
    public void GenerateGrid(){

        
        int totalNumberOfTurnNeeded=0;
        Vector3 startPosition = transform.position;

        for (int row = 0; row < levelPipeType.GetLength(0); row++)
        {
            for (int col = 0; col < levelPipeType.GetLength(1); col++)
            {
                int pipeType=levelPipeType[row,col];
                Vector3 position = startPosition + new Vector3(col * cellSize, -row * cellSize, 0);

                int randomRotationDegree = Random.Range(0, 4) * 90;

                // Quaternion.Euler(0,0,randomRotation)
                GameObject newPipe = Instantiate(pipePrefabs[pipeType], position, Quaternion.Euler(0,0,randomRotationDegree));

                newPipe.transform.parent = transform;

                if(levelCorrectPath[row,col]==1){
                    totalNumberOfTurnNeeded+=CalculateTapNeed(row,col,randomRotationDegree);
                }
            }
        }
        Debug.Log($"Number of Turn needed  {totalNumberOfTurnNeeded} ");
        totalNumberOfTurnNeeded+=15;
        attemptScore.text=totalNumberOfTurnNeeded.ToString();
    }

    public int CalculateTapNeed(int row, int col, int randomRotation)
{
    switch (levelPipeType[row, col])
    {
        case 0:
            return FindCorner(row, col, randomRotation);
        case 1:
            return FindStraigth(row, col, randomRotation);
        case 2:
            return FindThreeWay(row, col, randomRotation);
        default:
            return 0; // Four way pipe always needs zero tap.
    }
}
    private int FindCorner(int row, int col, int randomRotation){
        int differenceBetweenAngles = (levelCorrectPipeAngle[row,col]-randomRotation)/90;
        return differenceBetweenAngles>=0 ? differenceBetweenAngles : 4+differenceBetweenAngles;
    }
    private int FindStraigth(int row, int col, int randomRotation){
        int correctAngle=levelCorrectPipeAngle[row,col];
        if(correctAngle==randomRotation ||(correctAngle+180)==randomRotation){
            return 0;
        }
        return 1;
    }

    private int FindThreeWay(int row, int col, int randomRotation){
        int correctAngle=levelCorrectPipeAngle[row,col];


        if(correctAngle==randomRotation || ((correctAngle+90)%360)==randomRotation){
            return 0;
        }
         int differenceBetweenAngles = (levelCorrectPipeAngle[row,col]-randomRotation)/90;
        return differenceBetweenAngles>=0 ? differenceBetweenAngles : 4+differenceBetweenAngles;
    }
}
