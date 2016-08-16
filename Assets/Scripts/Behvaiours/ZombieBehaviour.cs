﻿using UnityEngine;
using System.Collections;
using System.Security.Permissions;

public class ZombieBehaviour : GameElement
{

    public float speed;
    private bool allowedToMove = false;

    // determines how clutered zombies will be
    public float DeadTime;

    private Animator anim;
    private CapsuleCollider headHitbox;
    private AudioSource thumpDie;

    void Awake()
    {
        anim = GetComponent<Animator>();
        headHitbox = GetComponent<CapsuleCollider>();
        thumpDie = GetComponent<AudioSource>();
    }

    void Start ()
    {
        this.RegisterListener(EventID.OnPlayerDie, (sender, param) => StopMovement());
	    this.RegisterListener(EventID.OnGameEnd, (sender, param) => ReturnToPool());       
	}

    private void StopMovement()
    {
        if (!gameObject.activeSelf)
            return;

        allowedToMove = false;
        anim.SetBool("Move",false);
    }


    void Update ()
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
            anim.SetBool("Dead", true);
            allowedToMove = false;
            headHitbox.enabled = false;
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
        if (col.tag == "Grandpa")
        {
            allowedToMove = false;
            anim.SetBool("Attack", true);
            this.PostEvent(EventID.OnPlayerDie, transform.position);
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

    private void Move()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * speed);        
    }

    public void SetUp(Vector3 grandpa)
    {                
        RotateTowards(grandpa);
        allowedToMove = true;
        anim.SetBool("Move", true);
        anim.SetBool("End", false);
        headHitbox.enabled = true;
    }

    private void RotateTowards(Vector3 grandpa)
    {
        transform.LookAt(grandpa);                
    }


}
