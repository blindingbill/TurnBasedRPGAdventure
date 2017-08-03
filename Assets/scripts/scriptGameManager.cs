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
	// <TODO> add currentTime later

    public float menuSelectionCooldownStandard;     // the standard cooldown set when a selection is made, to slightly buffer the next selection 

	// Private Variables
	private GameObject playerCurrentLevelLocation;
	private GameObject playerCurrentRoomLocation;
    private float menuAxisInputCooldownTimestamp = 0;   // the timestamp for the next time that the menu selection will be able to be changed, to prevent uncontrollably fast menu selection with even a slight input


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

    // CHECK FOR USER CONTROL INPUTS: run appropriate functions based on user inputs <TODO> Replace specific key controls with generic controls
	void checkInputs() 
	{
        var scriptMenuManager = menuManager.GetComponent<scriptMenuManager>();
        var scriptWorld = world.GetComponent<scriptWorld>();
        var scriptCurrentLevelLocation = playerCurrentLevelLocation.GetComponent<scriptLevel>();


        // MOVE MENU SELECTION WITH DIRECTIONAL INPUT: If any directional input was pushed, execute selection
        float menuHorizontalMovement = Mathf.Ceil(Input.GetAxis("MenuHorizontal"));
        float menuVerticalMovement = Mathf.Ceil(Input.GetAxis("MenuVertical"));
            
        if ((menuHorizontalMovement != 0 || menuVerticalMovement != 0) && menuAxisInputCooldownTimestamp <= Time.time)          // if there is directional input, and the current time has passed the selection cooldown
		{
            menuAxisInputCooldownTimestamp = Time.time + menuSelectionCooldownStandard;                                         // add to the menu selection cooldown to prevent rapid uncontrollable selection

            scriptWorld.translateEntityInTheirLevel(player.gameObject, menuHorizontalMovement, menuVerticalMovement);           // <HACK> Moving the player with the D-inputs is just a temporary testing tool
            scriptMenuManager.moveCurrentMenuDivSelectionUsingDirectionalInput(menuHorizontalMovement, menuVerticalMovement);
		}

            
        // HOTKEY FOR MAP MENU: Check for button input corrosponding to the Map Menu, and toggle that menu if so
        if (Input.GetButtonDown("Map"))
        {
            GameObject mapMenuSearchResult = scriptMenuManager.majorMenuDivs.SingleOrDefault(majorMenuDiv => majorMenuDiv.GetComponent<scriptMenuDiv>().tag == "LevelMapMenu");

            if (mapMenuSearchResult != null)
            {
                scriptMenuManager.changeSelectedMajorMenuDivUsingHotkey(mapMenuSearchResult);
            }
        }
	}
}
