using UnityEngine;

public class Gate : MonoBehaviour
{

    [Header("Colliders")]
    public BoxCollider2D solidCollider;
    public BoxCollider2D triggerZone;

    [Header("Gate States")]
    public GameObject closedGate;
    public GameObject openedGate;

    private bool isOpen = false;

    // Open the gate
    // Closed gate -> Opened gate, disable solid collider.
    // Called by LevelManager script.
    public void Open()
    {
        if (isOpen) return;
        isOpen = true;

        if (solidCollider)
        {
            solidCollider.enabled = false;
        }
        if (triggerZone)
        {
            triggerZone.enabled = true;
        }
        if (closedGate)
        {
            closedGate.SetActive(false);
        }
        if (openedGate)
        {
            openedGate.SetActive(true);
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isOpen) return;
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Level Complete!");
        }
    }
    
}
