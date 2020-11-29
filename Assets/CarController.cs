using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    // Values that control the vehicle
    public float acceleration;
    public float rotationRate;

    // Values that fake nice turn display
    public float turnRotationAngle;
    public float turnRotationSeekSpeed;

    // Reference variables we don't directly use:
    private float rotationVelocity;
    private float groundAngleVelocity;

    [SerializeField]
    string verticalAxis;

    [SerializeField]
    string horizontalAxis;

    void FixedUpdate() {
        // Check if we are on the ground
        if (Physics.Raycast(transform.position, transform.up * -1, 3f)) {
            // We are on the ground; enable the accelerator and increase drag
            GetComponent<Rigidbody>().drag = 1;

            // Calculate forward force
            Vector3 forwardForce = transform.forward * acceleration * Input.GetAxis(verticalAxis);
            // Correct force deltatime and mass
            forwardForce = forwardForce * Time.deltaTime * GetComponent<Rigidbody>().mass;

            // GetComponent<Rigidbody>().ForceAdd(forwardForce);
            GetComponent<Rigidbody>().AddForce(forwardForce);
        } else {
            // reduce drag because we are not on the ground and we don't want to halt mid-air
            GetComponent<Rigidbody>().drag = 0;
        }

        // You can turn in air or on the ground
        Vector3 turnTorque = Vector3.up * rotationRate * Input.GetAxis(horizontalAxis);
        // Correct force deltatime and mass
        turnTorque = turnTorque * Time.deltaTime * GetComponent<Rigidbody>().mass;
        GetComponent<Rigidbody>().AddTorque(turnTorque);

        // Fake rotate the car when you are turning
        Vector3 newRotation = transform.eulerAngles;
        newRotation.z = Mathf.SmoothDampAngle(newRotation.z, Input.GetAxis(horizontalAxis) * -turnRotationAngle, ref rotationVelocity, turnRotationSeekSpeed);
        transform.eulerAngles =     newRotation;
    }
}
