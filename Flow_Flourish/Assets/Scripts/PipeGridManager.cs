using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeGridMAnager : MonoBehaviour
{
    
    public GameObject[] pipePrefabs; // Assign different pipe prefabs in Inspector
    public int rows = 6;
    public int columns = 5;
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
        Vector3 startPosition = transform.position;

        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
            // Calculate position (Now includes Z-axis for 3D grid)
                Vector3 position = startPosition + new Vector3(col * cellSize, -row * cellSize, 0); // Modify Z if necessary for depth

            // Debugging: Log the position of each pipe
                Debug.Log("Pipe Position: " + position);

            // Select a random pipe type (Optional)
                GameObject selectedPipe = pipePrefabs[Random.Range(0, pipePrefabs.Length)];

            // Instantiate pipe at 3D position
                GameObject newPipe = Instantiate(selectedPipe, position, Quaternion.identity);

            // Parent to this manager for better scene organization
                newPipe.transform.parent = transform;
            }
        }
    }
}
