using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingManager : Singleton<SettingManager>
{
    #region Attributes
    [SerializeField]
    Toggle fullscreenToggle;
    [SerializeField]
    Dropdown resolutionDropdown;
    [SerializeField]
    Dropdown antialisingDropdown;
    [SerializeField]
    Dropdown textureQualityDropdown;
    [SerializeField]
    Dropdown vSyncDropdown;
    [SerializeField] Slider musicVolSlider;
    [SerializeField]
    Slider soundVolSlider;
    [SerializeField]
    Resolution[] resolutions;
    [SerializeField]
    GameSettings gameSettings;
    [SerializeField]
    Toggle splitScreen;

    private bool vertical; //Camera split type
    #endregion

    #region Properties
    public bool Vertical
    {
        get
        {
            return vertical;
        }
        set
        {
            vertical = value;
        }
    }
    #endregion

    #region inBuiltMethods
    protected SettingManager() { }

    private void Start()
    {
      //  splitScreenChange(); //Call this now so the toggle button works from start
    }

    void Awake()
    {
        //dont touch me when we load a new scene
        DontDestroyOnLoad(this);
    }

    /*public void OnEnable()
    {
        //initializing for null values
        gameSettings = new GameSettings();

        //link the methods to the toggle button
        fullscreenToggle.onValueChanged.AddListener(delegate
        {
            onFullScreenToggle();
        });
        //link the methods to the Resolution dropdown 
        resolutionDropdown.onValueChanged.AddListener(delegate
        {
            onResolutionChange();
        });
        //link the methods to the texture Quality Dropdown
        textureQualityDropdown.onValueChanged.AddListener(delegate
        {
            onTextureQualityChange();
        });
        //link the methods to the antialiasning dropdown 
        antialisingDropdown.onValueChanged.AddListener(delegate
        {
            onAntialiasingChange();
        });
        //link the methods to the vsync dropdown
        vSyncDropdown.onValueChanged.AddListener(delegate
        {
            onVsyncChange();
        });
        //link the methods to the music slider
        musicVolSlider.onValueChanged.AddListener(delegate
        {
            onMusicVolChange();
        });
        //link the methods to the sound slider
        soundVolSlider.onValueChanged.AddListener(delegate
        {
            onSoundVolChange();
        });

        resolutions = Screen.resolutions;
        //fill in options data
        foreach (Resolution resolution in resolutions)
        {
            //add options
            resolutionDropdown.options.Add(new Dropdown.OptionData(resolution.ToString()));
        }

        splitScreen.onValueChanged.AddListener(delegate
        {
            splitScreenChange();
        });
    }*/
    #endregion

    #region helperMethods
    public void onFullScreenToggle()
    {
        //assign to the value of fullscreen
        //if on true, if not false
        //sending info to gameSettings
        gameSettings.fullscreen = Screen.fullScreen = fullscreenToggle.isOn;
    }
    
    public void onResolutionChange()
    {
        //Set the resolution to the index of resolution dropdowns value
        //screen size stays the same
        Screen.SetResolution(resolutions[resolutionDropdown.value].width, resolutions[resolutionDropdown.value].height, Screen.fullScreen);
    }

    public void onTextureQualityChange()
    {
        //pass in the dropdown value
        QualitySettings.masterTextureLimit = gameSettings.textureQuality = textureQualityDropdown.value;
    }
    public void onAntialiasingChange()
    {
        //get the value and make it a power of 2
        //seding the info to gameSettings
        QualitySettings.antiAliasing = gameSettings.antialiasing = (int) Mathf.Pow(2, antialisingDropdown.value);
    }

    public void onVsyncChange()
    {

        QualitySettings.vSyncCount = gameSettings.vSync = vSyncDropdown.value;
    }

    public void onMusicVolChange()
    {

    }

    public void onSoundVolChange()
    {

    }

    public void splitScreenChange()
    {
        //originally at verticle, if unchecked then horizontal
        if(splitScreen.isOn)
        {
            vertical = true;
        }
        else
        {
            vertical = false;
        }
    }
    #endregion
}
