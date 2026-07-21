using UnityEngine;

[CreateAssetMenu(fileName = "New Consumable", menuName = "Inventory/Consumable Item Data")]
public class ConsumableItemData : ItemData
{
    [Header("Consumable Stats")]
    public int healAmount;

    public virtual void Use(PlayerController player)
    {
        if (healAmount > 0)
        {
            player.Heal(healAmount);
        }
    }
}
