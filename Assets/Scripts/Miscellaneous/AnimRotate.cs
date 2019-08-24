// -----------------------------------------------------
//                      Pigmentone
//                          by 
//                 Sliverbroom Studios
//       Copyright 2015 Roman Lara & Humberto Lara
// -----------------------------------------------------

using UnityEngine;
using System.Collections;

public class AnimRotate : MonoBehaviour 
{
	public float speed = 5;
	public bool isUI = false;

	public void Update () 
	{
		if (Pause.current && Pause.current.IsPaused && !isUI) return;

		transform.Rotate(Vector3.forward * speed);
	}
}
