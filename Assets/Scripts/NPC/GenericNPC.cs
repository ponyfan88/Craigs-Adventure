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

    NPC thisNpc;

    // name for our npc (eg: "jack")
    public string npcName;

    // interact range for our npc
    public float interactRange;

    // dialog that plays when we enter the room
    [TextArea(1, 5)]
    public string[] roomEnterDialog;

    // time that our room enter dialog stays for
    public float hangTime;

    // dialog that plays when we interact with this npc
    [TextArea(1, 5)]
    public string[] interactDialog;

    // dialog that plays when we hurt this npc
    [TextArea(1, 5)]
    public string[] attackDialog;


    #endregion

    #region Default Methods

    // Start is called before the first frame update
    void Start()
    {
        thisNpc = new NPC();

        thisNpc.Name = name;

        thisNpc.InteractRange = interactRange;

        thisNpc.InteractDialog = interactDialog;
        thisNpc.RoomEnterDialog = roomEnterDialog;
        thisNpc.AttackDialog = attackDialog;

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #endregion
}
