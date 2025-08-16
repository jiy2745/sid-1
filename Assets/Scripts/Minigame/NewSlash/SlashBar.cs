using System;
using UnityEngine;

public class SlashBar : MonoBehaviour
{
    public RectTransform pointer;   // Reference to the pointer
    public float pointerSpeed = 200f; // Speed at which the pointer moves

    public RectTransform safeZone; // Reference to the safe zone area
    public float[] safeZoneWidths = { 150f, 100f, 60f }; // Widths for different difficulty levels

    private bool movingRight = true; // Direction of pointer movement
    private float barWidth;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        barWidth = GetComponent<RectTransform>().rect.width; // Get the width of the bar
    }

    // Update is called once per frame
    void Update()
    {
        MovePointer();
    }

    private void MovePointer()
    {
        float moveAmount = pointerSpeed * Time.deltaTime * (movingRight ? 1 : -1);
        pointer.localPosition += new Vector3(moveAmount, 0, 0);

        float halfBar = barWidth / 2;
        if (pointer.localPosition.x >= halfBar)
        {
            pointer.localPosition = new Vector3(halfBar, pointer.localPosition.y, 0);
            movingRight = false;
        }
        else if (pointer.localPosition.x <= -halfBar)
        {
            pointer.localPosition = new Vector3(-halfBar, pointer.localPosition.y, 0);
            movingRight = true;
        }
    }

    public void GenerateSafeZone(int difficulty)
    {
        float width = safeZoneWidths[difficulty];
        safeZone.sizeDelta = new Vector2(width, safeZone.sizeDelta.y); // Set the width of the safe zone

        // Random position within the bar width
        float positionX = UnityEngine.Random.Range(-barWidth / 2 + width / 2, barWidth / 2 - width / 2);

        // Reset pointer position
        pointer.localPosition = new Vector3(-barWidth / 2, pointer.localPosition.y, 0);
        movingRight = true; // Reset direction to right
    }
    
    public bool CheckIfInSafeZone()
    {
        float pointerX = pointer.localPosition.x;
        float safeLeft = safeZone.localPosition.x - (safeZone.sizeDelta.x / 2);
        float safeRight = safeZone.localPosition.x + (safeZone.sizeDelta.x / 2);

        return pointerX >= safeLeft && pointerX <= safeRight;
    }
}
