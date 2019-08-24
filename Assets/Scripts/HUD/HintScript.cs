// -----------------------------------------------------
//                      Pigmentone
//                          by 
//                 Sliverbroom Studios
//       Copyright 2015 Roman Lara & Humberto Lara
// -----------------------------------------------------

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HintScript : MonoBehaviour 
{
	public static bool showHint;
	public float hintDuration = 5f;
	public float delay = 30f;

	[HideInInspector] public GameTimer gameTimer;

	private Selectable m_Selectable;
	private Image m_Clock;
	private bool m_Play;
	private float m_Timer;

	public void Awake ()
	{
		m_Selectable = GetComponent<Selectable>();
		m_Clock = transform.FindChild("Clock").GetComponent<Image>();
	}
	
	public void Start () 
	{
		showHint = false;
		m_Play = false;
		m_Clock.fillAmount = 1f;
		m_Timer = 0f;
		m_Selectable.interactable = true;
	}

	public void OnDisable ()
	{
		Start();
	}

	public void Update () 
	{
		if (m_Play)
		{
			m_Selectable.interactable = false;
			m_Clock.fillAmount = Mathf.Lerp(m_Clock.fillAmount, m_Timer / delay, 0.5f * delay);
			m_Timer += Time.deltaTime;

			if (m_Timer >= hintDuration)
				showHint = false;
		}

		if (m_Timer >= delay)
		{
			m_Play = false;
			m_Selectable.interactable = true;
			m_Timer = 0f;
		}
	}

	public void Play ()
	{
		m_Play = true;
		showHint = true;
		m_Clock.fillAmount = 0f;
		gameTimer.Penalize();
	}
}
