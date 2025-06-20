using UnityEngine;

public class WeightPuzzleTrigger : MonoBehaviour
{
    public GameObject doorToOpen;
    public float requiredSize = 2f;
    public float tolerance = 0.3f;

    private bool triggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (triggered) return;

        if (other.CompareTag("Grabbable"))
        {
            float objectSize = other.transform.localScale.x;

            if (Mathf.Abs(objectSize - requiredSize) <= tolerance)
            {
                doorToOpen.SetActive(false); // hides or "opens" the door
                triggered = true;
                Debug.Log("Puzzle solved: Door opened.");
            }
            else
            {
                Debug.Log("❌ Object too small or too big.");
            }
        }
    }
}
