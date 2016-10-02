using UnityEngine;
using System.Collections;

public class MissleBehaviour : MonoBehaviour
{    

    [Header("Properties")]
    public float MaxSpeed;
    public float TimeTillBlow;
    bool launched = false;
    bool blownUp = false;
    private Vector3 force;
    
    private Vector3 hidePosition;

    Rigidbody rb;
    AudioSource audio;

    public GameObject TrailSmoke;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        audio = GetComponent<AudioSource>();
    }

    void Start()
    {
        hidePosition = transform.position;
    }

    void Update()
    {
        if (launched)
        {
            rb.velocity = force;
        }
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Zombie"))
        {
            BlowUp();
        }
    }

    public void SetUp(Vector3 direction)
    {
        force = direction * MaxSpeed;
        launched = true;
        blownUp = false;
        audio.Play();
        StartCoroutine(WaitTillExplode());
        StartCoroutine(SpawnTrail());
    }

    private IEnumerator SpawnTrail()
    {
        WaitForSeconds wait = new WaitForSeconds(0.05f);

        while (!blownUp)
        {
            PoolManager.Instance.spawnObject(TrailSmoke, transform.position, Quaternion.identity);
            yield return wait;            
        }
    }

    private IEnumerator WaitTillExplode()
    {
        yield return new WaitForSeconds(TimeTillBlow);
        Debug.Log("boom");

        if (!blownUp)
            BlowUp();
    }

    private void BlowUp()
    {        
        audio.Stop();

        blownUp = true;
        launched = false;
        rb.velocity = Vector3.zero;
        this.PostEvent(EventID.OnMissleBlow, transform.position);
        transform.position = hidePosition;
    }
}
