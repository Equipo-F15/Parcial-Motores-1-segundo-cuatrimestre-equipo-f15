using UnityEngine;
using TMPro;

public class DoorDialogue : MonoBehaviour, IInteractable
{
    [Header("Configuración del diálogo")]
    public AudioClip dialogueAudio; // Audio que reproducirá esta puerta.

    [TextArea(2, 4)]
    public string subtitle; // Frase que aparecerá como subtítulo mientras suena el audio.

    [Header("Interacción")]
    public float reachRange = 3f; // Distancia máxima desde la cámara para interactuar con la puerta.

    [Header("Subtítulos")]
    public TextMeshProUGUI subtitleText; // Referencia al texto que creamos dentro de DialogueCanvas.

    private AudioSource audioSource; // Componente encargado de reproducir la voz.
    private bool dialoguePlaying; // Indica si esta puerta está reproduciendo un diálogo.

    // Esta variable es compartida por todas las puertas que tengan DoorDialogue. Guarda qué puerta está hablando actualmente.
    private static DoorDialogue activeDialogue;

    void Start()
    {
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
        // Si esta puerta estaba reproduciendo un diálogo y el audio terminó...
        if (dialoguePlaying && !audioSource.isPlaying)
        {
            // Indica que el diálogo terminó.
            dialoguePlaying = false;

            // Comprueba que esta puerta sea la que estaba hablando.
            if (activeDialogue == this)
            {
                // Libera el sistema para permitir otros diálogos.
                activeDialogue = null;

                // Oculta los subtítulos cuando termina la voz.
                if (subtitleText != null)
                {
                    subtitleText.gameObject.SetActive(false);
                }
            }
        }
    }

    public void Interact()
    {
        // Si otra puerta está hablando, no permite iniciar un nuevo diálogo.
        if (activeDialogue != null)
        {
            return;
        }

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
