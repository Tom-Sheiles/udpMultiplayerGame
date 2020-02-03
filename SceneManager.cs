using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    [SerializeField] Toggle isFullScreen;
    Resolution[] resolutions;
    [SerializeField] Dropdown resolutionDropDown;
    [SerializeField] Slider fovslider;
    [SerializeField] Text fovText;
    [SerializeField] InputField mouseSenseFactor;
    [SerializeField] Camera playerCamera;
    
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

    private void Start()
    {
        resolutions = Screen.resolutions;
        List<string> resList = new List<string>();

        foreach(Resolution res in resolutions)
        {
            resList.Add(res.ToString().Substring(0, res.ToString().Length - 6));
        }

        resolutionDropDown.AddOptions(resList);

        fovText.text = fovslider.value.ToString();
        playerCamera.fieldOfView = fovslider.value;
        mouseSenseFactor.text = "1.5";
    }

    private void Update()
    {
        if(SettingsObject.activeInHierarchy)
            userSettings();
    }

    private void userSettings()
    {
        if (isFullScreen.isOn)
        {
            Screen.fullScreen = true;
        }
        else
        {
            Screen.fullScreen = false;
        }
    }

    public void changedResolution()
    {
        Resolution chosenRes = resolutions[resolutionDropDown.value - 1];

        Screen.SetResolution(chosenRes.width, chosenRes.height, isFullScreen);
    }

    public void changeFOV()
    {
        playerCamera.fieldOfView = fovslider.value;
        fovText.text = fovslider.value.ToString();
    }

    public void changeSense()
    {
        playerCamera.gameObject.GetComponent<FirstPersonCameraController>().mouseSenseValue = float.Parse(mouseSenseFactor.text);
    }
}
