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

    public void GenerateNextRoomLayout(Component sender, object data)
    {
        Debug.Log("GenerateNextRoomLayout called");
        Debug.Log("test hit!" + data + "another test:" + sender.gameObject.name);
        // who cares about a grid, just generate a completely blank room. determine the door to spawn in it that you walked through, then randomly spawn other doors to pick from
        // note: need to spawn at least one door. no dead ends unless you're at max count
        if (currentRoomCount < maxRoomCount)
        {
            currentRoomCount++;

            Vector2 newFloorCoords = Vector2.zero;
            // use direction to help determine where to place empty floor position
            /*from 0,0:

            room height 2.25 top -2.25 bot
            room width -4 left 4 right

             */
            string desiredDirection = "";
            if (data is GameObject)
            {
                var doorGameObject = (GameObject)data;
                desiredDirection = ParseDoorNameForDesiredDirection(doorGameObject.name);
                Debug.Log($"desired direction: {desiredDirection}");

                switch (desiredDirection)
                {
                    case "North":
                        newFloorCoords.x = doorGameObject.transform.position.x;
                        newFloorCoords.y = doorGameObject.transform.position.y + 2.25f;
                    break;
                    case "South":
                        newFloorCoords.x = doorGameObject.transform.position.x;
                        newFloorCoords.y = doorGameObject.transform.position.y - 2.25f;
                        break;
                    case "East":
                        newFloorCoords.x = doorGameObject.transform.position.x + 4f;
                        newFloorCoords.y = doorGameObject.transform.position.y;
                        break;
                    case "West":
                        newFloorCoords.x = doorGameObject.transform.position.x - 4f;
                        newFloorCoords.y = doorGameObject.transform.position.y;
                        break;

                }
            }

            var newFloor = Instantiate(emptyFloorPrefab, emptyFloorPrefab.transform);
            newFloor.transform.localPosition = newFloorCoords;
            //TODO: add tracking in currentSpawnedRoomCoordinates
        }
    }

    private string ParseDoorNameForDesiredDirection(string doorName)
    {
        //Door is expected to be given in format similar to "Door_Horizontal_North"
        return doorName.Split('_').Last();
    }

}
