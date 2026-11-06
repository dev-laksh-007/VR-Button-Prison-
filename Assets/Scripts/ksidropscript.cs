using System.Collections;
using UnityEngine;

public class DiscoSequenceController : MonoBehaviour
{
    [Header("Assigned Objects")]
    public Transform discoBall;
    public Light discoLight;
    public Transform personModel;
    public Rigidbody personRigidbody;
    public AudioSource backgroundMusic;

    [Header("Sequence Settings")]
    public float slowDropSpeed = 1.5f;
    public float spinSpeed = 60f;
    public float dropDistance = 5f; // How far it moves down before dropping
    public float flickerSpeed = 0.15f;

    [Header("Neon/Disco Colors")]
    public Color[] lightColors; // Add vibrant colors in the Inspector (Cyan, Magenta, etc.)

    private float startingYPosition;
    private bool halfwayReached = false;

    void Start()
    {
        // Record where the object spawned to calculate the halfway drop point
        startingYPosition = transform.position.y;

        // Ensure the person doesn't fall immediately
        if (personRigidbody != null)
        {
            personRigidbody.isKinematic = true;
        }

        // Start music and light flickering
        if (backgroundMusic != null) backgroundMusic.Play();
        StartCoroutine(FlickerLights());
    }

    void Update()
    {
        // If we haven't reached the halfway point yet, do the scripted movement
        if (!halfwayReached)
        {
            // Move the entire root object down slowly
            transform.Translate(Vector3.down * slowDropSpeed * Time.deltaTime, Space.World);

            // Spin the disco ball and the person
            if (discoBall != null) discoBall.Rotate(Vector3.up * spinSpeed * Time.deltaTime);
            if (personModel != null) personModel.Rotate(Vector3.up * (spinSpeed / 2f) * Time.deltaTime); // Person spins a bit slower

            // Check if we have moved down far enough to trigger the drop
            if (startingYPosition - transform.position.y >= dropDistance)
            {
                TriggerHalfwayDrop();
            }
        }
    }

    void TriggerHalfwayDrop()
    {
        halfwayReached = true;

        // Make the disco ball and light disappear
        if (discoBall != null) discoBall.gameObject.SetActive(false);

        // Detach the person from the root so its physics scale properly
        if (personModel != null)
        {
            personModel.SetParent(null);

            // Re-enable gravity for the person!
            if (personRigidbody != null)
            {
                personRigidbody.isKinematic = false;

                // Optional: Preserve the spinning momentum as it falls
                personRigidbody.angularVelocity = new Vector3(0, spinSpeed * Mathf.Deg2Rad, 0);
            }
        }

        // Cleanup the root object after 30 seconds so it doesn't clutter the scene
        Destroy(gameObject, 30f);

        // Also destroy the person model eventually if needed
        if (personModel != null) Destroy(personModel.gameObject, 30f);
    }

    IEnumerator FlickerLights()
    {
        // Keep flickering until we hit the halfway mark
        while (!halfwayReached && discoLight != null && lightColors.Length > 0)
        {
            // Pick a random color from the array
            discoLight.color = lightColors[Random.Range(0, lightColors.Length)];
            yield return new WaitForSeconds(flickerSpeed);
        }
    }
}