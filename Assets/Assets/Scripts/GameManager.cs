using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameLoopManager : MonoBehaviour
{
    [Header("Game Settings")]
    public float startingTime = 360f;
    public int itemsNeededToWin = 4;
    public float correctItemBonus = 10f;
    public float wrongItemPenalty = 5f;

    [Header("UI References")]
    public Image timeBarFill;
    public TMP_Text timeText;
    public Image taskBarFill;
    public TMP_Text taskText;

    [Header("End Game UI")]
    [Tooltip("Drag your new UI Button GameObject here.")]
    public GameObject escapeButton;
    [Tooltip("Drag a TextMeshPro element here to show WIN/LOSS messages.")]
    public TMP_Text statusText;

    // Internal tracking variables
    private float currentTime;
    private int itemsDeposited = 0;
    private bool isGameActive = true;
    private bool canEscape = false; // Tracks if the player unlocked the button

    private void Start()
    {
        currentTime = startingTime;

        // 1. Hide the button and status text when the game starts
        if (escapeButton != null) escapeButton.SetActive(false);
        if (statusText != null) statusText.gameObject.SetActive(false);

        UpdateTaskUI();
    }

    private void Update()
    {
        if (!isGameActive) return; // Stop counting if game is over

        currentTime -= Time.deltaTime;

        // 2. Check for Loss (Time runs out before escaping)
        if (currentTime <= 0)
        {
            currentTime = 0;
            LoseGame();
        }

        UpdateTimeUI();
    }

    public void CorrectItemDeposited()
    {
        if (!isGameActive) return;

        itemsDeposited++;
        currentTime += correctItemBonus;

        UpdateTaskUI();

        // 3. Check if we have enough items to reveal the button!
        if (itemsDeposited >= itemsNeededToWin && !canEscape)
        {
            RevealEscapeButton();
        }
    }

    public void WrongItemDeposited()
    {
        if (!isGameActive) return;
        currentTime -= wrongItemPenalty;
    }

    private void UpdateTimeUI()
    {
        int minutes = Mathf.FloorToInt(currentTime / 60);
        int seconds = Mathf.FloorToInt(currentTime % 60);
        timeText.text = string.Format("Time: {0:00}:{1:00}", minutes, seconds);

        timeBarFill.fillAmount = currentTime / startingTime;

        if (currentTime < 30f) timeBarFill.color = Color.red;
        else if (currentTime < 60f) timeBarFill.color = Color.yellow;
        else timeBarFill.color = Color.green;
    }

    private void UpdateTaskUI()
    {
        taskText.text = "Items Collected: " + itemsDeposited + " / " + itemsNeededToWin;
        taskBarFill.fillAmount = (float)itemsDeposited / (float)itemsNeededToWin;
    }

    // --- NEW END GAME LOGIC ---

    private void RevealEscapeButton()
    {
        canEscape = true;
        // Turn the button ON so the player can see and click it
        if (escapeButton != null) escapeButton.SetActive(true);
    }

    /// <summary>
    /// This function must be PUBLIC so the UI Button can call it when clicked!
    /// </summary>
    public void ClickEscapeButton()
    {
        // Only allow winning if the button was unlocked and the game isn't over
        if (canEscape && isGameActive)
        {
            WinGame();
        }
    }

    private void WinGame()
    {
        isGameActive = false; // Stops the timer

        if (escapeButton != null) escapeButton.SetActive(false); // Hide the button

        if (statusText != null)
        {
            statusText.gameObject.SetActive(true);
            statusText.text = "<color=green>VICTORY!</color>"; // Show Win text
        }
    }

    private void LoseGame()
    {
        isGameActive = false; // Stops the timer

        if (escapeButton != null) escapeButton.SetActive(false); // Hide button if it was showing

        if (statusText != null)
        {
            statusText.gameObject.SetActive(true);
            statusText.text = "<color=red>YOU LOST!</color>"; // Show Lose text
        }
    }
}