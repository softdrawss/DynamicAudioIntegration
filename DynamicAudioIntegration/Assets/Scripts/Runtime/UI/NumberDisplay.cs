using UnityEngine;

public class NumberDisplay : MonoBehaviour
{
    public static NumberDisplay Instance { get; private set; }
    public NumbersBase numbersDatabase;

    private Transform[] digitSlots;
    private int numberOfDigits = 1, digit;
    public int currentDigit = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("Another NumberDisplay already exists. Destroying this.");
            Destroy(gameObject);
            return;
        }

        // Prepare slots
        digitSlots = new Transform[numberOfDigits];

        for (int i = 0; i < numberOfDigits; i++)
        {
            Transform slot = new GameObject("Digit_" + i).transform;
            slot.SetParent(this.transform);
            slot.localPosition = new Vector3(i * 1.0f, 0, 0);
            digitSlots[i] = slot;
        }
    }

    private void Start()
    {
        UpdateDigits(currentDigit);
    }

    private void UpdateDigits(int number)
    {
        string numberString = number.ToString().PadLeft(digitSlots.Length, '0');

        for (int i = 0; i < numberString.Length; i++)
        {
            digit = int.Parse(numberString[i].ToString());

            // Destroy previous
            for (int j = 0; j < digitSlots[i].childCount; j++)
            {
                Destroy(digitSlots[i].GetChild(j).gameObject);
            }
            // Spawn new
            Instantiate(numbersDatabase.numberPrefabs[digit],
                        digitSlots[i].position,
                        digitSlots[i].rotation,
                        digitSlots[i]);
        }
    }

    public void Increment()
    {
        currentDigit++;
        UpdateDigits(currentDigit);
    }
}
