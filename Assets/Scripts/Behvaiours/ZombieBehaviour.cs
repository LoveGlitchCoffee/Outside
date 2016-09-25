using UnityEngine;
using System.Collections;
using System.Security.Permissions;

public class ZombieBehaviour : GameElement
{

    public float OriginalSpeed;
    private float speed;
    private bool allowedToMove = false;

    // determines how clutered zombies will be
    public float DeadTime; // not used
    public float FallForce;

    public bool dead = false;

    bool chaseGrandpa;

    private Animator anim;
    private Collider headHitbox;
    private Collider body;
    private Collider senseTrigger;
    private Rigidbody rb;

    void Awake()
    {
        anim = GetComponent<Animator>();
        headHitbox = GetComponent<CapsuleCollider>();
        rb = GetComponent<Rigidbody>();
        senseTrigger = GetComponent<SphereCollider>();
        body = GetComponent<BoxCollider>();
    }

    void Start()
    {
        this.RegisterListener(EventID.OnGameStart, (sender, param) => Reset());


        this.RegisterListener(EventID.OnPlayerDie, (sender, param) => StopMovement());
        this.RegisterListener(EventID.OnGameStart, (sender, param) => ReturnToPool());

        this.RegisterListener(EventID.OnBarrierDown, (sender, param) => TargetGrandpa());

        this.RegisterListener(EventID.OnMissleBlow, (sender, param) => ReactToExplode((Vector3)param));
    }

    private void Reset()
    {
        chaseGrandpa = false;
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

        senseTrigger.enabled = false;

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
        if (!dead && col.gameObject.tag == "Bullet")
        {
            anim.SetBool("AttackBarrier", false);
            anim.SetBool("Dead", true);

            //Debug.Log("hit zombie");

            this.PostEvent(EventID.OnEnemyDie);

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
        if (dead)
            return;

        if (col.CompareTag("Barricade"))
        {
            allowedToMove = false;
            anim.SetBool("AttackBarrier", true);
            StartCoroutine(AttackBarrier());
        }
        else if (col.CompareTag("Grandpa"))
        {
            if (!GameManager.isPlaying())
                return;

            allowedToMove = false;
            anim.SetBool("Attack", true);
            this.PostEvent(EventID.OnPlayerDie, transform.position);
            Debug.Log("finishe game");
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
        anim.SetBool("Run", false);
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
        if (!gameObject.activeSelf || dead)
            return;

        rb.isKinematic = false;
        anim.enabled = false;
        body.enabled = true;

        this.PostEvent(EventID.OnEnemyDie);

        StartCoroutine(DeadAndReturnToPool());

        rb.AddExplosionForce(GameManager.model.special.ExplosionForce, explosionPos, GameManager.model.special.ExplosionRadius, 1, ForceMode.Impulse);
    }

    public void SetUp(bool fast)
    {
        anim.enabled = true;
        rb.isKinematic = true;
        body.enabled = false;

        senseTrigger.enabled = true;

        //RotateTowards(grandpa);
        transform.rotation = Quaternion.AngleAxis(180, Vector3.up);
        allowedToMove = true;

        if (fast)
        {
            anim.SetBool("Run", true);
            speed = OriginalSpeed * 2f;
        }
        else
        {
            anim.SetBool("Move", true);
            speed = OriginalSpeed;
        }

        anim.SetBool("End", false);
        dead = false;
        headHitbox.enabled = true;
    }

    private void RotateTowards(Vector3 grandpa)
    {
        transform.LookAt(grandpa);
    }


}
