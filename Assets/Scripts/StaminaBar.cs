using UnityEngine;
using UnityEngine.UI;

public class StaminaBarTest : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] PlayerMovement playerscript;
    public Slider slider;
    public float maxStamina;
    public float currentStamina;
    

    void Start()
    {
        playerscript = player.GetComponent<PlayerMovement>();
        maxStamina = playerscript.maxStamina;
        slider.maxValue = playerscript.maxStamina;
        slider.value = currentStamina;
    }

    void Update()
    {
        currentStamina = playerscript.sprintTimer;
        //currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);
        slider.value = currentStamina;
    }
}