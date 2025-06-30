using UnityEngine;
using static UnityEngine.UI.Image;

public class AudioPlane : MonoBehaviour
{
    // Colors
    public Color rayColor = Color.yellow;
    public Color hitColor = Color.green;

    // Steps
    public int steps = 4;
    protected int currentStep = 0;

    // Angles
    protected float baseAngle = 0f;
    protected float currentAngle;
    protected float rayAngle;        //Angle between rays

    // Ray
    protected Vector3 direction;
    public float maxDistance = 20f;
    public int rays = 3;
    private Ray ray;

    // Buffers
    protected float[] stepDistances = new float[3];
    protected float[] averageDistances = new float[4];
    protected Vector3[] origins;

    protected void DrawRay(Vector3[] origins)
    {
        for (int i = 0; i < rays; i++)
        {
            currentAngle = (baseAngle + i * rayAngle) * Mathf.Deg2Rad;
            //Debug.Log("Current angle: " + currentAngle);

            direction = GetDirection(currentAngle);

            ray = new Ray(origins[i], direction);

            if (Physics.Raycast(ray, out RaycastHit hit, maxDistance))
            {
                stepDistances[i] = hit.distance;
                //Debug.Log("Ray hit!: Distance: " + stepDistances[i]);
                //Debug.DrawRay(origins[i], direction * stepDistances[i], rayColor, 2.0f);
                //Debug.DrawLine(hit.point, hit.point + Vector3.up * 0.2f, hitColor, 50f);
            }
            else
            {
                stepDistances[i] = 100f;
                //Debug.Log("No hit!: Distance: " + stepDistances[i]);
                //Debug.DrawRay(origins[i], direction * 100f, rayColor, 2.0f);
            }
        }

        SortDistances();
    }

    public virtual void HandlePlane() { }

    // Get ray origins
    protected virtual Vector3[] GetRayOrigins() { return origins; }

    //Calculate the Direction where the ray will be accoding to the angle
    protected virtual Vector3 GetDirection(float angle) { return Vector3.zero; }

    protected Vector3 GetHorizontalDirection(float angle)
    {
        return new Vector3(Mathf.Cos(angle), 0f, Mathf.Sin(angle));
    }

    // Depending on if we are using Maxima or Minima Azimuth estimation
    protected void SortDistances()
    {
        System.Array.Sort(stepDistances);

        //Debug.Log("Sorted distances: " + stepDistances[0] + ", " + stepDistances[1] + ", " + stepDistances[2]);
    }

    protected virtual void CalculateStepAverage() { }

    protected float CalculateAverageDistance()
    {
        System.Array.Sort(averageDistances);
        //Debug.Log("Average sorted distance 0: " + averageDistances[0]);
        //Debug.Log("Average sorted distance 1: " + averageDistances[1]);
        //Debug.Log("Average sorted distance 2: " + averageDistances[2]);
        //Debug.Log("Average sorted distance 3: " + averageDistances[3]);

        return (averageDistances[0] + averageDistances[1] + averageDistances[2]) / 3f;
    }

    protected void ResetStep()
    {
        baseAngle = 0f;
        currentStep = 0;
    } 
}
