using UnityEngine;

public class PosterGrabber : MonoBehaviour
{
    [Header("Poster Setup")]
    public GameObject[] availableObjects; // Drag prefabs here in Inspector
    public float baseScale = 1f;
    public bool singleUse = true;

    [Header("Spawn Settings")]
    public float spawnOffset = 1f; // How far from wall to spawn

    private bool hasBeenUsed = false;
    private Camera playerCamera;

    void Start()
    {
        playerCamera = Camera.main;
    }

    void Update()
    {
        // Check if player clicked on this poster
        if (Input.GetMouseButtonDown(0) && !hasBeenUsed)
        {
            CheckPosterClick();
        }
    }

    void CheckPosterClick()
    {
        Ray ray = playerCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.collider.gameObject == gameObject)
            {
                SpawnObjectFromPoster(hit);
            }
        }
    }

    void SpawnObjectFromPoster(RaycastHit hit)
    {
        // Pick random object (or specific one)
        GameObject objectToSpawn = availableObjects[Random.Range(0, availableObjects.Length)];

        // Calculate spawn position (in front of poster)
        Vector3 spawnPosition = hit.point + hit.normal * spawnOffset;

        // Calculate scale based on viewing distance (Superliminal magic!)
        float viewingDistance = Vector3.Distance(playerCamera.transform.position, hit.point);
        float calculatedScale = baseScale * (viewingDistance / 10f); // Adjust this formula

        // Spawn the object
        GameObject spawnedObject = Instantiate(objectToSpawn, spawnPosition, Quaternion.identity);
        spawnedObject.transform.localScale = Vector3.one * calculatedScale;

        // Add physics components
        if (spawnedObject.GetComponent<Rigidbody>() == null)
            spawnedObject.AddComponent<Rigidbody>();

        if (spawnedObject.GetComponent<Collider>() == null)
            spawnedObject.AddComponent<BoxCollider>();

        // Make it grabbable
        spawnedObject.tag = "Grabbable";

        // Handle single use
        if (singleUse)
        {
            hasBeenUsed = true;
            GetComponent<Renderer>().material.color = Color.gray;

        }

        Debug.Log($"Spawned {objectToSpawn.name} from poster at scale {calculatedScale:F2}");
    }
}