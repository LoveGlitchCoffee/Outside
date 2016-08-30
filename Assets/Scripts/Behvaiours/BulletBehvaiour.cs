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
    private bool landed;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        trail = transform.GetChild(1).GetComponent<TrailRenderer>();
    }

    void Start()
    {
        this.RegisterListener(EventID.OnGameEnd, (sender, param) => ReturnToPool());
    }

    void FixedUpdate()
    {
        if (projecting)
        {
            rb.velocity = bulletForce;
        }
    }

    void OnCollisionEnter(Collision col)
    {
        projecting = false;

        if (col.gameObject.CompareTag("Zombie"))
        {
            enemyHit++;

            if (enemyHit == 2 && !landed)
            {
                this.PostEvent(EventID.OnDoubleKill);
            }
            else if (enemyHit > 2 && !landed)
            {
                this.PostEvent(EventID.OnMultiKill);
            }
            else
            {
                this.PostEvent(EventID.OnNormalKill);
            }
        }
        else if (col.gameObject.CompareTag("Ground"))
            landed = true;
    }

    public void Project(Vector3 force)
    {
        rb.isKinematic = false;
        projecting = true;

        transform.SetParent(null);

        bulletForce = force;

        trail.enabled = true;
        landed = false;

        StartCoroutine(WaitTillNoForce());
        StartCoroutine(WaitTillDissapear());
    }

    private IEnumerator WaitTillNoForce()
    {
        yield return new WaitForSeconds(0.2f);

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
    }
}
