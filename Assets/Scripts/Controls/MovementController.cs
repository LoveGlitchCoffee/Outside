using UnityEngine;
using System.Collections;

public class MovementController : MonoBehaviour
{

    public float Speed = 0.1f;

    private Vector3 moveForce;

    void Start()
    {
        moveForce = new Vector3(Speed, Speed, Speed);
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            transform.position += transform.TransformDirection(Vector3.Scale(Vector3.forward, moveForce));
        }
        if (Input.GetKey(KeyCode.S))
        {
			transform.position += transform.TransformDirection(Vector3.Scale(Vector3.back, moveForce));
        }
        if (Input.GetKey(KeyCode.A))
        {
			transform.position += transform.TransformDirection(Vector3.Scale(Vector3.left, moveForce));
        }
        if (Input.GetKey(KeyCode.D))
        {
			transform.position += transform.TransformDirection(Vector3.Scale(Vector3.right, moveForce));
        }
    }
}
