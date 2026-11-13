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
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            // Cambia el estado del inventario.
            isOpen = !isOpen;

            // Muestra u oculta el panel.
            inventoryPanel.SetActive(isOpen);

            if (inventoryUI != null)
            {
                if (isOpen)
                {
                    // Fuerza a Unity a actualizar las posiciones de los elementos de la interfaz.
                    Canvas.ForceUpdateCanvases();

                    // Actualiza los íconos con el panel abierto.
                    inventoryUI.UpdateInventoryUI();
                }
                else
                {
                    // Cancela cualquier combinación pendiente.
                    inventoryUI.CancelCombination();
                }
            }
        }
    }
}
