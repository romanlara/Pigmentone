// -----------------------------------------------------
//                      Pigmentone
//                          by 
//                 Sliverbroom Studios
//       Copyright 2015 Roman Lara & Humberto Lara
// -----------------------------------------------------

using UnityEngine;
using System.Collections;

public class NextPuzzleButton : MonoBehaviour 
{
	public void NextPuzzle ()
	{
		if (PuzzleManager.current == null) return;
		PuzzleManager.current.NextPuzzle();
	}
}
