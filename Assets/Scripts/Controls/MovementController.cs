using UnityEngine;
using System.Collections;

public class MovementController : MonoBehaviour
{

    public float Speed = 0.1f;
    Rigidbody rb;

    private Vector3 moveForce;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Start()
    {
        moveForce = new Vector3(Speed, Speed, Speed);
        rb.freezeRotation = true;
        rb.drag = 4.0f;
    }

    void Update()
    {
        // will need to take care of movement if hit zombie

        if (Input.GetKey(KeyCode.W))
        {
            rb.AddForce(transform.TransformDirection(Vector3.Scale(Vector3.forward, moveForce)));
        }
        if (Input.GetKey(KeyCode.S))
        {
            rb.AddForce(transform.TransformDirection(Vector3.Scale(Vector3.back, moveForce)));
        }
        if (Input.GetKey(KeyCode.A))
        {
            rb.AddForce(transform.TransformDirection(Vector3.Scale(Vector3.left, moveForce)));
        }
        if (Input.GetKey(KeyCode.D))
        {
            rb.AddForce(transform.TransformDirection(Vector3.Scale(Vector3.right, moveForce)));
        }
    }

    void FixedUpdate()
    {
        
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag("Wall"))
        {
            Debug.Log("hit wall");
            rb.velocity = Vector3.zero;
        }
    }
}
