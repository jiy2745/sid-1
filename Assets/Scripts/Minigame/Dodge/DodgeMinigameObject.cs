using UnityEngine;

public class DodgeMinigameObject : MonoBehaviour
{
    // Initialized by spawner object
    public float moveSpeed;
    public float lifetime;
    public Vector3 moveDirection;
    [SerializeField] private GameObject EffectOnDestroyPrefab;      // Effect prefab for key prefab destruction

    private float timer;
    private Color spriteColor;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        spriteColor = spriteRenderer.color;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= lifetime)
        {
            Destroy(gameObject);
        }
        transform.position += moveDirection * moveSpeed * Time.deltaTime;
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
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
            //TODO: handle penalty (player health loss etc.)
            GetComponentInParent<DodgeMinigame>().DecreaseHealth();
            Destroy(gameObject);
        }
    }
}
