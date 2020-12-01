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

    DirectionTrend GetDirectionTrend(Direction previous, Direction current) {
        if (previous == current) {
            return DirectionTrend.STRAIGHT;
        }

        if ((previous == Direction.NORTH && current == Direction.EAST) ||
            (previous == Direction.EAST && current == Direction.SOUTH) ||
            (previous == Direction.SOUTH && current == Direction.WEST) ||
            (previous == Direction.WEST && current == Direction.NORTH)
        ) {
            return DirectionTrend.RIGHT;
        }

        if ((current == Direction.NORTH && previous == Direction.EAST) ||
            (current == Direction.EAST && previous == Direction.SOUTH) ||
            (current == Direction.SOUTH && previous == Direction.WEST) ||
            (current == Direction.WEST && previous == Direction.NORTH)
        ) {
            return DirectionTrend.LEFT;
        }

        return DirectionTrend.STRAIGHT;
    }

    Transform GenerateTurn(Vector3 position)
    {
        float size = trackWidth * tileSize;
        position -= new Vector3(size / 2, 0, size / 2);
        Transform tile = Instantiate(tilePrefab, position, Quaternion.identity, transform);
        tile.transform.localScale = new Vector3(size, 0.1f, size);
        return tile;
    }

    void Start()
    {
        // TODO: Add more checks
        if (tilePrefab == null) {
            Debug.Log("tilePrefab is missing from the LevelGenerator script!");
            return;
        }

        GenerateTurn(new Vector3(-50, 0, -50));

        heightmap = new float[length];

        Transform wall = Instantiate(wallPrefab, new Vector3(trackWidth * tileSize / 2 - 1, 2, -10.5f * tileSize), Quaternion.Euler(0, 0, 0));
        wall.transform.localScale = new Vector3(trackWidth * tileSize, 5, 0.1f);

        for (int i = 10; i > 0; i--) {
            GenerateSlice(trackWidth, tileSize, 0, (0 - i), tilePrefab, wallPrefab, gameObject);
        }

        GenerateSlice(trackWidth, tileSize, 0, 0, startFinishTilePrefab, wallPrefab, gameObject);

        GameObject[] straights = new GameObject[10];
        for (int s = 0; s < 10; s++) {
            straights[s] = new GameObject();

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
                        Instantiate(wallPrefab, new Vector3(-1, 2 + height, i * tileSize), Quaternion.identity, straights[s].transform);
                        for (int j = 0; j < trackWidth; j++) {
                            Transform upTile = Instantiate(upTilePrefab, new Vector3(trackWidth * tileSize / 2 - 1, height, i * tileSize), upTilePrefab.rotation, straights[s].transform);
                            Vector3 positionAdjustVector = new Vector3(0, 0.532f, 0);
                            upTile.transform.position -= positionAdjustVector;                  
                            upTile.transform.localScale = new Vector3(trackWidth * tileSize, 0.1f, 1.41f);
                        }
                        Instantiate(wallPrefab, new Vector3((trackWidth - 1) * tileSize, 2 + height, i * tileSize), Quaternion.identity, straights[s].transform);
                        continue;
                    } else if (height < previousHeight) {
                        Instantiate(wallPrefab, new Vector3(-1, 2 + height, i * tileSize), Quaternion.identity, straights[s].transform);
                        Transform downTile = Instantiate(downTilePrefab, new Vector3(trackWidth * tileSize / 2 - 1, height, i * tileSize), downTilePrefab.rotation, straights[s].transform);
                        Vector3 positionAdjustVector = new Vector3(0, 0.468f, 0);
                        downTile.transform.position += positionAdjustVector;
                        downTile.transform.localScale = new Vector3(trackWidth * tileSize, 0.1f, 1.41f);
                        Instantiate(wallPrefab, new Vector3((trackWidth - 1) * tileSize, 2 + height, i * tileSize), Quaternion.identity, straights[s].transform);
                        continue;
                    }
                }

                GenerateSlice(trackWidth, tileSize, height, i * tileSize, tilePrefab, wallPrefab, straights[s]);
            }
        }
        
        Direction direction = Direction.NORTH;
        bool isPossibleDirection = false;
        Direction previousDirection = direction;
        float nextChunkX = 0;
        float nextChunkZ = 0;
        float nextRotationY = 0;
        float nextTurnX = 0;
        float nextTurnZ = length;
        foreach (GameObject straight in straights) {
            straight.transform.position = new Vector3(nextChunkX, 0, nextChunkZ);
            straight.transform.rotation = Quaternion.Euler(0, nextRotationY, 0);

            Transform turn = GenerateTurn(new Vector3(nextTurnX, 0, nextTurnZ));

            Direction oppositeDirection = GetOppositeDirection(direction);
            isPossibleDirection = false;
            previousDirection = direction;
            while (!isPossibleDirection) {
                direction = (Direction)Random.Range(0, 3);
                if (direction != oppositeDirection) {
                    isPossibleDirection = true;
                    Debug.Log("direction: " + direction);
                    DirectionTrend directionTrend = GetDirectionTrend(previousDirection, direction);
                    Debug.Log("directionTrend: " + directionTrend);
                    switch (directionTrend) {
                        case DirectionTrend.STRAIGHT:
                            nextRotationY = nextRotationY;
                            switch (direction) {
                                case Direction.NORTH:
                                    nextChunkX = nextChunkX;
                                    nextChunkZ = nextChunkZ + length + trackWidth;

                                    nextTurnX = nextTurnX;
                                    nextTurnZ = nextTurnZ + length;
                                    break;
                                case Direction.SOUTH:
                                    nextChunkX = nextChunkX;
                                    nextChunkZ = nextChunkZ - length - trackWidth;

                                    nextTurnX = nextTurnX;
                                    nextTurnZ = nextTurnZ - length;
                                    break;
                                case Direction.WEST:
                                    nextChunkX = nextChunkX - length - trackWidth;
                                    nextChunkZ = nextChunkZ;

                                    nextTurnX = nextTurnX + length;
                                    nextTurnZ = nextTurnZ;
                                    break;
                                case Direction.EAST:
                                    nextChunkX = nextChunkX + length + trackWidth;
                                    nextChunkZ = nextChunkZ;

                                    nextTurnX = nextTurnX - length;
                                    nextTurnZ = nextTurnZ;
                                    break;
                            }
                            break;
                        case DirectionTrend.RIGHT:
                            nextRotationY += 90.0f;
                            switch (direction) {
                                case Direction.NORTH:
                                    nextChunkX = nextChunkX - trackWidth - length;
                                    nextChunkZ = nextChunkZ + trackWidth;

                                    nextTurnX = nextTurnX + trackWidth;
                                    nextTurnZ = nextTurnZ + length;
                                    break;
                                case Direction.SOUTH:
                                    nextChunkX = nextChunkX + trackWidth + length;
                                    nextChunkZ = nextChunkZ - trackWidth;

                                    nextTurnX = nextTurnX;
                                    nextTurnZ = nextTurnZ - length;
                                    break;
                                case Direction.WEST:
                                    nextChunkX = nextChunkX - trackWidth;
                                    nextChunkZ = nextChunkZ - trackWidth - length;

                                    nextTurnX = nextTurnX - length - trackWidth;
                                    nextTurnZ = nextTurnZ;
                                    break;
                                case Direction.EAST:
                                    nextChunkX = nextChunkX + trackWidth;
                                    nextChunkZ = nextChunkZ + trackWidth + length;

                                    nextTurnX = nextTurnX + length;
                                    nextTurnZ = nextTurnZ;
                                    break;
                            }
                            break;
                        case DirectionTrend.LEFT:
                            nextRotationY += -90.0f;
                            switch (direction) {
                                case Direction.NORTH:
                                    nextChunkX = nextChunkX + length;
                                    nextChunkZ = nextChunkZ;

                                    nextTurnX = nextTurnX;
                                    nextTurnZ = nextTurnZ + length;
                                    break;
                                case Direction.SOUTH:
                                    nextChunkX = nextChunkX - length;
                                    nextChunkZ = nextChunkZ;

                                    nextTurnX = nextTurnX;
                                    nextTurnZ = nextTurnZ - length;
                                    break;
                                case Direction.WEST:
                                    nextChunkX = nextChunkX;
                                    nextChunkZ = nextChunkZ + length;

                                    nextTurnX = nextTurnX - length;
                                    nextTurnZ = nextTurnZ;
                                    break;
                                case Direction.EAST:
                                    nextChunkX = nextChunkX;
                                    nextChunkZ = nextChunkZ - length;

                                    nextTurnX = nextTurnX + length;
                                    nextTurnZ = nextTurnZ;
                                    break;
                            }
                            break;
                    }
                }
            }
        }

        // GenerateSlice(trackWidth, tileSize, height, (length - 1), startFinishTilePrefab, wallPrefab, straights[s]);


    }

    private void GenerateSlice(float trackWidth, float tileSize, float y, float x, Transform tilePrefab, Transform wallPrefab, GameObject straight) 
    {
        Instantiate(wallPrefab, new Vector3(-1, 2 + y, x * tileSize), Quaternion.identity, straight.transform);
        Transform tile = Instantiate(tilePrefab, new Vector3(trackWidth * tileSize / 2 - 1, y, x * tileSize), Quaternion.identity, straight.transform);
        tile.transform.localScale = new Vector3(trackWidth * tileSize, 0.1f, 1);
        Instantiate(wallPrefab, new Vector3((trackWidth - 1) * tileSize, 2 + y, x * tileSize), Quaternion.identity, straight.transform);
    }
}

