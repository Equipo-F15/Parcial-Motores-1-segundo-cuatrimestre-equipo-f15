using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speedWalk= 10f;

    [SerializeField] private float speedRun= 20f;

    private float sprintTimer = 10;

    private float currentspeed;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
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

        if (Input.GetKey(KeyCode.LeftShift))
        {
            if (sprintTimer > 0)
            {
                currentspeed = speedRun;
                sprintTimer = -Time.deltaTime;
                Debug.Log(sprintTimer);

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

        if (Input.GetKey(KeyCode.A))
        {

            //angulo euler rotar en y
            transform.Rotate(0, 30.0f * Time.deltaTime, 0, Space.Self );

        }

        if (Input.GetKey(KeyCode.D))
        {

            //angulo euler rotar en y
            transform.Rotate(0, -30.0f * Time.deltaTime, 0, Space.Self);

        }
    }
}
