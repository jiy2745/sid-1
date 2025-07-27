using UnityEngine;

public class SlashMinigameObject : MonoBehaviour
{
    [SerializeField] private GameObject EffectOnDestroyPrefab;      // Effect prefab for key prefab destruction
    private Rigidbody2D rb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // If mouse pointer enters the collider
    private void OnMouseEnter()
    {
        if (Input.GetMouseButton(0))
        {
            Slash();
        }
    }

    // If player fails to slash and object hits the 'ground'
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("SlashMinigameGround"))
        {
            //TODO: handle penalty (player health loss etc.)
            Destroy(gameObject);
        }
    }

    // Slashed object is destroyed
    public void Slash()
    {
        if (EffectOnDestroyPrefab != null)
        {
            Instantiate(EffectOnDestroyPrefab, transform.position, Quaternion.identity);
        }
        Destroy(gameObject);
    }
}
