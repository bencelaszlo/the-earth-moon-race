using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartTimerHandler : MonoBehaviour
{
    public float timeRemaining = 10;
    public GameObject [] players;

    public void Start()
    {
        foreach( GameObject spaceship in players)
        {
            spaceship.GetComponent<CarController>().acceleration = 0;
            spaceship.GetComponent<CarController>().turnRotationAngle = 0;
        }
    }
    void Update()
    {
        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
        }
        else
        {
            foreach( GameObject spaceship in players)
            {
                spaceship.GetComponent<CarController>().acceleration = 1000;
                spaceship.GetComponent<CarController>().turnRotationAngle = 40;
            }
            enabled = false;
        }
    }

}
