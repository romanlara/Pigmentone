// -----------------------------------------------------
//                      Pigmentone
//                          by 
//                 Sliverbroom Studios
//       Copyright 2015 Roman Lara & Humberto Lara
// -----------------------------------------------------

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class GameTimer : MonoBehaviour 
{
	[Serializable] public class ClockColor
	{
		public Color gold;
		public Color silver;
		public Color bronze;
		public Color player;
	}

	[HideInInspector] public Image clockImage;
	[HideInInspector] public Text timeText;
	[HideInInspector] public Text penaltyText;
	public float penalty = 5f;
	public int beepTimes = 5;
	public float beepDuration = 1f;
	public AudioClip beepClip;
	public AudioClip timeUpClip;
	public AudioClip penaltyClip;
	public ClockColor colors;

	private Animator m_Anim;
	private AudioSource m_Source;
	private Puzzle m_Puzzle;

//	private Puzzle.BeatTime m_CurrentTime;
//	private float m_Timer;
	private float m_BeepTimer;
	private bool m_AppliedPenalty;

	public void Awake ()
	{
		m_Anim = GetComponent<Animator>();
		m_Source = GameObject.Find("Canvas/SoundFx").GetComponent<AudioSource>();
	}

	public void Start () 
	{
//		m_CurrentTime = new Puzzle.BeatTime();
//		m_Timer = 0f;
		m_BeepTimer = 0f;

		clockImage.color = colors.gold;
		timeText.color = colors.gold;
	}

	public void Update () 
	{
		if (Pause.current && Pause.current.IsPaused) return;

		if (m_Puzzle == null) 
		{
			try { 
				m_Puzzle = PuzzleManager.current.puzzle; 
			} catch (NullReferenceException) {}
			return;
		}

		if (m_Puzzle.levelExceeded) return;

		if (!m_AppliedPenalty)
		{
			if (m_Puzzle.Timer < m_Puzzle.goldTime.FromSeconds)
			{
				clockImage.color = colors.gold;
				timeText.color = colors.gold;
			}
			else if (m_Puzzle.Timer < m_Puzzle.silverTime.FromSeconds)
			{
				clockImage.color = colors.silver;
				timeText.color = colors.silver;
			}
			else if (m_Puzzle.Timer < m_Puzzle.bronzeTime.FromSeconds)
			{
				clockImage.color = colors.bronze;
				timeText.color = colors.bronze;
			}
			else
			{
				clockImage.color = colors.player;
				timeText.color = colors.player;
			}

			if ((m_Puzzle.Timer > m_Puzzle.goldTime.FromSeconds - (beepTimes + 1)) && 
			    (m_Puzzle.Timer < m_Puzzle.goldTime.FromSeconds + 1))
			{
				Beep(m_Puzzle.goldTime.FromSeconds);
			}

			if ((m_Puzzle.Timer > m_Puzzle.silverTime.FromSeconds - beepTimes) &&
			    (m_Puzzle.Timer < m_Puzzle.silverTime.FromSeconds + 1))
			{
				Beep(m_Puzzle.silverTime.FromSeconds);
			}

			if ((m_Puzzle.Timer > m_Puzzle.bronzeTime.FromSeconds - beepTimes) &&
			    (m_Puzzle.Timer < m_Puzzle.bronzeTime.FromSeconds + 1))
			{
				Beep(m_Puzzle.bronzeTime.FromSeconds);
			}
		}

		if (m_AppliedPenalty)
		{
			m_Anim.SetTrigger("Penalty");
			PlaySound(penaltyClip);
			m_Puzzle.Timer += penalty;
			m_AppliedPenalty = false;
		}

//		m_CurrentTime.FromSeconds = m_Puzzle.Timer;
		timeText.text = m_Puzzle.playerTime.ToString();//m_CurrentTime.ToString();
//		m_Timer += Time.deltaTime;
	}

	public void Penalize ()
	{
		m_AppliedPenalty = true;
		m_BeepTimer = 0f;
	}

	private void PlaySound (AudioClip clip)
	{
		m_Source.PlayOneShot(clip);
	}

	private void Beep (double seconds)
	{
		if (m_BeepTimer >= beepDuration)
		{
			int beepsCounter = beepTimes - ((int) seconds - (int) m_Puzzle.Timer);

			if (beepsCounter < beepTimes)
			{
//				Debug.Log("Time: " + m_Timer + " Beep: " + m_BeepsCounter + " BeepClip " + clock);
				m_Anim.SetTrigger("Show");
				PlaySound(beepClip);
			}
			else
			{
//				Debug.Log("Time: " + m_Timer + " Beep: " + m_BeepsCounter + " PenaltyClip " + clock);
				PlaySound(timeUpClip);
				m_Anim.SetTrigger("Hide");
			}

			m_BeepTimer = 0f;
		}

		m_BeepTimer += Time.deltaTime;
	}
}
