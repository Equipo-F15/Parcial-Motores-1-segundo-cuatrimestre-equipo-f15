using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField]float attackRange;
    [SerializeField] private Transform rayOrigin;
    private LifeScript lifeScript;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            bool huboImpacto = Physics.Raycast(rayOrigin.position, rayOrigin.forward, out hit, attackRange);
            if (huboImpacto)
            {
                //Debug.Log("Le pegó a: " + hit.collider.name);

                lifeScript = hit.collider.GetComponentInParent<LifeScript>();

                if (lifeScript != null)
                {
                    //Debug.Log("Tiene IInteractable, llamando Interact()");
                    lifeScript.TakesDamage(1);
                }
                else
                {
                    Debug.Log("El objeto NO tiene IInteractable");
                }
            }

        }
    }
}
