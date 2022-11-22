/*
 * programmers: TODO
 * purpose: TODO
 * inputs: TODO
 * outputs: TODO
 */
    

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDialog : MonoBehaviour
{
    [SerializeField] private Dialog dialogue;
    [SerializeField] private DialogManager manager;

    public void Trigger()
    {
        manager.DialogStart(dialogue);
    }
}
