using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class scriptMenuSection: MonoBehaviour 
{
    // Inspector Variables
    public List<GameObject> subsections;        // subsections that make up this menu section, can line up vertically, horizontally, and in a grid (if the selection reaches the boundary of a section, it will check if it can jump to another section in that direction)
    public float xDefaultSubsectionSelection;   // the x coordinate of the default subsection that is selected when entering this section
    public float yDefaultSubsectionSelection;   // the x coordinate of the default subsection that is selected when entering this section
    public float xCurrentSubsectionSelection;   // the x coordinate of the currently selected subsection
    public float yCurrentSubsectionSelection;   // the y coordinate of the currently selected subsection


    // Private Variables



    // Use this for initialization
    void Start() 
    {
        xCurrentSubsectionSelection = xDefaultSubsectionSelection; // set the currently selected subsection to the default subsection
        yCurrentSubsectionSelection = yDefaultSubsectionSelection;
    }

    // Update is called once per frame
    void Update() 
    {

    }

    // Return the subsection object located at the coordinate parameters
    public GameObject getSubsectionByCoordinates(float xInput, float yInput) 
    {
        return subsections.SingleOrDefault(subsection =>
            subsection.GetComponent<scriptMenuSubsection>().xSimplePosition == xInput &&
            subsection.GetComponent<scriptMenuSubsection>().ySimplePosition == yInput);
    }
}