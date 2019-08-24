// -----------------------------------------------------
//                      Pigmentone
//                          by 
//                 Sliverbroom Studios
//       Copyright 2015 Roman Lara & Humberto Lara
// -----------------------------------------------------

using UnityEngine;
using System.Collections;

public class PipeEvent : MonoBehaviour 
{
	private Pipeline m_Pipeline;
	private bool m_bMouseOver;

	public void Start () 
	{
		m_Pipeline = GetComponentInParent<Pipeline>();
	}

	public void Update ()
	{
		if (Pause.current && Pause.current.IsPaused) return;
		if (!m_bMouseOver) return;

		if (Input.GetKeyDown(KeyCode.Mouse0)) 
			m_Pipeline.OnClick(0);
		else if (Input.GetKeyDown(KeyCode.Mouse1)) 
			m_Pipeline.OnClick(1);
	}

	public void OnMouseOver ()
	{
		m_bMouseOver = true;
	}

	public void OnMouseExit ()
	{
		m_bMouseOver = false;
	}
}
