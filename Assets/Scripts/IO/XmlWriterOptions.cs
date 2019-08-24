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

public class XmlWriterOptions 
{
	private string m_FilePath;
	private XmlDocument m_Doc;
	private XmlElement m_Root;

	public XmlWriterOptions (string filePath)
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
			m_Root = m_Doc.CreateElement("Settings");
			m_Doc.AppendChild(m_Root);
		}
	}

	public void Save ()
	{
		m_Doc.Save(m_FilePath);
	}

	public void Write (OptionCache option)
	{
		XmlElement elem_MusicVol = m_Doc.CreateElement("MusicVol");
		elem_MusicVol.InnerText = option.musicVol.ToString();
		m_Root.AppendChild(elem_MusicVol);

		XmlElement elem_SfxVol = m_Doc.CreateElement("SfxVol");
		elem_SfxVol.InnerText = option.sfxVol.ToString();
		m_Root.AppendChild(elem_SfxVol);

		XmlElement elem_Resolution = m_Doc.CreateElement("Resolution");
		elem_Resolution.SetAttribute("index", option.resIndex.ToString());
		elem_Resolution.InnerText = option.resolution;
		m_Root.AppendChild(elem_Resolution);

		XmlElement elem_Fullscreen = m_Doc.CreateElement("Fullscreen");
		elem_Fullscreen.InnerText = option.fullscreen.ToString().ToLower();
		m_Root.AppendChild(elem_Fullscreen);

		XmlElement elem_Tutorial = m_Doc.CreateElement("Tutorial");
		elem_Tutorial.InnerText = option.tutorial.ToString().ToLower();
		m_Root.AppendChild(elem_Tutorial);
	}
}
