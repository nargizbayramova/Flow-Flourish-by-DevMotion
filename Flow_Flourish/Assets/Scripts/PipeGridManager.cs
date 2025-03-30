using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeGridMAnager : MonoBehaviour
{
    // 0 - Corner
    // 1 - Straight
    // 2 - 3-Way
    // 3 - 4-Way
    public GameObject[] pipePrefabs; // Assign different pipe prefabs in Inspector
    public float cellSize = 10.0f; // Adjust spacing for 3D grid

    public void Start()
    {
        GenerateGrid();
    }

// Update is called once per frame
    public void Update()
    {
        
    }
    public void GenerateGrid(){

        int[,] levelPipeType = {
            { 0, 3,  1,  3,  0 },
            { 1, 2,  1,  0,  2 },
            { 2, 0,  0,  0,  3 },
            { 0, 3,  0,  0,  2 },
            { 0, 1,  2,  2,  1 },
            { 0, 1,  0,  1,  1 }
        };

        int[,] levelCorrectPath = {
            { 0, 0,  1,  0,  0 },
            { 0, 0,  1,  0,  0 },
            { 0, 0,  1,  1,  0 },
            { 0, 0,  1,  1,  0 },
            { 1, 1,  1,  0,  0 },
            { 1, 1,  1,  0,  0 }
        };

        int[,] levelCorrectPipeAngle = {
            { 0, 0,  90,  0,  0 },
            { 0, 0,  90,  0,  0 },
            { 0, 0,  180,  0,  0 },
            { 0, 0,  90,  270,  0 },
            { 90, 0,  0,  0,  0 },
            { 180, 0,  0,  0,  0 }
        };
//21
        int totalNumberOfTurnNeeded=0;
        Vector3 startPosition = transform.position;

        for (int row = 0; row < levelPipeType.GetLength(0); row++)
        {
            for (int col = 0; col < levelPipeType.GetLength(1); col++)
            {
                int pipeType=levelPipeType[row,col];
                Vector3 position = startPosition + new Vector3(col * cellSize, -row * cellSize, 0); // Modify Z if necessary for depth

                int randomRotationDegree = Random.Range(0, 4) * 90;

                // Quaternion.Euler(0,0,randomRotation)
                GameObject newPipe = Instantiate(pipePrefabs[pipeType], position, Quaternion.Euler(0,0,randomRotationDegree));

                newPipe.transform.parent = transform;

                if(levelCorrectPath[row,col]==1){
                    int differenceBetweenAngles = (levelCorrectPipeAngle[row,col]-randomRotationDegree)/90;
                    int count=differenceBetweenAngles>=0 ? differenceBetweenAngles : 4+differenceBetweenAngles;
                    totalNumberOfTurnNeeded+=count;
                    Debug.Log($"Count: {count} Rotation: {randomRotationDegree}° , Angles {newPipe.transform.eulerAngles}");
                }
            }
        }
                Debug.Log($"Number of Turn needed  {totalNumberOfTurnNeeded} ");
    }
}
