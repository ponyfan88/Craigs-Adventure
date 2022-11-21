/* 
 * Programmers: Jack Kennedy
 * Purpose: npc class
 * Inputs: set values
 * Outputs: get values, generic npc values and things
 */

public class NPC
{
    // name for our npc (eg: "jack")
    private string name;

    // interact range for our npc
    private float interactRange;

    // dialog that plays when we enter the room
    private Dialog roomEnterDialog;

    // time that our room enter dialog stays for
    private float hangTime;

    // dialog that plays when we interact with this npc
    private Dialog interactDialog;

    // dialog that plays when we hurt this npc
    private Dialog attackDialog;

    // public name we can get and set
    public string Name
    {
        get
        {
            return name;
        }
        set
        {
            name = value;

            // set proper dialog names (these are incorrectly set by default)
            roomEnterDialog.name = name;
            interactDialog.name = name;
            attackDialog.name = name;
        }
    }

    // public interactranger we can get and set
    public float InteractRange
    {
        get
        {
            return interactRange;
        }
        set
        {
            interactRange = value;
        }
    }

    // public interactDialog we can get and set
    public float HangTime
    {
        get
        {
            return hangTime;
        }
        set
        {
            hangTime = value;
        }
    }

    // public interactDialog we can get and set
    // use type object so we can get and set different types
    public object RoomEnterDialog
    {
        get
        {
            // getting has type Dialog
            return roomEnterDialog; // we need to return the proper dialog class
        }
        set
        {
            // setting has type string[], cast as such
            roomEnterDialog.lines = (string[])value; // if we SET the dialog, we'll just set the lines
        }
    }

    // public interactDialog we can get and set
    public object InteractDialog
    {
        // see RoomEnterDialog for why we use object and why we cast as string
        get
        {
            return interactDialog; // we need to return the proper dialog class
        }
        set
        {
            interactDialog.lines = (string[])value; // if we SET the dialog, we'll just set the lines
        }
    }

    // public interactDialog we can get and set
    public object AttackDialog
    {
        get
        {
            return attackDialog; // we need to return the proper dialog class
        }
        set
        {
            attackDialog.lines = (string[])value; // if we SET the dialog, we'll just set the lines
        }
    }
}
