using UnityEngine;

public class InventoryUIManager : MonoBehaviour
{
    public GameObject inventoryUI;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ToggleInventoryUI();
        }
    }
    
    public void ToggleInventoryUI()
    {
        if (inventoryUI != null)
        {
            inventoryUI.SetActive(!inventoryUI.activeSelf);
        }
    }
}
