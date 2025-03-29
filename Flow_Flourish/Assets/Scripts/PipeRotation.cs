using UnityEngine;

public class PipeRotation : MonoBehaviour
{
    public float rotationAngle = 90f; // Angle to rotate on each click
    public float rotationSpeed = 300f;  // Speed of the rotation (higher value = faster rotation)
    private bool isRotating = false;   // Flag to track if the rotation is in progress
    private float targetRotation; // Target rotation angle
    private Transform pivotPoint; // Reference to the pivot point (parent)

    void Start()
    {
        // Get the reference to the parent object (pivot point)
        pivotPoint = transform.parent;
    }

    void OnMouseDown()
    {
        if (!isRotating)  // Only start rotation if it's not already rotating
        {
            isRotating = true;
            targetRotation = pivotPoint.eulerAngles.z + rotationAngle;  // Calculate target rotation (current pivot rotation + rotationAngle)
        }
    }

    void Update()
    {
        if (isRotating)
        {
            // Smoothly rotate the pivot point towards the target rotation
            float currentRotation = Mathf.MoveTowardsAngle(pivotPoint.eulerAngles.z, targetRotation, rotationSpeed * Time.deltaTime);

            // Apply the new rotation to the pivot (parent) object
            pivotPoint.eulerAngles = new Vector3(0, 0, currentRotation);

            // Stop rotating once we have reached the target rotation (within a small threshold)
            if (Mathf.Abs(targetRotation - currentRotation) < 0.1f)  // Small tolerance for accuracy
            {
                Debug.Log("Current Rotation: " + currentRotation);
                Debug.Log("Target Rotation: " + targetRotation);
                isRotating = false;  // Stop rotation when we reach the target
            }
        }
    }
}
