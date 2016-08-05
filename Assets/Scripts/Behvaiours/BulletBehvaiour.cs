using UnityEngine;
using System.Collections;
using System.Runtime.Remoting.Messaging;

public class BulletBehvaiour : MonoBehaviour
{
    private Rigidbody rb;
    WaitForSeconds dissWait = new WaitForSeconds(5f);
    private bool projecting;

    private Vector3 bulletForce;
    private float speed = 10f;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Start()
    {
        
    }

    void FixedUpdate()
    {
        if (projecting)
            rb.velocity = bulletForce;
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
        PoolManager.Instance.ReturnToPool(gameObject);
    }
}
