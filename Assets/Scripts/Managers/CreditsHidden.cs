// -----------------------------------------------------
//                      Pigmentone
//                          by 
//                 Sliverbroom Studios
//       Copyright 2015 Roman Lara & Humberto Lara
// -----------------------------------------------------

using UnityEngine;
using UnityEngine.Analytics;
using System.Collections.Generic;
using System.Collections;

public class CreditsHidden : MonoBehaviour 
{
	public float delay = 2f;
	public AudioClip soundClip;
	[HideInInspector] public bool levelExceeded;

	private ShowPanels m_ShowPanels;					// Reference to the ShowPanel script
	private AudioSource m_SoundFx;						// Reference to the SoundFx audio source

	private Receiver[] m_Receivers;
	private Emitter[] m_Emitters;

	private Vector3[] m_ReceiversPos;
	private Quaternion[] m_ReceiversRot;
	private Vector3[] m_EmittersPos;
	private float m_TimerToCongratulate;

	public void OnEnable ()
	{
		levelExceeded = false;
		m_TimerToCongratulate = 0;
	}
	
	public void Awake () 
	{
		// Get a component reference to ShowPanel.
		m_ShowPanels = GameObject.Find("Canvas").GetComponent<ShowPanels>();
		// Get a component reference to AudioSource.
		m_SoundFx = GameObject.Find("Canvas/SoundFx").GetComponent<AudioSource>();
		// Get all receivers.
		m_Receivers = transform.GetComponentsInChildren<Receiver>();
		// Get all emitters.
		m_Emitters = transform.GetComponentsInChildren<Emitter>();

		m_ReceiversPos = new Vector3[m_Receivers.Length];
		m_ReceiversRot = new Quaternion[m_Receivers.Length];
		for (int i = 0; i < m_Receivers.Length; i++)
		{
			m_ReceiversPos[i] = m_Receivers[i].transform.position;
			m_ReceiversRot[i] = m_Receivers[i].transform.rotation;
		}

		m_EmittersPos = new Vector3[m_Emitters.Length];
		for (int i = 0; i < m_Emitters.Length; i++)
			 m_EmittersPos[i] = m_Emitters[i].transform.position;
	}

	public void Update () 
	{
		if (m_TimerToCongratulate >= delay)
		{
			m_TimerToCongratulate = 0;

			if (Truth(levelExceeded, m_Receivers.Length - 1, m_Receivers))
			{
				levelExceeded = true;
				DoCredits();
			}
		}

		if (Truth(levelExceeded, m_Receivers.Length - 1, m_Receivers))
			m_TimerToCongratulate += Time.deltaTime;
	}

	private bool Truth (bool onces, int threshold, params Receiver[] receivers)
	{
		// Counter the true's.
		int trueCnt = 0;
		// When is 'false' this run all the time.
		if (!onces)
		{
			// Travel the array.
			foreach (Receiver r in receivers)
				// If the first sentence is 'false' the rest does not run;
				// and if the previous sentence is 'true' the counter to increment.
				if (r.ready && (++trueCnt > threshold))
					// Finish when the counter is top to the threshold.
					return true;
		}
		// If the travel failed or onces is 'true', then returns false.
		return false;
	}

	private void DoCredits ()
	{
		m_SoundFx.clip = soundClip;
		m_SoundFx.Play();

		for (int i = 0; i < m_Receivers.Length; i++)
		{
			m_Receivers[i].transform.position = m_ReceiversPos[i];
			m_Receivers[i].transform.rotation = m_ReceiversRot[i];
		}

		for (int i = 0; i < m_Emitters.Length; i++)
			m_Emitters[i].transform.position = m_EmittersPos[i];

		Analytics.CustomEvent("credits", new Dictionary<string, object>() {
			{ "uncovered", true }
		});

		m_ShowPanels.ShowCreditsPanel();
		m_ShowPanels.HideMenu();
	}
}
