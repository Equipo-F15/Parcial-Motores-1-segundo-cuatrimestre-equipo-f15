using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speedwalk= 15f;

    [SerializeField] private float speedrun= 3.5f;

    private float currentspeed;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentspeed = speedwalk;
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
            currentspeed = speedrun;
        }
        else
        {
            currentspeed = speedwalk;// para cambio de velocidad, se aceptan sugerencias
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
