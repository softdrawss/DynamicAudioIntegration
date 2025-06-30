using UnityEngine;
using UnityEditor;
using UnityEngine.Audio;

public class AudioSystemManagerEditorWindow : EditorWindow
{
    private AudioSystemManager audioManager;

    // Foldouts
    private bool _foldoutBounds = true;
    private bool _foldoutSnapshots = true;
    private bool _foldoutReverb = true;
    private bool _foldoutPlanes = true;
    private bool _foldoutAzimuth = true;
    private bool _foldoutZenith = true;
    private bool _showDebugInfo = false;

    // Drop menu for Azimuth
    private string[] azimuthOptions = new string[] { "Minima", "Maxima" };
    private int azimuthOptionIndex = 0;

    // Drop menu for Zenith
    private string[] zenithOptions = new string[] { "Azimuth", "Offset" };
    private int zenithOptionIndex = 0;

    // Edtor variables
    private bool _isAudioManagerSelected;

    [MenuItem("Window/Audio System Manager")]
    public static void ShowWindow()
    {
        GetWindow<AudioSystemManagerEditorWindow>("Audio System Manager", false);
    }

    void OnGUI()
    {
        GUILayout.Space(5);
        EditorGUILayout.LabelField("Audio System Manager", EditorStyles.boldLabel);
        EditorGUILayout.Space();

        if (audioManager == null)
            audioManager = AudioSystemManager.Instance;

        if (audioManager == null)
        {
            audioManager = FindAnyObjectByType<AudioSystemManager>();
            if (audioManager == null)
            {
                EditorGUILayout.HelpBox("There is no Audio System Manager in this scene. Make sure to add the component to the scene.", MessageType.Warning);
                return;
            }
            if (!audioManager.enabled)
            {
                EditorGUILayout.HelpBox("AudioSystemManager is not enabled. Enable it to be able to see this window.", MessageType.Warning);
                return;
            }
        }

        _isAudioManagerSelected = Selection.activeGameObject != null &&
                                      Selection.activeGameObject.GetComponent<AudioSystemManager>() == audioManager;

        if (!_isAudioManagerSelected)
        {
            EditorGUILayout.HelpBox("Select the GameObject with AudioSystemManager to edit settings.", MessageType.Info);
            return;
        }

        // === Snapshot Bounds ===
        _foldoutBounds = EditorGUILayout.BeginFoldoutHeaderGroup(_foldoutBounds, "System Bounds");
        if (_foldoutBounds)
        {
            EditorGUILayout.BeginVertical("box");
            audioManager.horizontalBound = EditorGUILayout.FloatField("Horizontal Bound", audioManager.horizontalBound);
            audioManager.verticalBound = EditorGUILayout.FloatField("Vertical Bound", audioManager.verticalBound);
            EditorGUILayout.EndVertical();
        }
        EditorGUILayout.EndFoldoutHeaderGroup();

        // === Audio Snapshots ===
        EditorGUILayout.Space(20);
        _foldoutSnapshots = EditorGUILayout.BeginFoldoutHeaderGroup(_foldoutSnapshots, "Audio Snapshots");
        if (_foldoutSnapshots)
        {
            EditorGUILayout.BeginVertical("box");

            // Transition Time
            audioManager.transitionTime = EditorGUILayout.FloatField("Transition Time", audioManager.transitionTime);

            // Snapshots
            EditorGUILayout.Space(4);
            audioManager.smallSmallSnapshot = (AudioMixerSnapshot)EditorGUILayout.ObjectField("Small - Small", audioManager.smallSmallSnapshot, typeof(AudioMixerSnapshot), true);
            audioManager.largeLargeSnapshot = (AudioMixerSnapshot)EditorGUILayout.ObjectField("Large - Large", audioManager.largeLargeSnapshot, typeof(AudioMixerSnapshot), true);
            audioManager.smallLargeSnapshot = (AudioMixerSnapshot)EditorGUILayout.ObjectField("Small - Large", audioManager.smallLargeSnapshot, typeof(AudioMixerSnapshot), true);
            audioManager.largeSmallSnapshot = (AudioMixerSnapshot)EditorGUILayout.ObjectField("Large - Small", audioManager.largeSmallSnapshot, typeof(AudioMixerSnapshot), true);
            EditorGUILayout.EndVertical();
        }
        EditorGUILayout.EndFoldoutHeaderGroup();

        // === Reverb ===
        EditorGUILayout.Space(20);
        _foldoutReverb = EditorGUILayout.BeginFoldoutHeaderGroup(_foldoutReverb, "Reverb Filter Presets");
        if (_foldoutReverb)
        {
            EditorGUILayout.BeginVertical("box");
            audioManager.smallSmallReverbPresetFilter = (AudioReverbPreset)EditorGUILayout.EnumPopup("Small - Small", audioManager.smallSmallReverbPresetFilter);
            audioManager.largeLargeReverbPresetFilter = (AudioReverbPreset)EditorGUILayout.EnumPopup("Large - Large", audioManager.largeLargeReverbPresetFilter);
            audioManager.smallLargeReverbPresetFilter = (AudioReverbPreset)EditorGUILayout.EnumPopup("Small - Large", audioManager.smallLargeReverbPresetFilter);
            audioManager.largeSmallReverbPresetFilter = (AudioReverbPreset)EditorGUILayout.EnumPopup("Large - Small", audioManager.largeSmallReverbPresetFilter);
            EditorGUILayout.EndVertical();
        }
        EditorGUILayout.EndFoldoutHeaderGroup();

        // === Plane Settings ===
        EditorGUILayout.Space(20);
        _foldoutPlanes = EditorGUILayout.BeginFoldoutHeaderGroup(_foldoutPlanes, "Audio Plane Settings");
        if (_foldoutPlanes)
        {
            EditorGUILayout.BeginVertical("box");
            GUILayout.Label("Audio Player", EditorStyles.boldLabel);
            audioManager.audioPlayer.useAzimuthPlane = EditorGUILayout.Toggle("Use Horitzontal Plane", audioManager.audioPlayer.useAzimuthPlane);
            audioManager.audioPlayer.useZenithPlane = EditorGUILayout.Toggle("Use Vertical Plane", audioManager.audioPlayer.useZenithPlane);
            
            EditorGUILayout.Space(10);
            _foldoutAzimuth = EditorGUILayout.Foldout(_foldoutAzimuth, "Horitzontal / Azimuth Plane", true);
            if (_foldoutAzimuth && audioManager._azimuthPlane != null && audioManager.audioPlayer.useAzimuthPlane)
            {
                GUILayout.Label("Colors", EditorStyles.boldLabel);
                audioManager._azimuthPlane.rayColor = EditorGUILayout.ColorField("Ray Color", audioManager._azimuthPlane.rayColor);
                audioManager._azimuthPlane.hitColor = EditorGUILayout.ColorField("Hit Color", audioManager._azimuthPlane.hitColor);

                GUILayout.Label("Steps and rays", EditorStyles.boldLabel);
                audioManager._azimuthPlane.steps = EditorGUILayout.IntField("Number of steps", audioManager._azimuthPlane.steps);
                audioManager._azimuthPlane.rays = EditorGUILayout.IntField("Number of rays", audioManager._azimuthPlane.rays);
                audioManager._azimuthPlane.maxDistance = EditorGUILayout.FloatField("Maximum Distance", audioManager._azimuthPlane.maxDistance);

                GUILayout.Label("Azimuth", EditorStyles.boldLabel);
                audioManager._azimuthPlane.circleColor = EditorGUILayout.ColorField("Circle Color", audioManager._azimuthPlane.circleColor);

                // Dropdown menu
                azimuthOptionIndex = audioManager._azimuthPlane.maximaAzimuth ? 1 : 0;
                azimuthOptionIndex = EditorGUILayout.Popup("Estimation", azimuthOptionIndex, azimuthOptions);
            }

            EditorGUILayout.Space(10);
            _foldoutZenith = EditorGUILayout.Foldout(_foldoutAzimuth, "Vertical / Zenith Plane", true);
            if (_foldoutZenith && audioManager._zenithPlane != null && audioManager.audioPlayer.useZenithPlane)
            {
                GUILayout.Label("Colors", EditorStyles.boldLabel);
                audioManager._zenithPlane.rayColor = EditorGUILayout.ColorField("Ray Color", audioManager._zenithPlane.rayColor);
                audioManager._zenithPlane.hitColor = EditorGUILayout.ColorField("Hit Color", audioManager._zenithPlane.hitColor);

                GUILayout.Label("Steps and rays", EditorStyles.boldLabel);
                audioManager._zenithPlane.steps = EditorGUILayout.IntField("Number of steps", audioManager._zenithPlane.steps);
                audioManager._zenithPlane.rays = EditorGUILayout.IntField("Number of rays", audioManager._zenithPlane.rays);
                audioManager._zenithPlane.maxDistance = EditorGUILayout.FloatField("Maximum Distance", audioManager._zenithPlane.maxDistance);

                GUILayout.Label("Zenith", EditorStyles.boldLabel);
                audioManager._zenithPlane.heightColor = EditorGUILayout.ColorField("Height Color", audioManager._zenithPlane.heightColor);
                
                // Dropdown menu
                zenithOptionIndex = audioManager._zenithPlane.useAzimuthRadius ? 0 : 1;
                zenithOptionIndex = EditorGUILayout.Popup("Ray cast from player", zenithOptionIndex, zenithOptions);
                audioManager._zenithPlane.useAzimuthRadius = (zenithOptionIndex == 0);

                if (zenithOptionIndex == 1)
                    audioManager._zenithPlane.offset = EditorGUILayout.FloatField("Offset", audioManager._zenithPlane.offset);
            }

            EditorGUILayout.EndVertical();
        }
        EditorGUILayout.EndFoldoutHeaderGroup();

        // === Debug Info ===
        GUILayout.Space(20);
        _showDebugInfo = EditorGUILayout.BeginFoldoutHeaderGroup(_showDebugInfo, "Show Debug Information");
        if (_showDebugInfo && audioManager.audioPlayer != null)
        {
            EditorGUILayout.BeginVertical("box");
            GUILayout.Label("Live Runtime Data", EditorStyles.boldLabel);

            EditorGUILayout.LabelField("Azimuth", $"{audioManager._azimuthPlane?.GetAzimuth():F2}");
            EditorGUILayout.LabelField("Zenith", $"{audioManager._zenithPlane?.GetZenith():F2}");
            EditorGUILayout.EndVertical();
        }

        EditorGUILayout.Space();

        if (GUI.changed)
        {
            EditorUtility.SetDirty(audioManager);
        }
    }

    private void OnEnable()
    {
        EditorApplication.update += RepaintIfPlaying;
    }

    private void OnDisable()
    {
        EditorApplication.update -= RepaintIfPlaying;
    }

    private void RepaintIfPlaying()
    {
        if (EditorApplication.isPlaying && this != null && _showDebugInfo)
            Repaint();
    }
}
