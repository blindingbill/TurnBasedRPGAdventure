using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

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
		renderPassagewayConnections(levelToRender, gridScale, xForFullsizeMapOriginPoint, yForOriginPoint, yScaleRooms);
	}

    public void renderMenuDiv(GameObject gameManager, GameObject menuDivToRender, GameObject parentOfMenuDivToRender = null)
    {
        // render this menu div
        // check for children
        // if children, render children with renderMenuDiv (this will cover everything I think)

        var scriptMenuDivToRender = menuDivToRender.GetComponent<scriptMenuDiv>();
        var menuDivRenderer = scriptMenuDivToRender.GetComponent<Renderer>();
        bool menuDivToRenderHasChildren = scriptMenuDivToRender.childMenuDivs.Count > 0;
        float xFinalRenderLocation = 0;
        float yFinalRenderLocation = 0;
        float zFinalRenderLocation = -0.1f;
        float zFinalRenderRotation = 0;
        float xGraphicScaleInCell = scriptMenuDivToRender.graphicCustomXScale;
        float yGraphicScaleInCell = scriptMenuDivToRender.graphicCustomYScale;
        float xCellLength = scriptMenuDivToRender.cellCustomXLength;
        float yCellLength = scriptMenuDivToRender.cellCustomYLength;
        float xFinalRenderScale = xCellLength * xGraphicScaleInCell;
        float yFinalRenderScale = yCellLength * yGraphicScaleInCell;

        if (parentOfMenuDivToRender != null)
        {
            var scriptParentOfMenuDivToRender = parentOfMenuDivToRender.GetComponent<scriptMenuDiv>();

            // Position
            if (scriptMenuDivToRender.isInParentsSimpleGrid)
            {
                var xOffset = scriptMenuDivToRender.xSimplePosition * scriptParentOfMenuDivToRender.childCellDefaultXLength;
                var yOffset = scriptMenuDivToRender.ySimplePosition * scriptParentOfMenuDivToRender.childCellDefaultYLength;

                xFinalRenderLocation = scriptParentOfMenuDivToRender.transform.position.x + xOffset;
                yFinalRenderLocation = scriptParentOfMenuDivToRender.transform.position.y + yOffset;


                // HACK: isSelected color
                GameObject currentlySelectedChild = scriptParentOfMenuDivToRender.getCurrentlySelectedChildMenuDiv();

                if (currentlySelectedChild != null)
                {
                    var scriptCurrentlySelectedChild = currentlySelectedChild.GetComponent<scriptMenuDiv>();
                    if (scriptCurrentlySelectedChild.isInParentsSimpleGrid)
                    {
                        bool isSelected = (menuDivToRender == currentlySelectedChild); // is this option selected by its subsection?

                        if (isSelected == true)
                        {
                            menuDivRenderer.material.color = new Color(1, 0, 0, 1);
                        }
                        else
                        {
                            menuDivRenderer.material.color = new Color(1, 1, 1, 1);
                        }
                    }
                }
            }

            // Graphic Scale
            if (xGraphicScaleInCell == 0)
                xGraphicScaleInCell = scriptParentOfMenuDivToRender.childGraphicDefaultXScale;
            if (yGraphicScaleInCell == 0)
                yGraphicScaleInCell = scriptParentOfMenuDivToRender.childGraphicDefaultXScale;

            // Cell Length
            if (xCellLength == 0)
                xCellLength = scriptParentOfMenuDivToRender.childCellDefaultXLength;
            if (yCellLength == 0)
                yCellLength = scriptParentOfMenuDivToRender.childCellDefaultYLength;

            xFinalRenderScale = xCellLength * xGraphicScaleInCell;
            yFinalRenderScale = yCellLength * yGraphicScaleInCell;


            // HACK: Passagewayconnections
            if (scriptMenuDivToRender.representedGameObject != null && scriptMenuDivToRender.representedGameObject.tag == "PassagewayConnection")
            {
                var scriptPassagewayConnection = scriptMenuDivToRender.representedGameObject.GetComponent<scriptPassagewayConnection>();
                var scriptCurrentWorld = gameManager.GetComponent<scriptGameManager>().world.GetComponent<scriptWorld>();
                var scriptPassagewayA = scriptPassagewayConnection.passagewayA.GetComponent<scriptEntity>();
                var scriptRoomA = scriptCurrentWorld.getLocationOfEntity(scriptPassagewayA.gameObject).GetComponent<scriptRoom>();
                var scriptMenuDivA = scriptParentOfMenuDivToRender.childMenuDivs.SingleOrDefault(childMenuDiv => childMenuDiv.GetComponent<scriptMenuDiv>().representedGameObject == scriptRoomA.gameObject).GetComponent<scriptMenuDiv>();
                var scriptPassagewayB = scriptPassagewayConnection.passagewayB.GetComponent<scriptEntity>();
                var scriptRoomB = scriptCurrentWorld.getLocationOfEntity(scriptPassagewayB.gameObject).GetComponent<scriptRoom>();
                var scriptMenuDivB = scriptParentOfMenuDivToRender.childMenuDivs.SingleOrDefault(childMenuDiv => childMenuDiv.GetComponent<scriptMenuDiv>().representedGameObject == scriptRoomB.gameObject).GetComponent<scriptMenuDiv>();

                float xDifference = scriptMenuDivA.transform.position.x - scriptMenuDivB.transform.position.x;
                float yDifference = scriptMenuDivA.transform.position.y - scriptMenuDivB.transform.position.y;
                zFinalRenderRotation = Mathf.Atan2(yDifference, xDifference) * (180 / Mathf.PI);

                // find the slope
                float slope = 0;
                bool isVertical = true;
                if (xDifference != 0)
                {
                    slope = (yDifference / xDifference);
                    isVertical = false;
                }

                Debug.Log(scriptMenuDivToRender.name + " : " + slope);

                // scale the length of the line to render
                if (slope != 0)         // if diagonal, increase x scale to fit diagonal distance between two menudivs
                {
                    xFinalRenderScale = Mathf.Sqrt(Mathf.Pow(scriptParentOfMenuDivToRender.childCellDefaultXLength, 2) + Mathf.Pow(scriptParentOfMenuDivToRender.childCellDefaultYLength, 2));
                }
                else if (slope == 0)    // if not diagonal
                {
                    if (isVertical)     // if vertical, set line length to vertical cell length
                        xFinalRenderScale = yCellLength;
                    else                // if horizontal, set line length to horizontal cell length
                        xFinalRenderScale = xCellLength;
                }

                yFinalRenderScale = 0.05f;  //HACK: Need to provide this value in an inspector somewhere

                // set render location at the midpoint between menudivs
                xFinalRenderLocation = (scriptMenuDivA.transform.position.x + scriptMenuDivB.transform.position.x) / 2;
                yFinalRenderLocation = (scriptMenuDivA.transform.position.y + scriptMenuDivB.transform.position.y) / 2;
            }
        }

        if (scriptMenuDivToRender.usesCustomGlobalOriginPoint)
        {
            xFinalRenderLocation = scriptMenuDivToRender.customGlobalOriginPointX;
            yFinalRenderLocation = scriptMenuDivToRender.customGlobalOriginPointY;
        }
            
        menuDivToRender.transform.position = new Vector3(xFinalRenderLocation,   // coordinate to render this menudiv at
            yFinalRenderLocation,                                                        
            zFinalRenderLocation);                                               // HACK: z coordinate to render on a specific layer, should be made customizable in inspector, but I think a lot of layering could end up changing based on context

        menuDivToRender.transform.localScale = new Vector3(xFinalRenderScale,
            yFinalRenderScale,
            zScaleAll);

        menuDivToRender.transform.eulerAngles = new Vector3(gameObject.transform.eulerAngles.x,
            gameObject.transform.eulerAngles.y,
            zFinalRenderRotation);


        // If this menudiv has children, render those
        if (menuDivToRenderHasChildren)
        {
            foreach (GameObject childMenuDiv in scriptMenuDivToRender.childMenuDivs)
            {
                renderMenuDiv(gameManager, childMenuDiv, menuDivToRender);
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