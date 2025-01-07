using UnityEngine;
using UnityEngine.UI;

public class ScaleChange : MonoBehaviour
{
    public RectTransform textTransform; // The RectTransform of the text
    public Vector3 startScale;      // The starting position off-screen (to the right)
    public Vector3 endScale;        // The ending position on-screen
    public float scaleDuration = 2.0f; // Time it takes to slide in
    private float elapsedTime = 0;     // Tracks elapsed time

    private bool isScaling = false;    // Determines if sliding is active

    void Start()
    {
        textTransform = GetComponent<RectTransform>();
        // Initialize text at the start position
        textTransform.localScale = startScale;
    }

    void Update()
    {
        if (isScaling)
        {
            // Calculate progress
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / scaleDuration);

            // Smooth interpolation using Mathf.SmoothStep for a smoother motion
            textTransform.localScale = Vector3.Lerp(startScale, endScale, t);

            // Stop sliding when finished
            if (t >= 1.0f)
            {
                isScaling = false;
            }
        }
    }

    // Call this method to start sliding the text
    public void StartScaling()
    {
        elapsedTime = 0;
        isScaling = true;
    }
}
