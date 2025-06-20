using UnityEngine;

public class ObjectGrabber : MonoBehaviour
{
    public Camera playerCamera;
    public float holdDistance = 2f;
    public LayerMask grabLayer;
    public float scaleMultiplier = 1.5f;
    public float minScale = 0.3f;
    public float maxScale = 5f;

    private GameObject heldObject;
    private Rigidbody heldRigidbody;

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
        // 1. Calculate how high you're looking
        float cameraPitch = playerCamera.transform.forward.y; // -1 (looking down) to +1 (looking up)

        // 2. Convert to a 0–1 scale
        float t = Mathf.InverseLerp(-1f, 1f, cameraPitch);

        // 3. Map to scale range
        float scaleFactor = Mathf.Lerp(minScale, maxScale, t);

        // 4. Apply scale
        heldObject.transform.localScale = Vector3.one * scaleFactor;

        // 5. Position it in front of camera at fixed distance
        Vector3 holdPosition = playerCamera.transform.position + playerCamera.transform.forward * (holdDistance + scaleFactor * 0.5f);
        heldObject.transform.position = holdPosition;
    }
}
