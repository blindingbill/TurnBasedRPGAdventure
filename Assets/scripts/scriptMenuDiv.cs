using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class scriptMenuDiv : MonoBehaviour {

    // Inspector Variables
    public bool isInParentsSimpleGrid;                              // does this menu div exist on the simple grid of its parent?
    public float xSimplePosition;                                   // simple coordinate for where this option is positioned in its parent, not nessesarily where it is rendered on the screen
    public float ySimplePosition;                                  
    public float zSimplePosition;                                  

    public float xDefaultChildSelection;                           // the coordinate of the current child that is selected in this menu div
    public float yDefaultChildSelection;                           
    public float zDefaultChildSelection;                           
    public float xCurrentChildSelection;                           // the coordinate of the default child that is selected when entering this menu div
    public float yCurrentChildSelection;                           
    public float zCurrentChildSelection;                           

    public List<GameObject> defaultCollectionsToConvertToChildren;  // a collection (ex: level of rooms, inventory of items, room of entities) to base the options for this section off of
    public List<GameObject> currentCollectionsToConvertToChildren;  
    public List<GameObject> childMenuDivs;                          // menu divs that belong under this menu div (sections, subsections, options, etc.)
    public GameObject representedGameObject;                  // OPTIONAL: the gameObject that this menudiv respresents
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
        currentCollectionsToConvertToChildren = defaultCollectionsToConvertToChildren;

        xCurrentChildSelection = xDefaultChildSelection;
        yCurrentChildSelection = yDefaultChildSelection;
        zCurrentChildSelection = zDefaultChildSelection;
    }

    // Update is called once per frame
    void Update() 
    {

    }

    public GameObject getChildMenuDivByCoordinates(float xInput, float yInput, float zInput = 0)
    {
        return childMenuDivs.SingleOrDefault(childMenuDiv =>
            childMenuDiv.GetComponent<scriptMenuDiv>().xSimplePosition == xInput &&
            childMenuDiv.GetComponent<scriptMenuDiv>().ySimplePosition == yInput &&
            childMenuDiv.GetComponent<scriptMenuDiv>().zSimplePosition == zInput &&
            childMenuDiv.GetComponent<scriptMenuDiv>().isInParentsSimpleGrid == true);
    }

    public GameObject getCurrentlySelectedChildMenuDiv()
    {
        return getChildMenuDivByCoordinates(xCurrentChildSelection, yCurrentChildSelection, zCurrentChildSelection);
    }

    public GameObject getLowestSelectedMenuDivContainingChildren()
    {
        GameObject tempParentMenuDiv = null;
        GameObject tempCurrentMenuDiv = this.gameObject;

        while (tempCurrentMenuDiv.GetComponent<scriptMenuDiv>().childMenuDivs.Count > 0)
        {
            tempParentMenuDiv = tempCurrentMenuDiv;
            tempCurrentMenuDiv = tempCurrentMenuDiv.GetComponent<scriptMenuDiv>().getCurrentlySelectedChildMenuDiv();
        }

        return tempParentMenuDiv;
    }
}
