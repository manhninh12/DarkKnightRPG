using UnityEngine;

[CreateAssetMenu(fileName = "New Speed Potion", menuName = "Inventory/Speed Potion Data")]
public class SpeedPotionData : ConsumableItemData
{
    [Header("Speed Buff Stats")]
    public float speedMultiplier = 1.5f; // Gấp rưỡi tốc độ
    public float duration = 5f; // Thời gian tác dụng (giây)

    public override void Use(PlayerController player)
    {
        base.Use(player); // Vẫn gọi base để có thể hồi máu nếu healAmount > 0
        player.ApplySpeedBuff(speedMultiplier, duration);
    }
}
