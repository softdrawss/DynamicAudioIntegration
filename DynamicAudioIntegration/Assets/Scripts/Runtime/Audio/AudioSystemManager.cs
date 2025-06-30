using UnityEngine;
using UnityEngine.Audio;

public class AudioSystemManager : MonoBehaviour
{
    [HideInInspector] public static AudioSystemManager Instance { get; private set; }

    // Player
    public AudioPlayer audioPlayer;

    // Reverb Zone
    public AudioReverbFilter _audioReverbFilter;

    // Audio Planes
    [HideInInspector] public AzimuthPlane _azimuthPlane;
    [HideInInspector] public ZenithPlane _zenithPlane;

    // Audio Source Systems
    [HideInInspector] public AudioSourceSystem[] audioSourceSystems;

    // Snapshot Bounds
    [HideInInspector] public float horizontalBound;
    [HideInInspector] public float verticalBound;
    private bool _isHorizontalLarge;
    private bool _isVerticalLarge;
    private bool _preHorizontal;
    private bool _preVertical;

    // Snaphots
    [HideInInspector] public float transitionTime;
    [HideInInspector] public AudioMixerSnapshot smallSmallSnapshot;       //Small width + small height
    [HideInInspector] public AudioMixerSnapshot largeLargeSnapshot;       //Large width + large height
    [HideInInspector] public AudioMixerSnapshot smallLargeSnapshot;       //Small width + large height
    [HideInInspector] public AudioMixerSnapshot largeSmallSnapshot;       //Large width + small height

    // Reverb Filter Presets
    [HideInInspector] public AudioReverbPreset smallSmallReverbPresetFilter;    //Small width + small height
    [HideInInspector] public AudioReverbPreset largeLargeReverbPresetFilter;    //Large width + large height
    [HideInInspector] public AudioReverbPreset smallLargeReverbPresetFilter;    //Small width + large height
    [HideInInspector] public AudioReverbPreset largeSmallReverbPresetFilter;    //Large width + small height

    private void OnValidate()
    {
        if (audioPlayer != null)
        {
            _azimuthPlane = audioPlayer.AzimuthPlane;
            _zenithPlane = audioPlayer.ZenithPlane;
        }
    }

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

        _azimuthPlane = audioPlayer.AzimuthPlane;
        _zenithPlane = audioPlayer.ZenithPlane;

        AudioSourceSystem[] allAudioSources = FindObjectsByType<AudioSourceSystem>(findObjectsInactive: FindObjectsInactive.Exclude, FindObjectsSortMode.None);
        audioSourceSystems = System.Array.FindAll(allAudioSources, source => source.enabled);

        foreach (AudioSourceSystem audioSourceSystem in audioSourceSystems)
        {
            audioSourceSystem.player = audioPlayer.transform;
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
        _isHorizontalLarge = _azimuthPlane.GetAzimuth() > horizontalBound;
        _isVerticalLarge = _zenithPlane.GetZenith() > verticalBound;

        if (_preHorizontal != _isHorizontalLarge || _preVertical != _isVerticalLarge)
        {
            // Select appropriate snapshot + reverb based on space type
            if (!_isHorizontalLarge)
            {
                if (!_isVerticalLarge)
                {
                    // Small width + small height
                    smallSmallSnapshot.TransitionTo(transitionTime);
                    _audioReverbFilter.reverbPreset = smallSmallReverbPresetFilter;

                    //Debug.Log("Small small");
                }
                else
                {
                    // Small width + large height
                    smallLargeSnapshot.TransitionTo(transitionTime);
                    _audioReverbFilter.reverbPreset = smallLargeReverbPresetFilter;

                    //Debug.Log("Small large");
                }
            }
            else
            {
                if (!_isVerticalLarge)
                {
                    // Large width + small height
                    largeSmallSnapshot.TransitionTo(transitionTime);
                    _audioReverbFilter.reverbPreset = largeSmallReverbPresetFilter;

                    //Debug.Log("Large small");
                }
                else
                {
                    // Large width + large height
                    largeLargeSnapshot.TransitionTo(transitionTime);
                    _audioReverbFilter.reverbPreset = largeLargeReverbPresetFilter;

                    //Debug.Log("Large Large");
                }
            }

            _preHorizontal = _isHorizontalLarge;
            _preVertical = _isVerticalLarge;
        }
    }
}
