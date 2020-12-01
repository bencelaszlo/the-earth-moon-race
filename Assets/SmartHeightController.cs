using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmartHeightController : MonoBehaviour
{

    [SerializeField]
    LevelGenerator levelGenerator;

    float timeLimit = 1f;
    float timeCounter = 0f;

    void FixedUpdate()
    {
        timeCounter += Time.deltaTime;
        if (timeCounter > timeLimit) {
            Transform transform = gameObject.GetComponent<Transform>();
            float carY = transform.position.x;
            float carX = transform.position.y;
            float[] nextTilesHeight = new float[3];
            int nextTilesStartIndex = Mathf.CeilToInt(carX);
            for (int nextTileIndex = nextTilesStartIndex; nextTileIndex < 3; nextTileIndex++) {
                nextTilesHeight[nextTileIndex] = levelGenerator.heightmap[nextTilesStartIndex + nextTileIndex];
            }

            float maxHeight = 0;
            for (int i = 0; i < 3; i++) {
                if (nextTilesHeight[i] > maxHeight) {
                    maxHeight = nextTilesHeight[i];
                }
            }

            gameObject.transform.position += new Vector3(0, maxHeight, 0);
        }
    }
}
