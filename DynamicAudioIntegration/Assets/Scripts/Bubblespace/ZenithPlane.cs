using UnityEngine;

public class ZenithPlane : AudioPlane
{
    // Colors
    public Color heightColor = Color.black;

    // Height
    private float _height = 5f;

    // Buffers
    private float[] _averageDistances = new float[4];

    // Azimuth plane
    public AzimuthPlane azimuthPlane;

    void Start()
    {
        rayAngle = 360f / rays;
        origins = new Vector3[rays];
        origins = GetRayOrigins();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            Debug.Log("Current step: " + currentStep);

            //Only to debug while not applied to Player
            if (currentStep == 0) origins = GetRayOrigins(); 

            DrawRay(origins);
            CalculateStepAverage();
            currentStep++;

            if (currentStep == steps)
            {
                _height = CalculateAverageDistance();
                Debug.Log("Height: " + _height);

                ResetStep();
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (!Application.isPlaying) return;

        Gizmos.color = heightColor;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.up * _height);
    }

    protected override Vector3[] GetRayOrigins()
    {
        for (int i = 0; i < rays; i++)
        {
            currentAngle = (baseAngle + i * rayAngle) * Mathf.Deg2Rad;
            origins[i] = transform.position + GetHorizontalDirection(currentAngle) * azimuthPlane.radius;
        }

        return origins;
    }

    // In Zenith Plane
    protected override Vector3 GetDirection(float angle)
    {
        return Vector3.up;
    }

    // Vertical Plane Exclusive
    protected override void CalculateStepAverage()
    {
        averageDistances[currentStep] = (stepDistances[0] + stepDistances[1]) / 2f;

        Debug.Log("Step distance: " + averageDistances[currentStep]);
    }
}
