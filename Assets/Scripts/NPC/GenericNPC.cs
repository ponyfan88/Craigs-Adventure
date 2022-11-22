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

    public NPC thisNpc;

    // name for our npc (eg: "jack")
    public string npcName = "Grug";

    // interact range for our npc
    private const float interactRange = 2.5f;

    // time that our room enter dialog stays for
    public float hangTime;

    // dialog that plays when we enter the room
    [TextArea(1, 5)]
    public string[] roomEnterDialog = { "CHANGEME" };

    // dialog that plays when we interact with this npc
    [TextArea(1, 5)]
    public string[] interactDialog = { "CHANGEME" };

    // dialog that plays when we hurt this npc
    [TextArea(1, 5)]
    public string[] attackDialog = { "CHANGEME" };

    private float timer = 0f;

    private bool startTimer;

    DialogManager dialogManager;

    Transform playerPos;

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

        thisNpc.Name = name; // we have a name

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
        distance = Mathf.Sqrt(Mathf.Pow(Mathf.Abs(playerPos.position.x - transform.position.x), 2) + Mathf.Pow(Mathf.Abs(playerPos.position.y - transform.position.y), 2));

        // if the distance from the player is within the interact range and we arent currently talking
        if (distance <= interactRange && !currentlyTalking)
        {
            if (Input.GetButtonDown("ItemAction")) // if we are pressing f
            {
                Debug.Log("ACTION");
                dialogManager.DialogStart((Dialog)thisNpc.InteractDialog); // start our interact dialog
                
                currentlyTalking = true; // we are now currently talking
            }
        }
        else if (currentlyTalking && !startTimer) // we are out of range and we are currently talking
        {
            dialogManager.StopDialog(); // stop all dialog
            
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
                dialogManager.StopDialog();

                startTimer = false;
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
        Debug.Log("ENTERED ROOM");
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

        dialogManager.StopDialog();
        Debug.Log("EXITED ROOM");
    }

    #endregion
}
