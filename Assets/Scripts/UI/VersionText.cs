// -----------------------------------------------------
//                      Pigmentone
//                          by 
//                 Sliverbroom Studios
//       Copyright 2015 Roman Lara & Humberto Lara
// -----------------------------------------------------

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class VersionText : MonoBehaviour 
{
	private string version = "v1.0";
	private Text text;
	
	void Start () 
	{
		text = GetComponent<Text>();
		text.text = version;
	}
}
