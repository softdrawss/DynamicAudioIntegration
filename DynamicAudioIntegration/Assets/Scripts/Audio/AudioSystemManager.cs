using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UIElements;

public class AudioSystemManager : MonoBehaviour
{
    public static AudioSystemManager Instance { get; private set; }

    // Player
    public Transform playerTransform;

    // Audio Planes
    public AzimuthPlane _azimuthPlane;
    public ZenithPlane _zenithPlane;
    
    // Audio Source Systems
    public AudioSourceSystem[] audioSourceSystems;

    // Snapshot Bounds
    public float horizontalBound;
    public float verticalBound;

    // Snaphots
    public AudioMixerSnapshot smallSmallSnapshot;       //Small width + small height
    public AudioMixerSnapshot largeLargeSnapshot;        //Large width + large height
    public AudioMixerSnapshot smallLargeSnapshot;       //Small width + large height
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
            if (audioSourceSystem != null)
                audioSourceSystem.HandleAudioBehaviour();
        }

        HandleAudioEffects();
    }

    private void HandleAudioEffects()
    {
        // Determine which "space" the player is in
        bool isHorizontalLarge = _azimuthPlane.GetAzimuth() > horizontalBound;
        bool isVerticalLarge = _zenithPlane.GetZenith() > verticalBound;

        // Select appropriate snapshot + reverb based on space type
        if (!isHorizontalLarge && !isVerticalLarge)
        {
            // Small width + small height
            smallSmallSnapshot.TransitionTo(0.2f);

            _audioReverbFilter.reverbPreset = AudioReverbPreset.SewerPipe;
            //_audioReverbFilter.decayTime = 0.5f;
            //_audioReverbFilter.reflectionsLevel = -200f;
            //_audioReverbFilter.reverbLevel = -1000f;
            //_audioReverbFilter.roomHF = -100f;
            //_audioReverbFilter.diffusion = 0.8f;
            //_audioReverbFilter.density = 1.0f;

            Debug.Log("Small small");
        }
        else if (isHorizontalLarge && isVerticalLarge)
        {
            // Large width + large height
            largeLargeSnapshot.TransitionTo(0.2f);

            _audioReverbFilter.reverbPreset = AudioReverbPreset.PaddedCell;
            //_audioReverbFilter.decayTime = 3.25f;
            //_audioReverbFilter.reflectionsLevel = -1500f;
            //_audioReverbFilter.reverbLevel = -1500f;
            //_audioReverbFilter.roomHF = -4000f;
            //_audioReverbFilter.diffusion = 0.7f;
            //_audioReverbFilter.density = 0.5f;

            Debug.Log("Large Large");
        }
        else if (!isHorizontalLarge && isVerticalLarge)
        {
            // Small width + large height
            smallLargeSnapshot.TransitionTo(0.2f);

            _audioReverbFilter.reverbPreset = AudioReverbPreset.Room;
            //_audioReverbFilter.decayTime = 1.85f;
            //_audioReverbFilter.reflectionsLevel = -500f;
            //_audioReverbFilter.reverbLevel = -700f;
            //_audioReverbFilter.roomHF = -1000f;
            //_audioReverbFilter.diffusion = 0.8f;
            //_audioReverbFilter.density = 0.8f;

            Debug.Log("Small large");
        }
        else if (isHorizontalLarge && !isVerticalLarge)
        {
            // Large width + small height
            largeSmallSnapshot.TransitionTo(0.2f);

            _audioReverbFilter.reverbPreset = AudioReverbPreset.Livingroom;
            //_audioReverbFilter.decayTime = 2.15f;
            //_audioReverbFilter.reflectionsLevel = -800f;
            //_audioReverbFilter.reverbLevel = -1000f;
            //_audioReverbFilter.roomHF = -2000f;
            //_audioReverbFilter.diffusion = 0.6f;
            //_audioReverbFilter.density = 0.7f;
            Debug.Log("Large small");
        }
    }
}
