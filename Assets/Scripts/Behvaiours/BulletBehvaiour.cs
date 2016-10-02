using UnityEngine;
using System.Collections;
using System.Runtime.Remoting.Messaging;

public class BulletBehvaiour : MonoBehaviour
{
    private TrailRenderer trail;

    private Rigidbody rb;
    WaitForSeconds dissWait = new WaitForSeconds(5f);
    public bool projecting;

    private Vector3 bulletForce;
    private float speed = 10f;

    private int enemyHit;
    private bool landed;

    Coroutine dissapearCoroutine;
    Coroutine forceCoroutine;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        trail = transform.GetChild(1).GetComponent<TrailRenderer>();
    }

    void Start()
    {
        this.RegisterListener(EventID.OnGameEnd, (sender, param) => ReturnToPool());
        this.RegisterListener(EventID.OnGameProceed, (sender, param) => ReturnToPool());
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

        StopCoroutine(forceCoroutine);

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
        if (forceCoroutine != null)
            StopCoroutine(forceCoroutine);

        if (dissapearCoroutine != null)
            StopCoroutine(dissapearCoroutine); // just to be sure

        rb.isKinematic = false;
        projecting = true;

        transform.SetParent(null);

        bulletForce = force;

        trail.enabled = true;
        landed = false;

        forceCoroutine = StartCoroutine(WaitTillNoForce());
        dissapearCoroutine = StartCoroutine(WaitTillDissapear());
    }

    private IEnumerator WaitTillNoForce()
    {
        yield return new WaitForSeconds(0.2f);

        Debug.Log(gameObject.name + " stop projecting ");
        projecting = false;
    }

    private IEnumerator WaitTillDissapear()
    {
        yield return dissWait;

        Debug.Log(gameObject.name + " returning to pool");
        ReturnToPool();
    }

    public void ReturnToPool()
    {
        // likely still calling to finish executing the rest of the coroutine
        StopCoroutine(forceCoroutine);
        StopCoroutine(dissapearCoroutine);

        rb.isKinematic = true;
        trail.enabled = false;
        PoolManager.Instance.releaseObject(gameObject);
    }

    public void SetUp()
    {
        enemyHit = 0;
    }
}
