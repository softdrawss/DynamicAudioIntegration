using UnityEngine;

[ExecuteAlways]
public class AudioPlayer : MonoBehaviour
{
    // Planes
    private AzimuthPlane _azimuthPlane;
    private ZenithPlane _zenithPlane;

    public AzimuthPlane AzimuthPlane => _azimuthPlane;
    public ZenithPlane ZenithPlane => _zenithPlane;

    // Bools
    public bool useAzimuthPlane;
    public bool useZenithPlane;

    private void OnValidate()
    {
        // Ensure Azimuth and Zenith Plane components exist
        if (_azimuthPlane == null)
        {
            _azimuthPlane = GetComponent<AzimuthPlane>();
            if (_azimuthPlane == null)
                _azimuthPlane = gameObject.AddComponent<AzimuthPlane>();
            _azimuthPlane.hideFlags = HideFlags.HideInInspector;
        }

        if (_zenithPlane == null)
        {
            _zenithPlane = GetComponent<ZenithPlane>();
            if (_zenithPlane == null)
                _zenithPlane = gameObject.AddComponent<ZenithPlane>();
            _zenithPlane.hideFlags = HideFlags.HideInInspector;
        }

        _zenithPlane.azimuthPlane = _azimuthPlane;
    }
}
