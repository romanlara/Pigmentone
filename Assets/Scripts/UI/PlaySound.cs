// -----------------------------------------------------
//                      Pigmentone
//                          by 
//                 Sliverbroom Studios
//       Copyright 2015 Roman Lara & Humberto Lara
// -----------------------------------------------------

using UnityEngine;
using System.Collections;

public class PlaySound : MonoBehaviour 
{
	public AudioClip soundClip;						// Sound clip to play

	private AudioSource m_SoundSource;				// Reference to the AudioSource which plays music

	public void Awake () 
	{
		// Get a component reference to the AudioSource attached to the UI game object
		m_SoundSource = GameObject.Find("Canvas/SoundFx").GetComponent<AudioSource>();
	}
	
	public void Play ()
	{
		try 
		{
			// Play the sound clip
			m_SoundSource.clip = soundClip;
			// Play the selected clip
			m_SoundSource.Play();
		}
		catch (System.NullReferenceException) {}
	}
}
