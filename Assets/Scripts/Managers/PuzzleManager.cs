// -----------------------------------------------------
//                      Pigmentone
//                          by 
//                 Sliverbroom Studios
//       Copyright 2015 Roman Lara & Humberto Lara
// -----------------------------------------------------

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class PuzzleManager : MonoBehaviour 
{
	public static PuzzleManager current;
	public static int puzzleToPlay;

//	public Puzzle[] puzzles;
	public Puzzle puzzle;
	
	private StartOptions m_StartScript;
//	private int m_puzzlePrevious;

	public void Awake () 
	{
		if (current == null)
			current = this;
		else if (current != null)
			Destroy(gameObject);
	}

	public void Start ()
	{
		if (GameObject.Find("Canvas"))
			m_StartScript = GameObject.Find("Canvas").GetComponent<StartOptions>();
	}

//	public void PlayPuzzle (int puzzle)
//	{
//		m_puzzlePrevious = puzzleToPlay;
//		puzzleToPlay = puzzle;
//
//		if (puzzleToPlay > puzzles.Length)
//			puzzleToPlay = 1;
//
//		IEnumerable<Puzzle> previousQuery = 
//			from p in puzzles
//			where (p.whatIsPuzzle == m_puzzlePrevious)
//			select p;
//
//		IEnumerable<Puzzle> playQuery =
//			from p in puzzles
//			where (p.whatIsPuzzle == puzzleToPlay)
//			select p;
//
//		foreach (Puzzle p in previousQuery)
//			p.gameObject.SetActive(false);
//
//		foreach (Puzzle p in playQuery)
//			p.gameObject.SetActive(true);
//	}

	public void NextPuzzle ()
	{	
		if (++puzzleToPlay > PuzzleCache.current.Length)
		{
			puzzleToPlay = 1;
			m_StartScript.GoThanks();
		}
		else
			m_StartScript.GoLevel(puzzleToPlay);
	}

	public void Restart ()
	{
		m_StartScript.GoLevel(puzzleToPlay);
	}
}
