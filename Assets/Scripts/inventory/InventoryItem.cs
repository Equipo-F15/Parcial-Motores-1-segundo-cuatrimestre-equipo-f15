using System;
using UnityEngine;

[Serializable]
public class InventoryItem
{
    // Datos generales del objeto.
    public ItemData itemData;

    // Usos restantes del objeto
    public int remainingUses;

    // Durabilidad actual del objeto
    public int currentDurability;

    // Constructor: se ejecuta cuando agregamos un objeto al inventario.
    public InventoryItem(ItemData data)
    {
        itemData = data;

        // Cada objeto comienza con todos sus usos disponibles.
        remainingUses = data.maxUses;

        // Si el objeto utiliza durabilidad, comienza con el valor máximo.
        if (data.hasDurability)
        {
            currentDurability = data.maxDurability;
        }
    }
}
