using UnityEngine;

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

    // Audio Source
    private AudioSource _audioSource;

    void Start()
    {
        if (!player) return;

        _audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            if (IsInsideRange())
                CheckSurroundings();
            else
                Debug.Log("Player is out of range, need to be closer to the audio source!");
        }
    }

    private bool IsInsideRange() { return Vector3.Distance(transform.position, player.position) < _audioSource.maxDistance; }

    private void CheckSurroundings()
    {
        leftClear = IsPathClear(transform.position + Vector3.left * xOffset);
        rightClear = IsPathClear(transform.position + Vector3.right * xOffset);

        if (leftClear && rightClear)
            Debug.Log("Sound is CLEAR");
        else if (!leftClear && !rightClear)
            Debug.Log("Sound is OCCLUDED");
        else
            Debug.Log("Sound is OBSTRUCTED");
    }

    private bool IsPathClear(Vector3 origin) { return !Physics.Linecast(origin, player.position, out RaycastHit hit) || hit.transform == player; }

    private void OnDrawGizmos()
    {
        if (!player) return;

        if (_audioSource == null)
            _audioSource = GetComponent<AudioSource>();

        // Rays
        Gizmos.color = rayColor;
        Gizmos.DrawLine(transform.position + Vector3.left * xOffset, player.position);
        Gizmos.DrawLine(transform.position + Vector3.right * xOffset, player.position);

        // Audio Source
        Gizmos.color = audioSourceColor;
        Gizmos.DrawWireSphere(transform.position, _audioSource.maxDistance);
    }
}