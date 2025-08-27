using UnityEngine;

public class SlashMinigameObject : MonoBehaviour
{
    [SerializeField] private GameObject EffectOnDestroyPrefab;      // Effect prefab for key prefab destruction
    private Rigidbody2D rb;
    private Color spriteColor;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        spriteColor = spriteRenderer.color;
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
        ParticleSystem ps = EffectOnDestroyPrefab.GetComponent<ParticleSystem>();
        if (ps != null)
        {
            var main = ps.main;
            main.startColor = spriteColor;

            var col = ps.colorOverLifetime;
            col.enabled = true;

            // Gradient fades to transparent
            Gradient gradient = new Gradient();
            gradient.SetKeys(
            new GradientColorKey[] {
                new GradientColorKey(spriteColor, 0.0f),
                new GradientColorKey(spriteColor, 1.0f)
            },
            new GradientAlphaKey[] {
                new GradientAlphaKey(spriteColor.a, 0.0f),
                new GradientAlphaKey(0.0f, 1.0f)
            }
            );
            col.color = new ParticleSystem.MinMaxGradient(gradient);
        }
        Destroy(gameObject);
    }
}
