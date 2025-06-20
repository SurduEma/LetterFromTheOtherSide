using UnityEngine;

[RequireComponent(typeof(Collider))]
public class RoomTeleporter : MonoBehaviour
{
    [Header("Teleport Settings")]
    public Transform teleportDestination;

    [Header("Debug")]
    public bool showDebugMessages = true;
    public bool drawGizmos = true;

    private Collider _triggerCollider;

    private void Awake()
    {
        _triggerCollider = GetComponent<Collider>();
        if (_triggerCollider == null)
        {
            Debug.LogError("RoomTeleporter requires a Collider component!", this);
            return;
        }

        if (!_triggerCollider.isTrigger)
        {
            Debug.LogWarning("Collider should be marked as IsTrigger for proper teleportation. Fixing automatically.", this);
            _triggerCollider.isTrigger = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (teleportDestination == null)
        {
            Debug.LogError("TeleportDestination is not assigned!", this);
            return;
        }

        if (showDebugMessages)
        {
            Debug.Log($"Teleporter activated by: {other.name} (Tag: {other.tag})", this);
            Debug.Log($"Attempting to teleport to: {teleportDestination.position}", this);
        }

        if (other.CompareTag("Player"))
        {
            TeleportPlayer(other.transform);
        }
        else if (showDebugMessages)
        {
            Debug.Log($"Object {other.name} entered trigger but isn't tagged as Player", this);
        }
    }

    private void TeleportPlayer(Transform playerTransform)
    {
        CharacterController cc = playerTransform.GetComponent<CharacterController>();
        Rigidbody rb = playerTransform.GetComponent<Rigidbody>();
        bool usedCharacterController = false;

        if (cc != null)
        {
            cc.enabled = false;
            playerTransform.position = teleportDestination.position;
            cc.enabled = true;
            usedCharacterController = true;
        }
        else
        {
            if (rb != null)
            {
                rb.linearVelocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }

            playerTransform.position = teleportDestination.position;
        }

        if (showDebugMessages)
        {
            string method = usedCharacterController ? "CharacterController" :
                          (rb != null ? "Rigidbody" : "Transform");
            Debug.Log($"Player teleported using {method} to {teleportDestination.position}", this);
        }
    }

    private void OnDrawGizmos()
    {
        if (!drawGizmos) return;

        Gizmos.color = Color.cyan;
        if (_triggerCollider != null)
        {
            Gizmos.DrawWireCube(transform.position + _triggerCollider.bounds.center, _triggerCollider.bounds.size);
        }

        if (teleportDestination != null)
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawLine(transform.position, teleportDestination.position);
            Gizmos.DrawWireSphere(teleportDestination.position, 0.5f);
        }
    }
}