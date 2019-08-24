// -----------------------------------------------------
//                      Pigmentone
//                          by 
//                 Sliverbroom Studios
//       Copyright 2015 Roman Lara & Humberto Lara
// -----------------------------------------------------

using UnityEngine;
using System.Collections;

public class RestartApplication : MonoBehaviour 
{
	public void Restart ()
	{
		if (PuzzleManager.current == null) return;
		PuzzleManager.current.Restart();
	}
}
