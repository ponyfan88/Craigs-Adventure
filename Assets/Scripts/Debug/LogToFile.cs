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

    public static List<string> logFile = new List<string>();

    #endregion

    #region Custom Methods

    public static void Log(string logString, bool timestamp = false)
    {
        logFile.Add(logString);
    }

    public static void DumpLogs(bool clear = true)
    {

        if (clear)
        {    
            File.WriteAllText("Assets/Debug/log.txt", string.Empty);
        }

        StreamWriter writer = new StreamWriter("Assets/Debug/log.txt", true);

        foreach (string logString in logFile)
        {
            writer.WriteLine(logString);
        }

        writer.Close();
    }

    #endregion
}
