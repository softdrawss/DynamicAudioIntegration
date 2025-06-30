using UnityEngine;

public class AzimuthTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (!AudioSystemManager.Instance._azimuthPlane.maximaAzimuth)
            AudioSystemManager.Instance._azimuthPlane.maximaAzimuth = true;
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (AudioSystemManager.Instance._azimuthPlane.maximaAzimuth)
            AudioSystemManager.Instance._azimuthPlane.maximaAzimuth = false;
    }
}
