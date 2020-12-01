using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartTimerHandler : MonoBehaviour
{
    public float timeRemaining = 10;

    [SerializeField]
    GameObject startText;

    public void Start()
    {
        gameObject.GetComponent<CarController>().acceleration = 0;
        gameObject.GetComponent<CarController>().turnRotationAngle = 0;
        startText.GetComponent<Text>().text = "Ready";
    }
    void FixedUpdate()
    {
        if (timeRemaining > 1)
        {
            timeRemaining -= Time.deltaTime;
        }
        else if (timeRemaining > 0)
        {
            gameObject.GetComponent<CarController>().acceleration = 2500;
            gameObject.GetComponent<CarController>().turnRotationAngle = 30;
            startText.GetComponent<Text>().text = "GO!";
            timeRemaining -= Time.deltaTime;
        } else {
            startText.SetActive(false);
            enabled = false;
        }
    }

}
