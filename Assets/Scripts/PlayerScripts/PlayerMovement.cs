using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    private Animator animator;
    [SerializeField] private float speedWalk= 10f;

    [SerializeField] private float speedRun= 20f;
    public float maxStamina;
    [SerializeField]  public float sprintTimer;
    [SerializeField] bool IsRunning;
    private float currentspeed;

    
    [SerializeField] private int currentLife;
    [SerializeField] private int maximunLife;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        currentLife = maximunLife;
        currentspeed = speedWalk;
        sprintTimer = maxStamina;
        
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetBool("IsWalking", false);//retorno a idle
        animator.SetBool("IsRunning", false);
        currentspeed = speedWalk;
        IsRunning = false;

        if (sprintTimer < maxStamina && IsRunning==false)
        {
            sprintTimer += Time.deltaTime;
            //Debug.Log(sprintTimer);
        }
        

        if (Input.GetKey(KeyCode.W) )
        {
            if (Input.GetKey(KeyCode.LeftShift) && sprintTimer > 0) //sprint con temporizador y solo hacia delante
            {
                IsRunning = true;
                transform.Translate(new Vector3(0, 0, 1) * currentspeed * Time.deltaTime);
                currentspeed = speedRun;
                animator.SetBool("IsRunning", true);
                sprintTimer = sprintTimer-2*Time.deltaTime;
                //Debug.Log(sprintTimer);
            }
            else
            {
                animator.SetBool("IsWalking", true);
                transform.Translate(new Vector3(0, 0, 1) * currentspeed * Time.deltaTime);
            }
        }


        if (Input.GetKey(KeyCode.S))
        {
            animator.SetBool("IsWalking", true);
            transform.Translate(new Vector3(0, 0, -1) * currentspeed * Time.deltaTime);
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

    //animaciones control
    //void OnAnimatorMove()
    //{
    //    if (animator.GetBool("IsWalking")| animator.GetBool("IsRunning"))
    //    {
    //        currentspeed = (animator.deltaPosition / Time.deltaTime).magnitude;
    //    }
    //}




}
