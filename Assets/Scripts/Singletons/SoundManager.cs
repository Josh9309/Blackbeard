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
        //musicLibrary.Add("backgroundMusic", Resources.Load("Sfx/backgroundMusic") as AudioClip);

        //loop background music
        musicSource.loop = true; //turn on looping
        musicSource.clip = musicLibrary["backgroundMusic"]; //set default song
        musicSource.Play(); //play music
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
    /// Plays sound
    /// </summary>
    /// <param name="name">key for sound to be played</param>
	public void PlaySfx(string name)
    { 
        //play sound from lib once
		sfxSource.PlayOneShot(sfxLibrary[name], sfxSource.volume);
	}

    /// <summary>
    /// Plays sound from a specific position in world space by creating its own source
    /// </summary>
    /// <param name="name">key for sound to be played</param>
    /// <param name="position">world cordinates to be played from</param>
    public void PlaySfxAt(string name, Vector3 position)
    {
        //doesnt use source bc it creates its own for this
        AudioSource.PlayClipAtPoint(sfxLibrary[name], position, sfxSource.volume);
    }

    /// <summary>
    /// changes sfx volume
    /// </summary>
    /// <param name="volume">number 0-100 of volume</param>
    public void ChangeSfxVolume(int volume)
    {
        //chnage volume into a float for the actual volume- and set to sfxVolume
        sfxSource.volume = (volume / 100);
    }

    /// <summary>
    /// changes music volume
    /// </summary>
    /// <param name="volume">number 0-100 of volume</param>
    public void ChangeMusicVolume(int volume)
    {
        //chnage volume into a float for the actual volume- and set to sfxVolume
        musicSource.volume = (volume / 100);
    }
}
