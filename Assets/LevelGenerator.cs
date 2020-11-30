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
    public int length = 100;
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

    enum DirectionTrend {
        STRAIGHT,
        LEFT,
        RIGHT
    }

    enum Direction {
        NORTH,
        SOUTH,
        WEST,
        EAST
    }

    public float[] heightmap;

     Direction GetOppositeDirection(Direction direction) {
        Direction oppositeDirection = Direction.NORTH;
        switch (direction) {
            case Direction.NORTH:
                oppositeDirection = Direction.SOUTH;
                break;
            case Direction.SOUTH:
                oppositeDirection = Direction.NORTH;
                break;
            case Direction.EAST:
                oppositeDirection = Direction.WEST;
                break;
            case Direction.WEST:
                oppositeDirection = Direction.EAST;
                break;
        }
        return oppositeDirection;
    }

    void Start()
    {
        // TODO: Add more checks
        if (tilePrefab == null) {
            Debug.Log("tilePrefab is missing from the LevelGenerator script!");
            return;
        }

        heightmap = new float[length];

        Transform wall = Instantiate(wallPrefab, new Vector3(trackWidth * tileSize / 2 - 1, 2, -10.5f * tileSize), Quaternion.Euler(0, 0, 0));
        wall.transform.localScale = new Vector3(trackWidth * tileSize, 5, 0.1f);

        for (int i = 10; i > 0; i--) {
            GenerateSlice(trackWidth, tileSize, 0, (0 - i), tilePrefab, wallPrefab);
        }

        GenerateSlice(trackWidth, tileSize, 0, 0, startFinishTilePrefab, wallPrefab);

        int MIN_TREND_LENGTH = 10;

        float previousHeight = 0;
        HeightTrend heightTrend = HeightTrend.FRESH_STEADY;
        int trendCounter = 1;

        Direction direction = Direction.NORTH;

        for (int i = 1; i < length - 1; i++) {
            if (i % 50 == 0) {
                direction = (Direction)Random.Range(0, 3);
                Debug.Log("direction: " + direction);
            }

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
                        Transform upTile = Instantiate(upTilePrefab, new Vector3(trackWidth * tileSize / 2 - 1, height, i * tileSize), upTilePrefab.rotation);
                        Vector3 positionAdjustVector = new Vector3(0, 0.532f, 0);
                        upTile.transform.position -= positionAdjustVector;                  
                        upTile.transform.localScale = new Vector3(trackWidth * tileSize, 0.1f, 1.41f);
                    }
                    Instantiate(wallPrefab, new Vector3((trackWidth - 1) * tileSize, 2 + height, i * tileSize), Quaternion.identity);
                    continue;
                } else if (height < previousHeight) {
                    Instantiate(wallPrefab, new Vector3(-1, 2 + height, i * tileSize), Quaternion.identity);
                    Transform downTile = Instantiate(downTilePrefab, new Vector3(trackWidth * tileSize / 2 - 1, height, i * tileSize), downTilePrefab.rotation);
                    Vector3 positionAdjustVector = new Vector3(0, 0.468f, 0);
                    downTile.transform.position += positionAdjustVector;
                    downTile.transform.localScale = new Vector3(trackWidth * tileSize, 0.1f, 1.41f);
                    Instantiate(wallPrefab, new Vector3((trackWidth - 1) * tileSize, 2 + height, i * tileSize), Quaternion.identity);
                    continue;
                }
            }

            GenerateSlice(trackWidth, tileSize, height, i * tileSize, tilePrefab, wallPrefab);
        }

        GenerateSlice(trackWidth, tileSize, height, (length - 1), startFinishTilePrefab, wallPrefab);
    }

    private void GenerateSlice(float trackWidth, float tileSize, float y, float x, Transform tilePrefab, Transform wallPrefab) {
        Instantiate(wallPrefab, new Vector3(-1, 2 + y, x * tileSize), Quaternion.identity);
        Transform tile = Instantiate(tilePrefab, new Vector3(trackWidth * tileSize / 2 - 1, y, x * tileSize), Quaternion.identity);
        tile.transform.localScale = new Vector3(trackWidth * tileSize, 0.1f, 1);
        Instantiate(wallPrefab, new Vector3((trackWidth - 1) * tileSize, 2 + y, x * tileSize), Quaternion.identity);
    }
}

