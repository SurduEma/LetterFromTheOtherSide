using UnityEngine;

public class ObjectGrabber : MonoBehaviour
{
    public Camera playerCamera;
    public float holdDistance = 2f;
    public LayerMask grabLayer;
    public float scaleMultiplier = 1.5f;
    public float minScale = 0.3f;
    public float maxScale = 6f;

    private GameObject heldObject;
    private Rigidbody heldRigidbody;
    private float grabStartDistance;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (heldObject == null)
                TryGrabObject();
            else
                DropObject();
        }

        if (heldObject != null)
        {
            ResizeAndHoldObject();
        }
    }

    void TryGrabObject()
    {
        Ray ray = playerCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
        if (Physics.Raycast(ray, out RaycastHit hit, 100f, grabLayer))
        {
            if (hit.collider.CompareTag("Grabbable"))
            {
                heldObject = hit.collider.gameObject;
                grabStartDistance = hit.distance;

                heldRigidbody = heldObject.GetComponent<Rigidbody>();
                if (heldRigidbody != null)
                    heldRigidbody.isKinematic = true;
            }
        }
    }

    void DropObject()
    {
        if (heldRigidbody != null)
            heldRigidbody.isKinematic = false;

        heldObject = null;
        heldRigidbody = null;
    }

    void ResizeAndHoldObject()
    {
        // Get vertical look direction (up/down)
        float cameraPitch = playerCamera.transform.forward.y;
        float t = Mathf.InverseLerp(-1f, 1f, cameraPitch);
        float scaleFactor = Mathf.Lerp(minScale, maxScale, t);

        // Apply scale FIRST
        heldObject.transform.localScale = Vector3.one * scaleFactor;

        // Then position it at a safe distance based on its size
        float safeDistance = holdDistance + scaleFactor * 0.5f; // offset to avoid clipping
        Vector3 holdPosition = playerCamera.transform.position + playerCamera.transform.forward * safeDistance;
        heldObject.transform.position = holdPosition;
    }


}
