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
            if (items > 0) //Si tiene mas de 0 items
            {
                items--;

            Debug.Log("Consumiste 1 medicamento, te quedan " + items);
            }
            else
            {
                Debug.Log("No te quedan medicamentos");
            }
        }
    }
    public void RecogerItemDesdeSuelo()
    {
        if (items < maxItems) //Si tiene menos de la cantidad máxima
        {
            items++;
            Debug.Log("¡Agarraste un medicamento del piso! Total: " + items);
        }
        else // Si no es asi
        {
            Debug.Log("Inventario lleno, no podés agarrarlo.");
        }
    }
}