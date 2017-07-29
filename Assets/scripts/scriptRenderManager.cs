using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scriptRenderManager: MonoBehaviour 
{
	// Inspector Variables
	public float fullsizeMapGridScale; // the scale of one grid cell on the fullsize map for the map menu
	public float fullsizeMapSmallestRoomScale; // the render scale for the smallest room size, as a base to scale up from for larger rooms
	public float xForFullsizeMapOriginPoint; // x coordinate for origin point / center of the fullsize map for the map menu
	public float yForFullsizeMapOriginPoint; // y coordinate for origin point / center of the fullsize map for the map menu

	public float yScaleRooms; // the vertical scale the rooms are rendered at on the map
	public float pathWidth; // the standard width that the passageway connections are rendered at

	public float zScaleAll; // the z scale of everything that's rendered, since the game is 2D I don't think this will matter and might be phased out later, keep it above 0 but not too big to affect layers

	public float thumbnailMapGridScale; // the scale of one grid cell on the thumbnail map for the HUD

	public GameObject levelToRender; // the current level that the rendermanager is rendering (this var will probably be phased out)


	// Private Variables



	// Use this for initialization
	void Start() 
    {

	}

	// Update is called once per frame
	void Update() 
    {
		renderLevelMap(levelToRender, fullsizeMapGridScale, fullsizeMapSmallestRoomScale, xForFullsizeMapOriginPoint, yForFullsizeMapOriginPoint, yScaleRooms); // renders the map for the isolated map menu
	}

	void renderLevelMap(GameObject levelToRender, float gridScale, float smallestRoomScale, float xForOriginPoint, float yForOriginPoint, float yScaleRooms) 
    {
		renderRooms(levelToRender, gridScale, smallestRoomScale, xForOriginPoint, yForOriginPoint, yScaleRooms);
		renderPassagewayConnections(levelToRender, gridScale, xForFullsizeMapOriginPoint, yForOriginPoint, yScaleRooms);
	}

    public void renderMenu(GameObject menuToRender)
    {
        var scriptMenuInput = menuToRender.GetComponent<scriptMenu>();
        foreach (GameObject menuSection in scriptMenuInput.menuSections)
        {
            renderMenuSection(menuSection);
        }
    }

    public void renderMenuSection(GameObject menuSectionToRender)
    {
        var scriptMenuSectionInput = menuSectionToRender.GetComponent<scriptMenuSection>();
        foreach (GameObject menuSubsection in scriptMenuSectionInput.subsections)
        {
            renderMenuSubsection(menuSubsection);
        }
    }

    public void renderMenuSubsection(GameObject menuSubsectionToRender)
    {
        var scriptMenuSubsectionInput = menuSubsectionToRender.GetComponent<scriptMenuSubsection>();
        foreach (GameObject option in scriptMenuSubsectionInput.options)
        {
            renderMenuOption(menuSubsectionToRender, option);
        }
    }

    public void renderMenuOption(GameObject subsectionContainingOptionToRender, GameObject menuOptionToRender)
    {
        var scriptSubsectionContainingOptionToRender = subsectionContainingOptionToRender.GetComponent<scriptMenuSubsection>();
        var scriptMenuOptionToRender = menuOptionToRender.GetComponent<scriptMenuOption>();
        var xToRenderAt = scriptSubsectionContainingOptionToRender.originPointX + (scriptMenuOptionToRender.xSimplePosition * scriptSubsectionContainingOptionToRender.optionCellDefaultXLength);
        var yToRenderAt = scriptSubsectionContainingOptionToRender.originPointY + (scriptMenuOptionToRender.ySimplePosition * scriptSubsectionContainingOptionToRender.optionCellDefaultYLength);
        var zToRenderAt = -0.1f;

        // Position option
        menuOptionToRender.transform.position = new Vector3(xToRenderAt,        // x coordinate to render this room at (offset from the map origin point by the game-world position of the room multiplied by the map scale)
            yToRenderAt,                                                        // y coordinate to render this room at (offset from the map origin point by the game-world position of the room multiplied by the map scale)
            zToRenderAt);                                                       // HACK: z coordinate to render on a specific layer, should be made customizable in inspector, but I think a lot of layering could end up changing based on context


        // Scale option
        float xScale = scriptSubsectionContainingOptionToRender.optionGraphicDefaultXScale;
        float yScale = scriptSubsectionContainingOptionToRender.optionGraphicDefaultYScale;
        if (scriptMenuOptionToRender.optionGraphicCustomXScale != 0)            // if an option has a custom xy scale, use that instead
            xScale = scriptMenuOptionToRender.optionGraphicCustomXScale;
        if (scriptMenuOptionToRender.optionGraphicCustomYScale != 0)
            yScale = scriptMenuOptionToRender.optionGraphicCustomYScale;

        scriptMenuOptionToRender.transform.localScale = new Vector3(scriptSubsectionContainingOptionToRender.optionCellDefaultXLength * xScale,
            scriptSubsectionContainingOptionToRender.optionCellDefaultYLength * yScale,
            zScaleAll);


        // Generate graphic based on if the option is selected by its subsection
        bool isSelected = (menuOptionToRender == scriptSubsectionContainingOptionToRender.getCurrentlySelectedOption()); // is this option selected by its subsection?

        var optionRenderer = scriptMenuOptionToRender.GetComponent<Renderer>();
        if (isSelected == true) 
        {
            optionRenderer.material.color = new Color(1, 0, 0, 1);
        } 
        else 
        {
            optionRenderer.material.color = new Color(1, 1, 1, 1);
        }

          // TO DEVELOP!! Extra case for rendering paths if the subsection is of the MenuSubsectionWithLevelMap tag
    }

	void renderRooms(GameObject levelToRender, float gridScale, float smallestRoomScale, float xForMapOriginPoint, float yForMapOriginPoint, float yScaleRooms) 
    {
		foreach(GameObject room in levelToRender.GetComponent<scriptLevel>().rooms) 
        {
			var scriptRoom = room.GetComponent<scriptRoom>();
			float xForGeographicalRoomPosition = scriptRoom.xSimplePosition; // x coordinate for this room's game-world position (not where it's being rendered on screen)
			float yForGeographicalRoomPosition = scriptRoom.ySimplePosition; // y coordinate for this room's game-world position (not where it's being rendered on screen)
			float geographicalRoomSize = scriptRoom.size; // this room's game-world size (not the size it's being rendered at on screen)

			// Position room on map
			room.transform.position = new Vector3(xForMapOriginPoint + (xForGeographicalRoomPosition * gridScale), // x coordinate to render this room at (offset from the map origin point by the game-world position of the room multiplied by the map scale)
				                                    yForMapOriginPoint + ((yForGeographicalRoomPosition * gridScale) * yScaleRooms), // y coordinate to render this room at (offset from the map origin point by the game-world position of the room multiplied by the map scale)
				                                    -0.1f); // z coordinate to render paths behind the rooms, this will probably be phased out later, or replaced with some better form of layering

			// Scale room on map
			float renderScale = smallestRoomScale * (geographicalRoomSize + 1); // scale room on map based on the smallest set size a room can be rendered, multiplied by the game-world size of the room 
			room.transform.localScale = new Vector3(renderScale, renderScale * yScaleRooms, zScaleAll);

			// Render room color based on selection (will probably be phased out or altered as the visual style is implemented)
			bool isSelected = scriptRoom.isSelected; // is this the room the player is selecting on the map?
			var roomRenderer = room.GetComponent <Renderer>();
			if (isSelected == true) 
            {
				roomRenderer.material.color = new Color(1, 0, 0, 1);
			} 
            else 
            {
				roomRenderer.material.color = new Color(1, 1, 1, 1);
			}
		}
	}

	void renderPassagewayConnections(GameObject levelToRender, float gridScale, float xForMapOriginPoint, float yForMapOriginPoint, float yScaleRooms) 
    {
		foreach(GameObject passagewayConnection in levelToRender.GetComponent <scriptLevel>().passagewayConnections) 
        {
			var scriptPassagewayConnection = passagewayConnection.GetComponent <scriptPassagewayConnection>();
			var scriptPassagewayA = scriptPassagewayConnection.passagewayA.GetComponent <scriptEntity>();
			var scriptPassagewayB = scriptPassagewayConnection.passagewayB.GetComponent <scriptEntity>();
			var scriptLevel = levelToRender.GetComponent <scriptLevel>();
			GameObject roomContainingPassagewayA = scriptLevel.getRoomThatContainsSpecifiedEntity(scriptPassagewayA.gameObject);
			GameObject roomContainingPassagewayB = scriptLevel.getRoomThatContainsSpecifiedEntity(scriptPassagewayB.gameObject);

			if (roomContainingPassagewayA != null && roomContainingPassagewayB != null) 
            {
				var scriptRoomContainingPassagewayA = roomContainingPassagewayA.GetComponent <scriptRoom>();
				var scriptRoomContainingPassagewayB = roomContainingPassagewayB.GetComponent <scriptRoom>();

				// check if the rooms are on the same floor
				if (scriptRoomContainingPassagewayA.zSimplePosition == scriptRoomContainingPassagewayB.zSimplePosition) 
                {
					// if true, render a passageway between two touching rooms on the same floor
					// find the angle for a line between to two rooms that it links together
					var xDifference = scriptRoomContainingPassagewayA.transform.position.x - scriptRoomContainingPassagewayB.transform.position.x;
					var yDifference = scriptRoomContainingPassagewayA.transform.position.y - scriptRoomContainingPassagewayB.transform.position.y;
					var passagewayConnectionAngle = Mathf.Atan2(yDifference, xDifference) * (180 / Mathf.PI);

					// check to make sure that the connected rooms are not overlapping, or further than 1,1 away from each other
					if (xDifference <= 1 &&
						xDifference >= -1 &&
						yDifference <= 1 &&
						yDifference >= -1 &&
						xDifference != 0 || yDifference != 0) 
                    {
						// render passageway-connection graphic at correct rotation
						passagewayConnection.transform.eulerAngles = new Vector3(
							gameObject.transform.eulerAngles.x,
							gameObject.transform.eulerAngles.y,
							passagewayConnectionAngle
						);


						// find the slope of the passageway connection
						float passagewayConnectionSlope = 0;
						if (xDifference != 0) 
                        {
							passagewayConnectionSlope = (yDifference / xDifference);
						}


						// scale the length of the line to accomodate the increase diagonal distance between room, or the y scale for vertical lines
						float tempRenderScale = gridScale;
						var verticalGridScale = gridScale * yScaleRooms;
						if (passagewayConnectionSlope == 1 || passagewayConnectionSlope == -1) // if passageway connection is in an ordinal direction, increase x scale to fit the diagonal distance
						{
							tempRenderScale = Mathf.Sqrt(Mathf.Pow(gridScale, 2) + Mathf.Pow(verticalGridScale, 2));
						} 
                        else if (passagewayConnectionSlope == 0) // else, if vertical, multiply x scale by y room scale
						{
							tempRenderScale = tempRenderScale * yScaleRooms;
						}
						passagewayConnection.transform.localScale = new Vector3(tempRenderScale, pathWidth, zScaleAll);


						// render passageway-connection graphic at the midpoint between rooms
						passagewayConnection.transform.position = new Vector3(
							(scriptRoomContainingPassagewayA.transform.position.x + scriptRoomContainingPassagewayB.transform.position.x) / 2,
							(scriptRoomContainingPassagewayA.transform.position.y + scriptRoomContainingPassagewayB.transform.position.y) / 2);
					} 
                    else 
                    {
						Debug.Log("ERROR: " + scriptPassagewayConnection.name + " connects two rooms that are either not touching, or exist in the same location. This will likely cause major issues, so adjust their locations to be touching by one of eight directions, or is touching from the floor above or below.");
					}
				} 
                else 
                {
					// if false, start proccess for z path rendering
					// DO THIS LATER
				}

			} 
            else 
            {
				Debug.Log("ERROR: " + scriptPassagewayConnection.name + " has one or more passageways that could somehow not be located. This will likely cause major issues with this passageway connection.");
			}
		}
	}
}