using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    [SerializeField]
    string horizontalAxis;
    [SerializeField]
    string verticalAxis;
    [SerializeField]
    float moveSpeed = 10.0f;
    Vector3 forward;
    Vector3 right;

    void Start()
    {
        // Define "Player" up and right axis
        forward = Camera.main.transform.forward;
        forward.y = 0;
        forward = Vector3.Normalize(forward);

        right = Quaternion.Euler(new Vector3(0, 90, 0)) * forward;

    }

    void Update()
    {
        if (Input.anyKey) {
            Move();
        }
    }

    void Move() {
        Vector3 direction = new Vector3(Input.GetAxis(horizontalAxis), 0, Input.GetAxis(verticalAxis));
        
        Vector3 rightMovement = right * moveSpeed * Time.deltaTime * Input.GetAxis(horizontalAxis);
        Vector3 upMovement = forward * moveSpeed * Time.deltaTime * Input.GetAxis(verticalAxis);

        Vector3 heading = Vector3.Normalize(rightMovement + upMovement);
        var originalTransform = transform.position;
        transform.forward = heading;
        transform.position += rightMovement;
        transform.position += upMovement;
    }
}
