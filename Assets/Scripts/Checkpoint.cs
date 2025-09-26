using System.Collections;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{

    [Header("Flag Parts")]
    [SerializeField] private Transform flagDown;
    [SerializeField] private Transform flagUp;
    [SerializeField] private Transform respawnPoint;

    [Header("Raise Animation")]
    [SerializeField] private float raiseTime = 0.5f;
    [SerializeField] private AnimationCurve ease = AnimationCurve.EaseInOut(0, 0, 1, 1);

    [Header("Visuals")]
    [SerializeField] private SpriteRenderer flagRenderer;
    [SerializeField] private Color currentColor = Color.white;
    [SerializeField] private Color pastColor = new Color(0.75f, 0.75f, 0.75f);

    private bool isActivated = false;
    private static Checkpoint current;


    // Initialize references and set collider to trigger.
    void Reset()
    {
        flagDown = transform.Find("FlagDown");
        flagUp = transform.Find("FlagUp");
        respawnPoint = transform.Find("RespawnPoint");
        flagRenderer = flagDown ? flagDown.GetComponent<SpriteRenderer>() : null;

        var col = GetComponent<Collider2D>();
        if (col) col.isTrigger = true;
    }

    // Ensure references are set.
    void Awake()
    {
        if (!flagDown) flagDown = transform.Find("FlagDown");
        if (!flagUp) flagUp = transform.Find("FlagUp");
        if (!respawnPoint) respawnPoint = transform.Find("RespawnPoint");
        if (!flagRenderer) flagRenderer = flagDown ? flagDown.GetComponent<SpriteRenderer>() : null;

        var col = GetComponent<Collider2D>();
        if (col) col.isTrigger = true;
    }

    // Detect player collision and activate checkpoint.
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isActivated) return;
        if (collision.CompareTag("Player"))
        {
            Activate();
        }
    }

    // Activate the checkpoint;
    // Set respawn point in LevelManager;
    // Update visuals;
    // Raise the flag.
    private void Activate()
    {
        isActivated = true;

        if (respawnPoint && LevelManager.Instance != null)
        {
            LevelManager.Instance.SetCheckpoint(respawnPoint.position);
        }

        if (current && current != this)
        {
            current.SetPastVisual();
        }

        current = this;
        SetCurrentVisual();

        if (flagDown && flagUp)
        {
            StartCoroutine(RaiseFlag());
        }

        var col = GetComponent<Collider2D>();
        if (col) col.enabled = false;
    }

    // Animate the flag raising.
    private IEnumerator RaiseFlag()
    {
        Vector3 startPos = flagDown.localPosition;
        Vector3 endPos = flagUp.localPosition;

        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / Mathf.Max(0.001f, raiseTime);
            float k = ease.Evaluate(Mathf.Clamp01(t));
            flagDown.localPosition = Vector3.LerpUnclamped(startPos, endPos, k);
            yield return null;
        }

        flagDown.localPosition = endPos;

    }

    // Set the color to indicate past checkpoint.
    private void SetPastVisual()
    {
        SetColor(pastColor);
    }

    // Set the color of the current checkpoint.
    private void SetCurrentVisual()
    {
        SetColor(currentColor);
    }

    // Helper to set the flag color.
    private void SetColor(Color c)
    {
        if (!flagRenderer) return;
        flagRenderer.color = c;
    }

}
