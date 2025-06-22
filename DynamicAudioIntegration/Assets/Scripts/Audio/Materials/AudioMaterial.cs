using UnityEngine;

[CreateAssetMenu(fileName = "AudioMaterial", menuName = "Scriptable Objects/Audio Material")]
public class AudioMaterial : ScriptableObject
{
    public string material;
    public string description; //Not sure if needed

    [Range(0.0f, 1.0f)]
    public float audioOclusion;
}
