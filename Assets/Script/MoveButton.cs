using UnityEngine;
using UnityEngine.UI;

public class MoveButton : MonoBehaviour
{
    public RectTransform textTransform; // The RectTransform of the text
    public Vector2 startPosition;      // The starting position off-screen (to the right)
    public Vector2 endPosition;        // The ending position on-screen
    public float slideDuration = 2.0f; // Time it takes to slide in
    private float elapsedTime = 0;     // Tracks elapsed time

    public bool isSliding = false;    // Determines if sliding is active
    public bool movingToEnd = false;   // Determines the direction of sliding

    void Start()
    {
        // Initialize text at the start position
        textTransform.anchoredPosition = startPosition;
    }

    void Update()
    {
        if (isSliding)
        {
            // Update elapsedTime based on direction
            elapsedTime += Time.deltaTime * (movingToEnd ? 1 : -1);

            // Calculate progress (clamped to valid range)
            float t = Mathf.Clamp01(elapsedTime / slideDuration);

            // Smooth interpolation using Mathf.SmoothStep for smoother motion
            textTransform.anchoredPosition = Vector2.Lerp(startPosition, endPosition, Mathf.SmoothStep(0, 1, t));

            // Stop sliding when the animation completes
            if (elapsedTime <= 0 || elapsedTime >= slideDuration)
            {
                isSliding = false;
                elapsedTime = Mathf.Clamp(elapsedTime, 0, slideDuration); // Ensure elapsedTime is within range
            }
        }
    }

    // Call this method to toggle sliding the text
    public void ToggleSliding()
    {

        if (!isSliding)
        {
            // Start sliding in the opposite direction
            movingToEnd = !movingToEnd;
            isSliding = true;
        }
        else
        {
            // Reverse direction mid-slide
            movingToEnd = !movingToEnd;

            // Adjust elapsedTime to account for the reversed direction
            // elapsedTime = slideDuration - elapsedTime;
        }
    }
}


