using System;
using System.Collections.Generic;
using UnityEngine;

public class ItemsManager : MonoBehaviour
{
    // Cantidad máxima de espacios del inventario.
    [SerializeField] private int maxSlots = 8;

    // Evento que avisa cuando cambia el contenido del inventario. Otros scripts como InventoryUI, podrán escuchar este aviso.
    public event Action OnInventoryChanged;

    // Lista que almacena los objetos que lleva el jugador.
    [SerializeField]
    private List<InventoryItem> items =
        new List<InventoryItem>();

    // Calcula cuántos espacios están ocupados.
    public int GetUsedSlots()
    {
        int usedSlots = 0;

        // Recorre todos los objetos del inventario.
        foreach (InventoryItem item in items)
        {
            usedSlots += item.itemData.slotsRequired;
        }

        return usedSlots;
    }

    // Comprueba si hay espacio suficiente para un objeto.
    public bool HasSpace(ItemData newItem)
    {
        int availableSlots = maxSlots - GetUsedSlots();

        return availableSlots >= newItem.slotsRequired;
    }

    // Intenta agregar un objeto al inventario.
    public bool AddItem(ItemData newItem)
    {
        // Comprobar que el objeto exista.
        if (newItem == null)
        {
            Debug.LogWarning("No se asignó ningún objeto.");
            return false;
        }

        // Si no hay espacio, no podemos recogerlo.
        if (!HasSpace(newItem))
        {
            Debug.Log("Inventario lleno. No hay espacio para "
                + newItem.itemName);

            return false;
        }

        // Crea una instancia del objeto con sus usos iniciales.
        InventoryItem inventoryItem = new InventoryItem(newItem);

        // Agrega el objeto a la lista.
        items.Add(inventoryItem);

        // Avisa que el inventario cambió.
        OnInventoryChanged?.Invoke();

        Debug.Log("Recogiste: " + newItem.itemName);

        Debug.Log("Espacios ocupados: "
            + GetUsedSlots() + "/" + maxSlots);

        return true;
    }

    // Permite consultar los objetos almacenados.
    public List<InventoryItem> GetItems()
    {
        return items;
    }

    // Elimina un objeto específico del inventario.
    public bool RemoveItem(InventoryItem item)
    {
        // Comprobar que el objeto exista.
        if (item == null)
        {
            return false;
        }

        // Intenta eliminarlo de la lista.
        bool removed = items.Remove(item);

        // Si consigue eliminarlo, avisa a la interfaz.
        if (removed)
        {
            Debug.Log("Objeto eliminado: " + item.itemData.itemName);

            // Avisa que cambió el contenido del inventario.
            OnInventoryChanged?.Invoke();
        }

        // Devuelve el resultado de la operación.
        return removed;
    }

}
