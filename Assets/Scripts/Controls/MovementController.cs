using UnityEngine;
using System.Collections;

public class MovementController : MonoBehaviour
{

    public float Speed = 1f;
    Rigidbody rb;

    Vector3 speedForce;
    float hDir;
    float vDir;
    Vector3 moveForce;

    void Awake()
    {
        speedForce = new Vector3(Speed, Speed, Speed);
        rb = GetComponent<Rigidbody>();
    }

    void Start()
    {
        rb.freezeRotation = true;
        rb.drag = 4.0f;
    }

    void Update()
    {

        hDir = Input.GetAxis("Horizontal");
        vDir = Input.GetAxis("Vertical");

        Debug.Log("h, v: "+ hDir + ", " + vDir);

        if (hDir == 0 && vDir == 0)
            moveForce = new Vector3(0,0,0);

        if (hDir > 0)
        {
            moveForce = transform.TransformDirection(Vector3.Scale(Vector3.right, speedForce));

        }
        else if (hDir < 0)
        {
            moveForce = transform.TransformDirection(Vector3.Scale(Vector3.left, speedForce));
        }

        if (vDir > 0)
        {
            moveForce += transform.TransformDirection(Vector3.Scale(Vector3.forward, speedForce));
        }
        else if (vDir < 0)
        {
            moveForce += transform.TransformDirection(Vector3.Scale(Vector3.back, speedForce));
        }

        moveForce = Vector3.ClampMagnitude(moveForce, Speed);        
    }

    void FixedUpdate()
    {
        rb.AddForce(moveForce);
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
