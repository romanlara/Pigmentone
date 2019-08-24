// -----------------------------------------------------
//                      Pigmentone
//                          by 
//                 Sliverbroom Studios
//       Copyright 2015 Roman Lara & Humberto Lara
// -----------------------------------------------------

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ReceiverProgress : MonoBehaviour 
{
	public Image cromatic;
	public Image circle;
	public Image ring;
	
	public void Start () 
	{
		UnComplete();
	}

	public void UnComplete ()
	{
		Color cromaticColor = cromatic.color;
		cromaticColor.a = 1f;
		cromatic.color = cromaticColor;

		Color circleColor = circle.color;
		circleColor.a = 0f;
		circle.color = circleColor;

		Color ringColor = ring.color;
		ringColor.a = 0f;
		ring.color = ringColor;
	}

	public void Complete () 
	{
		Color cromaticColor = cromatic.color;
		cromaticColor.a = 0f;
		cromatic.color = cromaticColor;
		
		Color circleColor = circle.color;
		circleColor.a = 1f;
		circle.color = circleColor;
		
		Color ringColor = ring.color;
		ringColor.a = 1f;
		ring.color = ringColor;
	}
}
