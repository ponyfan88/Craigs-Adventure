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
        lines = new Queue<string> ();
    }

    #endregion

    #region Custom Methods

    //Starts the dialog cutscene
    public void DialogStart (Dialog dialog)
    {
        animator.SetBool("Opened", true); //open box

        //Clear out any old unnecessary lines of dialogue
        lines.Clear();

        foreach (string line in dialog.lines)
        {
            lines.Enqueue(line);
        }

        WriteDialog();
    }

    //Writes the dialogueueueue
    public void WriteDialog()
    {
        if (lines.Count == 0)
        {
            StopDialog();
            return;
        }

        string line = lines.Dequeue(); //store the next line in a variable
        StopAllCoroutines(); //stop previous animation
        StartCoroutine(TypeLine(line)); //output line (animated)
    }

    IEnumerator TypeLine(string words)
    {
        textMesh.text = "";
        foreach (char let in words.ToCharArray())
        {
            textMesh.text += let;
            yield return null;
        }
    }

    private void StopDialog()
    {
        animator.SetBool("Opened", false); //close box
        //UNPAUSE THE GODDAMN GAME
        Debug.Log("End");
    }

    #endregion
}
