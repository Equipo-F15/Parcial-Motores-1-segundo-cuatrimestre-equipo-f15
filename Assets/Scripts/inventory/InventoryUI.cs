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
    // Imágenes utilizadas para mostrar objetos de dos espacios. Cada objeto grande tendrá su propia imagen.
    [SerializeField] private Image[] largeItemIcons = new Image[4];

    // Texto que muestra el nombre del objeto seleccionado.
    [SerializeField] private TMP_Text itemNameText;
    // Texto que muestra la descripción del objeto seleccionado.
    [SerializeField] private TMP_Text itemDescriptionText;
    // Texto que muestra los usos restantes del objeto.
    [SerializeField] private TMP_Text itemUsesText;
    // Guarda una referencia al objeto que seleccionamos. Si no hay ningún objeto seleccionado, su valor será null.
    private InventoryItem selectedItem = null;
    // Guarda la pieza que queremos combinar con otro objeto.
    private InventoryItem itemToCombine = null;

    [SerializeField] private Button useButton; // Botón que permitirá utilizar el objeto seleccionado.
    [SerializeField] private Button combineButton; // Botón que permitirá combinar objetos del inventario.

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
        // Conecta el botón COMBINAR con su función.
        if (combineButton != null)
        {
            combineButton.onClick.AddListener(StartCombination);
        }
        // Al comenzar la partida, oculta el botón USAR.
        if (useButton != null)
        {
            useButton.gameObject.SetActive(false);
        }
        // Al comenzar, oculta el botón COMBINAR.
        if (combineButton != null)
        {
            combineButton.gameObject.SetActive(false);
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

        foreach (var item in items)
        {
            // Obtiene la posición real del objeto.
            int startSlot = item.startSlot;

            // Consulta cuántos espacios ocupa.
            int slotsRequired = item.itemData.slotsRequired;

            // Comprueba si hicimos clic en alguna de sus casillas.
            if (slotIndex >= startSlot && slotIndex < startSlot + slotsRequired)
            {

                // Si estamos combinando objetos, esta selección corresponde al segundo objeto de la combinación.
                if (itemToCombine != null)
                {
                    // Guarda la pieza antes de finalizar la combinación.
                    InventoryItem repairPart = itemToCombine;

                    // Desactiva el modo combinación.
                    itemToCombine = null;

                    // Si selecciona la misma pieza, cancelamos.
                    if (item == repairPart)
                    {
                        Debug.Log("Combinación cancelada.");
                    }
                    // Si selecciona un objeto con durabilidad, intenta repararlo utilizando la pieza.
                    else if (item.itemData.hasDurability)
                    {
                        bool repaired = itemsManager.RepairItem(
                            item.itemData,
                            repairPart.itemData
                        );

                        if (repaired)
                        {
                            Debug.Log("Combinación realizada correctamente.");
                        }
                        else
                        {
                            Debug.Log("No se pudo realizar la reparación.");
                        }
                    }
                    else
                    {
                        Debug.Log("Estos objetos no se pueden combinar.");
                    }
                }

                // Guarda qué objeto seleccionó el jugador. Así puede identificarlo cuando quiera usarlo o eliminarlo.
                selectedItem = item;
                // Muestra COMBINAR solamente para piezas de reparación.
                if (combineButton != null)
                {
                    bool canCombine = item.itemData.itemType == ItemType.Repair;
                    combineButton.gameObject.SetActive(canCombine);
                }

                // Permite usar cualquier consumible que tenga usos disponibles.
                bool canUse = item.itemData.itemType == ItemType.Consumable && item.remainingUses > 0;

                // Muestra u oculta el botón según el resultado.
                if (useButton != null)
                {
                    useButton.gameObject.SetActive(canUse);
                }
                // Muestra el nombre del objeto.
                itemNameText.text = item.itemData.itemName;

                // Muestra su descripción.
                itemDescriptionText.text = item.itemData.description;

                // Si el objeto tiene durabilidad, muestra su estado.
                if (item.itemData.hasDurability)
                {
                    itemUsesText.text = "Durabilidad: <color=#8FCF9B>" +
                        item.currentDurability + "/" +
                        item.itemData.maxDurability + "</color>";
                }
                else
                {
                    // Para los demás objetos sigue mostrando sus usos.
                    itemUsesText.text = "Cantidad restante: <color=#8FCF9B>" +
                        item.remainingUses + "/" +
                        item.itemData.maxUses + "</color>";
                }

                return;
            }
        }
        // Si selecciona una casilla vacía, cancela cualquier combinación pendiente.
        if (itemToCombine != null)
        {
            itemToCombine = null;
            Debug.Log("Combinación cancelada.");
        }

        // Si la casilla está vacía, quita la selección.
        selectedItem = null;
        // No hay objeto seleccionado, oculta COMBINAR.
        if (combineButton != null)
        {
            combineButton.gameObject.SetActive(false);
        }
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

        // Solamente permite utilizar objetos consumibles.
        if (selectedItem.itemData.itemType != ItemType.Consumable)
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
        List<InventoryItem> items = itemsManager.GetItems();

        int firstSlot = -1;
        int lastSlot = -1;

        foreach (InventoryItem item in items)
        {
            // Obtiene la posición real del objeto.
            int startSlot = item.startSlot;

            // Consulta cuántas casillas ocupa.
            int slotsRequired = item.itemData.slotsRequired;

            // Comprueba si seleccionamos alguna de sus casillas.
            if (selectedIndex >= startSlot &&
                selectedIndex < startSlot + slotsRequired)
            {
                // Primera casilla ocupada por el objeto.
                firstSlot = startSlot;

                // Última casilla ocupada por el objeto.
                lastSlot = startSlot + slotsRequired - 1;

                break;
            }
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
        // Primero limpia los íconos de las 8 casillas.
        foreach (Image icon in slotIcons)
        {
            if (icon != null)
            {
                icon.sprite = null;
                icon.gameObject.SetActive(false);
            }
        }

        // Después oculta todas las imágenes grandes.
        foreach (Image icon in largeItemIcons)
        {
            if (icon != null)
            {
                icon.gameObject.SetActive(false);
            }
        }

        List<InventoryItem> items = itemsManager.GetItems();

        // Indica qué imagen grande vamos a utilizar.
        int largeIconIndex = 0;

        foreach (InventoryItem item in items)
        {
            // Posición real del objeto dentro del inventario.
            int slotIndex = item.startSlot;

            if (slotIndex < 0 || slotIndex >= slotIcons.Length)
                continue;

            ItemData data = item.itemData;

            // Objetos que ocupan dos casillas.
            if (data.slotsRequired == 2)
            {
                // Busca una imagen grande disponible.
                if (largeIconIndex < largeItemIcons.Length)
                {
                    Image largeIcon = largeItemIcons[largeIconIndex];

                    if (largeIcon != null && data.icon != null)
                    {
                        // Asigna el sprite correspondiente.
                        largeIcon.sprite = data.icon;

                        // Evita deformar la imagen.
                        largeIcon.preserveAspect = true;

                        // Activa la imagen.
                        largeIcon.gameObject.SetActive(true);

                        // La coloca sobre las dos casillas.
                        PositionLargeItemIcon(item, largeIcon);
                    }

                    // La próxima imagen grande será para otro objeto.
                    largeIconIndex++;
                }
            }
            else
            {
                // Los objetos de una casilla siguen funcionando igual.
                if (slotIcons[slotIndex] != null && data.icon != null)
                {
                    slotIcons[slotIndex].sprite = data.icon;
                    slotIcons[slotIndex].gameObject.SetActive(true);
                }
            }
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

            // El objeto seleccionado desapareció del inventario.
            if (combineButton != null)
            {
                combineButton.gameObject.SetActive(false);
            }

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
            // Mantiene el resaltado en la posición actual del objeto.
            HighlightSelectedSlot(selectedItem.startSlot);
            // Actualiza el texto según el tipo de objeto seleccionado.
            if (selectedItem.itemData.hasDurability)
            {
                itemUsesText.text = "Durabilidad: <color=#8FCF9B>" +
                    selectedItem.currentDurability + "/" +
                    selectedItem.itemData.maxDurability + "</color>";
            }
            else
            {
                itemUsesText.text = "Cantidad restante: <color=#8FCF9B>" +
                    selectedItem.remainingUses + "/" +
                    selectedItem.itemData.maxUses + "</color>";
            }
        }
    }

    // Inicia el proceso de combinación de objetos.
    private void StartCombination()
    {
        // Comprobacion que haya un objeto seleccionado.
        if (selectedItem == null)
        {
            return;
        }

        // Por ahora solo permite iniciar combinaciones utilizando piezas de reparación.
        if (selectedItem.itemData.itemType != ItemType.Repair)
        {
            return;
        }

        // Guarda la pieza seleccionada.
        itemToCombine = selectedItem;

        // Indica al jugador que seleccione el segundo objeto.
        itemDescriptionText.text =
            "Seleccioná el arma que querés reparar.";

        // Oculta los botones mientras elegimos el arma.
        if (combineButton != null)
        {
            combineButton.gameObject.SetActive(false);
        }

        if (useButton != null)
        {
            useButton.gameObject.SetActive(false);
        }

        Debug.Log("Modo combinación activado.");
    }

    // Cancela cualquier combinación que haya quedado pendiente.
    public void CancelCombination()
    {
        // Si no estamos combinando, no hacemos nada.
        if (itemToCombine == null)
        {
            return;
        }

        // Cancela la combinación.
        itemToCombine = null;

        // Quita la selección actual.
        selectedItem = null;

        // Limpia los textos del panel.
        itemNameText.text = "";
        itemDescriptionText.text = "";
        itemUsesText.text = "";

        // Oculta los botones.
        if (useButton != null)
            useButton.gameObject.SetActive(false);

        if (combineButton != null)
            combineButton.gameObject.SetActive(false);

        // Restaura el borde normal de todas las casillas.
        foreach (Outline outline in slotOutlines)
        {
            if (outline != null)
            {
                outline.effectColor = new Color32(89, 100, 116, 255);
                outline.effectDistance = new Vector2(1, 1);
            }
        }

        Debug.Log("Combinación cancelada al cerrar el inventario.");
    }

    // Coloca una imagen grande sobre las dos casillas de un objeto.
    private void PositionLargeItemIcon(InventoryItem item, Image icon)
    {
        // Comprobacion que el objeto y la imagen sean válidos.
        if (item == null || icon == null)
            return;

        int startSlot = item.startSlot;

        // Solamente trabaja con objetos de dos espacios.
        if (item.itemData.slotsRequired != 2)
            return;

        // Evita posiciones fuera del inventario.
        if (startSlot < 0 || startSlot + 1 >= slotButtons.Length)
            return;

        // Obtiene las dos casillas ocupadas.
        RectTransform firstSlot =
            slotButtons[startSlot].GetComponent<RectTransform>();

        RectTransform secondSlot =
            slotButtons[startSlot + 1].GetComponent<RectTransform>();

        // Obtiene el RectTransform de la imagen recibida.
        RectTransform largeRect =
            icon.GetComponent<RectTransform>();

        RectTransform parentRect =
            largeRect.parent as RectTransform;

        // Calcula las posiciones respecto al padre.
        Vector3[] corners = new Vector3[4];

        firstSlot.GetWorldCorners(corners);
        Vector3 firstLeft = parentRect.InverseTransformPoint(corners[0]);

        secondSlot.GetWorldCorners(corners);
        Vector3 secondRight = parentRect.InverseTransformPoint(corners[2]);

        // Calcula el centro y el tamaño.
        Vector3 center = (firstLeft + secondRight) / 2f;

        float width = secondRight.x - firstLeft.x;
        float height = secondRight.y - firstLeft.y;

        float margin = 12f;

        largeRect.anchorMin = new Vector2(0.5f, 0.5f);
        largeRect.anchorMax = new Vector2(0.5f, 0.5f);
        largeRect.pivot = new Vector2(0.5f, 0.5f);

        largeRect.anchoredPosition = new Vector2(center.x, center.y);

        largeRect.sizeDelta = new Vector2(
            Mathf.Max(0f, width - margin * 2f),
            Mathf.Max(0f, height - margin * 2f)
        );
    }
}

