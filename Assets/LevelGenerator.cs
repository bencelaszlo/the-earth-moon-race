using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField]
    Transform tilePrefab;

    [SerializeField]
    Transform upTilePrefab;

    [SerializeField]
    Transform downTilePrefab;

    [SerializeField]
    Transform startFinishTilePrefab;

    [SerializeField]
    Transform wallPrefab;

    [SerializeField]
    int length = 100;
    [SerializeField]
    float trackWidth = 3.0f;
    [SerializeField]
    float tileSize = 1.0f;

    private float height = 0;
    private float generatorHeight = 0;
    [SerializeField]
    int maxHeight;
    [SerializeField]
    int minHeight;
    enum HeightTrend {
            STEADY,
            FRESH_STEADY, // STEADY LESSER THAN 3 TILES AGO
            UPWARD,
            DOWNWARD

    }

    public float[] heightmap;

    void Start()
    {
        // TODO: Add more check
        if (tilePrefab == null) {
            Debug.Log("tilePrefab is missing from the LevelGenerator script!");
            return;
        }

        heightmap = new float[length];

        for (int i = 0; i < trackWidth; i++) {
            Instantiate(wallPrefab, new Vector3(tileSize * -0.5f + i * tileSize, 2, -10.5f * tileSize), Quaternion.Euler(0, 90, 0));
        }

        for (int i = 10; i > 0; i--) {
            Instantiate(wallPrefab, new Vector3(-1, 2, (0 - i) * tileSize), Quaternion.identity);
            for (int j = 0; j < trackWidth; j++) {
                Instantiate(tilePrefab, new Vector3(tileSize * -0.5f + j * tileSize, 0, (0 - i) * tileSize), Quaternion.identity);
            }
            Instantiate(wallPrefab, new Vector3((trackWidth - 1) * tileSize, 2, (0 - i) * tileSize), Quaternion.identity);
        }

        Instantiate(wallPrefab, new Vector3(-1, 2, 0), Quaternion.identity);
        for (int j = 0; j < trackWidth; j++) {
            Instantiate(startFinishTilePrefab, new Vector3(tileSize * -0.5f + j * tileSize, 0, 0), Quaternion.identity);
        }
        Instantiate(wallPrefab, new Vector3((trackWidth - 1) * tileSize, 2, 0), Quaternion.identity);

        int MIN_TREND_LENGTH = 10;

        float previousHeight = 0;
        HeightTrend heightTrend = HeightTrend.FRESH_STEADY;
        int trendCounter = 1;
        for (int i = 1; i < length - 1; i++) {
            previousHeight = height;
            if (i > 30) {
                if (heightTrend == HeightTrend.FRESH_STEADY) {
                    if (trendCounter > MIN_TREND_LENGTH) {
                        heightTrend = HeightTrend.STEADY;
                        trendCounter = 0;
                    } else {
                        trendCounter++;
                    }
                } else if (heightTrend == HeightTrend.STEADY) {
                    generatorHeight = Random.Range(generatorHeight - 1, generatorHeight + 1);
                    if (generatorHeight > maxHeight) {
                        generatorHeight = maxHeight;
                    } else if (generatorHeight < minHeight) {
                        generatorHeight = minHeight;
                    }
                    height = Mathf.FloorToInt(generatorHeight);

                    if (height > previousHeight) {
                        heightTrend = HeightTrend.UPWARD;
                        trendCounter = 0;
                    } else if (height < previousHeight) {
                        heightTrend = HeightTrend.DOWNWARD;
                        trendCounter = 0;
                    } else {
                        trendCounter++;
                    }
                } else if (heightTrend == HeightTrend.DOWNWARD) {
                    generatorHeight = Random.Range(generatorHeight - 1, generatorHeight);
                    if (generatorHeight > maxHeight) {
                        generatorHeight = maxHeight;
                    } else if (generatorHeight < minHeight) {
                        generatorHeight = minHeight;
                    }
                    height = Mathf.FloorToInt(generatorHeight);

                    if (height > previousHeight) {
                        heightTrend = HeightTrend.UPWARD;
                        trendCounter = 0;
                    } else if (height < previousHeight) {
                        heightTrend = HeightTrend.DOWNWARD;
                        trendCounter++;
                    } else {
                        heightTrend = HeightTrend.FRESH_STEADY;
                        trendCounter = 0;
                    }
                }  else if (heightTrend == HeightTrend.UPWARD) {
                    generatorHeight = Random.Range(generatorHeight, generatorHeight + 1);
                    if (generatorHeight > maxHeight) {
                        generatorHeight = maxHeight;
                    } else if (generatorHeight < minHeight) {
                        generatorHeight = minHeight;
                    }
                    height = Mathf.FloorToInt(generatorHeight);

                    if (height > previousHeight) {
                        heightTrend = HeightTrend.UPWARD;
                        trendCounter++;
                    } else if (height < previousHeight) {
                        heightTrend = HeightTrend.DOWNWARD;
                        trendCounter = 0;
                    } else {
                        heightTrend = HeightTrend.FRESH_STEADY;
                        trendCounter = 0;
                    }
                }

                heightmap[i] = height;
            

                if (height > previousHeight) {
                    Instantiate(wallPrefab, new Vector3(-1, 2 + height, i * tileSize), Quaternion.identity);
                    for (int j = 0; j < trackWidth; j++) {
                        Transform upTile = Instantiate(upTilePrefab, new Vector3(tileSize * -0.5f + j * tileSize, height, i * tileSize), upTilePrefab.rotation);
                        Vector3 positionAdjustVector = new Vector3(0, 0.532f, 0);
                        upTile.transform.position -= positionAdjustVector;
                    }
                    Instantiate(wallPrefab, new Vector3((trackWidth - 1) * tileSize, 2 + height, i * tileSize), Quaternion.identity);
                    continue;
                } else if (height < previousHeight) {
                    Instantiate(wallPrefab, new Vector3(-1, 2 + height, i * tileSize), Quaternion.identity);
                    for (int j = 0; j < trackWidth; j++) {
                        Transform downTile = Instantiate(downTilePrefab, new Vector3(tileSize * -0.5f + j * tileSize, height, i * tileSize), downTilePrefab.rotation);
                        Vector3 positionAdjustVector = new Vector3(0, 0.468f, 0);
                        downTile.transform.position += positionAdjustVector;
                    }
                    Instantiate(wallPrefab, new Vector3((trackWidth - 1) * tileSize, 2 + height, i * tileSize), Quaternion.identity);
                    continue;
                }
            }

            Instantiate(wallPrefab, new Vector3(-1, 2 + height, i * tileSize), Quaternion.identity);
            for (int j = 0; j < trackWidth; j++) {
                Instantiate(tilePrefab, new Vector3(tileSize * -0.5f + j * tileSize, height, i * tileSize), tilePrefab.rotation);
 
            }
            Instantiate(wallPrefab, new Vector3((trackWidth - 1) * tileSize, 2 + height, i * tileSize), Quaternion.identity);
        }

        Instantiate(wallPrefab, new Vector3(-1, 2 + height, (length -1) * tileSize), Quaternion.identity);
        for (int j = 0; j < trackWidth; j++) {
            Instantiate(startFinishTilePrefab, new Vector3(tileSize * -0.5f + j * tileSize, height, (length - 1) * tileSize), Quaternion.identity);
        }
        Instantiate(wallPrefab, new Vector3((trackWidth - 1) * tileSize, 2 + height, (length -1) * tileSize), Quaternion.identity);
    }
}
