using UnityEngine;

[CreateAssetMenu(fileName = "NumbersDatabase", menuName = "UI/NumbersBase")]
public class NumbersBase : ScriptableObject
{
    public GameObject[] numberPrefabs; // 0-9 in order
}
