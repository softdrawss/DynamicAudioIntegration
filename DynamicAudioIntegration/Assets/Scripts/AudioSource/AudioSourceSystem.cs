using UnityEngine;

public class AudioSourceSystem : MonoBehaviour
{
    // Colors
    public Color frustumColor = Color.white;

    // Offset
    public float xOffset = 5f;
    public float yOffset;

    // Player 
    public Transform player;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnDrawGizmos()
    {
        //if (!Application.isPlaying) return;

        Gizmos.color = frustumColor;
        Gizmos.DrawLine(transform.position + Vector3.left * xOffset, player.position);
        Gizmos.DrawLine(transform.position + Vector3.left * -xOffset, player.position);
    }
}
