using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class scriptMenuSubsection: MonoBehaviour 
{
    // Inspector Variables
    public GameObject currentCollectionToRepresentWithOptions;      // a collection (ex: level of rooms, inventory of items, room of entities) to base the options for this section off of
    public GameObject defaultCollectionToRepresentWithOptions;
    public float xDefaultOptionSelection;                           // the x coordinate of the default option that is selected when entering this subsection
    public float yDefaultOptionSelection;                           // the y coordinate of the default option that is selected when entering this subsection
    public float xSimplePosition;                                   // simple x coordinate for where this subsection is positioned in the section, not nessesarily where it is rendered on the screen
    public float ySimplePosition;                                   // simple y coordinate for where this subsection is positioned in the section, not nessesarily where it is rendered on the screen
    public float originPointX;                                      // the origin point that this subsection will render at
    public float originPointY;
    public float optionGraphicDefaultXScale;                        // the default horizontal length scale (scale within cell) of an option graphic for this subsection
    public float optionGraphicDefaultYScale;                        // the default vertical length scale (scale within cell) of an option graphic for this subsection
    public float optionCellDefaultXLength;                          // the default horizontal length of a grid cell for this subsection
    public float optionCellDefaultYLength;                          // the default vertical length of a grid cell for this subsection
    public float xCurrentOptionSelection;                           // the x coordinate of the currently selected option
    public float yCurrentOptionSelection;                           // the y coordinate of the currently selected option
    public List<GameObject> options;                                // the option gameobjects generated from the collectionToRepresentWithOptions


    // Private Variables



    // Use this for initialization
    void Start() 
    {
        xCurrentOptionSelection = xDefaultOptionSelection;                                  // set currently selected option to the default section
        yCurrentOptionSelection = yDefaultOptionSelection;
    }

    // Update is called once per frame
    void Update() 
    {

    }

    // Return the subsection object located at the coordinate parameters
    public GameObject getOptionByCoordinates(float xInput, float yInput) 
    {
        return options.SingleOrDefault(option =>
            option.GetComponent<scriptMenuOption>().xSimplePosition == xInput &&
            option.GetComponent<scriptMenuOption>().ySimplePosition == yInput);
    }

    public GameObject getCurrentlySelectedOption()
    {
        return getOptionByCoordinates(xCurrentOptionSelection, yCurrentOptionSelection);
    }
}