using UnityEngine;
using System.Collections;

public class ItemCollector : MonoBehaviour
{

    public GameLoopManager gameManager;
    [Header("System References")]
    public RandomItemDisplay displayManager;

    [Header("Roof Light Settings")]
    public Renderer roofRenderer;
    public string colorPropertyName = "_BaseColor";
    public Color normalColor = Color.white;
    public Color correctColor = Color.green;
    public Color wrongColor = Color.red;
    public float lightDuration = 4.5f;

    [Header("Point Light Settings")]
    [Tooltip("Drag your 4 Point Lights into this list.")]
    public Light[] pointLights; // This holds your 4 Unity Lights

    private Coroutine colorRoutine;

    private void Start()
    {
        // 1. Set the roof to its normal color
        if (roofRenderer != null)
        {
            roofRenderer.material.SetColor(colorPropertyName, normalColor);
        }

        // 2. Make sure all 4 Point Lights start completely deactivated (turned off)
        foreach (Light pLight in pointLights)
        {
            if (pLight != null)
            {
                pLight.enabled = false;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) return;

        GameObject itemRoot = other.gameObject;
        if (other.attachedRigidbody != null)
        {
            itemRoot = other.attachedRigidbody.gameObject;
        }
        else
        {
            itemRoot = other.transform.root.gameObject;
        }

        string cleanItemName = itemRoot.name.Replace("(Clone)", "").Trim();
        bool isCorrectItem = displayManager.CheckAndCrossOffItem(cleanItemName);

        if (isCorrectItem)
        {
            Debug.Log("<color=green>SUCCESS:</color> " + cleanItemName + " was collected!");

            // Tell the Game Manager to add points and time!
            if (gameManager != null) gameManager.CorrectItemDeposited();

            TriggerLights(correctColor);
            Destroy(itemRoot);
        }
        else
        {
            Debug.LogError("ERROR: Wrong item!");

            // Tell the Game Manager to subtract time!
            if (gameManager != null) gameManager.WrongItemDeposited();

            TriggerLights(wrongColor);
            Destroy(itemRoot);
        }
    }

    private void TriggerLights(Color targetColor)
    {
        if (colorRoutine != null)
        {
            StopCoroutine(colorRoutine);
        }

        colorRoutine = StartCoroutine(ColorChangeRoutine(targetColor));
    }

    private IEnumerator ColorChangeRoutine(Color targetColor)
    {
        // --- TURN ON PHASE ---

        // 1. Change Roof Color
        if (roofRenderer != null)
        {
            roofRenderer.material.SetColor(colorPropertyName, targetColor);
        }

        // 2. Turn ON the 4 Point Lights and set their color
        foreach (Light pLight in pointLights)
        {
            if (pLight != null)
            {
                pLight.color = targetColor; // Change to Red or Green
                pLight.enabled = true;      // Turn the light switch ON
            }
        }

        // --- WAIT PHASE ---
        yield return new WaitForSeconds(lightDuration);

        // --- TURN OFF PHASE ---

        // 3. Change Roof Color back to normal
        if (roofRenderer != null)
        {
            roofRenderer.material.SetColor(colorPropertyName, normalColor);
        }

        // 4. Turn OFF the 4 Point Lights again
        foreach (Light pLight in pointLights)
        {
            if (pLight != null)
            {
                pLight.enabled = false; // Turn the light switch OFF
            }
        }
    }
}