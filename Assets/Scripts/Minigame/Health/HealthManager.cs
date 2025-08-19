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
        // Check for game over
        if (currentHealth <= 0)
        {
            GameOver();
            return;
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

    private void GameOver()
    {
        // TODO: Handle game over logic here
        Debug.Log("Game Over! Health reached zero.");

        // Need to check with game manager for item to revive
        // Then set minigame to paused
    }

    public void ResetHealth()
    {
        currentHealth = maxHealth;
        heartsUI.InitHearts(maxHealth);
    }

    public void ShowHeartsUI(bool show)
    {
        heartsUI.gameObject.SetActive(show);
    }
}