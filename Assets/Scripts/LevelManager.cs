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
            var pc = p.GetComponent<PlayerController2D>();

            if (rb) rb.SetRotation(0f);                
            p.transform.rotation = Quaternion.identity; 
            var s = p.transform.localScale;              
            p.transform.localScale = new Vector3(s.x >= 0 ? 1f : -1f, 1f, 1f);

            // 重力朝下
            if (rb)
            {
                rb.gravityScale = pc ? Mathf.Abs(pc.gravityMagnitude) : Mathf.Abs(rb.gravityScale);
                rb.linearVelocity = Vector2.zero;
                rb.angularVelocity = 0f;
                rb.simulated = false;                   
            }

            if (pc) pc.enabled = false;                
        }

        yield return new WaitForSeconds(freezeSeconds);

        // unfreeze
        foreach (var p in players)
        {
            if (!p) continue;

            var rb = p.GetComponent<Rigidbody2D>();
            var pc = p.GetComponent<PlayerController2D>();

            if (rb) rb.simulated = true;
            if (pc) pc.enabled = true;
        }

        isRespawning = false;
    }


}
