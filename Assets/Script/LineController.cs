using System.Net;
using Radishmouse;
using UnityEngine;

public class LineController : MonoBehaviour
{
    private UILineRenderer lr;
    private RectTransform[] points;
    public RectTransform FishingLineSlot;
    public RectTransform Bobber;
    public int newPoints;

    private void Awake()
    {
        lr = GetComponent<UILineRenderer>();
    }

    private void Update()
    {
        Vector2 startPoint = new Vector2(FishingLineSlot.anchoredPosition.x - 110, FishingLineSlot.anchoredPosition.y - 50);
        Vector2 endPoint = new Vector2(Bobber.anchoredPosition.x - 60, Bobber.anchoredPosition.y - 45);
        Vector2[] curvePoints = CurveUtils.GenerateDownwardCurve(startPoint, endPoint, newPoints);
        lr.points = curvePoints;
        lr.SetAllDirty();
    }
}
public class CurveUtils
{
    public static Vector2[] GenerateDownwardCurve(Vector2 startPoint, Vector2 endPoint, int numberOfPoints)
    {
        // Calculate the control point to create a downward curve
        Vector2 controlPoint = new Vector2(
            (startPoint.x + endPoint.x) / 2, // Midpoint on the x-axis
            Mathf.Min(startPoint.y, endPoint.y) - Mathf.Abs(startPoint.y - endPoint.y) * 0.5f // Below the midpoint on the y-axis
        );

        Vector2[] points = new Vector2[numberOfPoints + 1];

        for (int i = 0; i <= numberOfPoints; i++)
        {
            float t = i / (float)numberOfPoints;
            points[i] = CalculateBezierPoint(t, startPoint, controlPoint, endPoint);
        }

        return points;
    }

    private static Vector2 CalculateBezierPoint(float t, Vector2 p0, Vector2 p1, Vector2 p2)
    {
        // Quadratic Bezier curve formula
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;

        return (uu * p0) + (2 * u * t * p1) + (tt * p2);
    }
}
