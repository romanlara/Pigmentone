// -----------------------------------------------------
//                      Pigmentone
//                          by 
//                 Sliverbroom Studios
//       Copyright 2015 Roman Lara & Humberto Lara
// -----------------------------------------------------

using UnityEngine;
using System.Collections;
using System.Xml;
using System.IO;

public class XmlReaderPuzzle
{
	private string m_FilePath;
	private XmlDocument m_Doc;
	private bool m_IsLoaded = false;
	
	public bool IsLoaded
	{
		get { return m_IsLoaded; }
	}
	
	public XmlReaderPuzzle (string filePath)
	{
		m_FilePath = filePath;
		m_Doc = new XmlDocument();
		LoadDocument();
	}
	
	private void LoadDocument () 
	{
		if (File.Exists(m_FilePath)) 
		{
			m_Doc.Load(m_FilePath);
			m_IsLoaded = true;
		} 
		else 
		{
			m_IsLoaded = false;
		}
	}

	public void Read ()
	{
		PuzzleCache puzzleCache = new PuzzleCache();
		string root = "//SaveGame";

		XmlNodeList node_Puzzles = m_Doc.SelectNodes(root + "/Puzzle");
		puzzleCache.Length = node_Puzzles.Count;
		puzzleCache.SetDefault();

		for (int i = 0; i < node_Puzzles.Count; i++)
		{
			puzzleCache.puzzles[i].num = (int) XmlConvert.ToDouble(((XmlElement) node_Puzzles[i]).GetAttribute("num"));
			puzzleCache.puzzles[i].stars = (int) XmlConvert.ToDouble(((XmlElement) node_Puzzles[i]).GetAttribute("stars"));
			puzzleCache.puzzles[i].time = (float) XmlConvert.ToDouble(((XmlElement) node_Puzzles[i]).GetAttribute("time"));
			puzzleCache.puzzles[i].blocked = XmlConvert.ToBoolean(((XmlElement) node_Puzzles[i]).GetAttribute("blocked"));
		}
	}
}
