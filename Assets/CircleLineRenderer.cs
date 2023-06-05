using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class CircleLineRenderer : MonoBehaviour
{
    [Range(3, 360)]
    public int segments = 50;
    public float radius = 1f;

    private LineRenderer lineRenderer;

    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        DrawCircle();
    }

    public void DrawCircle()
    {
        lineRenderer.positionCount = segments + 1; // +1 to close the circle
        float angleStep = 360f / segments;
        for (int i = 0; i <= segments; i++) // <= to close the circle
        {
            float angle = i * angleStep * Mathf.Deg2Rad; // Convert from degrees to radians
            Vector3 point = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * radius;
            lineRenderer.SetPosition(i, point);
        }
    }
}
