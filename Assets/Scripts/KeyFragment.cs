using UnityEngine;

public class KeyFragment : MonoBehaviour
{

    private bool isActive = false;
    private SpriteRenderer spriteRenderer;
    private Collider2D col;

    [Header("Collect Effect")]
    public GameObject collectEffect;


    // Get components and set key fragments' initial state
    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();
        ApplyActiveState();

    }

    // Enable the fragment 2 after all enemies are defeated.
    void OnEnable()
    {
        isActive = true;
        ApplyActiveState();
    }

    // To make sure the fragment is only triggered when it should be.
    private void ApplyActiveState()
    {
        if (spriteRenderer)
        {
            spriteRenderer.enabled = isActive;
        }
        if (col)
        {
            col.enabled = isActive;
        }
    }

    // Handle player collision to collect the fragments.
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isActive) return;
        if (collision.CompareTag("Player"))
        {
            LevelManager.Instance.CollectKeyFragment();
            
            if (collectEffect)
            {
                Instantiate(collectEffect, transform.position, Quaternion.identity);
            }
            
            Destroy(gameObject);
        }
    }

}
