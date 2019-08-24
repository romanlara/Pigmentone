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

public class SetOptions : MonoBehaviour 
{
	public Slider musicVolOption;
	public Slider sfxVolOption;
	public ResolutionOption resolutionOption;
	public Toggle fullscreenOption;
	
	public void OnEnable ()
	{
//		char separator = Path.DirectorySeparatorChar;
//		string dataPath = Path.GetDirectoryName(Application.dataPath);
//		string filePath = dataPath + separator + "SaveGame" + separator + "Settings.xml";
//
//		XmlReaderOptions xmlReader = new XmlReaderOptions(filePath);
//
//		if (xmlReader.IsLoaded)
//		{
//			OptionCache option = xmlReader.Read();
//
//			musicVolOption.value = option.musicVol;
//			sfxVolOption.value = option.sfxVol;
//			resolutionOption.index = option.resIndex;
//			fullscreenOption.isOn = option.fullscreen;
//
//			Debug.Log("Readed Xml options");
//		}
//		else
//		{
//			Debug.Log("Not readed Xml options");
//		}

		OptionCache option = PerlipManager.settingsLip.GetValue<OptionCache>("Option");

		musicVolOption.value = option.musicVol;
		sfxVolOption.value = option.sfxVol;
		resolutionOption.index = option.resIndex;
		fullscreenOption.isOn = option.fullscreen;
	}
}
