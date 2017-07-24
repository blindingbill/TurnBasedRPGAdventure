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
}