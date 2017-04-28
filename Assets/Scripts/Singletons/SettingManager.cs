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
    [SerializeField] private Toggle invert;

    private bool vertical; //Camera split type
    private bool invertCam; //invert cam controls or not 
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

    public bool InvertCam
    {
        get { return invertCam; }
    }
    #endregion

    #region inBuiltMethods
    protected SettingManager() { }

    private void Start()
    {
      //  splitScreenChange(); //Call this now so the toggle button works from start
        invertCam = true;
    }

    void Awake()
    {
        //dont touch me when we load a new scene
        DontDestroyOnLoad(this);
    }

    public void OnEnable()
    {
        //initializing for null values
        gameSettings = new GameSettings();

		//check if each option menu item has something linked up, if so hook up event listener for chnages to options
		if (fullscreenToggle != null) 
		{
			//link the methods to the toggle button
			fullscreenToggle.onValueChanged.AddListener(delegate
				{
					onFullScreenToggle();
				});
		}
        
		if (resolutionDropdown != null)
		{
			//link the methods to the Resolution dropdown 
			resolutionDropdown.onValueChanged.AddListener(delegate
				{
					onResolutionChange();
				});


			//fill in options data for resolution
			resolutions = Screen.resolutions;
			foreach (Resolution resolution in resolutions)
			{
				//add options
				resolutionDropdown.options.Add(new Dropdown.OptionData(resolution.ToString()));
			}
		}

		if (textureQualityDropdown != null)
		{
			//link the methods to the texture Quality Dropdown
			textureQualityDropdown.onValueChanged.AddListener(delegate
				{
					onTextureQualityChange();
				});
		}

		if (antialisingDropdown != null)
		{
			//link the methods to the antialiasning dropdown 
			antialisingDropdown.onValueChanged.AddListener(delegate
				{
					onAntialiasingChange();
				});
		}

		if (vSyncDropdown != null)
		{
			//link the methods to the vsync dropdown
			vSyncDropdown.onValueChanged.AddListener(delegate
				{
					onVsyncChange();
				});
		}

		if (musicVolSlider != null)
		{
			//link the methods to the music slider
			musicVolSlider.onValueChanged.AddListener(delegate
				{
					onMusicVolChange();
				});
		}

		if (soundVolSlider != null)
		{
			//link the methods to the sound slider
			soundVolSlider.onValueChanged.AddListener(delegate
				{
					onSoundVolChange();
				});
		}

		if (splitScreen != null)
		{
			splitScreen.onValueChanged.AddListener(delegate
				{
					splitScreenChange();
				});
		}

		if (invert != null)
		{
			invert.onValueChanged.AddListener(delegate
				{
					invertCameraChange();
				});
		}
    }
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
        Debug.Log("SPLIT SPLAT WHAT IS THAT??");
        //originally at verticle, if unchecked then horizontal
        if (splitScreen.isOn)
        {
            vertical = true;


        }
        else
        {
            vertical = false;

        }
    }

    private void invertCameraChange()
    {
        //invert if checked
        invertCam = invert.isOn;
    }
    #endregion
}
