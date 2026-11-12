using UnityEngine;

// Enum que define los diferentes tipos de objetos que podemos tener dentro del inventario.
public enum ItemType
{
    Weapon,      // Armas
    Consumable,  // Objetos que se consumen, como medicamentos
    Repair,      // Materiales para reparar armas
    KeyItem      // Objetos importantes como la llave
}

// Permite crear objetos desde el menú Create de Unity. En Create > Inventory > Item.
[CreateAssetMenu(fileName = "NewItem", menuName = "Inventory/Item")]

public class ItemData : ScriptableObject
{
    // Nombre del objeto que aparecerá en el inventario.
    public string itemName;

    // Descripción que se mostrará al seleccionar el objeto. TextArea permite escribir varias líneas en el Inspector.
    [TextArea]
    public string description;

    // Imagen que representará al objeto en su casilla.
    public Sprite icon;

    // Indica a qué categoría pertenece el objeto.
    public ItemType itemType;

    // Cantidad de espacios que ocupa en el inventario. Range limita el valor entre 1 y 8 en el Inspector.
    [Range(1, 8)]
    public int slotsRequired = 1;

    // Indica si podemos acumular varias unidades del mismo objeto en un solo espacio.
    public bool canStack = false;

    // Cantidad máxima de unidades que podemos acumular. Min evita que coloquemos un valor menor a 1.
    [Min(1)]
    public int maxStack = 1;

    // Cantidad de usos que tiene un objeto al recogerlo.
    [Min(1)]
    public int maxUses = 1;

    // Indica si el objeto utiliza un sistema de durabilidad.
    public bool hasDurability = false;

    // Durabilidad máxima del objeto.
    [Min(1)]
    public int maxDurability = 100;

    // Cantidad de vida que recupera el objeto.
    [Min(0)]
    public int healthRecovery = 0;

    // Cantidad de cordura que recupera el objeto.
    [Min(0)]
    public int sanityRecovery = 0;
}
