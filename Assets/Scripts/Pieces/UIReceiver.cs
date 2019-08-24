// -----------------------------------------------------
//                      Pigmentone
//                          by 
//                 Sliverbroom Studios
//       Copyright 2015 Roman Lara & Humberto Lara
// -----------------------------------------------------

using UnityEngine;
using System.Collections;

public class UIReceiver : Receiver 
{
	public SpriteRenderer icon;
	public Color normalColor;
	public Color highlightColor;
	public Animator m_Anim;

	private bool m_TriggeredShow;
	private bool m_TriggeredHide;

	public override void Awake ()
	{
		base.Awake();
	}
	
	public override void Start () 
	{
		base.Start();
	}

	public void OnEnable ()
	{
		m_TriggeredShow = false;
		m_TriggeredHide = false;
	}

	public override void Update () 
	{
		base.Update();

		if (base.receivedColors.Length == 0)
			icon.color = Color.Lerp(icon.color, normalColor, 0.5f * base.speedInking * Time.deltaTime);
		else
			icon.color = Color.Lerp(icon.color, highlightColor, 0.5f * base.speedInking * Time.deltaTime);

		if (base.ready)
		{
			if (!m_TriggeredShow)
			{
				m_TriggeredShow = true;
				m_TriggeredHide = false;
				m_Anim.SetTrigger("Show");
			}
		}
		else
		{
			if (!m_TriggeredHide)
			{
				m_TriggeredShow = false;
				m_TriggeredHide = true;
				m_Anim.SetTrigger("Hide");
			}
		}
	}
}
