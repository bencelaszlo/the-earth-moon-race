using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartTimerHandler : MonoBehaviour
{
    public float timeRemaining = 3;

    [SerializeField]
    GameObject startText;

    public void Start()
    {
        gameObject.GetComponent<TestController>().forwardAcceleration = 0;
        gameObject.GetComponent<TestController>().reverseAcceleration = 0;
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
            gameObject.GetComponent<TestController>().forwardAcceleration = 150000;
            gameObject.GetComponent<TestController>().reverseAcceleration = 5000;
            startText.GetComponent<Text>().text = "GO!";
            timeRemaining -= Time.deltaTime;
        } else {
            startText.SetActive(false);
            enabled = false;
        }
    }

}
