using UnityEngine;

public class TrompeLoeilObject : MonoBehaviour
{
    [Header("Setup")]
    public GameObject realObjectPrefab; // What spawns when realized
    public float perfectDistance = 5f; // Sweet spot distance
    public float distanceTolerance = 1f;
    public float angleTolerance = 10f;

    [Header("Debug")]
    public bool showDebugInfo = true;

    private Camera playerCamera;
    private bool hasBeenRealized = false;
    private Renderer fakeRenderer;

    void Start()
    {
        playerCamera = Camera.main;
        fakeRenderer = GetComponent<Renderer>();
    }

    void Update()
    {
        if (!hasBeenRealized && Input.GetMouseButtonDown(0))
        {
            CheckIfCanRealize();
        }

        if (showDebugInfo)
            ShowDebugInfo();
    }

    void CheckIfCanRealize()
    {
        // Check if player is looking directly at this object
        Ray ray = playerCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.collider.gameObject == gameObject)
            {
                float distance = Vector3.Distance(playerCamera.transform.position, transform.position);

                // Check if at perfect distance
                if (Mathf.Abs(distance - perfectDistance) <= distanceTolerance)
                {
                    RealizeObject(distance);
                }
                else
                {
                    Debug.Log($"Wrong distance! Need {perfectDistance}, you're at {distance:F1}");
                }
            }
        }
    }

    void RealizeObject(float viewDistance)
    {
        hasBeenRealized = true;

        // Spawn real object at same position
        GameObject realObject = Instantiate(realObjectPrefab, transform.position, transform.rotation);

        // Scale based on viewing distance (key Superliminal mechanic!)
        float scale = viewDistance / perfectDistance;
        realObject.transform.localScale = Vector3.one * scale;

        // Add components if missing
        if (realObject.GetComponent<Rigidbody>() == null)
            realObject.AddComponent<Rigidbody>();

        if (realObject.GetComponent<Collider>() == null)
            realObject.AddComponent<BoxCollider>();

        // Tag it as grabbable
        realObject.tag = "Grabbable";

        // Hide the fake painted version
        fakeRenderer.enabled = false;

        Debug.Log($"Trompe-l'œil realized! Scale: {scale:F2}");
    }

    void ShowDebugInfo()
    {
        if (playerCamera != null)
        {
            float distance = Vector3.Distance(playerCamera.transform.position, transform.position);
            Debug.Log($"Distance to {gameObject.name}: {distance:F1} (need {perfectDistance})");
        }
    }
}