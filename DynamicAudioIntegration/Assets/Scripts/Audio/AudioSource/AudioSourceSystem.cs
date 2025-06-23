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

    // Rays
    public float maxDistance = 20f;
    bool leftClear, rightClear;
    [Header("Ray Cone Settings")]
    public int rayCount = 2; // Number of rays in the cone
    public float coneAngle = 45f; // Angle in degrees

    // Cutoff Frequencies
    [Header("Cutoff Frequencies")]
    public float unoccludedCutoff = 22000f;
    public float fullyOccludedCutoff = 800f;
    public float transitionSpeed = 5f;
    private float _currentCutoff;

    // Clarity
    private float _clarityRatio;

    // Audio Source
    private AudioSource _audioSource;

    // Audio Low Pass Filter
    private AudioLowPassFilter _filter;

    [Header("Clarity Mapping")]
    public AnimationCurve clarityCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    void Start()
    {
        if (!player) return;

        _audioSource = GetComponent<AudioSource>();

        _filter = GetComponent<AudioLowPassFilter>();
        _currentCutoff = unoccludedCutoff;
    }

    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.G))
        //{
        //    if (IsInsideRange())
        //        HandleAudioBehaviour();
        //    else
        //        Debug.Log("Player is out of range, need to be closer to the audio source!");
        //}
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
        else
            Debug.Log("Player is out of range, need to be closer to the audio source!");
    }

    // Need to change behavviour so it checks materials
    private void CheckSurroundings()
    {
        //leftClear = IsPathClear(transform.position + Vector3.left * xOffset);
        //rightClear = IsPathClear(transform.position + Vector3.right * xOffset);
        //
        //// CLEAR: Cutoff normal
        //// Occluded: Calculated level
        //// Obstructed: ?
        //
        //if (leftClear && rightClear)
        //    Debug.Log("Sound is CLEAR");
        //else if (!leftClear && !rightClear)
        //    Debug.Log("Sound is OCCLUDED");
        //else
        //    Debug.Log("Sound is OBSTRUCTED");

        Vector3 directionToPlayer = (player.position - transform.position).normalized;

        int occludedRays = 0;
        float totalOcclusion = 0f;

        for (int i = 0; i < rayCount; i++)
        {
            float angle = Mathf.Lerp(-coneAngle / 2, coneAngle / 2, (float)i / (rayCount - 1));
            Quaternion offsetRotation = Quaternion.AngleAxis(angle, Vector3.up);
            Vector3 rayDirection = offsetRotation * directionToPlayer;

            if (Physics.Raycast(transform.position, rayDirection, out RaycastHit hit, maxDistance) && hit.transform != player)
            {
                AudioMaterialComponent audioMaterial = hit.collider.GetComponent <AudioMaterialComponent>();
                float occlusion = audioMaterial != null ? Mathf.Clamp01(audioMaterial.material.audioOclusion) : 0.0f; // Default fallback
                Debug.Log("Occlusion: " + occlusion);
                totalOcclusion += occlusion;
                occludedRays++;
            }

            //Debug.DrawRay(transform.position, rayDirection * maxDistance, Color.yellow, 1f); // Optional debug
        }

        if (occludedRays > 0)
            _clarityRatio = 1 - (totalOcclusion / occludedRays);
        else
            _clarityRatio = 1f;
        
        Debug.Log("Clarity ratio: " + _clarityRatio);
        HandleAudioFilter(_clarityRatio);
    }

    private bool IsRayClear(Vector3 origin, Vector3 direction)
    {
        return !Physics.Raycast(origin, direction, out RaycastHit hit, maxDistance) || hit.transform == player;
    }

    private void HandleAudioFilter(float clarity)
    {
        float curvedClarity = clarityCurve.Evaluate(clarity);
        //Debug.Log("Curved clarity: " + curvedClarity);
        float targetCutoff = Mathf.Lerp(fullyOccludedCutoff, unoccludedCutoff, curvedClarity);
        //_currentCutoff = targetCutoff; // DEBBUG
        _currentCutoff = Mathf.Lerp(_currentCutoff, targetCutoff, Time.deltaTime * transitionSpeed);
        _filter.cutoffFrequency = _currentCutoff;
        //_audioSource.volume = 
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
        //Gizmos.DrawLine(transform.position + Vector3.left * xOffset, player.position);
        //Gizmos.DrawLine(transform.position + Vector3.right * xOffset, player.position);

        // Audio Source
        // Could change it to circle
        Gizmos.color = audioSourceColor;
        Gizmos.DrawWireSphere(transform.position, _audioSource.maxDistance); // MAX DISTANCE
        Gizmos.DrawWireSphere(transform.position, _audioSource.minDistance); // MIN DISTANCE

        if (IsInsideRange())
        {
            Vector3 directionToPlayer = (player.position - transform.position).normalized;

            for (int i = 0; i < rayCount; i++)
            {
                float angle = Mathf.Lerp(-coneAngle / 2, coneAngle / 2, (float)i / (rayCount - 1));
                Quaternion offsetRotation = Quaternion.AngleAxis(angle, Vector3.up);
                Vector3 rayDirection = offsetRotation * directionToPlayer;

                Gizmos.color = rayColor;
                Gizmos.DrawLine(transform.position, transform.position + rayDirection * maxDistance);
            }
        }
    }
}