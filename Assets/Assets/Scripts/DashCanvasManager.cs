using UnityEngine;
using System.Collections;

public class DashCanvasManager : MonoBehaviour
{
    [Header("Canvas & Panels")]
    [Tooltip("The parent GameObject that holds all these UI elements.")]
    public GameObject dashCanvas;

    public GameObject welcomePage;
    public GameObject mainMenuPanel;
    public GameObject controlsPanel;
    public GameObject instructionsPanel;

    [Header("Settings")]
    [Tooltip("How long the welcome page stays on screen before showing the menu.")]
    public float welcomeDelay = 4.5f;

    private void Start()
    {
        // 1. Ensure the main canvas is active when the game starts
        if (dashCanvas != null) dashCanvas.SetActive(true);

        // 2. Hide everything first so we have a clean slate
        HideAllPanels();

        // 3. Show the Welcome Page and start the countdown timer
        if (welcomePage != null)
        {
            welcomePage.SetActive(true);
            StartCoroutine(WelcomeTimerRoutine());
        }
    }

    /// <summary>
    /// Waits for 4-5 seconds, then hides the Welcome page and shows the Main Menu.
    /// </summary>
    private IEnumerator WelcomeTimerRoutine()
    {
        yield return new WaitForSeconds(welcomeDelay);

        welcomePage.SetActive(false);
        mainMenuPanel.SetActive(true);
    }

    // ==========================================
    // BUTTON CLICK METHODS
    // ==========================================

    public void OnStartButtonClicked()
    {
        // Hide the entire Dash Canvas
        dashCanvas.SetActive(false);

        // TODO: If you have a separate GameManager script, tell it the game has started here!
        Debug.Log("Gameplay has started!");
    }

    public void OnControlsButtonClicked()
    {
        mainMenuPanel.SetActive(false);
        controlsPanel.SetActive(true);
    }

    public void OnInstructionsButtonClicked()
    {
        mainMenuPanel.SetActive(false);
        instructionsPanel.SetActive(true);
    }

    /// <summary>
    /// Use this same method for BOTH the Controls Back Button and Instructions Back Button.
    /// </summary>
    public void OnBackButtonClicked()
    {
        HideAllPanels();
        mainMenuPanel.SetActive(true);
    }

    // ==========================================
    // MID-GAMEPLAY METHODS
    // ==========================================

    /// <summary>
    /// Call this from your in-game UI button to reopen the menu.
    /// </summary>
    public void ReopenDashCanvas()
    {
        // 1. Turn the canvas back on
        dashCanvas.SetActive(true);

        // 2. Hide all panels to ensure no overlapping UI
        HideAllPanels();

        // 3. Directly open the Main Menu (skipping the Welcome Page entirely)
        mainMenuPanel.SetActive(true);
    }

    // ==========================================
    // HELPER METHODS
    // ==========================================

    private void HideAllPanels()
    {
        if (welcomePage != null) welcomePage.SetActive(false);
        if (mainMenuPanel != null) mainMenuPanel.SetActive(false);
        if (controlsPanel != null) controlsPanel.SetActive(false);
        if (instructionsPanel != null) instructionsPanel.SetActive(false);
    }
}