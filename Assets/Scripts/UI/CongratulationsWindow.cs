// -----------------------------------------------------
//                      Pigmentone
//                          by 
//                 Sliverbroom Studios
//       Copyright 2015 Roman Lara & Humberto Lara
// -----------------------------------------------------

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Analytics;
using System.Collections.Generic;
using System.Collections;

public class CongratulationsWindow : MonoBehaviour 
{
	[HideInInspector] public StartOptions startScript;
	[HideInInspector] public AnimationClip showClockAnimClip;
	[HideInInspector] public AnimationClip showStarAnimClip;
	[HideInInspector] public AudioSource soundSource;

	public Text puzzleText;
	public Transform bronzeTime;
	public Transform silverTime;
	public Transform goldTime;
	public Transform playerTime;
	public AudioClip soundClip;

	private Text m_BronzetimeText;
	private Text m_SilvertimeText;
	private Text m_GoldtimeText;
	private Text m_PlayertimeText;

	private Animator m_BronzeAnim;
	private Animator m_SilverAnim;
	private Animator m_GoldAnim;
	private Animator m_PlayerAnim;

	private Puzzle m_CurrentPuzzle;
	private int m_Stars;

	public void Awake ()
	{
		m_BronzetimeText = bronzeTime.GetComponentInChildren<Text>();
		m_SilvertimeText = silverTime.GetComponentInChildren<Text>();
		m_GoldtimeText = goldTime.GetComponentInChildren<Text>();
		m_PlayertimeText = playerTime.GetComponentInChildren<Text>();

		m_BronzeAnim = bronzeTime.GetComponent<Animator>();
		m_SilverAnim = silverTime.GetComponent<Animator>();
		m_GoldAnim = goldTime.GetComponent<Animator>();
		m_PlayerAnim = playerTime.GetComponent<Animator>();
	}

	public void OnEnable () 
	{
		m_Stars = 0;
		puzzleText.text = "Level " + PuzzleManager.puzzleToPlay;

		m_CurrentPuzzle = PuzzleManager.current.puzzle;//PuzzleManager.current.puzzles[PuzzleManager.puzzleToPlay - 1];
		m_BronzetimeText.text = m_CurrentPuzzle.bronzeTime.ToString();
		m_SilvertimeText.text = m_CurrentPuzzle.silverTime.ToString();
		m_GoldtimeText.text = m_CurrentPuzzle.goldTime.ToString();
		m_PlayertimeText.text = m_CurrentPuzzle.playerTime.ToString();

		// Analytics
		Analytics.CustomEvent("levelsCompleted", new Dictionary<string, object>() {
			{ "level", PuzzleManager.puzzleToPlay },
			{ "playerTime", m_PlayertimeText.text },
			{ "bronzeTime", m_BronzetimeText.text },
			{ "silverTime", m_SilvertimeText.text },
			{ "goldTime", m_GoldtimeText.text }
		});

		// Show clocks
		Invoke("ShowClocks", startScript.fadeColorAnimationClip.length * .5f);
	}

	public void ShowClocks ()
	{
		m_BronzeAnim.SetTrigger("ShowClock");
		m_SilverAnim.SetTrigger("ShowClock");
		m_GoldAnim.SetTrigger("ShowClock");
		m_PlayerAnim.SetTrigger("ShowClock");

		if (m_CurrentPuzzle.playerTime.IsLessThanOrEquals(m_CurrentPuzzle.bronzeTime))
			Invoke("ShowBronzeStar", showStarAnimClip.length);

		if (m_CurrentPuzzle.playerTime.IsLessThanOrEquals(m_CurrentPuzzle.silverTime))
			Invoke("ShowSilverStar", showStarAnimClip.length * 2);

		if (m_CurrentPuzzle.playerTime.IsLessThanOrEquals(m_CurrentPuzzle.goldTime))
			Invoke("ShowGoldStar", showStarAnimClip.length * 3);
	}

	public void ShowBronzeStar ()
	{
		m_Stars += 1;
		m_BronzeAnim.SetTrigger("ShowStar");
		soundSource.clip = soundClip;
		soundSource.Play();

		if (m_Stars > PuzzleCache.current.puzzles[PuzzleManager.puzzleToPlay - 1].stars)
			PuzzleCache.current.puzzles[PuzzleManager.puzzleToPlay - 1].stars = m_Stars;
	}

	public void ShowSilverStar ()
	{
		m_Stars += 1;
		m_SilverAnim.SetTrigger("ShowStar");
		soundSource.clip = soundClip;
		soundSource.Play();

		if (m_Stars > PuzzleCache.current.puzzles[PuzzleManager.puzzleToPlay - 1].stars)
			PuzzleCache.current.puzzles[PuzzleManager.puzzleToPlay - 1].stars = m_Stars;
	}

	public void ShowGoldStar ()
	{
		m_Stars += 1;
		m_GoldAnim.SetTrigger("ShowStar");
		soundSource.clip = soundClip;
		soundSource.Play();

		if (m_Stars > PuzzleCache.current.puzzles[PuzzleManager.puzzleToPlay - 1].stars)
			PuzzleCache.current.puzzles[PuzzleManager.puzzleToPlay - 1].stars = m_Stars;
	}
}
