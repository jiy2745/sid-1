using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class HeartsUI : MonoBehaviour
{
    public GameObject heartPrefab;          // Prefab with Image + Heart.cs
    public Transform player;                // Reference to player transform
    public Vector3 screenOffset = new Vector3(0, 50, 0);

    public float heartSpacing = 40f; // Spacing between hearts

    private List<Heart> hearts = new List<Heart>();
    private int maxHealth;

    public void InitHearts(int max)
    {
        maxHealth = max;

        // Clear old hearts
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
        hearts.Clear();

        // Calculate center offset
        float totalWidth = (maxHealth - 1) * heartSpacing;
        float halfWidth = totalWidth / 2f;

        // Create new hearts
        for (int i = 0; i < maxHealth; i++)
        {
            GameObject newHeart = Instantiate(heartPrefab, transform);

            RectTransform rt = newHeart.GetComponent<RectTransform>();
            rt.anchoredPosition = new Vector2(i * heartSpacing - halfWidth, 0); // Position hearts horizontally

            hearts.Add(newHeart.GetComponent<Heart>());
        }
    }

    public void UpdateHearts(int currentHealth)
    {
        for (int i = 0; i < hearts.Count; i++)
        {
            if (i < currentHealth)
            {
                hearts[i].SetFull();
            }
            else
            {
                hearts[i].SetEmpty();
            }
        }
    }

    private void LateUpdate()
    {
        if (player != null)
        {
            Vector3 screenPos = Camera.main.WorldToScreenPoint(player.position);
            transform.position = screenPos + screenOffset;
        }
    }
}
