using UnityEngine;

public class DurabilityTester : MonoBehaviour
{
    // Referencia al inventario del jugador.
    [SerializeField] private ItemsManager itemsManager;

    // Objeto al que queremos reducirle la durabilidad.
    [SerializeField] private ItemData knifeData;

    private void Update()
    {
        // Al presionar K, simulamos el desgaste del cuchillo.
        if (Input.GetKeyDown(KeyCode.K))
        {
            // Descontar 25 puntos de durabilidad.
            bool success = itemsManager.ReduceDurability(knifeData, 25);

            if (!success)
            {
                Debug.Log("No se pudo desgastar el cuchillo.");
            }
        }
    }
}
