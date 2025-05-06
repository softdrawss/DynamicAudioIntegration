using UnityEngine;

public class AzimuthPlane : AudioPlane
{
    // Colors
    public Color circleColor = Color.red;

    // Circle
    private int _segments;
    public float radius = 5f;
    private Vector3 _firstPoint, _prevPoint, _newPoint;

    // Angles
    private float _incrementAngle;  //Angle is incremented when changing step
    
    // Options
    public bool maximaAzimuth = false;

    private void Start()
    {
        _segments = steps * rays;
        _incrementAngle = 360f / _segments;         //360 / 12 = 30 (Angle to increment in each step)
        rayAngle = 360f / rays;                     //30 * 4 = 120 (Angle between each ray)

        origins = new Vector3[rays];
        origins = GetRayOrigins();

        //Debug.Log("Increment angle: " + _incrementAngle);
        //Debug.Log("Ray angle: " + _rayAngle);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("Current step: " + currentStep);

            DrawRay(origins);
            CalculateStepAverage();
            baseAngle += _incrementAngle;
            currentStep++;

            if (currentStep == steps)
            {
                radius = CalculateAverageDistance();
                Debug.Log("Radius: " + radius);

                ResetStep();
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (!Application.isPlaying) return;

        Gizmos.color = circleColor;
        DrawCircle();
    }

    // Horizontal Plane Exclusive
    private void DrawCircle() 
    {
        _firstPoint = transform.position + GetDirection(0f) * radius;
        _prevPoint = _firstPoint;

        for (int i = 1; i <= _segments; i++)
        {
            currentAngle = _incrementAngle * i * Mathf.Deg2Rad;
            _newPoint = transform.position + GetDirection(currentAngle) * radius;
            Gizmos.DrawLine(_prevPoint, _newPoint);
            _prevPoint = _newPoint;
        }

        Gizmos.DrawLine(_prevPoint, _firstPoint);
    }

    protected override Vector3[] GetRayOrigins()
    {
        for (int i = 0; i < rays; i++)
            origins[i] = transform.position;

        return origins;
    }

    // Horizontal Plane Exclusive
    protected override Vector3 GetDirection(float angle)
    {
        return GetHorizontalDirection(angle);
    }

    // Horizontal Plane Exclusive
    protected override void CalculateStepAverage()
    {
        if (maximaAzimuth)
            averageDistances[currentStep] = (stepDistances[1] + stepDistances[2]) / 2f;
        else
            averageDistances[currentStep] = (stepDistances[0] + stepDistances[1]) / 2f;

        Debug.Log("Step distance: " + averageDistances[currentStep]);
    }

}
