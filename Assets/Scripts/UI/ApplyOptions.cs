// -----------------------------------------------------
//                      Pigmentone
//                          by 
//                 Sliverbroom Studios
//       Copyright 2015 Roman Lara & Humberto Lara
// -----------------------------------------------------

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.IO;

public class ApplyOptions : MonoBehaviour 
{
	public Slider musicVolOption;
	public Slider sfxVolOption;
	public ResolutionOption resolutionOption;
	public Toggle fullscreenOption;

	public void Apply ()
	{
		int width = resolutionOption.width;
		int height = resolutionOption.height;
		bool fullscreen = fullscreenOption.isOn;

//		char separator = Path.DirectorySeparatorChar;
//		string dataPath = Path.GetDirectoryName(Application.dataPath);
//		string folderPath = dataPath + separator + "SaveGame";
//		string filePath = folderPath + separator + "Settings.xml";

		OptionCache option = new OptionCache();
		option.musicVol = musicVolOption.value;
		option.sfxVol = sfxVolOption.value;
		option.resolution = resolutionOption.currentElem;
		option.resIndex = resolutionOption.index;
		option.fullscreen = fullscreen;
		option.tutorial = true;

//		DirectoryInfo dir = new DirectoryInfo(folderPath);
//		if (!dir.Exists) dir.Create();
//
//		XmlWriterOptions xmlWrite = new XmlWriterOptions(filePath);
//		xmlWrite.Write(option);
//		xmlWrite.Save();

		PerlipManager.settingsLip.SetValue("Option", option);
		PerlipManager.settingsLip.Save();

		Screen.SetResolution(width, height, fullscreen);

		Debug.Log("Set Options -> MusicVol(" + musicVolOption.value + 
		          "), SfxVol(" + sfxVolOption.value + 
		          "), Resolution(" + width + "x" + height + ":" + resolutionOption.index +
		          "), Fullscreen(" + fullscreen + ")");
	}
}
