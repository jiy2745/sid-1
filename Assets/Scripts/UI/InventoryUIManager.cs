using UnityEngine;

public class InventoryUIManager : MonoBehaviour
{
    public static InventoryUIManager instance;
    public GameObject inventoryUIPrefab;
    private GameObject inventoryUIInstance;
    
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ToggleInventoryUI();
        }
    }

    public void ToggleInventoryUI()
    {
        if (inventoryUIInstance == null)
        {
            inventoryUIInstance = Instantiate(inventoryUIPrefab);
            return;
        }
        
        inventoryUIInstance.SetActive(!inventoryUIInstance.activeSelf);
    }
}
