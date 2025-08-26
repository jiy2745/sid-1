using UnityEngine;
using UnityEngine.Events;

public class HealthManager : MonoBehaviour
{
    public int maxHealth = 3;
    public int currentHealth;

    public HeartsUI heartsUI;
    public GameOverUI gameOverUI; // Reference to GameOver UI

    [Header("Damage Flash")]
    public DamageFlash damageFlash;   // reference to DamageFlash component
    public UnityEvent onMinigameLost;

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
            onMinigameLost?.Invoke();
            //GameOver();
            return;
        }
    }

    public void IncreaseHealth()
    {
        ChangeHealth(1);
    }

    public void DecreaseHealth()
    {
        ChangeHealth(-1);
    }

    public void GameOver()
    {
        Debug.Log("Game Over! Health reached zero.");

        // TODO: Need to check with game manager for item to revive

        // For now, just show game over UI
        if (gameOverUI != null)
        {
            gameOverUI.Show();
        }
        else
        {
            Debug.LogError("GameOverUI is not assigned in HealthManager!");
        }
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