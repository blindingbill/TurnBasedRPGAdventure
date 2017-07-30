using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class scriptRenderManager: MonoBehaviour 
{
	// Inspector Variables
    public float zScaleAll;                         // the z scale of everything that's rendered, since the game is 2D I don't think this will matter and might be phased out later, keep it above 0 but not too big to affect layers
    public float passagewayConnectionDefaultYScale; // the standard scale (relative to the menuDiv cell dimensions) that the passageway connections are rendered at


	// Private Variables



	// Use this for initialization
	void Start() 
    {

	}

	// Update is called once per frame
	void Update()
    {
        
    }

    // GENERIC MENUDIV RENDERER: Render a generic menuDiv using all the potential custom rendering functions based on what gameobject it represents
    public void renderMenuDiv(GameObject gameManager, GameObject menuDivToRender, GameObject parentOfMenuDivToRender = null)
    {
        var scriptMenuDivToRender = menuDivToRender.GetComponent<scriptMenuDiv>();          // the current menuDiv to render, all of its children will be rendered as well
        var menuDivToRenderRenderer = scriptMenuDivToRender.GetComponent<Renderer>();       // the renderer component for the menuDivToRender
        var scriptGameManager = gameManager.GetComponent<scriptGameManager>();              // the game manager
        var scriptCurrentWorld = scriptGameManager.world.GetComponent<scriptWorld>();       // the world of this game
        bool menuDivToRenderHasChildren = scriptMenuDivToRender.childMenuDivs.Count > 0;    // does the menuDiv to render have children?
        float xFinalRenderLocation = 0;                                                     // the xyz coordinates that this menuDiv will ultimately be rendered at, defaulted to 0
        float yFinalRenderLocation = 0;
        float zFinalRenderLocation = -0.1f;                                                 // TODO: The zFinalRenderLocation should probably be used for layers, but it'll have to wait until multiple menu functionality is more fleshed out.
        float zFinalRenderRotation = 0;                                                     // the angle that this menuDiv will ultimately be rendered at, defaulted to 0
        float xGraphicScaleInCell = scriptMenuDivToRender.graphicCustomXScale;              // the xy scale of a menuDiv graphic relative to the length of its cell
        float yGraphicScaleInCell = scriptMenuDivToRender.graphicCustomYScale;
        float xCellLength = scriptMenuDivToRender.cellCustomXLength;                        // the xy length of the menuDiv cell (the amount of space it takes up, even if its not all filled with the graphic)
        float yCellLength = scriptMenuDivToRender.cellCustomYLength;
        float xFinalRenderScale = xCellLength * xGraphicScaleInCell;                        // the xy scale that this menuDiv will ultimately be rendered at, defaulted to the product of the custom graphicScale and cell length, is likely to be 0
        float yFinalRenderScale = yCellLength * yGraphicScaleInCell;


        // DOES THIS MENUDIV HAVE A PARENT?: If the menuDivToRender has a parent (provided in the optional parameter for renderMenuDiv)
        if (parentOfMenuDivToRender != null)
        {
            var scriptParentOfMenuDivToRender = parentOfMenuDivToRender.GetComponent<scriptMenuDiv>();                                          // the parent menuDiv to the menuDivToRender
            var scriptCurrentlySelectedChild = scriptParentOfMenuDivToRender.getCurrentlySelectedChildMenuDiv().GetComponent<scriptMenuDiv>();  // the currently selected menuDiv of the parent


            // IS THIS MENUDIV PART OF PARENTS SIMPLEGRID?: If the menuDivToRender is part of its parents simple grid (meaning that it is selectable by the player, and not just a detail menuDiv)
            if (scriptMenuDivToRender.isInParentsSimpleGrid)
            {
                // SET POSITION USING SIMPLE GRID: Position menuDivToRender based on its simple grid location and cell length
                var xOffsetFromParent = scriptMenuDivToRender.xSimplePosition * scriptParentOfMenuDivToRender.childCellDefaultXLength;    // TODO: Set up a system to account for custom cell length
                var yOffsetFromParent = scriptMenuDivToRender.ySimplePosition * scriptParentOfMenuDivToRender.childCellDefaultYLength;
                xFinalRenderLocation = scriptParentOfMenuDivToRender.transform.position.x + xOffsetFromParent;                            // Offset the menuDivToRender from its parent's location
                yFinalRenderLocation = scriptParentOfMenuDivToRender.transform.position.y + yOffsetFromParent;


                // IS THIS MENUDIV SELECTED BY ITS PARENT?: If menuDivToRender is selected by its parent, then make appropriate adjustments to how it will render
                bool isSelected = (menuDivToRender == scriptCurrentlySelectedChild.gameObject);

                if (isSelected == true) // <TODO> As a placeholder, selection turns the menuDiv's material red, eventually this will need to be changed and possibly customizable.
                {
                    menuDivToRenderRenderer.material.color = new Color(1, 0, 0, 1);
                }
                else
                {
                    menuDivToRenderRenderer.material.color = new Color(1, 1, 1, 1);
                }
            }


            // CUSTOM GRAPHIC SCALE & CELL LENGTH: Set xy graphic scale and cell length to its parent's child-defaults if menuDivToRender has 0's for custom values
            if (xGraphicScaleInCell == 0)
                xGraphicScaleInCell = scriptParentOfMenuDivToRender.childGraphicDefaultXScale;
            if (yGraphicScaleInCell == 0)
                yGraphicScaleInCell = scriptParentOfMenuDivToRender.childGraphicDefaultXScale;
            if (xCellLength == 0)
                xCellLength = scriptParentOfMenuDivToRender.childCellDefaultXLength;
            if (yCellLength == 0)
                yCellLength = scriptParentOfMenuDivToRender.childCellDefaultYLength;

            // Adjust the finalRenderScale now that potential custom values have been taken into account
            xFinalRenderScale = xCellLength * xGraphicScaleInCell;
            yFinalRenderScale = yCellLength * yGraphicScaleInCell;


            // IF PASSAGEWAY CONNECTION: <HACK> If the menuDiv is a passagewayConnection, then do the nessesary rendering steps for that (I think there will ultimately be more custom cases for menuDivs, but currently this feels dirty)
            if (scriptMenuDivToRender.representedGameObject != null && scriptMenuDivToRender.representedGameObject.tag == "PassagewayConnection")
            {
                // DIFFERENCE BETWEEN TWO MENUDIVS: find the difference between the two menuDivs that represnt the rooms connected by the passagewayConnection
                var scriptPassagewayConnection = scriptMenuDivToRender.representedGameObject.GetComponent<scriptPassagewayConnection>();                // the passagewayConnection this menuDiv represents
                List<GameObject> connectedRooms = scriptCurrentWorld.getConnectedRoomsFromPassagewayConnection(scriptPassagewayConnection.gameObject);  // the two rooms connected by the passagewayConnection
                var scriptRoomA = connectedRooms[0].GetComponent<scriptRoom>();
                var scriptRoomB = connectedRooms[1].GetComponent<scriptRoom>();

                // search the children of the parent of menuDivToRender to find the children menuDivs that represent the two rooms that the passagewayConnection connects
                var scriptMenuDivA = scriptParentOfMenuDivToRender.childMenuDivs.SingleOrDefault(childMenuDiv =>
                    childMenuDiv.GetComponent<scriptMenuDiv>().representedGameObject == scriptRoomA.gameObject).GetComponent<scriptMenuDiv>();
                var scriptMenuDivB = scriptParentOfMenuDivToRender.childMenuDivs.SingleOrDefault(childMenuDiv => 
                    childMenuDiv.GetComponent<scriptMenuDiv>().representedGameObject == scriptRoomB.gameObject).GetComponent<scriptMenuDiv>();

                float xDifferenceBetweenMenuDivs = scriptMenuDivA.transform.position.x - scriptMenuDivB.transform.position.x;   // the difference between the two menuDivs that a line will be rendered between
                float yDifferenceBetweenMenuDivs = scriptMenuDivA.transform.position.y - scriptMenuDivB.transform.position.y;


                // LINE ROTATION: adjust the rotation so that the menuDivToRender will render at to an angle that touches the two menuDivs it is between
                zFinalRenderRotation = Mathf.Atan2(yDifferenceBetweenMenuDivs, xDifferenceBetweenMenuDivs) * (180 / Mathf.PI);


                // LINE SLOPE: find the slope of what a line connecting the two menuDivs would be
                float slope = 0;                        // the defaults assume that the line is vertical, since calculating a vertical slope requires dividing by 0
                bool isVertical = true;                 // since both vertical and horizontal lines have a slope of 0, a boolean will be set to distinguish between them
                if (xDifferenceBetweenMenuDivs != 0)    // check if the xDifference is 0, to avoid dividing by 0, if it isn't, then set the slope accordingly
                {
                    slope = (yDifferenceBetweenMenuDivs / xDifferenceBetweenMenuDivs);
                    isVertical = false;
                }


                // LINE LENGTH: scale the length of the line to accomodate for rotation and the scale of the cell its in
                if (slope != 0)         // if diagonal, increase x scale to fit diagonal distance between two menuDivs
                {
                    xFinalRenderScale = Mathf.Sqrt(Mathf.Pow(scriptParentOfMenuDivToRender.childCellDefaultXLength, 2) + Mathf.Pow(scriptParentOfMenuDivToRender.childCellDefaultYLength, 2));
                }
                else if (slope == 0)    // if not diagonal, then check if vertical or horizontal
                {
                    if (isVertical)     // if vertical, set line length to vertical cell length
                        xFinalRenderScale = yCellLength;
                    else                // if horizontal, set line length to horizontal cell length
                        xFinalRenderScale = xCellLength;
                }

                yFinalRenderScale = (passagewayConnectionDefaultYScale * ((xCellLength + yCellLength) / 2));

                // set render location at the midpoint between menudivs
                xFinalRenderLocation = (scriptMenuDivA.transform.position.x + scriptMenuDivB.transform.position.x) / 2;
                yFinalRenderLocation = (scriptMenuDivA.transform.position.y + scriptMenuDivB.transform.position.y) / 2;
            }
        }


        // CUSTOM ORIGIN POINT: If menuDivToRender uses a custom origin point, override the current render position for it
        if (scriptMenuDivToRender.usesCustomGlobalOriginPoint)
        {
            xFinalRenderLocation = scriptMenuDivToRender.customGlobalOriginPointX;
            yFinalRenderLocation = scriptMenuDivToRender.customGlobalOriginPointY;
        }
            

        // RENDER POSITION, SCALE, ROTAION: render the menuDivToRender (position, scale, rotate)
        menuDivToRender.transform.position = new Vector3(xFinalRenderLocation,
            yFinalRenderLocation,                                                        
            zFinalRenderLocation);                                                                  // TODO: z coordinate to render on a specific layer, should be made customizable in inspector, but I think a lot of layering could end up changing based on context

        menuDivToRender.transform.localScale = new Vector3(xFinalRenderScale,
            yFinalRenderScale,
            zScaleAll);

        menuDivToRender.transform.eulerAngles = new Vector3(gameObject.transform.eulerAngles.x,
            gameObject.transform.eulerAngles.y,
            zFinalRenderRotation);                                                                  // Since the game is 2D, only the Z rotation seems to matter


        // RENDER CHILDREN: If menuDivToRender has children, render them all
        if (menuDivToRenderHasChildren)
        {
            foreach (GameObject childMenuDiv in scriptMenuDivToRender.childMenuDivs)
            {
                renderMenuDiv(gameManager, childMenuDiv, menuDivToRender);
            }
        }
    }
}