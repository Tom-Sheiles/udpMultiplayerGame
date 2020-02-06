using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugConsole : MonoBehaviour
{
    /*[SerializeField] GameObject mainWindow;
    [SerializeField] Text text;

    //private static Queue<string> commands;
    private static int line = 0;
    private static int maxLines = 11;
    private static string[] commands;

    void Start()
    {
        mainWindow.SetActive(false);
        commands = new string[maxLines];
    } 

    void Update()
    {
        if (Input.GetButtonDown("Tilde"))
        {
            if (mainWindow.activeInHierarchy)
            {
                mainWindow.SetActive(false);
            }
            else
            {
                mainWindow.SetActive(true);
            }
        }
          
    }*/

    public static void ConsoleLog(string command)
    {
        /*if (line + 1 < maxLines)
        {
            commands[line] = command;
            line++;
        }else if(line >= maxLines)
        {
            line = 0;
        }*/
    }
}
