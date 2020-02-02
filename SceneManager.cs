using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManager : MonoBehaviour
{

    [Header("MainMenu")]
    [SerializeField] GameObject mainMenu;
    [SerializeField] GameObject TitleText;
    [SerializeField] GameObject Tagline;

    [Header("MultiplayerMenu")]
    [SerializeField] GameObject multiplayerObject;

    [Header("SettingsMenu")]
    [SerializeField] GameObject SettingsObject;
    
    public void QuitGame()
    {
        Application.Quit();
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }

    public void multiplayerMenu()
    {
        if (mainMenu.activeInHierarchy)
        {
            mainMenu.SetActive(false);
        }
        else
        {
            mainMenu.SetActive(true);
        }

        if (Tagline.activeInHierarchy)
        {
            Tagline.SetActive(false);
        }
        else
        {
            Tagline.SetActive(true);
        }
        if (multiplayerObject.activeInHierarchy)
        {
            multiplayerObject.SetActive(false);
        }
        else
        {
            multiplayerObject.SetActive(true);
        }
    }

    public void settingsMenu()
    {
        if (mainMenu.activeInHierarchy){ mainMenu.SetActive(false); }
        else{ mainMenu.SetActive(true); }

        if (Tagline.activeInHierarchy) { Tagline.SetActive(false); }
        else{ Tagline.SetActive(true); }

        if(SettingsObject.activeInHierarchy) { SettingsObject.SetActive(false); }
        else { SettingsObject.SetActive(true);  }
    }
}
