// -----------------------------------------------------
//                      Pigmentone
//                          by 
//                 Sliverbroom Studios
//       Copyright 2015 Roman Lara & Humberto Lara
// -----------------------------------------------------

using UnityEngine;
using System.Collections;

[System.Serializable]

public class OptionCache 
{
	private class Default
	{
		public static float musicVol = -10f;
		public static float sfxVol = -5f;
		public static string resolution = "800x500";
		public static int resIndex = 0;
		public static bool fullscreen = true;
		public static bool tutorial = false;
	}

	public float musicVol = Default.musicVol;
	public float sfxVol = Default.sfxVol;
	public string resolution = Default.resolution;
	public int resIndex = Default.resIndex;
	public bool fullscreen = Default.fullscreen;
	public bool tutorial = Default.tutorial;

	public void SetDefault ()
	{
		musicVol = Default.musicVol;
		sfxVol = Default.sfxVol;
		resolution = Default.resolution;
		resIndex = Default.resIndex;
		fullscreen = Default.fullscreen;
		tutorial = Default.tutorial;
	}
}
