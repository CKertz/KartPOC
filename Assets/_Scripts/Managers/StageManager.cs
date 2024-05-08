using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    private int currentRoomCount = 1;
    private int maxRoomCount = 10;

    private List<(float,float)> currentSpawnedRoomCoordinates = new List<(float, float)> { (0,0) };

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

            string desiredDirection = "";
            if (data is GameObject)
            {
                var doorGameObject = (GameObject)data;
                desiredDirection = ParseDoorNameForDesiredDirection(doorGameObject.name);
                Debug.Log($"desired direction: {desiredDirection}");

                //origin point is at 4,2.25f currently, so need to add this to the spawn position to fix rooms spawning off of the grid
                Vector3 gridOffset = new Vector3(4, 2.25f);
                //convert turn the world coords of door being entered into grid coords
                Vector3Int doorGridPosition = stageGrid.WorldToCell(doorGameObject.transform.position);
                Debug.Log("door is on grid point: (" + doorGridPosition.x + ", " + doorGridPosition.y + ")");

                switch (desiredDirection)
                {
                    case "North":
                        //use those coords and depending on direction, determine the direction to spawn new room
                        Vector3Int spawnPositionNorth = doorGridPosition + new Vector3Int(0, 1);
                        Debug.Log("spawnPosition is on grid point: (" + spawnPositionNorth.x + ", " + spawnPositionNorth.y + ")");
                        Vector3 spawnWorldPositionNorth = stageGrid.CellToWorld(spawnPositionNorth) + gridOffset; 
                        Debug.Log("spawnWorldPosition is on grid point: (" + spawnWorldPositionNorth.x + ", " + spawnWorldPositionNorth.y + ")");

                        //spawn new floor, set parent
                        Instantiate(emptyFloorPrefab, spawnWorldPositionNorth, Quaternion.identity, stageGrid.transform);
                        currentSpawnedRoomCoordinates.Add((spawnPositionNorth.x, spawnPositionNorth.y));
                        break;
                    case "South":
                        Vector3Int spawnPositionSouth = doorGridPosition + new Vector3Int(0, -1);
                        Debug.Log("spawnPosition is on grid point: (" + spawnPositionSouth.x + ", " + spawnPositionSouth.y + ")");

                        Vector3 spawnWorldPositionSouth = stageGrid.CellToWorld(spawnPositionSouth) + gridOffset; 
                        Debug.Log("spawnWorldPosition is on grid point: (" + spawnWorldPositionSouth.x + ", " + spawnWorldPositionSouth.y + ")");

                        Instantiate(emptyFloorPrefab, spawnWorldPositionSouth, Quaternion.identity, stageGrid.transform);
                        currentSpawnedRoomCoordinates.Add((spawnPositionSouth.x, spawnPositionSouth.y));

                        break;
                    case "East":
                        Vector3Int spawnPositionEast = doorGridPosition + new Vector3Int(1, 0);
                        Debug.Log("spawnPosition is on grid point: (" + spawnPositionEast.x + ", " + spawnPositionEast.y + ")");

                        Vector3 spawnWorldPositionEast = stageGrid.CellToWorld(spawnPositionEast) + gridOffset; 
                        Debug.Log("spawnWorldPosition is on grid point: (" + spawnWorldPositionEast.x + ", " + spawnWorldPositionEast.y + ")");

                        Instantiate(emptyFloorPrefab, spawnWorldPositionEast, Quaternion.identity, stageGrid.transform);
                        currentSpawnedRoomCoordinates.Add((spawnPositionEast.x, spawnPositionEast.y));
                        break;
                    case "West":
                        Vector3Int spawnPositionWest = doorGridPosition + new Vector3Int(-1, 0);
                        Debug.Log("spawnPosition is on grid point: (" + spawnPositionWest.x + ", " + spawnPositionWest.y + ")");

                        Vector3 spawnWorldPositionWest = stageGrid.CellToWorld(spawnPositionWest) + gridOffset; 
                        Debug.Log("spawnWorldPosition is on grid point: (" + spawnWorldPositionWest.x + ", " + spawnWorldPositionWest.y + ")");

                        Instantiate(emptyFloorPrefab, spawnWorldPositionWest, Quaternion.identity, stageGrid.transform);
                        currentSpawnedRoomCoordinates.Add((spawnPositionWest.x, spawnPositionWest.y));
                        break;

                }
            }
            //TODO: extract this junk out to method
        }
    }

    private string ParseDoorNameForDesiredDirection(string doorName)
    {
        //Door is expected to be given in format similar to "Door_Horizontal_North"
        return doorName.Split('_').Last();
    }

}
