// -----------------------------------------------------
//                      Pigmentone
//                          by 
//                 Sliverbroom Studios
//       Copyright 2015 Roman Lara & Humberto Lara
// -----------------------------------------------------

using UnityEngine;
using UnityEngine.Audio;
//using System.IO;
using System.Collections;
using System.Collections.Generic;

public class LogoScript : MonoBehaviour 
{
	public AudioMixer mainMixer;
	public AnimationClip animClip;

	private Animator m_Animator;
	private OptionCache option;
	
	public void Start () 
	{
		m_Animator = GetComponent<Animator>();

//		char dash = Path.DirectorySeparatorChar;
//		string dataPath = Path.GetDirectoryName(Application.dataPath);
//		string folderPath = dataPath + dash + "SaveGame";
//		string filePath = folderPath + dash + "Settings.xml";
//
//		DirectoryInfo dir = new DirectoryInfo(folderPath);
//		if (!dir.Exists) dir.Create();
//
//		XmlReaderOptions xmlReader = new XmlReaderOptions(filePath);
//		
//		if (xmlReader.IsLoaded)
//		{
//			option = xmlReader.Read();
//
//			// Set Audio
//			mainMixer.SetFloat("musicVol", option.musicVol);
//			mainMixer.SetFloat("sfxVol", option.sfxVol);
//
//			// Set Resolution
//			int separator = option.resolution.LastIndexOf('x');
//			int width = int.Parse(option.resolution.Substring(0, separator));
//			int height = int.Parse(option.resolution.Substring(separator + 1));
//
//			Screen.SetResolution(width, height, option.fullscreen);
//		}
//		else
//		{
//			option = new OptionCache();
//			option.SetDefault();
//
//			// Set Audio
//			mainMixer.SetFloat("musicVol", option.musicVol);
//			mainMixer.SetFloat("sfxVol", option.sfxVol);
//
//			// Set Resolution
//			int separator = option.resolution.LastIndexOf('x');
//			int width = int.Parse(option.resolution.Substring(0, separator));
//			int height = int.Parse(option.resolution.Substring(separator + 1));
//			Screen.SetResolution(width, height, option.fullscreen);
//		}

		PerlipManager.Open();

		if (PerlipManager.settingsLip.HasKey("Option"))
		{
			option = PerlipManager.settingsLip.GetValue<OptionCache>("Option");

			// Set Audio
			mainMixer.SetFloat("musicVol", option.musicVol);
			mainMixer.SetFloat("sfxVol", option.sfxVol);

			// Set Resolution
			int separator = option.resolution.LastIndexOf('x');
			int width = int.Parse(option.resolution.Substring(0, separator));
			int height = int.Parse(option.resolution.Substring(separator + 1));

			Screen.SetResolution(width, height, option.fullscreen);
		}
		else
		{
			option = new OptionCache();
			option.SetDefault();

			PerlipManager.settingsLip.SetValue("Option", option);
			PerlipManager.settingsLip.Save();

			// Set Audio
			mainMixer.SetFloat("musicVol", option.musicVol);
			mainMixer.SetFloat("sfxVol", option.sfxVol);
			
			// Set Resolution
			int separator = option.resolution.LastIndexOf('x');
			int width = int.Parse(option.resolution.Substring(0, separator));
			int height = int.Parse(option.resolution.Substring(separator + 1));

			Screen.SetResolution(width, height, option.fullscreen);
		}

		if (PerlipManager.saveGameLip.HasKey("Puzzle0"))
		{
			PuzzleCache cache = new PuzzleCache();
			cache.puzzles = new PuzzleCache.PuzzleParam[cache.Length];

			for (int i = 0; i < cache.Length; i++)
				cache.puzzles[i] = PerlipManager.saveGameLip.GetValue<PuzzleCache.PuzzleParam>("Puzzle" + i);
		}
		else
		{
			PuzzleCache cache = new PuzzleCache();
			cache.SetDefault();

			for (int i = 0; i < cache.Length; i++)
				PerlipManager.saveGameLip.SetValue("Puzzle" + i, PuzzleCache.current.puzzles[i]);

			PerlipManager.saveGameLip.Save();
		}

		m_Animator.SetTrigger("Show");
		Invoke("GoNextScene", animClip.length);
	}

	public void GoNextScene () 
	{
		if (option.tutorial)
			Application.LoadLevel("Menu");
		else
			Application.LoadLevel("Tutorial");
	}
}
