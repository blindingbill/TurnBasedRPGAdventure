using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scriptWorld : MonoBehaviour 
{

	// Inspector Variables
	public List<GameObject> levels;                         // All the levels (locations on the world map) in this entire game world (one world per game)
	public float width;                                     // the width of the game world (flavor is miles)
	public float height;                                    // the height of the game world (flavor is miles)

	// Private Variables


	// Use this for initialization
	void Start () 
	{

	}

	// Update is called once per frame
	void Update () 
	{

	}

	// Return the room that contains the specified entity (can return either the specific room or general level)
	public GameObject getLocationOfEntity(GameObject inputEntity, bool getLevel = false) 
	{
		foreach(GameObject level in levels) 
		{
			var roomSearchResult = level.GetComponent<scriptLevel>().getRoomThatContainsSpecifiedEntity(inputEntity);

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

		Debug.Log("ERROR: " + inputEntity.name + " could not be found, or there is an issue with this finding function.");
		return null;
	}

	// Move a specified entity to a specified room
	public void moveEntityToSpecificRoom(GameObject entityToMove, GameObject targetRoom)
	{
		GameObject initialRoom = getLocationOfEntity(entityToMove);				    // find the current room the entity lives in
		initialRoom.GetComponent<scriptRoom>().removeEntityFromRoom(entityToMove);	// remove entity from initial room
        targetRoom.GetComponent<scriptRoom>().entities.Add(entityToMove);		    // add entity to target room
	}

	// Move an entity to a room in their current level using an xy translation
	public void translateEntityInTheirLevel(GameObject entityToMove, float xChange, float yChange) 
	{
        var scriptLevelContainingEntityToMove = getLocationOfEntity(entityToMove, true).GetComponent<scriptLevel>();
        var scriptRoomContainingEntityToMove = scriptLevelContainingEntityToMove.getRoomThatContainsSpecifiedEntity(entityToMove).GetComponent<scriptRoom>();
        GameObject targetRoom = scriptLevelContainingEntityToMove.getRoomByCoordinates(scriptRoomContainingEntityToMove.xPosition + xChange,
                                                                                        scriptRoomContainingEntityToMove.yPosition + yChange);

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
