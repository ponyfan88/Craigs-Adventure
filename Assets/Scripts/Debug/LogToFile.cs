/* 
 * Programmers: Jack Kennedy
 * Purpose: Log text to file
 * Inputs: Log text
 * Outputs: Log text into the log file
 */

using System.Collections.Generic;
using UnityEngine;
using System.IO;

public static class LogToFile
{
    #region Variables

    // list representing our log file
    public static List<string> logFile = new List<string>();

    #endregion

    #region Custom Methods

    /*
     * purpose: adds a given log string to our log list
     * inputs: a string to log
     * outputs: string into logFile list
     */
    public static void Log(string logString)
    {
        logFile.Add(logString);
    }

    /*
     * purpose: dumps logs to log.txt
     * inputs: if we should clear the current log
     * outputs: log file log.txt
     */
    public static void DumpLogs(bool clear = true)
    {
        if (!Directory.Exists(Application.dataPath + "/Debug/"))
        {
            LogToFile.Log("debug dir did not exist");
            Directory.CreateDirectory(Application.dataPath + "/Debug/");
        }

        if (clear)
        {    
            File.WriteAllText(Application.dataPath + "/Debug/log.txt", string.Empty);
        }

        StreamWriter writer = new StreamWriter(Application.dataPath + "/Debug/log.txt", true);

        foreach (string logString in logFile)
        {
            writer.WriteLine(logString);
        }

        writer.Close();
    }

    #endregion
}
