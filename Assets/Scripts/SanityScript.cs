using UnityEngine;

public class SanityScript : MonoBehaviour
{

    public float sanity;
    [SerializeField] float sanityMax;
    public float sanityToSpawn;

    public float timerFakeMSpawner;
    [SerializeField] float timerFakeMSpawnerMax;

    public GameObject fakeMutant;

    public float spawnerRange; //rango por el cual patrulla
    public Transform spawnerCenterPoint;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        sanity = Mathf.Clamp(sanity, 0.0f, sanityMax);//sanity limiter

        sanity-=Time.deltaTime;

        if (sanity <= sanityToSpawn)
        {
            timerFakeMSpawner -= Time.deltaTime;
            if (timerFakeMSpawner < 0.0f)
            { timerFakeMSpawner = timerFakeMSpawnerMax;
                Vector3 point;
                if (RandomPoint(transform.position, spawnerRange, out point))
                {
                    
                   //instanciate fakeMutant(point);
                }
            }
        }

        
    }
    bool RandomPoint(Vector3 center, float range, out Vector3 result)
    {
        Vector3 randomPoint = center + Random.insideUnitSphere * range;
        UnityEngine.AI.NavMeshHit hit;
        if (UnityEngine.AI.NavMesh.SamplePosition(randomPoint, out hit, 1.0f, UnityEngine.AI.NavMesh.AllAreas))
        {
            //the 1.0f is the max distance from the random point to a point on the navmesh, might want to increase if range is big
            result = hit.position;
            return true;
        }
        result = Vector3.zero;
        return false;
    }
}
