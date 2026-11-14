using UnityEngine;
using TMPro;

public class DoorDialogue : MonoBehaviour
{
    [Header("Configuración del diálogo")]
    public AudioClip dialogueAudio; // Audio que reproducirá esta puerta.

    [TextArea(2, 4)]
    public string subtitle; // Frase que aparecerá como subtítulo mientras suena el audio.

    [Header("Interacción")]
    public float reachRange = 3f; // Distancia máxima desde la cámara para interactuar con la puerta.

    [Header("Subtítulos")]
    public TextMeshProUGUI subtitleText; // Referencia al texto que creamos dentro de DialogueCanvas.

    private Camera fpsCam; // Guarda la cámara principal del jugador.
    private AudioSource audioSource; // Componente encargado de reproducir la voz.
    private bool dialoguePlaying; // Indica si esta puerta está reproduciendo un diálogo.

    // Esta variable es compartida por todas las puertas que tengan DoorDialogue. Guarda qué puerta está hablando actualmente.
    private static DoorDialogue activeDialogue;

    void Start()
    {
        // Busca la cámara principal del jugador.
        fpsCam = Camera.main;

        // Busca si esta puerta ya tiene un componente AudioSource.
        audioSource = GetComponent<AudioSource>();

        // Si no tiene AudioSource, agrega uno automáticamente.
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Evita que el audio se reproduzca automáticamente al iniciar el juego.
        audioSource.playOnAwake = false;

        // Evita que el audio se repita en bucle.
        audioSource.loop = false;

        // Oculta el subtítulo al iniciar, siempre que no haya otro diálogo activo.
        if (subtitleText != null && activeDialogue == null)
        {
            subtitleText.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        // Si esta puerta estaba hablando, pero el audio ya no suena...
        if (dialoguePlaying && !audioSource.isPlaying)
        {
            dialoguePlaying = false; // Indica que terminó el diálogo.

            // Comprobacion que esta puerta sea la que estaba hablando.
            if (activeDialogue == this)
            {
                activeDialogue = null; // Libera el sistema para que otra puerta pueda hablar.

                // Oculta el subtítulo cuando termina el audio.
                if (subtitleText != null)
                {
                    subtitleText.gameObject.SetActive(false);
                }
            }
        }
        // Si otra puerta está hablando, no permite iniciar un nuevo diálogo.
        if (activeDialogue != null)
        {
            return;
        }

        // Si no encuentra la cámara o no asigna un audio, detiene este Update.
        if (fpsCam == null || dialogueAudio == null)
        {
            return;
        }

        // Crea un rayo que sale desde el centro de la cámara hacia adelante.
        Ray ray = fpsCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));

        // Esta variable guardará la información del objeto que toque el rayo.
        RaycastHit hit;

        // Comprobacion si el rayo toca algún Collider dentro de la distancia permitida.
        if (Physics.Raycast(ray, out hit, reachRange))
        {
            // Comprobar si el Collider pertenece a esta puerta o a uno de sus hijos.
            if (hit.collider.transform == transform || hit.collider.transform.IsChildOf(transform))
            {
                // Si el jugador suelta la tecla E mientras apunta a esta puerta...
                if (Input.GetKeyUp(KeyCode.E))
                {
                    // Asigna al AudioSource el archivo de voz configurado.
                    audioSource.clip = dialogueAudio;

                    // Reproduce la voz del habitante.
                    audioSource.Play();

                    // Indica que esta puerta está reproduciendo un diálogo.
                    dialoguePlaying = true;

                    // Registra esta puerta como la que está hablando actualmente.
                    activeDialogue = this;

                    // Si asigna un texto de subtítulos en el Inspector...
                    if (subtitleText != null)
                    {
                        subtitleText.text = subtitle; // Muestra la frase correspondiente al audio.
                        subtitleText.gameObject.SetActive(true); // Hace visible el subtítulo.
                    }
                }
            }
        }
    }
    // Unity ejecuta este método cuando el componente o su GameObject se desactiva.
    void OnDisable()
    {
        // Comprueba si la puerta desactivada era la que estaba hablando.
        if (activeDialogue == this)
        {
            activeDialogue = null; // Permite que otras puertas puedan reproducir diálogos.

            // Oculta el subtítulo para que no quede en pantalla.
            if (subtitleText != null)
            {
                subtitleText.gameObject.SetActive(false);
            }
        }
    }
}
