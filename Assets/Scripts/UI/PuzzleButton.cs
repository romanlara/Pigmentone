// -----------------------------------------------------
//                      Pigmentone
//                          by 
//                 Sliverbroom Studios
//       Copyright 2015 Roman Lara & Humberto Lara
// -----------------------------------------------------

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PuzzleButton : MonoBehaviour 
{
	public StartOptions startScript;
	public Color starColorDisable;
	public Color starColorEnable;
	public int numLevel = 0;
	public int numStars = 0;
	public bool blocked = false;

	private Button m_Button;
	private Text m_Text;
	private Image m_Padlock;
	private Transform m_AchievementStars;
	private Image m_Star1;
	private Image m_Star2;
	private Image m_Star3;

	public void Start () 
	{
		m_Button = GetComponent<Button>();
		m_Text = transform.FindChild("PuzzleText").GetComponent<Text>();
		m_Padlock = transform.FindChild("Padlock").GetComponent<Image>();

		m_AchievementStars = transform.FindChild("AchievementStars");
		m_Star1 = m_AchievementStars.FindChild("StarBorder 1/Star").GetComponent<Image>();
		m_Star2 = m_AchievementStars.FindChild("StarBorder 2/Star").GetComponent<Image>();
		m_Star3 = m_AchievementStars.FindChild("StarBorder 3/Star").GetComponent<Image>();
	}

	public void LateUpdate () 
	{
		blocked = PuzzleCache.current.puzzles[numLevel - 1].blocked;
		numStars = PuzzleCache.current.puzzles[numLevel - 1].stars;
		
		m_Text.text = numLevel.ToString();

		if (blocked)
		{
			m_Button.interactable = false;
			m_Text.gameObject.SetActive(false);
			m_Padlock.gameObject.SetActive(true);
			m_AchievementStars.gameObject.SetActive(false);
		}
		else
		{
			m_Button.interactable = true;
			m_Text.gameObject.SetActive(true);
			m_Padlock.gameObject.SetActive(false);
			m_AchievementStars.gameObject.SetActive(true);
			
			m_Star1.color = (numStars > 0) ? starColorEnable : starColorDisable;
			m_Star2.color = (numStars > 1) ? starColorEnable : starColorDisable;
			m_Star3.color = (numStars > 2) ? starColorEnable : starColorDisable;
		}
	}

	public void GoLevel ()
	{
		startScript.GoLevel(numLevel);
	}
}
