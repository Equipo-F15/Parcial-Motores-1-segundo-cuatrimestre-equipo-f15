using UnityEngine;
using UnityEngine.SceneManagement;

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

    public int CurrentLife => currentLife; // Permite que otros scripts consulten la vida actual del jugador.
    public int MaximumLife => maximunLife;  // Permite que otros scripts consulten la vida máxima del jugador.

    [SerializeField] GameObject dolor;
    [SerializeField] AudioSource dolorAudioSource;
    [SerializeField] private AudioSource cansancioAudioSource; // AudioSource del objeto Cansancio_Jugador.
    private bool sonidoCansancioReproducido = false; // Evita repetir el sonido mientras la energía esté agotada.

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        dolorAudioSource=dolor.GetComponent<AudioSource>();
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
        // Permite volver a reproducir el cansancio cuando se haya recuperado suficiente energía.
        if (sprintTimer >= maxStamina * 0.5f)
        {
            sonidoCansancioReproducido = false;
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
                if (sprintTimer <= 0 && !sonidoCansancioReproducido) // Si la energía llegó a cero, reproduce el sonido de cansancio una sola vez.
                {
                    cansancioAudioSource.PlayOneShot(cansancioAudioSource.clip);
                    sonidoCansancioReproducido = true;
                }
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

    public void Healing(int amount)
    {
        currentLife += amount;
        currentLife = Mathf.Clamp(currentLife, 0, maximunLife);
    }

    public void TakeDamage(int damage)
    {
        currentLife -= damage;
    }

    public void GameOver()
    {
        SceneManager.LoadScene("GameOver");
            }
     
    void OnCollisionEnter(Collision Collision)
    {
        if (Collision.gameObject.CompareTag("Enemy"))
        {
            dolorAudioSource.Play();
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
