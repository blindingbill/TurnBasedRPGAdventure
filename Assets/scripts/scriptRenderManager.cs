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

    public void renderMenuDiv(GameObject gameManager, GameObject menuDivToRender, GameObject parentOfMenuDivToRender = null)
    {
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

                yFinalRenderScale = (passagewayConnectionDefaultYScale * ((xCellLength + yCellLength) / 2));

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
}