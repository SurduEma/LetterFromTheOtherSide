using UnityEngine;
using TMPro;

public class FloatingFadeText : MonoBehaviour
{
    public float floatSpeed = 0.5f;
    public float floatHeight = 0.25f;
    public float displayTime = 3f;
    public float fadeTime = 1f;

    private Vector3 startPos;
    private TextMeshProUGUI text;
    private float timer = 0f;
    private CanvasGroup canvasGroup;

    void Start()
    {
        startPos = transform.localPosition;
        text = GetComponentInChildren<TextMeshProUGUI>();
        canvasGroup = gameObject.AddComponent<CanvasGroup>();
    }

    void Update()
    {
        // Floating up and down
        float newY = startPos.y + Mathf.Sin(Time.time * floatSpeed) * floatHeight;
        transform.localPosition = new Vector3(startPos.x, newY, startPos.z);

        // Timer for fade
        timer += Time.deltaTime;
        if (timer > displayTime)
        {
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, (timer - displayTime) / fadeTime);
            if (canvasGroup.alpha <= 0f)
                Destroy(gameObject); // remove from scene after fade
        }
    }
}
