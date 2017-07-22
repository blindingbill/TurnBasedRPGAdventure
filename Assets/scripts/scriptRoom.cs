using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class scriptRoom: MonoBehaviour 
{
	// Inspector Variables
	public float xPosition; // x coordinate for this room in its level
	public float yPosition; // y coordinate for this room in its level
	public float zPosition; // the floor of the level the room is on (0 is ground level)
	public int size; // smallest is 0, determines the size the room is rendered on the map, and is the multiplier for in-game room size effects
	public List <GameObject> entities; // all the objects rendered on the viewport lineup (characters, landmarks, items)    

	// Private Variables
	public bool isSelected = false; // is this the room the player is selecting on the map?


	// Use this for initialization
	void Start() 
	{
		// Set room to selected if it is at position 0, 0 (default position)
		if (xPosition == 0 && yPosition == 0) 
		{
			isSelected = true;
		}
	}

	// Update is called once per frame
	void Update() 
	{

	}

	// Remove a specified entity from the room
	public void removeEntityFromRoom(GameObject entityToRemove)
	{
		var entityInThisRoom = entities.SingleOrDefault(entity => entity == entityToRemove);
		if (entityInThisRoom != null)
		{
			entities.Remove(entityInThisRoom);
		}
	}
}