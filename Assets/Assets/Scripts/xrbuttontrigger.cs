using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit; // Required for XR

// This line ensures Unity automatically adds an XR Simple Interactable if it's missing
[RequireComponent(typeof(UnityEngine.XR.Interaction.Toolkit.Interactables.XRSimpleInteractable))]
public class XRButtonTrigger : MonoBehaviour
{
    [Tooltip("Drag the SpawnerManager here.")]
    public ModelSpawner spawnerManager;

    [Tooltip("Which model should this button spawn? (0 to 49)")]
    public int myModelIndex;

    private UnityEngine.XR.Interaction.Toolkit.Interactables.XRSimpleInteractable interactable;

    private void Awake()
    {
        // Get the XR interactable component on this button
        interactable = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRSimpleInteractable>();

        // Subscribe to the Select Entered event via code
        // This listens for when the VR controller interacts with the button
        interactable.selectEntered.AddListener(OnButtonPressed);
    }

    // This function runs when the button is pressed in VR
    private void OnButtonPressed(SelectEnterEventArgs args)
    {
        if (spawnerManager != null)
        {
            spawnerManager.SpawnModel(myModelIndex);
        }
        else
        {
            Debug.LogWarning("Spawner Manager is not assigned on " + gameObject.name);
        }
    }

  /*  private void OnDestroy()
    {
        // Good practice: Always remove listeners when the object is destroyed to prevent memory leaks
        if (interactable != null)
        {
            interactable.selectEntered.RemoveListener(OnButtonPressed);
        }
    } */
}