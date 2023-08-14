using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardEventHandler : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetAxis("Exit") != 0) { QuitGame("Escape keypress"); }
    }

    void QuitGame (string reason, string additionalMessage = "")
    {
        Debug.Log( string.Format(
            "Exiting application at time {0} seconds. Reason: {1}. {2}",
            Time.realtimeSinceStartup,
            reason,
            additionalMessage
        ));
        Application.Quit();
    }
}
