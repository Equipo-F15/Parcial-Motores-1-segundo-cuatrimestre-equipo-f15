using UnityEngine;
using System.Collections;

public class EnemyVision : MonoBehaviour
{

    bool playerIsInAngle;
    bool playerIsInRange;
    bool playerIsNotHidden;

    [SerializeField] private GameObject player;

    [SerializeField] private float detectRange;
    [SerializeField] private float detectAngle;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(transform.position, player.transform.position) < detectRange)
        {
            playerIsInRange = true;
            Debug.Log("Player in range");
        }
        else
        { playerIsInRange = false; }

        RaycastHit hit;
        if (Physics.Raycast(transform.position, (player.transform.position - transform.position), out hit, Mathf.Infinity))
        {
            if (hit.transform == player.transform)
            {
                playerIsNotHidden = true;
                Debug.Log("Player in open");
            }
            else { playerIsNotHidden = false; }
        }
        
        Vector3 side1 = player.transform.position - transform.position;
        Vector3 side2 = transform.forward;
        float angle=  Vector3.SignedAngle(side1, side2, Vector3.up);

        if(angle<detectAngle && angle>-1* detectAngle)
        { playerIsInAngle=true;
            Debug.Log("Player in angle");
        }
        else { playerIsInAngle = false; }

        if(playerIsInAngle == true && playerIsInRange == true && playerIsNotHidden == true)
        { Debug.Log("Player in vision"); }

    }
}
