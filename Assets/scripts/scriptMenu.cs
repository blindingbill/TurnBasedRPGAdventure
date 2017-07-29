using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scriptMenu: MonoBehaviour 
{
    // Inspector Variables
    public List<GameObject> menuSections;           // sections (ex: top nav bar, entity selection lineup, action selection dropdown, inventory grid) that make up a menu 
    public GameObject currentlySelectedMenuSection; // the current menu section that the player's cursor is in
    public GameObject defaultSelectedMenuSection;   // the initial menu section that the player cursor will be on for the menu it belongs to


    // Private Variables


    // Use this for initialization
    void Start() 
    {
        currentlySelectedMenuSection = defaultSelectedMenuSection; // set the currently selected section to the default section
    }

    // Update is called once per frame
    void Update() 
    {

    }

    // Move the current option selection in this menu based on xy input
    public void moveCurrentOptionSelectionUsingDirectionalInput(float xInput, float yInput)
    {
        var scriptCurrentlySelectedSubsection = getCurrentlySelectedSubsection().GetComponent<scriptMenuSubsection>();
        GameObject targetOption = scriptCurrentlySelectedSubsection.getOptionByCoordinates(scriptCurrentlySelectedSubsection.xCurrentOptionSelection + xInput,
            scriptCurrentlySelectedSubsection.yCurrentOptionSelection + yInput);
        if (targetOption != null)
        {
            scriptCurrentlySelectedSubsection.xCurrentOptionSelection += xInput;
            scriptCurrentlySelectedSubsection.yCurrentOptionSelection += yInput;
        } 
        else 
        {
            Debug.Log("WARNING: There is no option to select based on player inputs, if there's an option in the direction being input, there may be an error.");
        }
    }

    // Get the subsection in the entire menu that the player currently has selected
    public GameObject getCurrentlySelectedSubsection()
    {
        var scriptCurrentlySelectedMenuSection = currentlySelectedMenuSection.GetComponent<scriptMenuSection>();
        GameObject currentlySelectedMenuSubsection = scriptCurrentlySelectedMenuSection.getSubsectionByCoordinates(
            scriptCurrentlySelectedMenuSection.xCurrentSubsectionSelection,
            scriptCurrentlySelectedMenuSection.yCurrentSubsectionSelection);

        return currentlySelectedMenuSubsection;
    }

    // Get the option in the entire menu that the player currently has selected
    public GameObject getCurrentlySelectedOption()
    {
        var scriptCurrentlySelectedMenuSection = currentlySelectedMenuSection.GetComponent<scriptMenuSection>();
        GameObject currentlySelectedMenuSubsection = scriptCurrentlySelectedMenuSection.getSubsectionByCoordinates(
                                                         scriptCurrentlySelectedMenuSection.xCurrentSubsectionSelection,
                                                         scriptCurrentlySelectedMenuSection.yCurrentSubsectionSelection);
        var scriptCurrentlySelectedMenuSubsection = currentlySelectedMenuSubsection.GetComponent<scriptMenuSubsection>();
        GameObject currentlySelectedOption = scriptCurrentlySelectedMenuSubsection.getOptionByCoordinates(
            scriptCurrentlySelectedMenuSubsection.xCurrentOptionSelection,
            scriptCurrentlySelectedMenuSubsection.yCurrentOptionSelection);

        return currentlySelectedOption;
    }
}