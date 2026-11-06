using UnityEngine;

public class ModelSpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    [Tooltip("The empty GameObject where models should appear.")]
    public Transform spawnPoint;

    [Tooltip("Drop your 50 3D Model Prefabs here.")]
    public GameObject[] modelPrefabs;

    /// <summary>
    /// Spawns a model based on its number in the list (0 to 49)
    /// </summary>
    public void SpawnModel(int modelIndex)
    {
        // 1. Check if the index is valid to prevent errors
        if (modelIndex < 0 || modelIndex >= modelPrefabs.Length)
        {
            Debug.LogWarning("Model index out of range! Check your button settings.");
            return;
        }

        // 2. Simply Instantiate (spawn) a new copy every single time the function is called!
        Instantiate(modelPrefabs[modelIndex], spawnPoint.position, spawnPoint.rotation);
    }
}