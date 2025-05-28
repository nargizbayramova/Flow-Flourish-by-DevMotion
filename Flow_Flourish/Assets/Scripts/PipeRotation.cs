using System;
using TMPro;
using UnityEditorInternal;
using UnityEngine;

public class PipeRotation : MonoBehaviour
{
    public float rotationAngle = 90f; // Angle to rotate on each click
    public float rotationSpeed = 300f;  // Speed of the rotation (higher value = faster rotation)
    private bool isRotating = false;   // Flag to track if the rotation is in progress
    private float targetRotation; // Target rotation angle
    private Transform pivotPoint; // Reference to the pivot point (parent)
    public Action OnRotationComplete;

    [SerializeField] TMP_Text tapScore;

    void Start()
    {
        tapScore= GameObject.Find("TapScore").GetComponent<TMP_Text>();
        // Get the reference to the parent object (pivot point)
        pivotPoint = transform.parent;
    }

    void OnMouseDown()
    {

        if (int.TryParse(tapScore.text, out int score) && score > 0 && !isRotating)  // Only start rotation if it's not already rotating
        {
            isRotating = true;
            score--;
            tapScore.text = score.ToString();
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
            if (Mathf.Abs(targetRotation - currentRotation) == 0.0f)  // Small tolerance for accuracy
            {
                isRotating = false;  // Stop rotation when we reach the target
                float snapped = Mathf.Round(currentRotation / 90f) * 90f;
                pivotPoint.eulerAngles = new Vector3(0, 0, snapped);
                OnRotationComplete?.Invoke();
            }
        }
    }
}
