using System.Net.NetworkInformation;
using UnityEngine;
using System.Collections;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }

    [Header("Gate")]
    public Gate gate;

    [Header("Respawn Point")]
    public Transform spawnPoint;
    private Vector3 currentRespawnPos;

    [Header("Respawn Freeze")]
    [SerializeField] private float freezeSeconds = 1f;
    bool isRespawning = false;


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

    public void SetCheckpoint(Vector3 pos)
    {
        currentRespawnPos = pos;
    }

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

        }

        yield return new WaitForSeconds(freezeSeconds);

        // unfreeze
        foreach (var p in players)
        {
            if (!p) continue;

            var rb = p.GetComponent<Rigidbody2D>();
            if (rb) rb.simulated = true;


        }

        isRespawning = false;
    }

}
