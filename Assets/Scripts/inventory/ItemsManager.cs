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

    // Comprueba si tenemos un objeto determinado en el inventario.
    public bool HasItem(ItemData itemToFind)
    {
        // Evita buscar un objeto que no existe.
        if (itemToFind == null)
        {
            return false;
        }

        // Recorre todos los objetos guardados.
        foreach (InventoryItem item in items)
        {
            // Compara el tipo de objeto que estamos buscando.
            if (item.itemData == itemToFind)
            {
                return true;
            }
        }

        // Termina de buscar y no lo encontramos.
        return false;
    }

    // Busca y elimina una unidad de un objeto específico.
    public bool RemoveItemByData(ItemData itemToRemove)
    {
        // Comprueba que recibimos un objeto válido.
        if (itemToRemove == null)
        {
            return false;
        }

        // Recorre los objetos del inventario.
        foreach (InventoryItem item in items)
        {
            // Comprueba si encontramos el objeto solicitado.
            if (item.itemData == itemToRemove)
            {
                // Aprovecha el método RemoveItem ya creado. Este también avisa a la interfaz para actualizarse.
                return RemoveItem(item);
            }
        }
        return false;
    }

    // Descuenta durabilidad de un objeto del inventario.
    public bool ReduceDurability(ItemData itemData, int amount)
    {
        // Comprueba que el objeto sea válido y tenga durabilidad.
        if (itemData == null || !itemData.hasDurability)
        {
            return false;
        }

        // No permite descontar cantidades negativas o cero.
        if (amount <= 0)
        {
            return false;
        }

        // Busca el objeto dentro del inventario.
        foreach (InventoryItem item in items)
        {
            if (item.itemData == itemData)
            {
                // Si ya está roto, no podemos desgastarlo más.
                if (item.currentDurability <= 0)
                {
                    return false;
                }

                // Descuenta la durabilidad sin bajar de cero.
                item.currentDurability =
                    Mathf.Max(0, item.currentDurability - amount);

                Debug.Log("Durabilidad de " + itemData.itemName +
                          ": " + item.currentDurability +
                          "/" + itemData.maxDurability);

                // Avisa a la interfaz que hubo un cambio.
                OnInventoryChanged?.Invoke();

                return true;
            }
        }

        // No encuentra el objeto en el inventario.
        return false;
    }

    // Repara un objeto utilizando una pieza de reparación.
    public bool RepairItem(ItemData itemToRepair, ItemData repairPart)
    {
        // Comprobacion que ambos objetos sean válidos.
        if (itemToRepair == null || repairPart == null)
        {
            return false;
        }

        // El objeto debe utilizar durabilidad.
        if (!itemToRepair.hasDurability)
        {
            return false;
        }

        // Comprobacion que tengamos una pieza de reparación.
        if (!HasItem(repairPart))
        {
            Debug.Log("No tenés piezas de reparación.");
            return false;
        }

        // Busca el objeto que queremos reparar.
        foreach (InventoryItem item in items)
        {
            if (item.itemData == itemToRepair)
            {
                // No gasta piezas si el objeto está en perfecto estado.
                if (item.currentDurability >= itemToRepair.maxDurability)
                {
                    Debug.Log("El objeto ya tiene la durabilidad máxima.");
                    return false;
                }

                // Restaura su durabilidad.
                item.currentDurability = itemToRepair.maxDurability;

                // Consume una pieza de reparación.
                RemoveItemByData(repairPart);

                Debug.Log("Reparaste: " + itemToRepair.itemName);

                // Avisa a la interfaz que hubo cambios.
                OnInventoryChanged?.Invoke();

                return true;
            }
        }

        // No encuentra el objeto que queremos reparar.
        return false;
    }

}
