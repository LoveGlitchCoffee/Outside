using UnityEngine;
using System.Collections;
using System.Security.Permissions;

public class ZombieBehaviour : GameElement
{

    public float speed;
    private bool allowedToMove = false;

    // determines how clutered zombies will be
    public float DeadTime; // not used

    bool dead = false;

    bool chaseGrandpa;

    private Animator anim;
    private CapsuleCollider headHitbox;
    private AudioSource thumpDie;

    void Awake()
    {
        anim = GetComponent<Animator>();
        headHitbox = GetComponent<CapsuleCollider>();
        thumpDie = GetComponent<AudioSource>();
    }

    void Start()
    {
        this.RegisterListener(EventID.OnPlayerDie, (sender, param) => StopMovement());
        this.RegisterListener(EventID.OnGameEnd, (sender, param) => ReturnToPool());

        this.RegisterListener(EventID.OnBarrierDown, (sender, param) => TargetGrandpa());
    }

    private void TargetGrandpa()
    {
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

            thumpDie.Play();

            anim.SetBool("AttackBarrier", false);
            anim.SetBool("Dead", true);

            allowedToMove = false;
            headHitbox.enabled = false;
            dead = true;
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
            yield return wait;
            this.PostEvent(EventID.OnHitBarrier, transform.position);
        }
    }

    private IEnumerator DeadAndReturnToPool()
    {
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

    public void SetUp()
    {
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
