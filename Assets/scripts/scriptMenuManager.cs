﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scriptMenuManager: MonoBehaviour 
{
    // Inspector Variables
    public List<GameObject> menus;              // the major menus that make up the game, a menu generally takes up an entire screen is overlayed over the main menu with a button
    public GameObject selectedMenu;             // the current menu that is in focus
    public GameObject defaultSelectedMenu;      // the initial menu that will is in focus
    public GameObject standardOptionGameObject; // a basic option template to instantiate options with

    // Private Variables


    // Use this for initialization
    void Start() 
    {
        selectedMenu = defaultSelectedMenu; // set currently selected menu to the default menu

        // HACK: this is just for testing
        foreach (GameObject menu in menus)
        {
            foreach (GameObject section in menu.GetComponent<scriptMenu>().menuSections)
            {
                foreach (GameObject subsection in section.GetComponent<scriptMenuSection>().subsections)
                {
                    generateOptionsForMenuSubsection(subsection);
                }
            }
        }
    }

    // Update is called once per frame
    void Update() 
    {

    }

    // Change the selected menu, and render the results
    public void changeSelectedMenu(GameObject menuSelectionInput)
    {
        // Change the menu manager's selected menu to the one provided, if that is already selected, return selection to default
        if (selectedMenu != menuSelectionInput)
        {
            selectedMenu = menuSelectionInput;
        }
        else
        {
            selectedMenu = defaultSelectedMenu;
        }
    }

    // Update all map menu subsections with the player's current level location for their default level to render
    public void updateAllMapSubsectionsWithPlayerLevelLocation(GameObject playerCurrentLevelLocation)
    {
        GameObject[] menuMapSubsections = GameObject.FindGameObjectsWithTag("MenuSubsectionWithLevelMap");

        foreach (GameObject subsection in menuMapSubsections)
        {
            subsection.GetComponent<scriptMenuSubsection>().defaultCollectionToRepresentWithOptions = playerCurrentLevelLocation;
        }
    }

    // Convert the collection of objects designated for a subsection into a list of options
    void generateOptionsForMenuSubsection(GameObject subsection)
    {
        var scriptMenuSubsection = subsection.GetComponent<scriptMenuSubsection>();

        // HACK: currently this process will probably only work if a collection of rooms is used, will need to add cases
        var i = 1;
        foreach (GameObject room in scriptMenuSubsection.currentCollectionToRepresentWithOptions.GetComponent<scriptLevel>().rooms)
        {
            
            GameObject newOption = Instantiate(standardOptionGameObject);
            var scriptNewOption = newOption.GetComponent<scriptMenuOption>();

            // CASE FOR ROOM
            var scriptRoom = room.GetComponent<scriptRoom>();
            scriptNewOption.xSimplePosition = scriptRoom.xSimplePosition;
            scriptNewOption.ySimplePosition = scriptRoom.ySimplePosition;
            scriptNewOption.zSimplePosition = scriptRoom.zSimplePosition;
            scriptNewOption.name = "Option" + i.ToString();
            i += 1;
            scriptMenuSubsection.options.Add(scriptNewOption.gameObject);
        }

    }
}