using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryUI : MonoBehaviour
{
    // Referencia al inventario del jugador.
    [SerializeField] private ItemsManager itemsManager;

    // Las 8 imágenes donde mostrar los objetos.
    [SerializeField] private Image[] slotIcons = new Image[8];
    // Los 8 botones que permiten seleccionar las casillas.
    [SerializeField] private Button[] slotButtons = new Button[8];
    // Guarda los bordes de las 8 casillas. Los utiliza para resaltar el objeto seleccionado.
    [SerializeField] private Outline[] slotOutlines = new Outline[8];

    // Texto que muestra el nombre del objeto seleccionado.
    [SerializeField] private TMP_Text itemNameText;
    // Texto que muestra la descripción del objeto seleccionado.
    [SerializeField] private TMP_Text itemDescriptionText;
    // Texto que muestra los usos restantes del objeto.
    [SerializeField] private TMP_Text itemUsesText;
    // Guarda una referencia al objeto que seleccionamos. Si no hay ningún objeto seleccionado, su valor será null.
    private InventoryItem selectedItem = null;
    // Botón que permitirá utilizar el objeto seleccionado.
    [SerializeField] private Button useButton;

    private void Start()
    {
        // Comprueba que exista una referencia al inventario.
        if (itemsManager == null)
        {
            Debug.LogWarning("InventoryUI: No se asignó ItemsManager.");
            return;
        }

        // Se suscribe al evento del inventario. Cada vez que cambie, se ejecutará UpdateInventoryUI().
        itemsManager.OnInventoryChanged += UpdateInventoryUI;

        // Muestra el inventario al iniciar.
        UpdateInventoryUI();

        // Limpia los textos al comenzar la partida. Solo aparecerá información cuando seleccionemos un objeto.
        itemNameText.text = "";
        itemDescriptionText.text = "";
        itemUsesText.text = "";

        // Recorre los 8 botones del inventario.
        for (int i = 0; i < slotButtons.Length; i++)
        {
            // Guarda la posición del botón actual. Esto permite que cada botón recuerde su propio número.
            int slotIndex = i;

            // Comprueba que el botón esté asignado.
            if (slotButtons[i] != null)
            {
                // Cuando hace clic, llama a SelectSlot() y le indica qué casilla seleccionamos.
                slotButtons[i].onClick.AddListener(() => SelectSlot(slotIndex));
            }
        }
        // Comporbar que el botón USAR esté asignado.
        if (useButton != null)
        {
            // Cuando presione USAR, ejecuta UseSelectedItem().
            useButton.onClick.AddListener(UseSelectedItem);
        }
        // Al comenzar la partida, oculta el botón USAR.
        if (useButton != null)
        {
            useButton.gameObject.SetActive(false);
        }
    }

    // Se ejecuta cuando se destruye este componente. Deja de escuchar el evento para evitar referencias innecesarias.
    private void OnDestroy()
    {
        if (itemsManager != null)
        {
            itemsManager.OnInventoryChanged -= UpdateInventoryUI;
        }
    }



    // Se ejecuta cuando hace clic en una casilla.
    private void SelectSlot(int slotIndex)
    {
        // Resalta la casilla sobre la que hicimos clic.
        HighlightSelectedSlot(slotIndex);

        // Obtiene los objetos guardados en el inventario.
        List<InventoryItem> items = itemsManager.GetItems();

        // Indica qué casilla esta recorriendo.
        int currentSlot = 0;

        // Recorre todos los objetos del inventario.
        foreach (InventoryItem item in items)
        {
            // Consulta cuántos espacios ocupa este objeto.
            int slotsRequired = item.itemData.slotsRequired;

            // Comprobar si la casilla seleccionada corresponde a este objeto.
            if (slotIndex >= currentSlot &&
                slotIndex < currentSlot + slotsRequired)
            {
                // Guarda qué objeto seleccionó el jugador. Así puede identificarlo cuando quiera usarlo o eliminarlo.
                selectedItem = item;

                // Comprobar si el objeto seleccionado puede utilizarse.
                bool canUse = item.itemData.itemType == ItemType.Consumable &&
                              item.itemData.maxUses > 1 &&
                              item.remainingUses > 0;

                // Muestra u oculta el botón según el resultado.
                if (useButton != null)
                {
                    useButton.gameObject.SetActive(canUse);
                }
                // Muestra el nombre del objeto.
                itemNameText.text = item.itemData.itemName;

                // Muestra su descripción.
                itemDescriptionText.text = item.itemData.description;

                // Muestra sus usos restantes. Las etiquetas <color> permiten colorear el número.
                itemUsesText.text = "Cantidad restante: <color=#8FCF9B>" +
                    item.remainingUses + "/" + item.itemData.maxUses +
                    "</color>";

                return;
            }

            // Avanza según los espacios que ocupa el objeto.
            currentSlot += slotsRequired;
        }

        // Si la casilla está vacía, quita la selección.
        selectedItem = null;
        // Oculta el botón porque no hay ningún objeto seleccionado.
        if (useButton != null)
        {
            useButton.gameObject.SetActive(false);
        }

        // Limpia la información del panel derecho.
        itemNameText.text = "";
        itemDescriptionText.text = "";
        itemUsesText.text = "";
    }

    // Se ejecuta cuando presione el botón USAR.
    private void UseSelectedItem()
    {
        // Comprueba que haya un objeto seleccionado.
        if (selectedItem == null)
        {
            Debug.Log("No hay ningún objeto seleccionado.");
            return;
        }

        // Por ahora solamente permite utilizar las píldoras. Comprueba que sea un consumible con más de un uso.
        if (selectedItem.itemData.itemType != ItemType.Consumable ||
            selectedItem.itemData.maxUses <= 1)
        {
            Debug.Log("Este objeto todavía no se puede utilizar.");
            return;
        }

        // Comprueba que todavía queden dosis.
        if (selectedItem.remainingUses <= 0)
        {
            Debug.Log("El frasco ya no tiene dosis.");
            return;
        }

        // Consume una del frasco seleccionado.
        selectedItem.remainingUses--;

        Debug.Log("Utilizaste una dosis de: " +
            selectedItem.itemData.itemName);

        // Actualiza el texto de usos restantes.
        itemUsesText.text = "Cantidad restante: <color=#8FCF9B>" +
            selectedItem.remainingUses + "/" +
            selectedItem.itemData.maxUses + "</color>";

        // Comprueba si acabamos de consumir la última.
        if (selectedItem.remainingUses <= 0)
        {
            Debug.Log("El frasco se quedó sin dosis.");

            // Elimina el frasco del inventario. ItemsManager avisará a InventoryUI para actualizar las casillas.
            itemsManager.RemoveItem(selectedItem);
        }
    }


    // Resalta todas las casillas que pertenecen al objeto seleccionado.
    private void HighlightSelectedSlot(int selectedIndex)
    {
        // Obtiene los objetos del inventario.
        List<InventoryItem> items = itemsManager.GetItems();

        // Indica dónde comienza cada objeto dentro de las casillas.
        int currentSlot = 0;

        // Por defecto, no hay ningún objeto seleccionado.
        int firstSlot = -1;
        int lastSlot = -1;

        // Busca a qué objeto pertenece la casilla seleccionada.
        foreach (InventoryItem item in items)
        {
            int slotsRequired = item.itemData.slotsRequired;

            // Comprueba si la casilla pertenece a este objeto.
            if (selectedIndex >= currentSlot &&
                selectedIndex < currentSlot + slotsRequired)
            {
                // Guarda la primera y última casilla del objeto.
                firstSlot = currentSlot;
                lastSlot = currentSlot + slotsRequired - 1;

                break;
            }

            // Avanza hasta el siguiente objeto.
            currentSlot += slotsRequired;
        }

        // Recorre los bordes de todas las casillas.
        for (int i = 0; i < slotOutlines.Length; i++)
        {
            if (slotOutlines[i] == null)
            {
                continue;
            }

            // Comprueba si esta casilla pertenece al objeto seleccionado.
            bool isSelected = i >= firstSlot && i <= lastSlot;

            // También permite seleccionar visualmente casillas vacías.
            if (firstSlot == -1)
            {
                isSelected = i == selectedIndex;
            }

            if (isSelected)
            {
                // Borde dorado para las casillas seleccionadas.
                slotOutlines[i].effectColor = new Color32(229, 199, 130, 255);
                slotOutlines[i].effectDistance = new Vector2(3, 3);
            }
            else
            {
                // Borde gris para las demás casillas.
                slotOutlines[i].effectColor = new Color32(89, 100, 116, 255);
                slotOutlines[i].effectDistance = new Vector2(1, 1);
            }
        }
    }

    // Actualiza las imágenes de las 8 casillas.
    public void UpdateInventoryUI()
    {
        // Primero limpia todas las casillas.
        foreach (Image icon in slotIcons)
        {
            if (icon != null)
            {
                icon.sprite = null;
                icon.gameObject.SetActive(false);
            }
        }

        // Obtiene los objetos del inventario.
        List<InventoryItem> items = itemsManager.GetItems();

        // Indica la próxima casilla disponible.
        int slotIndex = 0;

        // Recorre todos los objetos recogidos.
        foreach (InventoryItem item in items)
        {
            // Comprueba que haya espacio.
            if (slotIndex >= slotIcons.Length)
            {
                break;
            }

            // Obtiene los datos del objeto.
            ItemData data = item.itemData;

            // Mostrar su imagen en la casilla.
            if (slotIcons[slotIndex] != null && data.icon != null)
            {
                slotIcons[slotIndex].sprite = data.icon;
                slotIcons[slotIndex].gameObject.SetActive(true);
            }

            // Avanza según los espacios que ocupa el objeto.
            slotIndex += data.slotsRequired;
        }
        // Si no tenemos ningún objeto seleccionado, no hace nada más.
        if (selectedItem == null)
        {
            return;
        }

        // Comprobar si el objeto seleccionado sigue en el inventario.
        if (!items.Contains(selectedItem))
        {
            // El objeto ya no existe en el inventario.
            selectedItem = null;

            // Oculta el botón porque el objeto seleccionado desapareció.
            if (useButton != null)
            {
                useButton.gameObject.SetActive(false);
            }

            // Limpia los textos del panel derecho.
            itemNameText.text = "";
            itemDescriptionText.text = "";
            itemUsesText.text = "";

            // Quita el resaltado de todas las casillas.
            foreach (Outline outline in slotOutlines)
            {
                if (outline != null)
                {
                    outline.effectColor = new Color32(89, 100, 116, 255);
                    outline.effectDistance = new Vector2(1, 1);
                }
            }
        }
        else
        {
            // El objeto sigue existiendo. Actualiza sus usos por si cambiaron.
            itemUsesText.text = "Cantidad restante: <color=#8FCF9B>" +
                selectedItem.remainingUses + "/" +
                selectedItem.itemData.maxUses + "</color>";
        }
    }
}

