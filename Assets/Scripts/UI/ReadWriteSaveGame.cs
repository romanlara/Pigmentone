// -----------------------------------------------------
//                      Pigmentone
//                          by 
//                 Sliverbroom Studios
//       Copyright 2015 Roman Lara & Humberto Lara
// -----------------------------------------------------

using UnityEngine;
using System.Collections;
using System.IO;

public class ReadWriteSaveGame : MonoBehaviour 
{
//	private char m_Separator;
//	private string m_DataPath;
//	private string m_FolderPath;
//	private string m_FilePath;
//
//	public void Awake ()
//	{
//		m_Separator = Path.DirectorySeparatorChar;
//		m_DataPath = Path.GetDirectoryName(Application.dataPath);
//		m_FolderPath = m_DataPath + m_Separator + "SaveGame";
//		m_FilePath = m_FolderPath + m_Separator + "SaveGame.xml";
//	}
//	
//	public void Start () 
//	{
//		XmlReaderPuzzle xmlReader = new XmlReaderPuzzle(m_FilePath);
//
//		if (xmlReader.IsLoaded)
//		{
//			xmlReader.Read();
//			Debug.Log("Read save game.");
//		}
//		else
//		{
//			PuzzleCache cache = new PuzzleCache();
//			cache.Length = 50;
//			cache.SetDefault();
//			Debug.Log("Not read save game.");
//		}
//	}

	public void SaveGame ()
	{
//		DirectoryInfo dir = new DirectoryInfo(m_FolderPath);
//		if (!dir.Exists) dir.Create();
//		
//		XmlWriterPuzzle xmlWrite = new XmlWriterPuzzle(m_FilePath);
//		xmlWrite.Write();
//		xmlWrite.Save();

		for (int i = 0; i < PuzzleCache.current.Length; i++)
			PerlipManager.saveGameLip.SetValue("Puzzle" + i, PuzzleCache.current.puzzles[i]);

		PerlipManager.saveGameLip.Save();

		Debug.Log("Game saved.");
	}
}
