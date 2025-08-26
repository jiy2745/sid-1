using UnityEngine;

public enum ItemID
{
    None = 0,
    FireAx = 1,
    Gasoline = 2,
    Lighter = 3,
    Matchstick = 4,
    Memo = 5,
    Scroll = 6
}

[CreateAssetMenu(fileName = "ItemData", menuName = "Scriptable Objects/ItemData")]
public class ItemData : ScriptableObject
{
    [Header("Item Info")]
    public ItemID id;
    public string itemName;
    public string description;
    public Sprite icon;
}
