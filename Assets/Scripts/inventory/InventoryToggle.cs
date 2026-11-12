using UnityEngine;

public class InventoryToggle : MonoBehaviour
{
    // Panel visual que se quiere abrir y cerrar.
    [SerializeField] private GameObject inventoryPanel;
    [SerializeField] private InventoryUI inventoryUI; // Referencia a la interfaz del inventario.

    // Indica si el inventario está abierto.
    private bool isOpen = false;

    private void Start()
    {
        // Al comenzar, el inventario estará cerrado.
        inventoryPanel.SetActive(false);
    }

    private void Update()
    {
        // Detecta cuando el jugador presiona Tab.
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            // Cambia entre abierto y cerrado.
            isOpen = !isOpen;

            // Muestra u oculta el panel.
            inventoryPanel.SetActive(isOpen);
        }
        // Si cerras el inventario, cancela una posible combinación.
        if (!isOpen && inventoryUI != null)
        {
            inventoryUI.CancelCombination();
        }
    }
}
