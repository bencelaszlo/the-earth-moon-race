using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFollowerCamera : MonoBehaviour
{
    [SerializeField]
    GameObject[] players;

    private Vector3 offSet;

    void Start()
    {
        if (players.Length == 0) {
            Debug.Log("players missing from the PlayerFollowerCamera script!");
            return;
        }

        offSet = transform.position - players[0].transform.position;
    }

    void Update()
    {
        Vector3 leadPosition = new Vector3(0, 0, 0);
        for (int i = 0; i < players.Length; i++) {
            if (players[i].transform.position.x > leadPosition.x) {
                leadPosition = players[i].transform.position;
            }
        }

        transform.position = leadPosition + offSet - new Vector3(5, 0, 0);
        // transform.LookAt(players[0].transform);
    }
}
