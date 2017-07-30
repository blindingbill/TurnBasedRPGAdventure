using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class scriptLevel: MonoBehaviour 
{
	// Inspector Variables
	public float xWorldMapPosition; // x coordinate for this level location on the world map
	public float yWorldMapPosition; // y coordinate for this level location on the world map

	public List <GameObject> rooms; // All the rooms in this level
	public List <GameObject> passagewayConnections; // All the passageway connections (two linked passageways connecting rooms) in this level


	// Private Variables
	private float xForCurrentRoomSelection = 0; // x coordinate for the currently selected room on the map interface
	private float yForCurrentRoomSelection = 0; // y coordinate for the currently selected room on the map interface


	// Use this for initialization
	void Start() 
	{

	}

	// Update is called once per frame
	void Update() 
	{
		
	}
		
	// USE SIMPLE XYZ COORDINATES TO FIND A ROOM IN THIS LEVEL: Return the room GameObject located at the coordinate parameters
	public GameObject getRoomByCoordinates(float xInput, float yInput)  // <TODO> Add Z coordinate search for floors
	{
		return rooms.SingleOrDefault(room =>
			room.GetComponent<scriptRoom>().xSimplePosition == xInput &&
            room.GetComponent<scriptRoom>().ySimplePosition == yInput);
	}

	// GET THE ROOM THAT A PROVIDED ENTITY IS IN: Return the room that contains the specified entity
	public GameObject getRoomThatContainsSpecifiedEntity(GameObject inputEntity) 
	{
		foreach(GameObject room in rooms) 
        {
			GameObject roomSearchResult = room.GetComponent<scriptRoom>().entities.SingleOrDefault(entity => entity == inputEntity);

			if (roomSearchResult != null) 
            {
				return room; // return room if the specified entity is located in it
			}
		}
			
		return null;
	}
}