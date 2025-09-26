using System.Net.NetworkInformation;
using UnityEngine;
using System.Collections;

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

    [Header("Respawn Freeze")]
    [SerializeField] private float freezeSeconds = 1f;
    bool isRespawning = false;

    public event System.Action OnAllKeyFragmentCollected;

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

    public void CollectKeyFragment()
    {
        collectedKeyFragments++;
        if (collectedKeyFragments == totalKeyFragments)
        {
            gate?.Open();
            OnAllKeyFragmentCollected?.Invoke();
        }
    }

    public void OnAllEnemiesDefeated()
    {
        if (fragment2)
        {
            fragment2.SetActive(true);
        }
    }
}
