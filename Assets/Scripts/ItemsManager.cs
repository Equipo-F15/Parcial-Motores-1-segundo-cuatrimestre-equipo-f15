using UnityEngine;

public class ItemsManager : MonoBehaviour
{
    [SerializeField] int items = 1;
    [SerializeField] int maxItems = 5;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Debug.Log("Tengo " + items + " medicamentos.");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            if (items > 0)
            {
                items--;

            Debug.Log("Consumiste 1 medicamento, te quedan " + items);
            }
            else
            {
                Debug.Log("No te quedan medicamentos");
            }
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (items < maxItems)
            {
                items++;
                Debug.Log("Recogiste 1 medicamento. Total: " + items);
            }
            else //Si ya tenes 5 (o más)
            {
                Debug.Log("No podés llevar más medicamentos. ¡Inventario lleno!");
            }
        }
    }
    public void RecogerItemDesdeSuelo()
    {
        if (items < maxItems)
        {
            items++;
            Debug.Log("¡Agarraste un medicamento del piso! Total: " + items);
        }
        else
        {
            Debug.Log("Inventario lleno, no podés levantarlo.");
        }
    }
}