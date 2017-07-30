using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scriptWorld : MonoBehaviour 
{

	// Inspector Variables
	public List<GameObject> levels;                         // All the levels (locations on the world map) in this entire game world (one world per game)
	public float xLength;                                   // the width of the game world (flavor is miles)
	public float yLength;                                   // the height of the game world (flavor is miles)

	// Private Variables


	// Use this for initialization
	void Start () 
	{

	}

	// Update is called once per frame
	void Update () 
	{

	}

	// FIND THE ROOM OR WORLD AN ENTITY IS IN: Return the room that contains the specified entity (can return either the specific room or general level)
	public GameObject getLocationOfEntity(GameObject entityInput, bool getLevel = false) 
	{
		foreach(GameObject level in levels) 
		{
            GameObject roomSearchResult = level.GetComponent<scriptLevel>().getRoomThatContainsSpecifiedEntity(entityInput);

			if (roomSearchResult != null) 
			{
				if (getLevel == true)
				{
					return level; 			// return level if the specified entity is located in it
				}
				else
				{
					return roomSearchResult;	// return room if the specified entity is located in it
				}
			}
		}

        Debug.Log("ERROR: " + entityInput.name + " could not be found, or there is an issue with this finding function.");
		return null;
	}

    // FIND THE TWO ROOMS CONNECTED BY A PASSAGEWAYCONNECTION
    public List<GameObject> getConnectedRoomsFromPassagewayConnection(GameObject inputPassagewayConnection)
    {
        var scriptInputPassagewayConnection = inputPassagewayConnection.GetComponent<scriptPassagewayConnection>();
        return new List<GameObject> { this.getLocationOfEntity(scriptInputPassagewayConnection.passagewayA), 
                                      this.getLocationOfEntity(scriptInputPassagewayConnection.passagewayB) };
    }

	// MOVE AN ENTITY INTO A SPECIFIC ROOM: Move a specified entity to a specified room
	public void moveEntityToSpecificRoom(GameObject entityToMove, GameObject targetRoom)
	{
		GameObject initialRoom = getLocationOfEntity(entityToMove);				    // find the current room the entity lives in
		initialRoom.GetComponent<scriptRoom>().removeEntityFromRoom(entityToMove);	// remove entity from initial room
        targetRoom.GetComponent<scriptRoom>().entities.Add(entityToMove);		    // add entity to target room
	}

	// XYZ TRANSLATE AN ENTITY IN THEIR LEVEL: Move an entity to a room in their current level using an xyz translation
	public void translateEntityInTheirLevel(GameObject entityToMove, float xChange, float yChange)  // <TODO> Need to add z translation to translateEntityInTheirLevel I think, for floor movement.
	{
        var scriptLevelContainingEntityToMove = getLocationOfEntity(entityToMove, true).GetComponent<scriptLevel>();
        var scriptRoomContainingEntityToMove = scriptLevelContainingEntityToMove.getRoomThatContainsSpecifiedEntity(entityToMove).GetComponent<scriptRoom>();
        GameObject targetRoom = scriptLevelContainingEntityToMove.getRoomByCoordinates(scriptRoomContainingEntityToMove.xSimplePosition + xChange,
                                                                                        scriptRoomContainingEntityToMove.ySimplePosition + yChange);

        if (targetRoom != null) // Check if target room exists
        {
            moveEntityToSpecificRoom(entityToMove, targetRoom);
        }
        else 
        {
            Debug.Log("WARNING: There was an attempt to move " + entityToMove.name + " to a location that apparantly doesn't exist. No movement has taken place, and unexpected errors may occur because of this.");
        }
        
	}
}
