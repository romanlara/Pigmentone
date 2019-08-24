// -----------------------------------------------------
//                      Pigmentone
//                          by 
//                 Sliverbroom Studios
//       Copyright 2015 Roman Lara & Humberto Lara
// -----------------------------------------------------

using UnityEngine;
using System.Collections;

public class ResetCredits : MonoBehaviour 
{
	public void OnEnable ()
	{
		RectTransform trans = GetComponent<RectTransform>();
		trans.position = new Vector3(trans.position.x, -73f, trans.position.z);
	}
}
