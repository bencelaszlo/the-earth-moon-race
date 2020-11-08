using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField]
    Transform tilePrefab;

    [SerializeField]
    Transform startFinishTilePrefab;
    int length = 100;
    [SerializeField]
    float trackWidth = 3.0f;
    [SerializeField]
    float tileSize = 10.0f;
    void Start()
    {
        if (tilePrefab == null) {
            Debug.Log("tilePrefab is missing from the LevelGenerator script!");
            return;
        }

        for (int j = 0; j < trackWidth; j++) {
            Instantiate(startFinishTilePrefab, new Vector3(0, 0, tileSize * -0.5f + j * tileSize), Quaternion.identity);
        }

        for (int i = 1; i < length - 1; i++) {
            for (int j = 0; j < trackWidth; j++) {
                Instantiate(tilePrefab, new Vector3(i * tileSize, 0, tileSize * -0.5f + j * tileSize), Quaternion.identity);
            }
        }

        for (int j = 0; j < trackWidth; j++) {
            Instantiate(startFinishTilePrefab, new Vector3((length - 1) * tileSize, 0, tileSize * -0.5f + j * tileSize), Quaternion.identity);
        }
    }
}
