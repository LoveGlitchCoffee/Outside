using UnityEngine;
using System.Collections;
using System.Runtime.Remoting.Messaging;

public class BulletBehvaiour : MonoBehaviour
{
    private TrailRenderer trail;

    private Rigidbody rb;
    WaitForSeconds dissWait = new WaitForSeconds(5f);
    private bool projecting;

    private Vector3 bulletForce;
    private float speed = 10f;

    private int enemyHit;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        trail = transform.GetChild(1).GetComponent<TrailRenderer>();
    }

    void Start()
    {
        
    }

    void FixedUpdate()
    {
        if (projecting)
            rb.velocity = bulletForce;
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Zombie")
        {
            enemyHit++;

            if (enemyHit == 2)
            {
                this.PostEvent(EventID.OnDoubleKill);
            }
            else if (enemyHit > 2)
            {
                this.PostEvent(EventID.OnMultiKill);
            }            
        }        
    }

    public void Project(Vector3 force)
    {
        rb.isKinematic = false;
        projecting = true;

        transform.SetParent(null);

        bulletForce = force;
        //Debug.Log(bulletForce);

        StartCoroutine(WaitTillNoForce());
        StartCoroutine(WaitTillDissapear());
    }

    private IEnumerator WaitTillNoForce()
    {
        yield return new WaitForSeconds(0.05f);

        projecting = false;
    }

    private IEnumerator WaitTillDissapear()
    {
        yield return dissWait;

        ReturnToPool();
    }

    public void ReturnToPool()
    {
        rb.isKinematic = true;
        trail.enabled = false;
        PoolManager.Instance.ReturnToPool(gameObject);
    }

    public void SetUp()
    {
        enemyHit = 0;
        trail.enabled = true;
    }
}
