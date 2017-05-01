using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SoundManager : Singleton<SoundManager>
{
    //FIELDS
	private AudioSource sfxSource; //what'll play effects
	private Dictionary<string, AudioClip> sfxLibrary = new Dictionary<string, AudioClip>(); //sound effect library

    private AudioSource musicSource; //source for looping background music
    private Dictionary<string, AudioClip> musicLibrary = new Dictionary<string, AudioClip>(); //music library

    protected SoundManager() {}

	void Awake()
	{
        //dont touch me when we load a new scene
		DontDestroyOnLoad(this);
	}

	void Start()
	{
		//add audio sources to sound manager obj
		sfxSource = gameObject.AddComponent<AudioSource>();
        musicSource = gameObject.AddComponent<AudioSource>();

        //add sounds to the lib
        musicLibrary.Add("ambientWaves", Resources.Load("sfx/ambientWaves") as AudioClip);

        sfxLibrary.Add("BoatMortgage", Resources.Load("sfx/BoatMortage") as AudioClip);
        sfxLibrary.Add("FineBooty", Resources.Load("sfx/FineBootyAtTheEndOfThisCourse") as AudioClip);
        sfxLibrary.Add("BootyPlunder", Resources.Load("sfx/meBootiesBeenPlundered") as AudioClip);
        sfxLibrary.Add("ShiverMeTimbers", Resources.Load("sfx/meTimbersHaveBeenShivered") as AudioClip);
        sfxLibrary.Add("BottleOfRoum", Resources.Load("sfx/standAroundAllDay_BottleOfRum") as AudioClip);
        sfxLibrary.Add("WhatYaStoppinForLad", Resources.Load("sfx/whatYaStoppingForLad") as AudioClip);
        sfxLibrary.Add("WhatYaStoppinForLad_BoatMortgage", Resources.Load("sfx/whatYaStoppingForLad_BoatMortage") as AudioClip);

        sfxLibrary.Add("hurt1", Resources.Load("sfx/hurt1") as AudioClip);
        sfxLibrary.Add("hurt2", Resources.Load("sfx/hurt2") as AudioClip);
        sfxLibrary.Add("hurt3", Resources.Load("sfx/hurt3") as AudioClip); //best
        sfxLibrary.Add("hurt4", Resources.Load("sfx/hurt4") as AudioClip);

        sfxLibrary.Add("drowning", Resources.Load("sfx/drowning1") as AudioClip); //kevs im drowning quote
        sfxLibrary.Add("splash", Resources.Load("sfx/splash") as AudioClip);

        sfxLibrary.Add("jump1", Resources.Load("sfx/jump1") as AudioClip);
        sfxLibrary.Add("jump2", Resources.Load("sfx/jump2") as AudioClip);

        sfxLibrary.Add("step", Resources.Load("sfx/step1") as AudioClip); //one step in a walk
        sfxLibrary.Add("stride", Resources.Load("sfx/stride") as AudioClip); //a full slow stride

        sfxLibrary.Add("squack1", Resources.Load("sfx/squack1") as AudioClip);
        sfxLibrary.Add("squack2", Resources.Load("sfx/squack2") as AudioClip);
        sfxLibrary.Add("squack3", Resources.Load("sfx/squack3") as AudioClip);
        sfxLibrary.Add("squack_extended", Resources.Load("sfx/squack_extended") as AudioClip);

        sfxLibrary.Add("chirpping", Resources.Load("sfx/chirpping") as AudioClip); //for the concussion birbs

        sfxLibrary.Add("wings1", Resources.Load("sfx/wings1") as AudioClip);
        sfxLibrary.Add("wings2", Resources.Load("sfx/wings2") as AudioClip);
        sfxLibrary.Add("wings3", Resources.Load("sfx/wings3") as AudioClip);
        sfxLibrary.Add("wings4", Resources.Load("sfx/wings4") as AudioClip);

        sfxLibrary.Add("chestOpening", Resources.Load("sfx/chestOpening") as AudioClip);

        //loop background music
        musicSource.loop = true; //turn on looping
        musicSource.clip = musicLibrary["ambientWaves"]; //set default song
        ChangeMusicVolume(0);
        musicSource.Play(); //play music
	}

    /// <summary>
    /// Plays random walk sound once
    /// </summary>
    public void PlayWalkSound()
    {
        
    }

    /// <summary>
    /// Plays random wing sound once
    /// </summary>
    public void PlayWingSound()
    {//chose wing sound
        switch (Random.Range(0, 3))
        {
            case 1:
                PlaySfxOnce("wings1", 100);
                break;
            case 2:
                Instance.PlaySfxOnce("wings2", 100);
                break;
            case 3:
                Instance.PlaySfxOnce("wings3", 100);
                break;
            case 4:
                Instance.PlaySfxOnce("wings4", 100);
                break;
        }   

    }

    /// <summary>
    /// Plays sfx once so it doesnt overlap itself
    /// </summary>
    /// <param name="name">Name of sound in dictionary</param>
    /// <param name="volume">Volume of sound</param>
    public void PlaySfxOnce(string name, int volume)
    {
        //PlaySfxAt(name, this.gameObject.transform.position, volume);
        //check if its currently playing
        if (!GameObject.Find("One shot audio"))
        {
            //play sound again
            PlaySfxAt(name, this.gameObject.transform.position, volume);
        }
    }

    /// <summary>
    /// chnage the background music
    /// </summary>
    /// <param name="name">key of the song to play</param>
    public void ChangeSong(string name)
    {
        musicSource.Stop(); //stop current song

        musicSource.clip = musicLibrary[name]; //set song
        musicSource.Play(); //play song
    }

    /// <summary>
    /// Plays sound at volume
    /// </summary>
    /// <param name="name">key for sound to be played</param>
    /// /// <param name="volume">volume of this sound</param> 
    public void PlaySfx(string name, int volume)
    { 
        //convert volume into percentage
        float vol = (float)(volume / 100.00);

        //scale volume by sourceVolume
        vol *= sfxSource.volume;

        //play sound from lib once
		sfxSource.PlayOneShot(sfxLibrary[name], vol);
	}

    /// <summary>
    /// Plays sound from a specific position in world space by creating its own source
    /// </summary>
    /// <param name="name">key for sound to be played</param>
    /// <param name="position">world cordinates to be played from</param>
    /// <param name="volume">volume of this sound</param> 
    public void PlaySfxAt(string name, Vector3 position, int volume)
    {
        //convert volume into percentage
        float vol = (float)(volume / 100.00);

        //scale volume by sourceVolume
        vol *= sfxSource.volume;

        //doesnt use source bc it creates its own for this
        AudioSource.PlayClipAtPoint(sfxLibrary[name], position, vol);
    }

    /// <summary>
    /// changes sfx volume
    /// </summary>
    /// <param name="volume">number 0-100 of volume</param>
    public void ChangeSfxVolume(int volume)
    {
        //chnage volume into a float for the actual volume- and set to sfxVolume
        sfxSource.volume = (float)(volume / 100.00);
    }

    /// <summary>
    /// changes music volume
    /// </summary>
    /// <param name="volume">number 0-100 of volume</param>
    public void ChangeMusicVolume(int volume)
    {
        //chnage volume into a float for the actual volume- and set to sfxVolume
        musicSource.volume = (float)(volume / 100.00);
    }
}
