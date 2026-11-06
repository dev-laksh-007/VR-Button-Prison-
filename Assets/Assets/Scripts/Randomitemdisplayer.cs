using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class RandomItemDisplay : MonoBehaviour
{
    [Header("References")]
    public ModelSpawner spawnerManager;
    public TMP_Text[] screenTexts;

    private void Start()
    {
        UpdateScreens();
    }

    public void UpdateScreens()
    {
        if (spawnerManager == null || spawnerManager.modelPrefabs.Length == 0) return;

        List<int> availableIndices = new List<int>();
        for (int i = 0; i < spawnerManager.modelPrefabs.Length; i++)
        {
            availableIndices.Add(i);
        }

        for (int i = 0; i < screenTexts.Length; i++)
        {
            if (availableIndices.Count == 0) break;

            int randomListPosition = Random.Range(0, availableIndices.Count);
            int chosenModelIndex = availableIndices[randomListPosition];
            availableIndices.RemoveAt(randomListPosition);

            string itemName = spawnerManager.modelPrefabs[chosenModelIndex].name;

            // Set the text and ensure it starts as standard white/black
            screenTexts[i].text = itemName;
            screenTexts[i].color = Color.white;
        }
    }

    /// <summary>
    /// Checks if the item is on the screen. If it is, turns it green and returns true.
    /// </summary>
    public bool CheckAndCrossOffItem(string itemName)
    {
        for (int i = 0; i < screenTexts.Length; i++)
        {
            // If the name matches AND it isn't already green (meaning it hasn't been collected yet)
            if (screenTexts[i].text == itemName && screenTexts[i].color != Color.green)
            {
                // Turn the UI text green
                screenTexts[i].color = Color.green;
                return true;
            }
        }

        // If we finish the loop and didn't find a match, return false
        return false;
    }
}