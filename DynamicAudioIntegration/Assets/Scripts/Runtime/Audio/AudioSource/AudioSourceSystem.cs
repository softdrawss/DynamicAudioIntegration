using UnityEngine;
using static UnityEngine.UI.Image;

public class AudioSourceSystem : MonoBehaviour
{
    // Colors
    public Color rayColor = Color.white;
    public Color audioSourceColor = Color.blue;

    // Offset
    public float xOffset = 5f;

    // Player
    public Transform player;
    private Vector3 _directionToPlayer;

    // Rays
    public float maxDistance = 20f;
    bool leftClear, rightClear;
    [Header("Ray Cone Settings")]
    public int rayCount = 2; // Number of rays in the cone
    public float coneAngle = 45f; // Angle in degrees

    // Occlusion
    private int _occludedRays = 0;
    private float _totalOcclusion = 0f;

    // Cutoff Frequencies
    [Header("Cutoff Frequencies")]
    public float unoccludedCutoff = 22000f;
    public float fullyOccludedCutoff = 800f;
    public float transitionSpeed = 5f;
    private float _targetCutoff;
    private float _currentCutoff;

    // Audio Source
    private AudioSource _audioSource;

    // Audio Low Pass Filter
    private AudioLowPassFilter _filter;

    // Clarity
    [Header("Clarity Mapping")]
    public AnimationCurve clarityCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    private float _clarityRatio;
    private float _curvedClarity;

    void Start()
    {
        if (!player) return;

        _audioSource = GetComponent<AudioSource>();

        _filter = GetComponent<AudioLowPassFilter>();
        _currentCutoff = unoccludedCutoff;
    }

    private bool IsInsideRange() { return Vector3.Distance(transform.position, player.position) < _audioSource.maxDistance; }

    public void HandleAudioBehaviour()
    {
        if (IsInsideRange())
        {
            if (!Physics.Linecast(transform.position, player.position, out RaycastHit hit) || hit.transform == player)
            {
                //Debug.Log("Sound is CLEAR");
                HandleAudioFilter(1f);
            }
            else
                CheckSurroundings();
        }
    }

    // Need to change behavviour so it checks materials
    private void CheckSurroundings()
    {


        for (int i = 0; i < rayCount; i++)
        {
            if (Physics.Raycast(transform.position, GetQuaternionRayDirection(i, rayCount), out RaycastHit hit, maxDistance) && hit.transform != player)
            {
                AudioMaterialComponent audioMaterial = hit.collider.GetComponent <AudioMaterialComponent>();
                float occlusion = audioMaterial != null ? Mathf.Clamp01(audioMaterial.material.audioOclusion) : 0.0f; // Default fallback
                //Debug.Log("Occlusion: " + occlusion);
                _totalOcclusion += occlusion;
                _occludedRays++;
            }
        }

        if (_occludedRays > 0)
            _clarityRatio = 1 - (_totalOcclusion / _occludedRays);
        else
            _clarityRatio = 1f;
        
        //Debug.Log("Clarity ratio: " + _clarityRatio);
        HandleAudioFilter(_clarityRatio);
    }

    private float GetConeAngle(int i, int rayCount) { return Mathf.Lerp(-coneAngle / 2, coneAngle / 2, (float)i / (rayCount - 1)); }

    private Vector3 GetQuaternionRayDirection(int i, int rayClount) { return Quaternion.AngleAxis(GetConeAngle(i, rayCount), Vector3.up) * (player.position - transform.position).normalized; }

    private bool IsRayClear(Vector3 origin, Vector3 direction)
    {
        return !Physics.Raycast(origin, direction, out RaycastHit hit, maxDistance) || hit.transform == player;
    }

    private void HandleAudioFilter(float clarity)
    {
        _curvedClarity = clarityCurve.Evaluate(clarity);
        //Debug.Log("Curved clarity: " + curvedClarity);
        _targetCutoff = Mathf.Lerp(fullyOccludedCutoff, unoccludedCutoff, _curvedClarity);
        _currentCutoff = Mathf.Lerp(_currentCutoff, _targetCutoff, Time.deltaTime * transitionSpeed);
        _filter.cutoffFrequency = _currentCutoff;
    }

    private void OnDrawGizmos()
    {
        if (!player) return;

        if (_audioSource == null)
            _audioSource = GetComponent<AudioSource>();

        if (!Application.isPlaying) return;

        // Rays
        Gizmos.color = rayColor;
        Gizmos.DrawLine(transform.position, player.position);

        // Audio Source
        Gizmos.color = audioSourceColor;
        Gizmos.DrawWireSphere(transform.position, _audioSource.maxDistance); // MAX DISTANCE

        if (IsInsideRange())
        {
            for (int i = 0; i < rayCount; i++)
            {
                Gizmos.color = rayColor;
                Gizmos.DrawLine(transform.position, transform.position + GetQuaternionRayDirection(i, rayCount) * maxDistance);
            }
        }
    }
}