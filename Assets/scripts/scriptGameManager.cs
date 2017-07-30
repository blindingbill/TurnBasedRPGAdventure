using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class scriptGameManager : MonoBehaviour 
{
	// Inspector Variables
	public GameObject renderManager;              	// Manages the scale and other details for the rendering of the UI
    public GameObject menuManager;                  // Manages the hierarchy of menus, and what is currently selected
	public GameObject world;                      	// The world that this instance of the game takes place in (there should only ever be one being developed, but who knows)
	public GameObject player;                     	// The player character for this instance of the game
	// TODO: add currentTime later


	// Private Variables
	private GameObject playerCurrentLevelLocation;
	private GameObject playerCurrentRoomLocation;


	// Use this for initialization
	void Start () 
	{

	}

	// Update is called once per frame
	void Update () 
	{
		// Find player location
        var scriptWorld = world.GetComponent<scriptWorld>();
        playerCurrentLevelLocation = scriptWorld.getLocationOfEntity(player, true);
        playerCurrentRoomLocation = scriptWorld.getLocationOfEntity(player);

        // Menu manager functions
        var scriptMenuManager = menuManager.GetComponent<scriptMenuManager>();
//        scriptMenuManager.updateAllMapSubsectionsWithPlayerLevelLocation(playerCurrentLevelLocation);

        // Render manager functions
        var scriptRenderManager = renderManager.GetComponent<scriptRenderManager>();
        foreach (GameObject majorMenuDiv in scriptMenuManager.majorMenuDivs)
        {
            scriptRenderManager.renderMenuDiv(this.gameObject, majorMenuDiv);
        }
            
		checkInputs();	// check for directional inputs and move menu selections accordingly
	}

	void checkInputs() 
	{
        var scriptMenuManager = menuManager.GetComponent<scriptMenuManager>();

        // Declare variables to define menu movement direction and value
        float xTranslation = 0;
        float yTranslation = 0;

        // Get components
        var scriptWorld = world.GetComponent<scriptWorld>();
        var scriptCurrentLevelLocation = playerCurrentLevelLocation.GetComponent<scriptLevel>();

        // Define menu translations
		if (Input.GetKeyDown(KeyCode.DownArrow)) 
		{
            yTranslation += -1;
		}

		if (Input.GetKeyDown(KeyCode.UpArrow)) 
		{
            yTranslation += 1;
		}

		if (Input.GetKeyDown(KeyCode.LeftArrow)) 
		{
            xTranslation += -1;
		}

		if (Input.GetKeyDown(KeyCode.RightArrow)) 
		{
            xTranslation += 1;
		}

        // If any directional input was pushed, execute selection
        if (Input.GetKeyDown(KeyCode.DownArrow) ||
            Input.GetKeyDown(KeyCode.UpArrow) ||
            Input.GetKeyDown(KeyCode.LeftArrow) ||
            Input.GetKeyDown(KeyCode.RightArrow))
        {
            scriptWorld.translateEntityInTheirLevel(player.gameObject, xTranslation, yTranslation);
            var scriptCurrentlySelectedMenuDiv = scriptMenuManager.currentlySelectedMenuDiv.GetComponent<scriptMenuDiv>();

            scriptMenuManager.moveCurrentMenuDivSelectionUsingDirectionalInput(xTranslation, yTranslation);
        }
            
        // Check for button input corrosponding to the Map Menu, and toggle that menu if so
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameObject mapMenuSearchResult = scriptMenuManager.majorMenuDivs.SingleOrDefault(majorMenuDiv => majorMenuDiv.GetComponent<scriptMenuDiv>().tag == "LevelMapMenu");

            if (mapMenuSearchResult != null)
            {
                scriptMenuManager.changeSelectedMajorMenuDivUsingHotkey(mapMenuSearchResult);
            }
        }
	}
}
