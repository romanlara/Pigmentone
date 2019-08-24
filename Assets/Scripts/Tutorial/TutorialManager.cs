// -----------------------------------------------------
//                      Pigmentone
//                          by 
//                 Sliverbroom Studios
//       Copyright 2015 Roman Lara & Humberto Lara
// -----------------------------------------------------

using UnityEngine;
using System.IO;
using System.Collections;

public class TutorialManager : MonoBehaviour 
{
	public GameObject[] tutorials;
	public int index;
	public NextScript nextScript;

	public void Start ()
	{
		index = 0;
	}
	
	public void NextTutorial ()
	{
		if (index < tutorials.Length - 1)
		{
			gameObject.SetActive(true);

			tutorials[index].SetActive(false);
			tutorials[++index].SetActive(true);
		}
		else
		{
			if (nextScript.levelExceeded)
				SaveSettings();
		}
	}

	public void SaveSettings ()
	{
//		char dash = Path.DirectorySeparatorChar;
//		string dataPath = Path.GetDirectoryName(Application.dataPath);
//		string folderPath = dataPath + dash + "SaveGame";
//		string filePath = folderPath + dash + "Settings.xml";
//
//		XmlReaderOptions xmlReader = new XmlReaderOptions(filePath);
//
//		if (xmlReader.IsLoaded)
//		{
//			OptionCache option = xmlReader.Read();
//			option.tutorial = true;
//
//			XmlWriterOptions xmlWrite = new XmlWriterOptions(filePath);
//			xmlWrite.Write(option);
//			xmlWrite.Save();
//		}
//		else
//		{
//			OptionCache option = new OptionCache();
//			option.SetDefault();
//			option.tutorial = true;
//			
//			XmlWriterOptions xmlWrite = new XmlWriterOptions(filePath);
//			xmlWrite.Write(option);
//			xmlWrite.Save();
//		}

		OptionCache option = PerlipManager.settingsLip.GetValue<OptionCache>("Option");
		option.tutorial = true;
		
		PerlipManager.settingsLip.SetValue("Option", option);
		PerlipManager.settingsLip.Save();

		Application.LoadLevel("Menu");
	}
}
