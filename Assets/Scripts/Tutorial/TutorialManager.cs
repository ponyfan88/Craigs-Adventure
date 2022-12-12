using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TutorialManager
{
    #region Variables

    private static bool tutorialActive = true;

    public static bool TutorialActive
    {
        get
        {
            return tutorialActive;
        }
        set
        {
            tutorialActive = value;
        }
    }

    #endregion
}
