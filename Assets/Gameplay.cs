using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gameplay : MonoBehaviour
{
    [SerializeField]
    LevelGenerator levelGenerator;

    [SerializeField]
    Transform opponent;

    [SerializeField]
    GameObject positionText;

    [SerializeField]
    GameObject winnerText;

    private int position;
    private bool isWinner;
    public bool isLoser;

    private bool isStarted;

    [SerializeField]
    string playerName;

    void Start()
    {
        if (opponent == null) {
            Debug.Log("Please assign an opponent Transform in the GamePlay script.");
        }

        position = 0;
        isWinner = false;
        isLoser = false;
        winnerText.SetActive(false);
        positionText.SetActive(false);
    }

    void Update()
    {
        if (!isStarted) {
            positionText.SetActive(true);
            isStarted = true;
        }

        Debug.Log("isWinner: " + isWinner);
        Debug.Log("Position: " + position);
        if (transform.position.z > opponent.position.z) {
            position = 1;
            if (transform.position.z > levelGenerator.length) {
                opponent.GetComponent<Gameplay>().isLoser = true;
                if (!isLoser) {
                    isWinner = true;
                    winnerText.SetActive(true);
                    winnerText.GetComponent<Text>().text = playerName + " " + "Wins!";
                    positionText.GetComponent<Text>().text = "";
                }
            }
        } else {
            position = 2;
        }
        positionText.GetComponent<Text>().text = position == 1 ? "First" : "Second";
    }
}
