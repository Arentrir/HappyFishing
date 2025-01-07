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
    private bool isClosed = true;
    private Vector3 privateStartScale;
    private Vector3 privateEndScale;


    void Start()
    {
        //isClosed = true;
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
            textTransform.localScale = Vector3.Lerp(privateStartScale, privateEndScale, t);

            // Stop sliding when finished
            if (t >= 1.0f)
            {
                isScaling = false;
                if (isClosed) 
                {
                    this.gameObject.SetActive(false);
                }
            }
        }
    }

    // Call this method to start sliding the text
    public void StartScaling()
    {
        if (isClosed)
        {
            elapsedTime = 0;
            isScaling = true;
            isClosed = false;
            privateStartScale = startScale;
            privateEndScale = endScale;
            this.gameObject.SetActive(true);
        }
    }
    public void ClosePanel()
    {
        if (!isClosed)
        {
            isClosed = true;
            isScaling = true;
            elapsedTime = 0;
            privateStartScale = endScale;
            privateEndScale = startScale;
        }
    }
}
