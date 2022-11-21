/* 
 * Programmers: Christopher Kowalewski 
 * Purpose: TODO
 * Inputs: TODO
 * Outputs: TODO
 */

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogManager : MonoBehaviour
{
    #region Variables

    private Queue<string> lines;
    [SerializeField] private TextMeshProUGUI textMesh;
    [SerializeField] private Animator animator;

    #endregion

    #region Default Methods

    // On start
    private void Start()
    {
        lines = new Queue<string>();
    }

    #endregion

    #region Custom Methods

    /*
     * purpose: starts dialog
     * inputs: dialog to say
     * outputs: loads the main menu and does practically what ResumeGame() does
     */
    public void DialogStart(Dialog dialog)
    {
        animator.SetBool("Opened", true); //open box

        //Clear out any old unnecessary lines of dialogue
        lines.Clear();

        // for every line, queue the line up for writing
        foreach (string line in dialog.lines)
        {
            lines.Enqueue(line);
        }

        // call writedialog
        WriteDialog();
    }

    /*
     * purpose: writes text to the dialog display
     * inputs: our current dialog
     * outputs: sends typeline the text we want to say
     */
    public void WriteDialog()
    {
        // test for if we arent writing anything
        if (lines.Count == 0)
        {
            StopDialog();
            return;
        }

        string line = lines.Dequeue(); //store the next line in a variable
        StopAllCoroutines(); //stop previous animation
        StartCoroutine(TypeLine(line)); //output line (animated)
    }

    /*
     * purpose: types an entire line on screen
     * inputs: text to say
     * outputs: on-screen dialog text
     */
    IEnumerator TypeLine(string words)
    {
        // clear the text we are typing
        textMesh.text = "";

        // for every character we need to add it to our text on screen
        foreach (char let in words.ToCharArray())
        {
            textMesh.text += let;
            yield return null;
        }
    }

    /*
     * purpose: stops all dialog
     * inputs: none
     * outputs: makes the dialog on screen go away
     */
    public void StopDialog()
    {
        animator.SetBool("Opened", false); //close box
    }

    #endregion
}
