using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class scriptRoom: MonoBehaviour 
{
	// Inspector Variables
	public float xSimplePosition;               // simple x coordinate for where this room is positioned in the level, not nessesarily where it is rendered on the screen
    public float ySimplePosition;               // simple y coordinate for where this room is positioned in the level, not nessesarily where it is rendered on the screen
	public float zSimplePosition;               // the floor of the level the room is on (0 is ground level)
	public int size;                            // smallest is 0, determines the size the room is rendered on the map, and is the multiplier for in-game room size effects
	public List <GameObject> entities;          // all the objects rendered on the viewport lineup (characters, landmarks, items)    

	// Private Variables



	// Use this for initialization
	void Start() 
	{

	}

	// Update is called once per frame
	void Update() 
	{

	}

	// Remove a specified entity from the room
	public void removeEntityFromRoom(GameObject entityToRemove)
	{
		GameObject entityInThisRoom = entities.SingleOrDefault(entity => entity == entityToRemove);
		if (entityInThisRoom != null)
		{
			entities.Remove(entityInThisRoom);
		}
	}
}