﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class scriptLevel : MonoBehaviour 
{

	// Inspector Variables
    public float xWorldMapPosition;                         // x coordinate for this level location on the world map
    public float yWorldMapPosition;                         // y coordinate for this level location on the world map

	public List<GameObject> rooms;                          // All the rooms in this level
    public List<GameObject> passagewayConnections;          // All the passageway connections (two linked passageways connecting rooms) in this level


	// Private Variables
	private float xForCurrentRoomSelection = 0;               // x coordinate for the currently selected room on the map interface
    private float yForCurrentRoomSelection = 0;               // y coordinate for the currently selected room on the map interface


	// Use this for initialization
	void Start () 
	{

	}

	// Update is called once per frame
	void Update () 
	{
        checkInputs();  // check for directional inputs and move selected room on map accordingly
	}

    void checkInputs ()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            moveRoomSelection(0, -1);
        }

        if (Input.GetKeyUp(KeyCode.UpArrow))
        {
            moveRoomSelection(0, 1);
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            moveRoomSelection(-1, 0);
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            moveRoomSelection(1, 0);
        }
    }

    // Return the room GameObject located at the coordinate parameters
    public GameObject getRoomByCoordinates (float xInput, float yInput)
    {
        return rooms.SingleOrDefault(room => 
            room.GetComponent<scriptRoom>().xPosition == xInput 
            && room.GetComponent<scriptRoom>().yPosition == yInput);
    }

    // Return the room GameObject that contains the specified entity
    public GameObject getRoomThatContainsSpecifiedEntity (GameObject inputEntity)
    {
        foreach (GameObject room in rooms)
        {
            var roomSearchResult = room.GetComponent<scriptRoom>().entities.SingleOrDefault(entity => entity == inputEntity);

            if (roomSearchResult != null)
            {
                return room;    // return room if the specified entity is located in it
            }
        }

        Debug.Log("ERROR: " + inputEntity.name + " could not be found, or there is an issue with this finding function.");
        return null;
    }

	// Move room selected based on key input
    void moveRoomSelection (float xChange, float yChange) {

        GameObject initialRoom = getRoomByCoordinates(xForCurrentRoomSelection, yForCurrentRoomSelection); // Get the GameObject for the currently selected room

		float xRoomTarget = xForCurrentRoomSelection + xChange;
        float yRoomTarget = yForCurrentRoomSelection + yChange;
        GameObject targetRoom = getRoomByCoordinates(xRoomTarget, yRoomTarget);    // Get the GameObject for the target room

        if (targetRoom != null)         // Check if target room exists
        {
            if (initialRoom != null)    // Check if currently selected room exists (just a precaution)
            {
                initialRoom.GetComponent<scriptRoom>().isSelected = false;  // Deselect currently selected room
            }
            else
            {
                Debug.Log("ERROR: Somehow, a room doesn't exist at the coordinates for the currently selected room.");
            }

            targetRoom.GetComponent<scriptRoom>().isSelected = true;    // Select room targeted by user input

            xForCurrentRoomSelection = xRoomTarget;                     // Set current room selection coordinates to target coordinates
            yForCurrentRoomSelection = yRoomTarget;
        }
        else
        {
            Debug.Log("WARNING: There is no room to select based on player inputs, if there is a room in the direction being pushed, there may be an error.");
        }
	}
}