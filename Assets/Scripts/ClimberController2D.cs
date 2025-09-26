using UnityEngine;

public class ClimberController2D : MonoBehaviour
{
    [Header("Move / Jump / Climb")]
    public float moveSpeed = 8f;
    public float jumpForce = 14f;
    public float climbSpeed = 5f;

    [Header("Refs")]
    public Rigidbody2D rb;
    public LayerMask groundMask;

    [Header("Gravity Flip")]
    public KeyCode flipKey = KeyCode.F;

    [Header("Wall Probe")]
    public float wallProbeRadius = 0.14f;
    public float wallProbeOffset = 0.52f;

    float inputX = 0f;
    bool gravityFlipped = false;

    bool canJump = false;
    Vector2 bestNormal = Vector2.up;

    bool touchingWall = false;

    void Awake()
    {
        if (!rb) rb = GetComponent<Rigidbody2D>();
        if (!rb) rb = gameObject.AddComponent<Rigidbody2D>();
        rb.gravityScale = 3f;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
    }

    void Update()
    {
        inputX = 0f;
        if (Input.GetKey(KeyCode.A)) inputX = -1f;
        if (Input.GetKey(KeyCode.D)) inputX = 1f;

        if (Input.GetKeyDown(KeyCode.Space) && canJump)
        {
            Vector2 dir = bestNormal;
            Vector2 gdir = Physics2D.gravity.normalized * Mathf.Sign(rb.gravityScale);
            if (Vector2.Dot(dir, gdir) > 0f) dir = -dir;

            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y);
            rb.AddForce(dir.normalized * jumpForce, ForceMode2D.Impulse);
        }

        bool onWall = touchingWall || ProbeWall();
        if (onWall && (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S)))
        {
            rb.gravityScale = 0f;
            float v = 0f;
            if (Input.GetKey(KeyCode.W)) v = 1f;
            if (Input.GetKey(KeyCode.S)) v = -1f;
            float hx = inputX * moveSpeed * 0.4f;
            rb.linearVelocity = new Vector2(hx, v * climbSpeed);
        }
        else
        {
            rb.gravityScale = gravityFlipped ? -3f : 3f;
        }

        if (Input.GetKeyDown(flipKey))
        {
            gravityFlipped = !gravityFlipped;
            rb.gravityScale = gravityFlipped ? -3f : 3f;
        }
    }

    void FixedUpdate()
    {
        if (rb.gravityScale != 0f)
        {
            rb.linearVelocity = new Vector2(inputX * moveSpeed, rb.linearVelocity.y);
            if (inputX != 0f)
            {
                var s = transform.localScale;
                transform.localScale = new Vector3(Mathf.Sign(inputX) * Mathf.Abs(s.x), s.y, s.z);
            }
        }

        if (!rb.IsTouchingLayers(groundMask))
        {
            canJump = false;
            touchingWall = false;
            bestNormal = gravityFlipped ? Vector2.down : Vector2.up;
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (((1 << collision.collider.gameObject.layer) & groundMask) == 0) return;

        Vector2 gdir = Physics2D.gravity.normalized * Mathf.Sign(rb.gravityScale) * -1f;
        float bestDot = -1f;

        bool wallNow = false;

        foreach (var c in collision.contacts)
        {
            Vector2 n = c.normal;
            if (Mathf.Abs(n.x) > 0.7f) wallNow = true;

            float d = Vector2.Dot(n.normalized, gdir.normalized);
            if (d > bestDot)
            {
                bestDot = d;
                bestNormal = n;
            }
        }

        touchingWall = wallNow || touchingWall;
        canJump = true;
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (((1 << collision.collider.gameObject.layer) & groundMask) == 0) return;
        touchingWall = ProbeWall();
    }

    bool ProbeWall()
    {
        Vector2 c = transform.position;
        Vector2 lp = c + Vector2.left * wallProbeOffset;
        Vector2 rp = c + Vector2.right * wallProbeOffset;

        bool leftHit  = Physics2D.OverlapCircle(lp, wallProbeRadius, groundMask);
        bool rightHit = Physics2D.OverlapCircle(rp, wallProbeRadius, groundMask);
        return leftHit || rightHit;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Vector2 c = transform.position;
        Gizmos.DrawWireSphere(c + Vector2.left * wallProbeOffset,  wallProbeRadius);
        Gizmos.DrawWireSphere(c + Vector2.right * wallProbeOffset, wallProbeRadius);

        Gizmos.color = Color.yellow;
        Vector3 p = transform.position + (Vector3)(bestNormal.normalized * 0.6f);
        Gizmos.DrawLine(transform.position, p);
        Gizmos.DrawSphere(p, 0.04f);
    }
}
