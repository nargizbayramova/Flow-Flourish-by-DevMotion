using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditorInternal;
using UnityEngine;

public class GridManager_4 : MonoBehaviour
{
    // 0 - Corner
    // 1 - Straight
    // 2 - 3-Way
    // 3 - 4-Way
    public GameObject[] pipePrefabs; // Assign different pipe prefabs in Inspector
    public float cellSize; // Adjust spacing for 3D grid
    public int[,] levelPipeType = new int[,]{
            { 1, 3,  1,  1,  3 },
            { 0, 1,  0,  1,  0 },
            { 0, 1,  1,  0,  1 },
            { 3, 0,  1,  1,  1 },
            { 1, 0,  3,  0,  1 },
        };
    int[,] levelCorrectPath = new int[,]{
            { 0, 0,  1,  0,  0 },
            { 1, 1,  1,  0,  0 },
            { 1, 1,  1,  1,  0 },
            { 0, 0,  0,  1,  0 },
            { 0, 0,  1,  1,  0 },
        };
    int[,] levelCorrectPipeAngle = new int[,] {
            { 0, 0,  90,  0,  0 },
            { 90, 0,  270,  0,  0 },
            { 180, 0,  0,  0,  0 },
            { 0, 0,  90,  90,  0 },
            { 90, 0,  0,  270,  0 },
        };

    bool isSuccess = false;

    [SerializeField] TMP_Text attemptScore;
    private GameObject[,] gridPipes;
    public void Start()
    {
        int rows = levelPipeType.GetLength(0);
        int cols = levelPipeType.GetLength(1);
        gridPipes = new GameObject[rows, cols];
        GenerateGrid();
    }

    // Update is called once per frame
    public void Update()
    {
        if (isSuccess)
        {
            attemptScore.text = "Success";
        }
        if (int.TryParse(attemptScore.text, out int score) && score == 0 && !isSuccess)
        {
            attemptScore.text = "Failed";
        }
    }
    public void GenerateGrid()
    {
        int totalNumberOfTurnNeeded = 0;
        Vector3 startPosition = transform.position;
        int rows = levelPipeType.GetLength(0);
        int cols = levelPipeType.GetLength(1);

        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < cols; col++)
            {
                int pipeType = levelPipeType[row, col];
                Vector3 position = startPosition + new Vector3(col * cellSize, -row * cellSize, 0);

                int randomRotationDegree = UnityEngine.Random.Range(0, 4) * 90;

                // Quaternion.Euler(0,0,randomRotation)
                GameObject newPipe = Instantiate(pipePrefabs[pipeType], position, Quaternion.Euler(0, 0, randomRotationDegree));
                gridPipes[row, col] = newPipe;
                newPipe.transform.parent = transform;

                if (levelCorrectPath[row, col] == 1)
                {
                    var rotScript = newPipe.GetComponentInChildren<PipeRotation>();

                    if (rotScript != null)
                    {
                        rotScript.OnRotationComplete = CheckSuccess;
                    }

                    totalNumberOfTurnNeeded += CalculateTapNeed(row, col, randomRotationDegree);
                }

            }
        }
        CheckSuccess();
        Debug.Log($"Number of Turn needed  {totalNumberOfTurnNeeded} ");
        totalNumberOfTurnNeeded += 15;
        attemptScore.text = totalNumberOfTurnNeeded.ToString();
    }

    // Tüm boruları kontrol et; eğer hepsi doğru açıda ise başarı mesajı
    public void CheckSuccess()
    {
        int rows = levelPipeType.GetLength(0);
        int cols = levelPipeType.GetLength(1);

        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < cols; c++)
            {
                // Yolu takip etmesi gereken boru mu?
                if (levelCorrectPath[r, c] == 1)
                {
                    float rawZ = gridPipes[r, c].transform.eulerAngles.z;
                    float curZ = Mathf.Round(rawZ / 90f) * 90f;
                    Debug.Log("Current Pipe angle: "+curZ);
                    if (!CheckPipeCorrection(r, c, curZ))
                    {
                        Debug.Log("row: " + r + " column: " + c);
                        Debug.Log("Not Complete");
                        return;
                    }

                }
            }
        }
        isSuccess = true;
        attemptScore.text = "Success";
        Debug.Log("Succes");
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
    private int FindCorner(int row, int col, int randomRotation)
    {
        int differenceBetweenAngles = (levelCorrectPipeAngle[row, col] - randomRotation) / 90;
        return differenceBetweenAngles >= 0 ? differenceBetweenAngles : 4 + differenceBetweenAngles;
    }
    private int FindStraigth(int row, int col, int randomRotation)
    {
        int correctAngle = levelCorrectPipeAngle[row, col];
        if (correctAngle == randomRotation || (correctAngle + 180) == randomRotation)
        {
            return 0;
        }
        return 1;
    }
    private int FindThreeWay(int row, int col, int randomRotation)
    {
        int correctAngle = levelCorrectPipeAngle[row, col];


        if (correctAngle == randomRotation || ((correctAngle + 90) % 360) == randomRotation)
        {
            return 0;
        }
        int differenceBetweenAngles = (levelCorrectPipeAngle[row, col] - randomRotation) / 90;
        return differenceBetweenAngles >= 0 ? differenceBetweenAngles : 4 + differenceBetweenAngles;
    }




    private bool CheckPipeCorrection(int row, int col, float currentPipeAngle)
    {

        switch (levelPipeType[row, col])
        {
            case 0:
                return CheckCornerCorrection(row, col, currentPipeAngle);
            case 1:
                return CheckStraigthCorrection(row, col, currentPipeAngle);
            case 2:
                return CheckThreeWayCorrection(row, col, currentPipeAngle);
            default:
                return true; // Four way pipe always needs zero tap.
        }
    }
    private bool CheckCornerCorrection(int row, int col, float currentPipeAngle) {
        return Math.Abs(levelCorrectPipeAngle[row, col] - currentPipeAngle) <=0.1f ;
    }
    private bool CheckStraigthCorrection(int row, int col, float currentPipeAngle) {
        float differenceBetweenAngles = Math.Abs(levelCorrectPipeAngle[row, col] - currentPipeAngle);
        if (differenceBetweenAngles == 0 || differenceBetweenAngles == 180)
        {
            return true;
        }
        return false;
    }
    private bool CheckThreeWayCorrection(int row, int col, float currentPipeAngle) {

        float correctAngle = levelCorrectPipeAngle[row, col];
        if (Math.Abs(correctAngle - currentPipeAngle) <= 0.1f || Math.Abs(correctAngle + 90 - currentPipeAngle) % 360 <= 0.1f)
        {
            return true;
        }
        return false;
    }

}
