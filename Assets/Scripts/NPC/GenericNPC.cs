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
    public float interactRange = 10f;

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


    #endregion

    #region Default Methods

    // Start is called before the first frame update
    private void Start()
    {
        startTimer = false;

        dialogManager = FindObjectOfType<DialogManager>(); // grab our dialog manager

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
        
    }

    private void FixedUpdate()
    {
        if (startTimer) // start our timer
        {
            // we need to increase our timer every frame by the time that has passed since the last one
            timer += Time.fixedDeltaTime;
        }

        if (timer >= hangTime) // once we pass that hangtime threshold, we'll clear the dialog
        {
            dialogManager.StopDialog();
        }
    }

    #endregion

    #region Custom Methods

    /*
     * purpose: TODO
     * inputs: TODO
     * outputs: TODO
     */
    public void EnterRoom()
    {
        timer = 0f; // reset our timer

        startTimer = true; // start our timer

        // we need to cast since its of type object in the npc class
        dialogManager.DialogStart((Dialog)thisNpc.RoomEnterDialog);
    }

    /*
     * purpose: TODO
     * inputs: TODO
     * outputs: TODO
     */
    public void ExitRoom()
    {
        dialogManager.StopDialog();
    }

    #endregion
}
