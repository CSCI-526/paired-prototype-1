using System.Net.NetworkInformation;
using UnityEngine;

public class LevelManager : MonoBehaviour
{

    public static LevelManager Instance { get; private set; }

    [Header("Keys / Gate")]
    private int totalKeyFragments = 2;
    private int collectedKeyFragments = 0;
    public Gate gate;

    [Header("Respawn Point")]
    public Transform spawnPoint;
    private Vector3 currentRespawnPos;

    [Header("Fragment 2")]
    public GameObject fragment2;

    public event System.Action OnAllKeyFragmentCollected;

    // Initialize the instance and set the spawn point.
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        if (spawnPoint == null)
        {
            Debug.LogError("Spawn Point is not assigned.");
        }
        else
        {
            currentRespawnPos = spawnPoint.position;
        }
    }

    // Set the checkpoint position for respawning.
    // Called by Checkpoint script.
    public void SetCheckpoint(Vector3 pos)
    {
        currentRespawnPos = pos;
    }

    // Find the player and set its position to the current respawn point.
    // Called by Hazard script.
    public void RespawnPlayer()
    {
        var player = GameObject.FindGameObjectsWithTag("Player")[0];
        if (player)
        {
            player.transform.position = currentRespawnPos;
        }
    }

    // Determine if all key fragments are collected and whether to open the gate.
    // Called by KeyFragment script.
    public void CollectKeyFragment()
    {
        collectedKeyFragments++;
        if (collectedKeyFragments == totalKeyFragments)
        {
            gate?.Open();
            OnAllKeyFragmentCollected?.Invoke();
        }
    }

    // Activate fragment 2 when all enemies are defeated.
    // Called by Enemy script.
    public void OnAllEnemiesDefeated()
    {
        if (fragment2)
        {
            fragment2.SetActive(true);
        }
    }

}
