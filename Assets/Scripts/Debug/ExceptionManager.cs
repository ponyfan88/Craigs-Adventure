/* 
 * Programmers: Jack Kennedy
 * Purpose: Handles Exceptions
 * Inputs: all exceptions
 * Outputs: calls a method in Email.cs to send us an email
 */

using UnityEngine;
using TMPro;

public class ExceptionManager : MonoBehaviour
{
    #region Variables

    Pause Pause; // import pausing
    Email Email; // import emailing

    public bool handleExceptions = true;

    public TextMeshProUGUI crashInfo;
    public TextMeshProUGUI emailBody;

    public Log mostRecentLog = new Log();

    #endregion

    #region Classes

    // doing this cuz' im lazy
    public class Log
    {
        public string logString
        {
            get; set;
        }
        public string stackTrace
        {
            get; set;
        }
    }

    #endregion

    #region Default Methods

    void Awake()
    {
        // every time we get a log message, run it through HandleException
        Application.logMessageReceived += HandleException;

        Pause = GameObject.Find("UI").GetComponent<Pause>(); // find our pause script
        Email = GameObject.Find("Exception Manager").GetComponent<Email>(); // find our email script

        mostRecentLog.logString = "error";
        mostRecentLog.stackTrace = "error";
    }

    #endregion

    #region Custom Methods

    /*
     * purpose: dumps the log to log.txt
     * inputs: none
     * outputs: tells LogToFile to dump to log.txt
     */
    public void UserDump()
    {
        LogToFile.Log("dump at " + Time.realtimeSinceStartup.ToString());
        LogToFile.DumpLogs();
    }

    /*
     * purpose: handles exceptions and dumps logs when needed
     * inputs: info about every single log
     * outputs: tells pause to show the email menu, tells LogToFile to dump the log
     */
    void HandleException(string logString, string stackTrace, LogType type)
    {   
        // if we got an error and we are currently handling exceptions
        if (type == LogType.Exception && handleExceptions)
        {
            LogToFile.Log("crash at " + Time.realtimeSinceStartup.ToString());
            LogToFile.DumpLogs(); // dump logs

            mostRecentLog.logString = logString;
            mostRecentLog.stackTrace = stackTrace;

            Pause.PauseCrashed(); // pause the game with our crash specific pause

            //Pause.emailUI.SetActive(true); //TODO:REMOVEME

            // the string we use for crashinfo
            string bug = "an exception has occured!\nlocation:\n" + mostRecentLog.stackTrace + "\nissue:\n" + mostRecentLog.logString;

            // should be shown like like:
            /*
             * an exception has occured!
             * location:
             * assets/scripts/badFile.cs
             * issue:
             * stack overflow something something
             */

            crashInfo.text = bug; // make sure we display to the user what happens
        }
    }

    /*
     * purpose: sends bug reports
     * inputs: none
     * outputs: tells Email to send an email, tells pause to unpause the game.
     */
    public void SendBugReport()
    {
        // the string we use for the actual email body
        string emailBug = "#### an exception has occured! ####\n\n\n#### location ####\n" + mostRecentLog.stackTrace + "\n\n#### issue ####\n" + mostRecentLog.logString;
        
        // should be sent like:
        /*
         * #### an exception has occurred! ####
         * 
         * 
         * #### location ####
         * assets/scripts/badFile.cs
         * 
         * 
         * #### issue ####
         * stack overflow something something
         * 
         * 
         * #### user input ####
         * i was just "jaunting with the boys" officer
         */

        Email.SendEmail(emailBug + "\n\n\n#### user input ####\n" + emailBody.text);

        // close our menu, making sure they cant send 2 emails
        Pause.ResumeCrashed();
        
    }

    #endregion
}