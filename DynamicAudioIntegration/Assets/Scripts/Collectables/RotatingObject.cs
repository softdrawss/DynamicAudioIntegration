using UnityEngine;

public class RotatingObject : MonoBehaviour
{
    public Vector3 rotationPerSecond = new Vector3(0, 50, 0);

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.Rotate(rotationPerSecond * Time.deltaTime);
    }
}
