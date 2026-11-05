using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class VariacionSFX : MonoBehaviour
{
    [Header("Configuración de Clips")]
    [SerializeField] private AudioClip[] clipsDeSonido;

    [Header("Rangos de Variación")]
    [Range(0.1f, 0.3f)][SerializeField] private float variacionVolume = 0.05f;
    [Range(0.1f, 0.3f)][SerializeField] private float variacionPitch = 0.05f;

    private AudioSource audioSource;
    private float volumenOriginal;
    private float pitchOriginal;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        volumenOriginal = audioSource.volume;
        pitchOriginal = audioSource.pitch;
    }

    // Esta es la función que se llamará desde otros scripts o eventos
    public void ReproducirSonidoConVariacion()
    {
        if (clipsDeSonido == null || clipsDeSonido.Length == 0) return;

        // 1. Elegir un clip al azar de la lista
        int indiceAleatorio = Random.Range(0, clipsDeSonido.Length);
        audioSource.clip = clipsDeSonido[indiceAleatorio];

        // 2. Aplicar ligeras variaciones para que no suene repetitivo
        audioSource.volume = volumenOriginal + Random.Range(-variacionVolume, variacionVolume);
        audioSource.pitch = pitchOriginal + Random.Range(-variacionPitch, variacionPitch);

        // 3. Reproducir
        audioSource.Play();
    }
}
