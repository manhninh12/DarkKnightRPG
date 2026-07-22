using UnityEngine;

[CreateAssetMenu(fileName = "New Strength Potion", menuName = "Inventory/Strength Potion Data")]
public class StrengthPotionData : ConsumableItemData
{
    [Header("Strength Buff Stats")]
    public int extraDamage = 1; // Sát thương cộng thêm
    public float duration = 5f; // Thời gian tác dụng (giây)

    public override void Use(PlayerController player)
    {
        base.Use(player); // Vẫn gọi base để có thể hồi máu nếu healAmount > 0
        player.ApplyDamageBuff(extraDamage, duration);
    }
}
