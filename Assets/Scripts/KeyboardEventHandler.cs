using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class to be attatched to the EventSystem object. Handles Special key events such as exiting the application.
/// </summary>
/// <lastUpdated> 2023-08-21 </lastUpdated>

public class KeyboardEventHandler : MonoBehaviour
{
    void FixedUpdate()
    {
        if (Input.GetAxis("Exit") != 0) { QuitGame("Escape keypress"); }
    }

    /// <summary>
    /// Handles the closing of the application/game
    /// </summary>
    /// <param name="reason">Cause of the exit, to be printed to logs</param>
    /// <param name="expected">Whether the quit was expected (or the result of an error), will affect the kind of log. Default true</param>
    void QuitGame(string reason, bool expected = true)
    {
        try //this REALLY shouldnt be able to fail, but make sure it doesnt
        {
            string logString = string.Format(
                    "Exiting application at time {0} seconds. Reason: {1}.",
                    Time.realtimeSinceStartup,
                    reason
                );
            if (expected) { Debug.Log(logString); }
            else          { Debug.LogWarning(logString); }
        }
        finally
        {
            Application.Quit();
        }
    }
}
