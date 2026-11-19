using UnityEngine;

public class InventoryToggle : MonoBehaviour
{
    // Panel visual que se quiere abrir y cerrar.
    [SerializeField] private GameObject inventoryPanel;
    [SerializeField] private InventoryUI inventoryUI; // Referencia a la interfaz del inventario.
    [Header("Sonidos del inventario")]
    [SerializeField] private AudioClip sonidoAbrir; // Sonido al abrir el inventario.
    [SerializeField] private AudioClip sonidoCerrar; // Sonido al cerrar el inventario.
    private AudioSource audioJugador; // Guarda el AudioSource del jugador.

    // Indica si el inventario está abierto.
    private bool isOpen = false;

    private void Start()
    {
        // Al comenzar, el inventario estará cerrado.
        inventoryPanel.SetActive(false);
        // Busca al jugador de la escena y obtiene su AudioSource.
        PlayerMovement jugador = FindFirstObjectByType<PlayerMovement>();

        if (jugador != null)
        {
            audioJugador = jugador.GetComponent<AudioSource>();
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            // Cambia el estado del inventario.
            isOpen = !isOpen;

            // Muestra u oculta el panel.
            inventoryPanel.SetActive(isOpen);

            // Si el inventario está abierto, libera el cursor. Si está cerrado, vuelve a bloquearlo.
            Cursor.lockState = isOpen ? CursorLockMode.None : CursorLockMode.Locked;
            // Muestra el cursor al abrir y lo oculta al cerrar.
            Cursor.visible = isOpen;

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
            // Selecciona el sonido según el estado del inventario.
            AudioClip sonido = isOpen ? sonidoAbrir : sonidoCerrar;

            // Reproduce el sonido desde el jugador.
            if (audioJugador != null && sonido != null)
            {
                audioJugador.PlayOneShot(sonido);
            }
        }
    }
}
