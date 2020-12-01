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
    
    [SerializeField]
    GameObject inGameMenu;


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

        Vector3 closestFinishPoint;
        float closetstFinishDistance = float.MaxValue;
        Vector3 opponentClosestFinishPoint;
        float opponentClosestDistance = float.MaxValue;

        for (int i = 0; i < levelGenerator.finishLine.Length; i++) {
            if (Vector3.Distance(levelGenerator.finishLine[i], transform.position) < closetstFinishDistance) {
                closestFinishPoint = levelGenerator.finishLine[i];
                closetstFinishDistance = Vector3.Distance(levelGenerator.finishLine[i], transform.position);
                Debug.Log("closetstFinishDistance: " + closetstFinishDistance);
            }

            if (Vector3.Distance(levelGenerator.finishLine[i], opponent.position) < opponentClosestDistance) {
                opponentClosestFinishPoint = levelGenerator.finishLine[i];
                opponentClosestDistance = Vector3.Distance(levelGenerator.finishLine[i], opponent.position);
            }
        }

        if (closetstFinishDistance < opponentClosestDistance) {
            position = 1;
            if (closetstFinishDistance < 5.0f) {
                opponent.GetComponent<Gameplay>().isLoser = true;
                if (!isLoser) {
                    isWinner = true;
                    winnerText.SetActive(true);
                    winnerText.GetComponent<Text>().text = playerName + " " + "Wins!";
                    positionText.GetComponent<Text>().text = "";
                    inGameMenu.SetActive(true);

                }
            }
        } else {
            position = 2;
        }

        positionText.GetComponent<Text>().text = position == 1 ? "First" : "Second";
    }
}
