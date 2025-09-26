using System.Net.NetworkInformation;
using UnityEngine;
<<<<<<< Updated upstream
using System.Collections;

public class LevelManager : MonoBehaviour
{
=======

public class LevelManager : MonoBehaviour
{

>>>>>>> Stashed changes
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

<<<<<<< Updated upstream
    [Header("Respawn Freeze")]
    [SerializeField] private float freezeSeconds = 1f;
    bool isRespawning = false;

    public event System.Action OnAllKeyFragmentCollected;

=======
    public event System.Action OnAllKeyFragmentCollected;


    // Initialize the instance and set the spawn point.
>>>>>>> Stashed changes
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

<<<<<<< Updated upstream
=======
    // Set the checkpoint position for respawning.
    // Called by Checkpoint script.
>>>>>>> Stashed changes
    public void SetCheckpoint(Vector3 pos)
    {
        currentRespawnPos = pos;
    }

<<<<<<< Updated upstream
    // freeze all players for freezeSeconds, then resume
    public void RespawnPlayer()
    {
        if (isRespawning) return;
        StartCoroutine(RespawnRoutine());
    }

    IEnumerator RespawnRoutine()
    {
        isRespawning = true;

        var players = GameObject.FindGameObjectsWithTag("Player");

        // teleport and freeze
        foreach (var p in players)
        {
            if (!p) continue;

            p.transform.position = currentRespawnPos;

            var rb = p.GetComponent<Rigidbody2D>();
            if (rb)
            {
                rb.linearVelocity = Vector2.zero;
                rb.angularVelocity = 0f;
                rb.simulated = false;
            }

            var f = p.GetComponent<FighterController2D>();
            var c = p.GetComponent<ClimberController2D>();
            if (f) f.enabled = false;
            if (c) c.enabled = false;
        }

        yield return new WaitForSeconds(freezeSeconds);

        // unfreeze
        foreach (var p in players)
        {
            if (!p) continue;

            var rb = p.GetComponent<Rigidbody2D>();
            if (rb) rb.simulated = true;

            var f = p.GetComponent<FighterController2D>();
            var c = p.GetComponent<ClimberController2D>();
            if (f) f.enabled = true;
            if (c) c.enabled = true;
        }

        isRespawning = false;
    }

=======
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
>>>>>>> Stashed changes
    public void CollectKeyFragment()
    {
        collectedKeyFragments++;
        if (collectedKeyFragments == totalKeyFragments)
        {
            gate?.Open();
            OnAllKeyFragmentCollected?.Invoke();
        }
    }

<<<<<<< Updated upstream
=======
    // Activate fragment 2 when all enemies are defeated.
    // Called by Enemy script.
>>>>>>> Stashed changes
    public void OnAllEnemiesDefeated()
    {
        if (fragment2)
        {
            fragment2.SetActive(true);
        }
    }
<<<<<<< Updated upstream
=======

>>>>>>> Stashed changes
}
