using UnityEngine;

public class FighterController2D : MonoBehaviour
{
    [Header("Move")]
    public float moveSpeed = 8f;
    public Rigidbody2D rb;

    [Header("Shoot")]
    public GameObject arrowPrefab;   
    public float arrowSpeed = 16f;
    public Transform firePoint;     

    float inputX;

    void Awake()
    {
        if (!rb) rb = GetComponent<Rigidbody2D>();

        if (!firePoint)
        {
            firePoint = new GameObject("FirePoint").transform;
            firePoint.SetParent(transform);
            firePoint.localPosition = new Vector3(0.6f, 0f, 0f);
        }
    }

    void Update()
    {
        inputX = 0f;
        if (Input.GetKey(KeyCode.A)) inputX = -1f;
        if (Input.GetKey(KeyCode.D)) inputX = 1f;

        if (Input.GetKeyDown(KeyCode.J))
        {
            Shoot();
        }
    }

    void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(inputX * moveSpeed, rb.linearVelocity.y);

        if (inputX != 0f)
        {
            var s = transform.localScale;
            transform.localScale = new Vector3(Mathf.Sign(inputX) * Mathf.Abs(s.x), s.y, s.z);
        }
    }

    void Shoot()
    {
        if (!arrowPrefab) return;
        float dir = Mathf.Sign(transform.localScale.x);
        var go = Instantiate(arrowPrefab, firePoint.position, Quaternion.identity);
        var rb2 = go.GetComponent<Rigidbody2D>();
        if (rb2) rb2.linearVelocity = new Vector2(dir * arrowSpeed, 0f);
        go.transform.localScale = new Vector3(dir, 1f, 1f);
    }
}
