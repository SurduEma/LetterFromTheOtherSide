using UnityEngine;

public class WeightPuzzleTrigger : MonoBehaviour
{
    public GameObject doorToOpen;
    public float requiredSize = 2f;
    public float tolerance = 0.3f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Grabbable"))
        {
            float objectSize = other.transform.localScale.x;

            if (Mathf.Abs(objectSize - requiredSize) <= tolerance)
            {
                // Perfect size → deschide ușa
                doorToOpen.SetActive(false);
                Debug.Log("Puzzle solved: Door opened.");
            }
            else
            {
                // Feedback pentru dimensiune greșită
                Debug.Log("Greșit: Obiect prea mic sau prea mare.");
            }
        }
    }
}
