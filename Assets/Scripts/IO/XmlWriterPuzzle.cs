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

public class XmlWriterPuzzle 
{
	private string m_FilePath;
	private XmlDocument m_Doc;
	private XmlElement m_Root;
	
	public XmlWriterPuzzle (string filePath)
	{
		m_FilePath = filePath;
		m_Doc = new XmlDocument();
		
		if (File.Exists(m_FilePath))
		{
			m_Doc.Load(m_FilePath);
			m_Root = m_Doc.DocumentElement;
			m_Root.RemoveAll();
		}
		else
		{
			m_Root = m_Doc.CreateElement("SaveGame");
			m_Doc.AppendChild(m_Root);
		}
	}
	
	public void Save ()
	{
		m_Doc.Save(m_FilePath);
	}

	public void Write ()
	{
		foreach (PuzzleCache.PuzzleParam param in PuzzleCache.current)
		{
			XmlElement elem_Puzzle = m_Doc.CreateElement("Puzzle");
			elem_Puzzle.SetAttribute("num", param.num.ToString());
			elem_Puzzle.SetAttribute("stars", param.stars.ToString());
			elem_Puzzle.SetAttribute("time", param.time.ToString());
			elem_Puzzle.SetAttribute("blocked", param.blocked.ToString().ToLower());
			m_Root.AppendChild(elem_Puzzle);
		}
	}
}
