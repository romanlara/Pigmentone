// -----------------------------------------------------
//                      Pigmentone
//                          by 
//                 Sliverbroom Studios
//       Copyright 2015 Roman Lara & Humberto Lara
// -----------------------------------------------------

using UnityEngine;
using System.Collections;
using UnityEngine.Audio;

public class PlayMusic : MonoBehaviour 
{	
	public AudioClip menuMusicClip;					// Menu music clip to play on main menu
	public AudioClip congratulateMusicClip;			// Congratulate music clip to play on the Congratulations menu
	public AudioClip[] musicClips;					// Array of music clips to play
	public AudioMixerSnapshot volumeDown;			// Reference to Audio mixer snapshot in which the master volume of main mixer is turned down
	public AudioMixerSnapshot volumeUp;				// Reference to Audio mixer snapshot in which the master volume of main mixer is turned up
	
	private AudioSource musicSource;				// Reference to the AudioSource which plays music
	private int musicIndex;

	public void Awake () 
	{
		// Get a component reference to the AudioSource attached to the UI game object
		musicSource = GetComponent<AudioSource>();
	}

	// Used to play menu music
	public void PlayMenuMusic ()
	{
		// Play the menu music clip
		musicSource.clip = menuMusicClip;
		// Play the selected clip
		musicSource.Play();
	}

	// Used to play any game music
	public void PlayGameMusic ()
	{
		// Get new index
		musicIndex = RandomChoice();
		// Play the menu music clip at array index musicIndex
		musicSource.clip = musicClips[musicIndex];
		// Play the selected clip
		musicSource.Play();
	}

	// Used to play congratulate music
	public void PlayCongratulateMusic ()
	{
		// Play the congratulate music cilp
		musicSource.clip = congratulateMusicClip;
		// Play the selected clip
		musicSource.Play();
	}
	
	// Used if running the game in a single scene, takes an integer music source allowing you to choose a clip by number and play.
	public void PlaySelectedMusic (int musicChoice)
	{
		// Play the music clip at the array index musicChoice
		musicSource.clip = musicClips[musicChoice];
		// Play the selected clip
		musicSource.Play();
	}

	// Call this function to very quickly fade up the volume of master mixer
	public void FadeUp (float fadeTime)
	{
		// call the TransitionTo function of the audioMixerSnapshot volumeUp;
		volumeUp.TransitionTo(fadeTime);
	}

	// Call this function to fade the volume to silence over the length of fadeTime
	public void FadeDown (float fadeTime)
	{
		// call the TransitionTo function of the audioMixerSnapshot volumeDown;
		volumeDown.TransitionTo(fadeTime);
	}

	// Call this function to get a index
	private int RandomChoice ()
	{
		// Choose a random index of the musicClips array.
		int i = Random.Range(0, musicClips.Length);
		
		// If it's the same as the previous clip...
		if(i == musicIndex)
			// ... try another random clip.
			return RandomChoice();
		else
			// Otherwise return this index.
			return i;
	}
}
