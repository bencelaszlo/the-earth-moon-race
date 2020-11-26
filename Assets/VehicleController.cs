using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleController : MonoBehaviour
{
    public float thrusterStrength;
    public float thrusterDistance;
    public Transform[] thrusters;
    void FixedUpdate()
    {
        RaycastHit hit;
        foreach (Transform thruster in thrusters) {
            Vector3 downforce;
            float distancePercentage;

            if (Physics.Raycast(thruster.position, thruster.up * -1, out hit, thrusterDistance))
            {
                // The thruster's distance from the ground (low is far, high is close)
                distancePercentage = 1 - (hit.distance / thrusterDistance);

                // force to push
                downforce = transform.up * thrusterStrength * distancePercentage;

                // correct force with delta time and mass
                downforce = downforce * Time.deltaTime * GetComponent<Rigidbody>().mass;

                // apply the force to the thruster's position
                GetComponent<Rigidbody>().AddForceAtPosition(downforce, thruster.position);
            }
        }
    }
}