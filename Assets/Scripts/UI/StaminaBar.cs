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
    if (player == null)
        player = GameObject.FindGameObjectWithTag("Player");

    if (player == null)
    {
        Debug.LogError("StaminaBarTest: no object with tag Player found", this);
        enabled = false;
        return;
    }

    playerscript = player.GetComponent<PlayerMovement>();
    if (playerscript == null)
    {
        Debug.LogError("StaminaBarTest: Player has no PlayerMovement script", this);
        enabled = false;
        return;
    }

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