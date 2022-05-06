using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Text))]
public class UIFPS : MonoBehaviour
{
    private TMP_Text fpsText;
    private int currentFPS;
    private float updateThresh = 20f;
    private float updateDelay = 0.20f;
    private float timer;
    private int lastFPS;

    private void Awake() => fpsText = GetComponent<TMP_Text>();

    private void LateUpdate()
    {
        currentFPS = Mathf.RoundToInt(1 / Time.deltaTime);

        if (CanUpdate())
        {
            timer = 0;
            fpsText.text = $"FPS: {currentFPS.ToString("N0")}";
        }
        lastFPS = currentFPS;
        timer += Time.deltaTime;
    }

    private bool CanUpdate()
    {
        return
            (timer >= updateDelay) &&
            (Mathf.Abs(lastFPS - currentFPS) > updateThresh);
    }
}
