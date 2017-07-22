using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scriptEntity : MonoBehaviour 
{

    // Inspector Variables
    public string description;

    public Sprite frontSprite;
    public Sprite backSprite;
    public Sprite deadOrDestroyedSprite;
    public List<Sprite> additionalSprites;

    public int baseStealth;
    public int currentStealth;

    public List<GameObject> customActionsWhenTargeted;
    public List<GameObject> customActionsWhenHeld;

    public List<GameObject> statusEffects;

    public bool isPassageway;
    public GameObject passageWayLinksTo;

    public bool hasSP;
    public int baseMaxSP;
    public int currentmaxSP;
    public int currentSP;

    public bool hasMP;
    public int baseMaxMP;
    public int currentmaxMP;
    public int currentMP;

    public bool hasHP;
    public int baseMaxHP;
    public int currentmaxHP;
    public int currentHP;

    public bool hasInventory;
    public List<GameObject> inventory;
    public int baseMaxCarryWeight;
    public int currentMaxCarryWeight;
    public int currentCarryWeight;

    public bool hasStrength;
    public int baseStrength;
    public int currentStrength;

    public bool hasAP;
    public int baseMaxAP;
    public int currentAP;
    public int currentmaxAP;

    public bool hasPoise;
    public int basePoise;
    public int currentPoise;

    public bool hasPerception;
    public int basePerception;
    public int currentPerception;
    public int currentSenseOfSight;
    public int currentSenseOfHearing;
    public int currentSenseOfSmell;

    public bool hasIntelligence;
    public int baseIntelligence;
    public int currentIntelligence;

    public bool hasFortitude;
    public int baseFortitude;
    public int currentFortitude;

    public bool canTalk;
    public int baseCharisma;
    public int currentCharisma;


    // Private Variables



	// Use this for initialization
	void Start () 
    {
        
	}
	
	// Update is called once per frame
	void Update () 
    {
		
	}
}
