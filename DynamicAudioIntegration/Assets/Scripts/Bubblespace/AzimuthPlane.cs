using UnityEditor;
using UnityEngine;

public class AzimuthPlane : MonoBehaviour
{
    // Colors
    public Color circleColor = Color.red;
    public Color rayColor = Color.yellow;

    // Circle
    public int segments = 12;
    public float radius = 5f;

    // Rays
    public int raysPerStep = 4;
    private float _angleBetweenRays; //For each step
    private float _baseAngle = 0f;

    private void Start()
    {
        _angleBetweenRays = 360f / segments;
    }
    private void Update()
    {
        _baseAngle += 1f;
        if (_baseAngle >= 360f) _baseAngle -= 360f;
    }

    private void OnDrawGizmos()
    {
        if (!Application.isPlaying) return;

        Gizmos.color = circleColor;
        DrawCircle();

        Gizmos.color = rayColor;
        DrawRay();
        
    }

    private void DrawCircle() 
    {
        Vector3 firstPoint = CalculatePoint(0f);
        Vector3 prevPoint = firstPoint;
        Vector3 newPoint;
        float angle;

        for (int i = 1; i <= segments; i++)
        {
            angle = _angleBetweenRays * i * Mathf.Deg2Rad;
            newPoint = CalculatePoint(angle);
            Gizmos.DrawLine(prevPoint, newPoint);
            prevPoint = newPoint;
        }

        Gizmos.DrawLine(prevPoint, firstPoint);
    }

    private void DrawRay()
    {
        float stepAngle = _angleBetweenRays * 4;            // 30 * 4 = 120°

        for (int i = 0; i < 3; i++)
        {
            float currentAngle = _baseAngle + i * stepAngle;
            float rad = currentAngle * Mathf.Deg2Rad;

            Vector3 direction = new Vector3(Mathf.Cos(rad), 0f, Mathf.Sin(rad));
            Gizmos.DrawRay(transform.position, direction * radius);
        }
    }
    
    private Vector3 CalculatePoint(float angle)
    {
        return transform.position + new Vector3(Mathf.Cos(angle), 0f, Mathf.Sin(angle)) * radius;
    }
}
