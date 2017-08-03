using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scriptMenuManager: MonoBehaviour 
{
    // Inspector Variables
    public List<GameObject> majorMenuDivs;              // the major menus that make up the game, a menu generally takes up an entire screen and is overlayed over the main menu with a player input
    public GameObject defaultSelectedMajorMenuDiv;      // the initial menu that will is in focus
    public List<GameObject> activeMajorMenuDivs;        // the list of on-screen menus, the order determines the layers they're rendered at, returning to a lower item on the list will remove the item you left from

    public GameObject prefabMenuDiv;                    // the basic menudiv template to instantiate menudivs with


    // Private Variables
    GameObject currentlySelectedMajorMenuDiv;

    // Use this for initialization
    void Start() 
    {
        activeMajorMenuDivs = new List<GameObject> {defaultSelectedMajorMenuDiv};   // set the lowest item in the activeMajorMenuDivs to the default major menuDiv

        // <HACK> this is just a testing setup to generate children specifically for the map
        foreach (GameObject majorMenuDiv in majorMenuDivs)
        {
            foreach (GameObject section in majorMenuDiv.GetComponent<scriptMenuDiv>().childMenuDivs)
            {
                foreach (GameObject subsection in section.GetComponent<scriptMenuDiv>().childMenuDivs)
                {
                    generateChildrenForMenuDiv(subsection);
                }
            }
        }
    }

    // Update is called once per frame
    void Update() 
    {
        // UPDATE CURRENTLY SELECTED MAJOR MENU DIV WITH LAST ACTIVE MENU DIV ITEM
        currentlySelectedMajorMenuDiv = activeMajorMenuDivs[activeMajorMenuDivs.Count - 1];
    }

    // CHANGE MAJOR MENU WITH HOTKEYS: Change the selected majorMenuDiv, and render the results
    public void changeSelectedMajorMenuDivUsingHotkey(GameObject menuDivSelectionInput)
    {
        if (menuDivSelectionInput == currentlySelectedMajorMenuDiv)
        {
            activeMajorMenuDivs.Remove(menuDivSelectionInput);
            // <TODO> Rerender removed menu AKA delete menuDiv and all its children
        }
        else
        {
            activeMajorMenuDivs.Add(menuDivSelectionInput);
            // <TODO> Render new menu
        }
    }

    // PLAYER DIRECTIONAL INPUTS MOVE SELECTION IN CURRENTLY SELECTED MENUDIV: directionally change the selected child in the currently selected menuDiv
    public void moveCurrentMenuDivSelectionUsingDirectionalInput(float xInput, float yInput)
    {
        GameObject currentlySelectedMajorMenuDiv = activeMajorMenuDivs[activeMajorMenuDivs.Count - 1];
        var scriptMenuDivContainingCurrentSelection = currentlySelectedMajorMenuDiv.GetComponent<scriptMenuDiv>().getLowestSelectedMenuDivContainingChildren().GetComponent<scriptMenuDiv>();
        float xTarget = scriptMenuDivContainingCurrentSelection.xCurrentChildSelection + xInput;
        float yTarget = scriptMenuDivContainingCurrentSelection.yCurrentChildSelection + yInput;

        GameObject targetMenuDiv = scriptMenuDivContainingCurrentSelection.getChildMenuDivByCoordinates(xTarget, yTarget);
        if (targetMenuDiv != null && targetMenuDiv.GetComponent<scriptMenuDiv>().isInParentsSimpleGrid)
        {
            scriptMenuDivContainingCurrentSelection.xCurrentChildSelection += xInput;
            scriptMenuDivContainingCurrentSelection.yCurrentChildSelection += yInput;
        }
        else
        {
            Debug.Log("WARNING: There is no option to select based on player inputs, if there's an option in the direction being input, there may be an error.");
        }
    }

    // <TODO> Update all map menu subsections with the player's current level location for their default level to render
//    public void updateAllMapSubsectionsWithPlayerLevelLocation(GameObject playerCurrentLevelLocation)
//    {
//        GameObject[] menuMapSubsections = GameObject.FindGameObjectsWithTag("MenuSubsectionWithLevelMap");
//
//        foreach (GameObject subsection in menuMapSubsections)
//        {
//            subsection.GetComponent<scriptMenuSubsection>().defaultCollectionToRepresentWithOptions = playerCurrentLevelLocation;
//        }
//    }

    // GENERATE MENUDIV CHILDREN FROM GAMEOBJECT CONTAINING GAMEOBJECTS: Convert the collection of objects designated for a menudiv into a list child menudivs
    void generateChildrenForMenuDiv(GameObject menuDiv)
    {
        var scriptMenuDiv = menuDiv.GetComponent<scriptMenuDiv>();
        var i = 1;                                                  // <TODO>: need to find a better way to generate names for these children, if it's even nessesary

        foreach (GameObject collection in scriptMenuDiv.currentCollectionsToConvertToChildren)
        {
            // LEVEL: convert the contents of a level into menudivs
            if (collection.tag == "Level")
            {
                // ROOMS: Convert rooms into menudivs
                foreach (GameObject room in collection.GetComponent<scriptLevel>().rooms)
                {
                    GameObject newMenuDivChild = Instantiate(prefabMenuDiv);
                    var scriptNewMenuDivChild = newMenuDivChild.GetComponent<scriptMenuDiv>();
                    var scriptRoom = room.GetComponent<scriptRoom>();
                    scriptNewMenuDivChild.representedGameObject = scriptRoom.gameObject;

                    scriptNewMenuDivChild.isInParentsSimpleGrid = true;
                    scriptNewMenuDivChild.xSimplePosition = scriptRoom.xSimplePosition;
                    scriptNewMenuDivChild.ySimplePosition = scriptRoom.ySimplePosition;
                    scriptNewMenuDivChild.zSimplePosition = scriptRoom.zSimplePosition;
                    scriptNewMenuDivChild.graphicCustomYScale = scriptMenuDiv.childGraphicDefaultYScale * (scriptRoom.size + 1);
                    scriptNewMenuDivChild.graphicCustomXScale = scriptMenuDiv.childGraphicDefaultXScale * (scriptRoom.size + 1);

                    scriptNewMenuDivChild.name = "menuDiv" + room.name;
                    scriptMenuDiv.childMenuDivs.Add(scriptNewMenuDivChild.gameObject);
                }

                // PASSAGEWAYCONNECTIONS: Convert passagewayConnections into menudivs
                foreach (GameObject passagewayConnection in collection.GetComponent<scriptLevel>().passagewayConnections)
                {
                    GameObject newMenuDivChild = Instantiate(prefabMenuDiv);
                    var scriptNewMenuDivChild = newMenuDivChild.GetComponent<scriptMenuDiv>();
                    var scriptPassagewayConnection = passagewayConnection.GetComponent<scriptPassagewayConnection>();
                    scriptNewMenuDivChild.representedGameObject = scriptPassagewayConnection.gameObject;

                    scriptNewMenuDivChild.name = "menuDiv" + passagewayConnection.name;
                    i += 1;
                    scriptMenuDiv.childMenuDivs.Add(scriptNewMenuDivChild.gameObject);
                }
            }
        }
    }
}