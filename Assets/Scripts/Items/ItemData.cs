using UnityEngine;

public enum ItemType
{
    Consumable,
    Equipment,
    Material,
    Quest
}

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item Data")]
public class ItemData : ScriptableObject
{
    public string itemName;
    [TextArea(3, 5)]
    public string description;
    public Sprite icon;
    public ItemType itemType;
}
