// -----------------------------------------------------
//                      Pigmentone
//                          by 
//                 Sliverbroom Studios
//       Copyright 2015 Roman Lara & Humberto Lara
// -----------------------------------------------------

using UnityEngine;
using System.IO;
using System.Collections;
using SardonicMe.Perlib;

public class PerlipManager 
{
	public static Perlib settingsLip;
	public static Perlib saveGameLip;

	private const string PASS_SETTINGS = "jdjd74shs901n3";
	private const string PASS_SAVEGAME = "laks793dhdk7ad";

	private static char slash;
	private static string dataPath;
	private static string folderPath;

	public static void Open ()
	{
		slash = Path.DirectorySeparatorChar;
		dataPath = Path.GetDirectoryName(Application.dataPath);

		folderPath = dataPath + slash + "SaveGame";

		settingsLip = new Perlib(new FileInfo(folderPath + slash + "Settings.sav"), PASS_SETTINGS);
		saveGameLip = new Perlib(new FileInfo(folderPath + slash + "SaveGame.sav"), PASS_SAVEGAME);

		settingsLip.Open();
		saveGameLip.Open();
	}
}
