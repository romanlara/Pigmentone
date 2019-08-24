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
using System;

public class Puzzle : MonoBehaviour 
{
	public enum Location
	{
		Up,
		Down,
		Left,
		Right
	};

	[System.Serializable] public class EmitterSetting
	{
		public Puzzle.Location locate = Puzzle.Location.Up;
		public Color colorToEmit;
		public float offset = 0f;
	}

	[System.Serializable] public class PipeSetting
	{
		public int normal = 0;
		public int mirror = 0;
	}

	[System.Serializable] public class BeatTime
	{
		public int hours = 0;
		public int minutes = 0;
		public int seconds = 0;

		private TimeSpan m_TimeSpan;

		public TimeSpan Span
		{
			get { return m_TimeSpan; }
		}

		public double FromSeconds
		{
			get { return m_TimeSpan.TotalSeconds; }
			set {
				m_TimeSpan = TimeSpan.FromSeconds(value); 

				hours = m_TimeSpan.Hours;
				minutes = m_TimeSpan.Minutes;
				seconds = m_TimeSpan.Seconds;
			}
		}

		public BeatTime ()
		{
			m_TimeSpan = new TimeSpan(hours, minutes, seconds);
		}

		public BeatTime (int h, int m, int s)
		{
			hours = h;
			minutes = m;
			seconds = s;

			m_TimeSpan = new TimeSpan(hours, minutes, seconds);
		}

		public int CompareTo (BeatTime beatTime)
		{
			return m_TimeSpan.CompareTo(beatTime.Span);
		}

		public bool IsLessThanOrEquals (BeatTime beatTime)
		{
			int compare = m_TimeSpan.CompareTo(beatTime.Span);

			if (compare <= 0)
				return true;
			else
				return false;
		}

		public override string ToString ()
		{
			string time = "";
			int h = m_TimeSpan.Hours, 
				m = m_TimeSpan.Minutes, 
				s = m_TimeSpan.Seconds;

			if (h > 0) 
				time += ((h < 10) ? "0" + h.ToString() : h.ToString()) + ":";
			time += ((m < 10) ? "0" + m.ToString() : m.ToString()) + "''";
			time += (s < 10) ? "0" + s.ToString() : s.ToString();

			return time;
		}
	}

	public int whatIsPuzzle;
	public float cameraSize = 5f;
	public float delay = 1f;
	[Space(8)]
//	public PipeSetting amountPipes = new PipeSetting();
	public bool usePipes;
	[Space(8)]
	public BeatTime goldTime;
	public BeatTime silverTime;
	public BeatTime bronzeTime;
	public BeatTime playerTime;
	[Space(8)]
	public GameObject emitterPrefab;
	public EmitterSetting[] emitters;
	[HideInInspector] public bool levelExceeded;

	private Receiver[] m_receivers;
	private StartOptions m_StartScript;					// Reference to the StartButton script
	private float m_TimerToCongratulate;
	private float m_PlayerTime;

	public float Timer 
	{
		get { return m_PlayerTime; }
		set { m_PlayerTime = value; }
	}

	public void Start () 
	{
		if (Pause.current)
			// Get a component reference to StartButton attached to this object, store in startScript variable
			m_StartScript = GameObject.Find("Canvas").GetComponent<StartOptions>();

		Camera.main.orthographicSize = cameraSize;
		m_receivers = transform.GetComponentsInChildren<Receiver>();

		RemoveEmitters();
		GenerateEmitters();
//		SetPipelineHUD();

		goldTime = new BeatTime(goldTime.hours, goldTime.minutes, goldTime.seconds);
		silverTime = new BeatTime(silverTime.hours, silverTime.minutes, silverTime.seconds);
		bronzeTime = new BeatTime(bronzeTime.hours, bronzeTime.minutes, bronzeTime.seconds);

		m_TimerToCongratulate = 0;
		m_PlayerTime = 0;

		// Analytics
		Analytics.CustomEvent("usersInLevel", new Dictionary<string, object>() {
			{ "level", whatIsPuzzle }
		});
	}

	public void Update () 
	{
		if (Pause.current && Pause.current.IsPaused) return;
		if (!gameObject.activeInHierarchy) return;

		if (m_TimerToCongratulate >= delay)
		{
			m_TimerToCongratulate = 0;

			if (Truth(levelExceeded, m_receivers.Length - 1, m_receivers))
			{
				levelExceeded = true;

				if (m_StartScript && !m_StartScript.inMainMenu)
					DoCongratulation();
			}
		}

		if (Truth(levelExceeded, m_receivers.Length - 1, m_receivers))
			m_TimerToCongratulate += Time.deltaTime;

		if (!levelExceeded)
			playerTime.FromSeconds = m_PlayerTime += Time.deltaTime;
	}

	private void DoCongratulation ()
	{
		PuzzleCache.current.puzzles[whatIsPuzzle - 1].time = m_PlayerTime;

		if (whatIsPuzzle - 1 < PuzzleCache.current.puzzles.Length)
			PuzzleCache.current.puzzles[whatIsPuzzle].blocked = false;

		Pause.current.DoCongratulation();
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

	private void RemoveEmitters ()
	{
		foreach (Transform child in transform)
			if (child.GetComponent<Emitter>())
				Destroy(child.gameObject);
	}

	private void GenerateEmitters ()
	{
		foreach (EmitterSetting setting in emitters)
		{
			GameObject go = Instantiate(emitterPrefab, transform.position, Quaternion.identity) as GameObject;
			go.transform.SetParent(transform);
			
			Emitter emitter = go.GetComponent<Emitter>();
			emitter.colorToEmit = setting.colorToEmit;
			emitter.iniOffset = setting.offset;
			
			switch (setting.locate)
			{
			case Location.Up: 
				emitter.orient = Emitter.Orientation.Horizontal;
				emitter.locate = Emitter.Location.Positive;
				break;
			case Location.Down: 
				emitter.orient = Emitter.Orientation.Horizontal;
				emitter.locate = Emitter.Location.Negative;
				break;
			case Location.Left: 
				emitter.orient = Emitter.Orientation.Vertical;
				emitter.locate = Emitter.Location.Negative;
				break;
			case Location.Right: 
				emitter.orient = Emitter.Orientation.Vertical;
				emitter.locate = Emitter.Location.Positive;
				break;
			}
		}
	}

//	private void SetPipelineHUD ()
//	{
//		if (!GameObject.Find("Canvas")) return;
//
//		PipelineHUD hud = GameObject.Find("Canvas")
//									.GetComponent<ShowPanels>()
//									.hudPanel.transform
//									.GetComponentInChildren<PipelineHUD>();
//		if (hud == null) return;
//
//		if (amountPipes.normal > 0 || amountPipes.mirror > 0)
//		{
//			hud.pipeNormal.amount = amountPipes.normal;
//			hud.pipeMirror.amount = amountPipes.mirror;
//			hud.gameObject.SetActive(true);
//		}
//		else
//		{
//			hud.gameObject.SetActive(false);
//		}
//	}
}
