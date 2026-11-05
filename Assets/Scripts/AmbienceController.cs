using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AmbienceController : MonoBehaviour
{
    [Header("Volume")]
    [SerializeField] private float outdoorVolume = 0.5f;
    [SerializeField] private float indoorVolume = 0.15f;

    [Header("Muffle")]
    [SerializeField] private AudioLowPassFilter lowPass;
    [SerializeField] private float outdoorCutoff = 22000f;
    [SerializeField] private float indoorCutoff = 1200f;

    [SerializeField] private float fadeSpeed = 2f;

    private AudioSource source;
    private bool isInside;

    private void Awake()
    {
        source = GetComponent<AudioSource>();
        source.volume = outdoorVolume;
        if (lowPass != null) lowPass.cutoffFrequency = outdoorCutoff;
    }

    private void Update()
    {
        float targetVolume = isInside ? indoorVolume : outdoorVolume;
        source.volume = Mathf.MoveTowards(source.volume, targetVolume, fadeSpeed * Time.deltaTime);

        if (lowPass != null)
        {
            float targetCutoff = isInside ? indoorCutoff : outdoorCutoff;
            lowPass.cutoffFrequency = Mathf.MoveTowards(lowPass.cutoffFrequency, targetCutoff, 20000f * Time.deltaTime);
        }
    }

    public void SetInside(bool inside)
    {
        isInside = inside;
    }
}
