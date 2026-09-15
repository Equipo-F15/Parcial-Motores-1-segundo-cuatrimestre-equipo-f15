using UnityEngine;

public class ComingSoonTrigger : MonoBehaviour
{
    public GameObject whiteScreen;

    //Cuando el jugador pasa por la puerta, se activa la pantalla blanca 
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            whiteScreen.SetActive(true);
        }
    }
}