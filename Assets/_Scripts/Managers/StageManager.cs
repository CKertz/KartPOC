using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum SpawnDirection
{
    North,
    East,
    South,
    West
}

public class StageManager : MonoBehaviour
{
    private int currentRoomCount = 1;
    private int maxRoomCount = 10;

    private List<(float,float)> currentSpawnedRoomCoordinates = new List<(float, float)> { (0,0) };

    //origin point is at 4,2.25f currently, so need to add this to the spawn position to fix rooms spawning off of the grid
    private Vector3 gridOffset = new Vector3(4, 2.25f);

    [SerializeField]
    GameObject emptyFloorPrefab;

    [SerializeField]
    Grid stageGrid;

    public void GenerateNextRoomLayout(Component sender, object data)
    {
        // who cares about a grid, just generate a completely blank room. determine the door to spawn in it that you walked through, then randomly spawn other doors to pick from
        // note: need to spawn at least one door. no dead ends unless you're at max count
        if (currentRoomCount < maxRoomCount)
        {
            currentRoomCount++;

            if (data is GameObject)
            {
                var doorGameObject = (GameObject)data;
                string desiredDirection = ParseDoorNameForDesiredDirection(doorGameObject.name);
                Debug.Log($"desired direction: {desiredDirection}");

                Vector3Int doorGridPosition = stageGrid.WorldToCell(doorGameObject.transform.position);
                Debug.Log("door is on grid point: (" + doorGridPosition.x + ", " + doorGridPosition.y + ")");

                if (Enum.TryParse(desiredDirection, out SpawnDirection direction))
                {
                    switch (direction)
                    {
                        case SpawnDirection.North:
                            SpawnEmptyRoom(doorGridPosition, new Vector3Int(0, 1));
                            break;
                        case SpawnDirection.South:
                            SpawnEmptyRoom(doorGridPosition, new Vector3Int(0, -1));
                            break;
                        case SpawnDirection.East:
                            SpawnEmptyRoom(doorGridPosition, new Vector3Int(1, 0));
                            break;
                        case SpawnDirection.West:
                            SpawnEmptyRoom(doorGridPosition, new Vector3Int(-1, 0));
                            break;
                        default:
                            break;
                    }
                }                 
            }
        }
    }

    private string ParseDoorNameForDesiredDirection(string doorName)
    {
        //Door is expected to be given in format similar to "Door_Horizontal_North"
        return doorName.Split('_').Last();
    }

    private void SpawnEmptyRoom(Vector3Int doorGridPosition, Vector3Int directionVector)
    {
        Vector3Int spawnPosition = doorGridPosition + directionVector;
        Debug.Log("spawnPosition is on grid point: (" + spawnPosition.x + ", " + spawnPosition.y + ")");

        Vector3 spawnWorldPosition = stageGrid.CellToWorld(spawnPosition) + gridOffset;
        Debug.Log("spawnWorldPosition is on grid point: (" + spawnWorldPosition.x + ", " + spawnWorldPosition.y + ")");

        Instantiate(emptyFloorPrefab, spawnWorldPosition, Quaternion.identity, stageGrid.transform);
        currentSpawnedRoomCoordinates.Add((spawnPosition.x, spawnPosition.y));
    }

}
