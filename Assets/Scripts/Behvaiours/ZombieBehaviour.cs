using UnityEngine;
using System.Collections;
using System.Security.Permissions;

public class ZombieBehaviour : GameElement
{

    public float speed;
    private bool allowedToMove = false;

    // determines how clutered zombies will be
    public float DeadTime; // not used
    public float FallForce;

    public bool dead = false;

    bool chaseGrandpa;

    private Animator anim;
    private SphereCollider headHitbox;
    private Collider body;
    private Rigidbody rb;

    void Awake()
    {
        anim = GetComponent<Animator>();
        headHitbox = GetComponent<SphereCollider>();
        rb = GetComponent<Rigidbody>();
        body = GetComponent<BoxCollider>();
    }

    void Start()
    {
        this.RegisterListener(EventID.OnPlayerDie, (sender, param) => StopMovement());
        this.RegisterListener(EventID.OnGameEnd, (sender, param) => ReturnToPool());

        this.RegisterListener(EventID.OnBarrierDown, (sender, param) => TargetGrandpa());

        this.RegisterListener(EventID.OnMissleBlow, (sender, param) => ReactToExplode((Vector3)param));
    }

    private void TargetGrandpa()
    {
        if (!dead)
            allowedToMove = true;

        chaseGrandpa = true;

        anim.SetBool("AttackBarrier", false);
    }

    private void StopMovement()
    {
        if (!gameObject.activeSelf)
            return;

        allowedToMove = false;
        anim.SetBool("Move", false);
    }


    void Update()
    {
        if (allowedToMove && GameManager.isPlaying())
            Move();
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Bullet")
        {
            this.PostEvent(EventID.OnEnemyDie, col);

            anim.SetBool("AttackBarrier", false);
            anim.SetBool("Dead", true);

            /*rb.isKinematic = false;
            anim.enabled = false;
            body.enabled = true;
            Vector3 dir =  transform.position - col.transform.position;
            Debug.Log("ball, zombie: " + col.transform.position + ", " + transform.position);
            Debug.Log("direciton: " + dir);
            rb.AddForceAtPosition(dir * FallForce, col.contacts[0].point, ForceMode.Impulse);*/


            StartCoroutine(DeadAndReturnToPool());
        }
    }

    public void ReturnToPool()
    {
        if (!gameObject.activeSelf)
            return;

        anim.SetBool("Dead", false);
        anim.SetBool("Move", false);
        anim.SetBool("Attack", false);
        anim.SetBool("End", true);
        PoolManager.Instance.ReturnToPool(gameObject);
    }

    void OnTriggerEnter(Collider col)
    {
        //Debug.Log("Collided with " + col.gameObject.tag);

        if (dead)
            return;

        if (col.gameObject.CompareTag("Barricade"))
        {
            //Debug.Log("hit barricade");
            allowedToMove = false;
            anim.SetBool("AttackBarrier", true);
            StartCoroutine(AttackBarrier());
        }
        else if (col.CompareTag("Grandpa"))
        {            
            allowedToMove = false;
            anim.SetBool("Attack", true);
            this.PostEvent(EventID.OnPlayerDie, transform.position);
        }
    }

    private IEnumerator AttackBarrier()
    {
        WaitForSeconds wait = new WaitForSeconds(2);

        while (!dead)
        {
            this.PostEvent(EventID.OnHitBarrier, transform.position);            
            yield return wait;
        }
    }

    private IEnumerator DeadAndReturnToPool()
    {
        allowedToMove = false;
        headHitbox.enabled = false;
        dead = true;

        yield return new WaitForSeconds(10);

        anim.SetBool("Dead", false);
        anim.SetBool("Move", false);
        anim.SetBool("Attack", false);
        PoolManager.Instance.ReturnToPool(gameObject);
    }

    // could change this
    private void Move()
    {
        if (chaseGrandpa)
            RotateTowards(GameManager.model.Grandpa.position);

        transform.Translate(Vector3.forward * Time.deltaTime * speed);
    }

    private void ReactToExplode(Vector3 explosionPos)
    {
        if (!gameObject.activeSelf)
            return;

        rb.isKinematic = false;
        anim.enabled = false;
        body.enabled = true;

        StartCoroutine(DeadAndReturnToPool());

        rb.AddExplosionForce(GameManager.model.special.ExplosionForce, explosionPos, GameManager.model.special.ExplosionRadius, 1, ForceMode.Impulse);
    }

    public void SetUp()
    {
        anim.enabled = true;
        rb.isKinematic = true;
        body.enabled = false;

        chaseGrandpa = false;

        //RotateTowards(grandpa);
        transform.rotation = Quaternion.AngleAxis(180, Vector3.up);
        allowedToMove = true;
        anim.SetBool("Move", true);
        anim.SetBool("End", false);
        dead = false;
        headHitbox.enabled = true;
    }

    private void RotateTowards(Vector3 grandpa)
    {
        transform.LookAt(grandpa);
    }


}
