using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryToggle : MonoBehaviour
{
    // Drag the root panel (the whole UI that contains slots) here in the Inspector
    [Header("UI References")]
    [SerializeField] private GameObject inventoryPanel;

    private void Awake()
    {
        // Ensure the inventory UI starts hidden when the game launches
        if (inventoryPanel != null)
            inventoryPanel.SetActive(false);
    }

    private void Update()
    {
        // New Input System – check if the "I" key was pressed this frame
        if (Keyboard.current != null && Keyboard.current.iKey.wasPressedThisFrame)
        {
            Toggle();
        }
        // Legacy Input fallback (in case the new system is not active)
        else if (Input.GetKeyDown(KeyCode.I))
        {
            Toggle();
        }
    }

    // Public method so you can also bind it to a UI button if you want
    public void Toggle()
    {
        if (inventoryPanel == null)
            return;

        bool isActive = inventoryPanel.activeSelf;
        inventoryPanel.SetActive(!isActive);
    }
}
