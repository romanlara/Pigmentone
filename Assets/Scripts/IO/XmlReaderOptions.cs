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

public class XmlReaderOptions 
{
	private string m_FilePath;
	private XmlDocument m_Doc;
	private bool m_IsLoaded = false;

	public bool IsLoaded
	{
		get { return m_IsLoaded; }
	}

	public XmlReaderOptions (string filePath)
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

	public OptionCache Read ()
	{
		OptionCache option = new OptionCache();

		string root = "//Settings";

		XmlNode node_MusicVol = m_Doc.SelectSingleNode(root + "/MusicVol");
		option.musicVol = (float) XmlConvert.ToDouble(node_MusicVol.InnerXml);

		XmlNode node_SfxVol = m_Doc.SelectSingleNode(root + "/SfxVol");
		option.sfxVol = (float) XmlConvert.ToDouble(node_SfxVol.InnerXml);

		XmlNode node_Resolution = m_Doc.SelectSingleNode(root + "/Resolution");
		option.resolution = node_Resolution.InnerXml;
		option.resIndex = (int) XmlConvert.ToDouble(((XmlElement) node_Resolution).GetAttribute("index"));

		XmlNode node_Fullscreen = m_Doc.SelectSingleNode(root + "/Fullscreen");
		option.fullscreen = XmlConvert.ToBoolean(node_Fullscreen.InnerXml);

		XmlNode node_Tutorial = m_Doc.SelectSingleNode(root + "/Tutorial");
		option.tutorial = XmlConvert.ToBoolean(node_Tutorial.InnerXml);

		return option;
	}
}
