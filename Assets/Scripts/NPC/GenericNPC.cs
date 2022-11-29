/* 
 * Programmers: Jack Kennedy
 * Purpose: sets up things for every generic NPC
 * Inputs: player health and if we are paused
 * Outputs: on screen hearts
 */

using UnityEngine;

public class GenericNPC : MonoBehaviour
{
    #region Variables

    private NPC thisNpc;

    // name for our npc (eg: "jack")
    [SerializeField] private string npcName = "Grug";

    // interact range for our npc
    private const float interactRange = 2.5f;

    // time that our room enter dialog stays for
    public float hangTime;

    // dialog that plays when we enter the room
    [TextArea(1, 5)]
    [SerializeField] private string[] roomEnterDialog = { "CHANGEME" };

    // dialog that plays when we interact with this npc
    [TextArea(1, 5)]
    [SerializeField] private string[] interactDialog = { "CHANGEME" };

    // dialog that plays when we hurt this npc
    [TextArea(1, 5)]
    [SerializeField] private string[] attackDialog = { "CHANGEME" };

    private float timer = 0f;

    private bool startTimer; // if we've started our timer

    private DialogManager dialogManager;

    private Transform playerPos;

    private bool currentlyTalking = false;

    private float distance = 69420f;

    #endregion

    #region Default Methods

    // Start is called before the first frame update
    private void Start()
    {
        startTimer = false;

        dialogManager = FindObjectOfType<DialogManager>(); // grab our dialog manager
        
        playerPos = GameObject.Find("player").transform; // grab the players transform

        thisNpc = new NPC(); // we are class npc

        thisNpc.Name = npcName; // we have a name

        thisNpc.InteractRange = interactRange; // a range we can be touched

        // and a bunch of dialog to say
        thisNpc.InteractDialog = interactDialog;
        thisNpc.RoomEnterDialog = roomEnterDialog;
        thisNpc.AttackDialog = attackDialog;

    }

    // Update is called once per frame
    private void Update()
    {
        // distance to player
        distance = JMath.Distance(playerPos.position, transform.position);

        // if the distance from the player is within the interact range and we arent currently talking
        if (distance <= interactRange && !currentlyTalking)
        {
            if (Input.GetButtonDown("ItemAction")) // if we are pressing f
            {
                // hangTime also applies to our interaction messages.
                timer = 0f; // reset our timer
                startTimer = true; // we can now start counting down
                
                dialogManager.DialogStart((Dialog)thisNpc.InteractDialog); // start our interact dialog
                
                currentlyTalking = true; // we are now currently talking
            }
        }
        /*
         * we are out of range and we are currently talking.
         * we check for these two because:
         * 1. "currentlytalking" - if we arent talking we dont need to run any of this code in the first place
         * 2. "distance > interactrange" - and distance on its own isnt enough to make us stop talking, considering we could be out of range and have entered the room
         */
        else if (distance > interactRange && currentlyTalking)
        {
            dialogManager.StopDialog(); // stop all dialog

            startTimer = false; // stop our timer; this check REQUIRES the timer to be active

            currentlyTalking = false; // we are not currently talking
        }
    }

    private void FixedUpdate()
    {
        if (startTimer) // start our timer
        {
            // we need to increase our timer every frame by the time that has passed since the last one
            timer += Time.fixedDeltaTime;

            if (timer >= hangTime) // once we pass that hangtime threshold, we'll clear the dialog
            {
                dialogManager.StopDialog(); // we can stop the dialog now

                startTimer = false; // our timer is no longer active
            }
        }
    }

    #endregion

    #region Custom Methods

    /*
     * purpose: code to run on room enter
     * inputs: none
     * outputs: says our room enter dialog
     */
    public void EnterRoom()
    {
        timer = 0f; // reset our timer

        startTimer = true; // start our timer

        // we need to cast since its of type object in the npc class
        dialogManager.DialogStart((Dialog)thisNpc.RoomEnterDialog);
    }

    /*
     * purpose: code to run on room exit
     * inputs: none
     * outputs: stops dialog
     */
    public void ExitRoom()
    {
        startTimer = false; // start our timer

        dialogManager.StopDialog(); // stop dialog
    }

    #endregion
}
