using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speedWalk= 10f;

    [SerializeField] private float speedRun= 20f;

    private float sprintTimer = 10;

    private float currentspeed;

   
    private int currentLife;
    private int maximunLife;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentLife = maximunLife;
        currentspeed = speedWalk;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            transform.Translate(new Vector3(0, 0, 1) * currentspeed * Time.deltaTime);
        }


        if (Input.GetKey(KeyCode.S))
        {
            transform.Translate(new Vector3(0, 0, -1) * currentspeed * Time.deltaTime);

        }

        if (Input.GetKey(KeyCode.LeftShift)) //sprint con temporizador y solo hacia delante
        {
            if (sprintTimer > 0)
            {
                if (Input.GetKey(KeyCode.W))
                {
                    currentspeed = speedRun;
                    sprintTimer = -Time.deltaTime;
                    Debug.Log(sprintTimer);
                }
            }
        }
        else
        {
            currentspeed = speedWalk;// para cambio de velocidad, se aceptan sugerencias
            if (sprintTimer < 10)
            {
                sprintTimer = +Time.deltaTime;
                Debug.Log(sprintTimer);
                    }
        }

        if (Input.GetKey(KeyCode.D))//control de jugador gira sobre si mismo
        {
            //angulo euler rotar en y
            transform.Rotate(0, 50.0f * Time.deltaTime, 0, Space.Self );

        }

        if (Input.GetKey(KeyCode.A))
        {
            //angulo euler rotar en y
            transform.Rotate(0, -50.0f * Time.deltaTime, 0, Space.Self);

        }
    }
    //fin update
     
    void OnCollisionEnter(Collision Collision)
    {
        if (Collision.gameObject.CompareTag("Enemy"))
        {
            currentLife--;
            Debug.Log("tu vida es " + currentLife);

            if (currentLife == 0)
            {
                Debug.Log("estas muerto");
            }
        }
    }




}
