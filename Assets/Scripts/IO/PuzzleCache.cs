// -----------------------------------------------------
//                      Pigmentone
//                          by 
//                 Sliverbroom Studios
//       Copyright 2015 Roman Lara & Humberto Lara
// -----------------------------------------------------

using UnityEngine;
using System.Collections;

[System.Serializable]

public class PuzzleCache : IEnumerable
{
	[System.Serializable] public class PuzzleParam
	{
		public int num;
		public int stars;
		public float time;
		public bool blocked;

//		public PuzzleParam (int n, int s, float t, bool b)
//		{
//			num = n;
//			stars = s;
//			time = t;
//			blocked = b;
//		}
	}

	[System.Serializable] public class PuzzleEnum : IEnumerator
	{
		private PuzzleParam[] m_Puzzles;
		private int position = -1;

		public PuzzleEnum (PuzzleParam[] list)
		{
			m_Puzzles = list;
		}

		public bool MoveNext ()
		{
			position++;
			return (position < m_Puzzles.Length);
		}

		public void Reset ()
		{
			position = -1;
		}

		public object Current
		{
			get {
				try {
					return m_Puzzles[position];
				}
				catch (System.IndexOutOfRangeException) {
					throw new System.InvalidOperationException();
				}
			}
		}
	}

	public static PuzzleCache current;
	public PuzzleParam[] puzzles;

	private int m_Length = 50;

	public int Length
	{
		get { return m_Length; }
		set { m_Length = value; }
	}

	public PuzzleCache ()
	{
		if (current == null)
			current = this;
	}

	IEnumerator IEnumerable.GetEnumerator ()
	{
		return new PuzzleEnum(puzzles);
	}

	public void SetDefault ()
	{
		puzzles = new PuzzleParam[m_Length];

		for (int i = 0; i < puzzles.Length; i++)
		puzzles[i] = new PuzzleParam(/*i + 1, 0, 0, true*/) 
		{ num = i + 1, stars = 0, time = 0, blocked = true };

		puzzles[0].blocked = false;
	}
}
