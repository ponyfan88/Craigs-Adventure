using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DetectController : MonoBehaviour
{
    #region Variables

    #endregion

    #region Default Methods

    private void Update()
    {
        if (Keyboard.current.anyKey.wasPressedThisFrame)
        {
            gameObject.transform.Find("KeyboardPrompts").gameObject.SetActive(true);
            gameObject.transform.Find("ControllerPrompts").gameObject.SetActive(false);
        }
        else if (Gamepad.current.buttonNorth.wasPressedThisFrame || Gamepad.current.buttonEast.wasPressedThisFrame || Gamepad.current.buttonSouth.wasPressedThisFrame || Gamepad.current.buttonWest.wasPressedThisFrame)
        {
            gameObject.transform.Find("ControllerPrompts").gameObject.SetActive(true);
            gameObject.transform.Find("KeyboardPrompts").gameObject.SetActive(false);
        }
    }

    #endregion
}
