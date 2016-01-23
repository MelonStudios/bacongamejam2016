using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class DebugController : MonoBehaviour
{
    private static List<string> logMessages;
    private static List<string> tempMessages;
    private bool newMessages;

    public static DebugController Instance { get; private set; }

    void Awake()
    {
        Instance = this;
        logMessages = new List<string>();
    }

    public void Log(string text)
    {
        newMessages = true;
        logMessages.Add(text);
    }

    public void LogLine(string text)
    {
        newMessages = true;
        logMessages.Add(text + '\n');
    }

    void OnGUI()
    {
        if (Application.isEditor)
        {
            if (newMessages)
            {
                tempMessages = logMessages.ToList();
                logMessages.Clear();
                newMessages = false;
            }

            GUI.Label(new Rect(10, 10, Screen.width - 10, Screen.height - 10), string.Join("", tempMessages.ToArray()));
            GUI.Label(new Rect(12, 12, Screen.width - 12, Screen.height - 12), string.Join("", tempMessages.ToArray()), new GUIStyle { normal = new GUIStyleState { textColor = Color.black } });
        }
    }
}