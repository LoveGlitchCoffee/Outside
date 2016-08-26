﻿using UnityEngine;
using System.Collections;

public class MovementController : MonoBehaviour
{

    public float Speed = 1f;
    Rigidbody rb;

    Vector3 speedForce;
    float hDir;
    float vDir;
    Vector3 moveForce;

    Vector3 startPosition;
    Quaternion startRotation;

    void Awake()
    {
        speedForce = new Vector3(Speed, Speed, Speed);
        rb = GetComponent<Rigidbody>();
    }

    void Start()
    {
        startPosition = transform.position;
        startRotation = Quaternion.identity;

        this.RegisterListener(EventID.OnGameStart , (sender, param) => RestartPlayer());
    }

    private void RestartPlayer()
    {
        transform.position = startPosition;
        transform.rotation = startRotation;
        Debug.Log("rotation " + transform.root.eulerAngles);
    }

    void Update()
    {

        hDir = Input.GetAxisRaw("Horizontal");
        vDir = Input.GetAxisRaw("Vertical");

        //Debug.Log("h, v: "+ hDir + ", " + vDir);

        if (hDir == 0 && vDir == 0)
            moveForce = new Vector3(0,0,0);

        if (hDir == 1)
        {
            moveForce = transform.TransformDirection(Vector3.Scale(Vector3.right, speedForce));

        }
        else if (hDir == -1)
        {
            moveForce = transform.TransformDirection(Vector3.Scale(Vector3.left, speedForce));
        }

        if (vDir == 1)
        {
            moveForce += transform.TransformDirection(Vector3.Scale(Vector3.forward, speedForce));
        }
        else if (vDir == -1)
        {
            moveForce += transform.TransformDirection(Vector3.Scale(Vector3.back, speedForce));
        }        

        moveForce = Vector3.ClampMagnitude(moveForce, Speed);        
    }

    void FixedUpdate()
    {
        rb.AddForce(moveForce);
    }
}
