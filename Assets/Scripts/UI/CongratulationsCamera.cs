// -----------------------------------------------------
//                      Pigmentone
//                          by 
//                 Sliverbroom Studios
//       Copyright 2015 Roman Lara & Humberto Lara
// -----------------------------------------------------

using UnityEngine;
using System.Collections;

public class CongratulationsCamera : MonoBehaviour 
{
	public static CongratulationsCamera current;
	
	public void Awake ()
	{
		if (current == null)
			current = this;
		else if (current != null)
			Destroy(gameObject);
	}
}
