using UnityEngine;


public class State : MonoBehaviour
{

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerEnter(Collider player)
    {
        if (player.CompareTag("Player"))
            {
            AudioMixer.SetFloat(VolumenTranuilidad, 0f);
            AudioMixer.SetFloat(VolumenExplore, -80f);
        }
    }

    void OnTriggerExit(Collider player)
    {
        if (player.CompareTag("Player"))
            {
            AudioMixer.SetFloat(VolumenTranuilidad, -80f);
            AudioMixer.SetFloat(VolumenExplore, 0f);
        }
    }

}
