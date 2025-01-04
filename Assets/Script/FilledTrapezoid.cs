using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class FilledTrapezoid : MonoBehaviour
{
    [Header("Trapezoid Points (Local Space)")]
    public Vector3[] trapezoidPoints = new Vector3[4]; // Four corners of the trapezoid

    [Header("Appearance")]
    public Color trapezoidColor = Color.white; // Color of the trapezoid

    [Header("Dynamic Movement")]
    [Range(0, 100)]
    public float dynamicValue; // Value that controls the interpolation

    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;
    private Material materialInstance;

    private bool needsMeshUpdate = false; // Flag to track if mesh needs updating

    void Awake()
    {
        Initialize();
    }

    void Start()
    {
        UpdateMesh();
        UpdateColor();
    }

    void OnValidate()
    {
        if (!Application.isPlaying)
        {
            Initialize(); // Ensure components are set up
        }

        needsMeshUpdate = true; // Mark for update
    }

    void Update()
    {
        if (needsMeshUpdate)
        {
            UpdateMesh();
            UpdateColor();
            needsMeshUpdate = false; // Reset flag
        }
    }

    private void Initialize()
    {
        meshFilter = GetComponent<MeshFilter>();
        meshRenderer = GetComponent<MeshRenderer>();

        if (materialInstance == null)
        {
            materialInstance = new Material(Shader.Find("Unlit/Color"));
            meshRenderer.sharedMaterial = materialInstance;
        }
    }

    public void UpdateMesh()
    {
        if (meshFilter == null) return;

        if (trapezoidPoints.Length != 4)
        {
            Debug.LogError("Trapezoid requires exactly 4 points!");
            return;
        }

        Mesh mesh = new Mesh
        {
            vertices = trapezoidPoints,
            triangles = new int[] {
                0, 1, 2, // Top-left, Top-right, Bottom-right
                2, 3, 0  // Bottom-right, Bottom-left, Top-left
            },
            uv = new Vector2[] {
                new Vector2(0, 1), // Top-left
                new Vector2(1, 1), // Top-right
                new Vector2(1, 0), // Bottom-right
                new Vector2(0, 0)  // Bottom-left
            }
        };

        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
        meshFilter.mesh = mesh;
    }

    public void UpdateColor()
    {
        if (materialInstance != null)
        {
            materialInstance.color = trapezoidColor; // Change the color
        }
    }


    public void SetPoints(Vector3 topLeft, Vector3 topRight, Vector3 bottomRight, Vector3 bottomLeft)
    {
        trapezoidPoints[0] = topLeft;
        trapezoidPoints[1] = topRight;
        trapezoidPoints[2] = bottomRight;
        trapezoidPoints[3] = bottomLeft;

        // Update the mesh with new points
        UpdateMesh();
        UpdateColor();
    }
}





