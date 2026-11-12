using System;
using UnityEngine;

[Serializable]
public class InventoryItem
{
    // Referencia al objeto que creamos con ItemData.
    public ItemData itemData;

    // Cantidad de usos que le quedan a ESTE objeto.
    public int remainingUses;

    // Constructor: se ejecuta cuando creamos un nuevo objeto dentro del inventario.
    public InventoryItem(ItemData data)
    {
        // Guardamos la información del objeto.
        itemData = data;

        // El objeto comienza con todos sus usos.
        remainingUses = data.maxUses;
    }
}

