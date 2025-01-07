using UnityEngine;
using UnityEngine.UI;

public class MoveScreen : MonoBehaviour
{
    public RectTransform textTransform; // The RectTransform of the text
    public Vector2 startPosition;      // The starting position off-screen (to the right)
    public Vector2 endPosition;        // The ending position on-screen
    public float slideDuration = 2.0f; // Time it takes to slide in
    private float elapsedTime = 0;     // Tracks elapsed time

    private bool isSliding = false;    // Determines if sliding is active

    void Start()
    {
        // Initialize text at the start position
        textTransform.anchoredPosition = startPosition;
    }

    void Update()
    {
        if (isSliding)
        {
            // Calculate progress
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / slideDuration);

            // Smooth interpolation using Mathf.SmoothStep for a smoother motion
            textTransform.anchoredPosition = Vector2.Lerp(startPosition, endPosition, t);

            // Stop sliding when finished
            if (t >= 1.0f)
            {
                isSliding = false;
            }
        }
    }

    // Call this method to start sliding the text
    public void StartSliding()
    {
        elapsedTime = 0;
        isSliding = true;
    }
}
