using UnityEngine;
using UnityEngine.Events;

public class HealthManager : MonoBehaviour
{
    public int maxHealth = 3;
    public int currentHealth;

    public HeartsUI heartsUI;

    [Header("Damage Flash")]
    public DamageFlash damageFlash;   // reference to DamageFlash component
    public UnityEvent onHealthChanged;

    private void Start()
    {
        currentHealth = maxHealth;
        heartsUI.InitHearts(maxHealth);
        heartsUI.UpdateHearts(currentHealth);
    }

    public void ChangeHealth(int amount)
    {
        int oldHealth = currentHealth;
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);

        // Update UI
        heartsUI.UpdateHearts(currentHealth);

        // Play animations
        if (amount > 0)
        {
            // Find the newly filled heart and animate
            heartsUI.GetComponentInChildren<Heart>().PlayFillAnimation();
        }
        else if (amount < 0)
        {
            // Animate empty heart
            heartsUI.GetComponentInChildren<Heart>().PlayEmptyAnimation();
            if (damageFlash != null && currentHealth < oldHealth)
            {
                damageFlash.PlayDamageFlash(); // Play damage flash effect
            }

        }

        onHealthChanged?.Invoke();
    }

    public void IncreaseHealth()
    {
        ChangeHealth(1);
    }

    public void DecreaseHealth()
    {
        ChangeHealth(-1);
    }
}