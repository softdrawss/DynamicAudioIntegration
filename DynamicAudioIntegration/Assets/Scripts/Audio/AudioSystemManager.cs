using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UIElements;

public class AudioSystemManager : MonoBehaviour
{
    public static AudioSystemManager Instance { get; private set; }

    // Player
    public Transform playerTransform;

    // Audio Planes
    private AzimuthPlane _azimuthPlane;
    private ZenithPlane _zenithPlane;
    
    // Audio Source Systems
    public AudioSourceSystem[] audioSourceSystems;

    // Snapshot Bounds
    public float horizontalBound;
    public float verticalBound;

    // Snaphots
    public AudioMixerSnapshot smallSmallSnapshot;     //Small width + small height
    public AudioMixerSnapshot largeLargeSnapshot;        //Large width + large height
    public AudioMixerSnapshot smallLargeSnapshot;    //Small width + large height
    public AudioMixerSnapshot largeSmallSnapshot;       //Large width + small height

    // Reverb Zone
    public AudioReverbFilter _audioReverbFilter;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Debug.LogWarning("Another NumberDisplay already exists. Destroying this.");
            Destroy(gameObject);
            return;
        }

        _azimuthPlane = playerTransform.GetComponent<AzimuthPlane>();
        _zenithPlane = playerTransform.GetComponent<ZenithPlane>();

        foreach (AudioSourceSystem audioSourceSystem in audioSourceSystems)
        {
            audioSourceSystem.player = playerTransform;
        }

        //_audioReverbZone = playerTransform.GetComponent<AudioReverbZone>();
    }

    public void HandleAudioSystem()
    {
        _azimuthPlane.HandlePlane();
        _zenithPlane.HandlePlane();

        foreach (AudioSourceSystem audioSourceSystem in audioSourceSystems) {
            audioSourceSystem.HandleAudioBehaviour();
        }

        HandleAudioEffects();
    }

    private void HandleAudioEffects()
    {
        float azimuth = _azimuthPlane.GetAzimuth();  // Width around player
        float zenith = _zenithPlane.GetZenith();     // Height above player

        // Determine which "space" the player is in
        bool isHorizontalLarge = azimuth > horizontalBound;
        bool isVerticalLarge = zenith > verticalBound;

        // Select appropriate snapshot + reverb based on space type
        if (!isHorizontalLarge && !isVerticalLarge)
        {
            // Small width + small height
            smallSmallSnapshot.TransitionTo(1.0f);
            _audioReverbFilter.reverbPreset = AudioReverbPreset.Bathroom;
            Debug.Log("Small small");
        }
        else if (isHorizontalLarge && isVerticalLarge)
        {
            // Large width + large height
            largeLargeSnapshot.TransitionTo(1.0f);
            _audioReverbFilter.reverbPreset = AudioReverbPreset.Cave;
            Debug.Log("Large Large");
        }
        else if (!isHorizontalLarge && isVerticalLarge)
        {
            // Small width + large height
            smallLargeSnapshot.TransitionTo(1.0f);
            _audioReverbFilter.reverbPreset = AudioReverbPreset.Auditorium;
            Debug.Log("Small large");
        }
        else if (isHorizontalLarge && !isVerticalLarge)
        {
            // Large width + small height
            largeSmallSnapshot.TransitionTo(1.0f);
            _audioReverbFilter.reverbPreset = AudioReverbPreset.ParkingLot;
            Debug.Log("Large small");
        }
    }
}
