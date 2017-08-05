using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class scriptMenuDiv : MonoBehaviour {

    // Inspector Variables
    public bool isOnlyVisibleWhenSelected;                          // if true, this menudiv and all its descendants are only visible when this menudiv is selected by its parent

    public bool isInParentsSimpleGrid;                              // does this menu div exist on the simple grid of its parent?
    public float xSimplePosition;                                   // simple coordinate for where this option is positioned in its parent, not nessesarily where it is rendered on the screen
    public float ySimplePosition;                                  
    public float zSimplePosition;                                  

    public GameObject defaultChildSelection;                        // the specific game object to select by default, if this is used then this div's simple grid seleciton values won't be taken into account
    public GameObject currentChildSelection;                        // the specific game object that is selected, automatically updated 
    public float xDefaultChildSelection;                            // the coordinate of the current child that is selected in this menu div
    public float yDefaultChildSelection;                           
    public float zDefaultChildSelection;
    public float xCurrentChildSelection;                            // the coordinate of the default child that is selected when entering this menu div
    public float yCurrentChildSelection;                           
    public float zCurrentChildSelection;                           

    public List<GameObject> defaultCollectionsToConvertToChildren;  // a collection (ex: level of rooms, inventory of items, room of entities) to base the options for this section off of
    public List<GameObject> currentCollectionsToConvertToChildren;  
    public List<GameObject> childMenuDivs;                          // menu divs that belong under this menu div (sections, subsections, options, etc.)
    public GameObject representedGameObject;                        // OPTIONAL: the gameObject that this menudiv respresents
    public float childCellDefaultXLength;                           // the default horizontal cell length of this div's children
    public float childCellDefaultYLength;                           // the default vertical cell length of this div's children
    public float childGraphicDefaultXScale;                         // the default horizontal graphic scale of this div's children
    public float childGraphicDefaultYScale;                         // the default vertical grapic scale of this div's children

    public bool usesCustomGlobalOriginPoint;                        // the forced global origin point of this menu div, should be primarily used for major menu sections
    public float customGlobalOriginPointX;
    public float customGlobalOriginPointY;
    public float cellCustomXLength;                                 // the custom horizontal length of this grid cell
    public float cellCustomYLength;                                 // the custom vertical length of this grid cell
    public float graphicCustomXScale;                               // the custom horizontal scale of this menu graphic
    public float graphicCustomYScale;                               // the custom vertical scale of this menu graphic


    // Private Variables



    // Use this for initialization
    void Start() 
    {
        // <TODO> Put this in a more appropriate place since this will be happening more than once
        currentCollectionsToConvertToChildren = defaultCollectionsToConvertToChildren;

        // Set simple grid selections to defaults
        xCurrentChildSelection = xDefaultChildSelection;
        yCurrentChildSelection = yDefaultChildSelection;
        zCurrentChildSelection = zDefaultChildSelection;

        // <TODO> If this menudiv has a default child selection, set it to that as a start. Eventually will need to be called at later points.
        if (defaultChildSelection != null)
        {
            currentChildSelection = defaultChildSelection;
        }
    }

    // Update is called once per frame
    void Update() 
    {
        // If menudiv doesn't use specific game object references to its children (if it doesn't have a default selection object reference), then always update with its simple grid child selection
        if (defaultChildSelection == null)
        {
            currentChildSelection = getChildMenuDivByCoordinates(xCurrentChildSelection, yCurrentChildSelection, zCurrentChildSelection);
        }
    }

    // USE SIMPLEGRID XYZ COORDINATES TO FIND CHILD MENUDIV IN THIS MENUDIV
    public GameObject getChildMenuDivByCoordinates(float xInput, float yInput, float zInput = 0)
    {
        return childMenuDivs.SingleOrDefault(childMenuDiv =>
            childMenuDiv.GetComponent<scriptMenuDiv>().xSimplePosition == xInput &&
            childMenuDiv.GetComponent<scriptMenuDiv>().ySimplePosition == yInput &&
            childMenuDiv.GetComponent<scriptMenuDiv>().zSimplePosition == zInput &&
            childMenuDiv.GetComponent<scriptMenuDiv>().isInParentsSimpleGrid == true);
    }

    // GET THE LOWEST CURRENTLY SELECTED MENU THAT CONTAINS CHILDREN: primarily used to get the container that the player's cursor has current control over
    public GameObject getLowestSelectedMenuDivContainingChildren()
    {
        GameObject tempParentMenuDiv = null;
        GameObject tempCurrentMenuDiv = this.gameObject;

        while (tempCurrentMenuDiv.GetComponent<scriptMenuDiv>().childMenuDivs.Count > 0)
        {
            tempParentMenuDiv = tempCurrentMenuDiv;
            tempCurrentMenuDiv = tempCurrentMenuDiv.GetComponent<scriptMenuDiv>().currentChildSelection;
        }

        return tempParentMenuDiv;
    }
}
