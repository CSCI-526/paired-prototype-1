using UnityEngine;

public class Checkpoint : MonoBehaviour
{

    private bool isActivated = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isActivated) return;
        if (!other.CompareTag("Player")) return;

        isActivated = true;
        LevelManager.Instance.SetActiveCheckpoint(this);
    }

}
