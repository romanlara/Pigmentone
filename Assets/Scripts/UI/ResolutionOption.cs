// -----------------------------------------------------
//                      Pigmentone
//                          by 
//                 Sliverbroom Studios
//       Copyright 2015 Roman Lara & Humberto Lara
// -----------------------------------------------------

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections;

[ExecuteInEditMode]

public class ResolutionOption : Selectable 
{
	[System.Serializable] public class ResolutionOptionValueChangedEvent : UnityEvent {}

	public Graphic arrowLeft;
	public Graphic arrowRight;
	public Graphic label;
	[Space(8)]
	public bool interactivity = false;
	public string[] resolutionsList;
	[Space(8)]
	// Event delegates triggered on value changed.
	[SerializeField] private ResolutionOptionValueChangedEvent m_OnValueChanged = new ResolutionOptionValueChangedEvent();

	private int m_Counter;
	private Text m_Label;

	protected ResolutionOption ()
	{}
	
	public int index
	{
		get { return m_Counter; }
		set { m_Counter = value; }
	}
	
	public string currentElem
	{
		get { return resolutionsList[index]; }
	}

	public int width
	{
		get {
			int separator = resolutionsList[index].Trim().LastIndexOf('x');
			string width = resolutionsList[index].Substring(0, separator);

			return int.Parse(width);
		}
	}

	public int height
	{
		get {
			int separator = resolutionsList[index].Trim().LastIndexOf('x');
			string height = resolutionsList[index].Substring(separator + 1);

			return int.Parse(height);
		}
	}
	
	public ResolutionOptionValueChangedEvent onValueChanged
	{
		get { return m_OnValueChanged; }
		set { m_OnValueChanged = value; }
	}

	protected override void Awake ()
	{
		base.Awake ();

		m_Label = label.GetComponent<Text>();
	}
	
	protected override void Start () 
	{
		base.Start();

		if (resolutionsList.Length > 0)
		{
			if (label != null)
				m_Label.text = resolutionsList[m_Counter];
			
			m_OnValueChanged.Invoke();
		}
	}

	public void Update () 
	{
		Selectable[] guis = GetComponentsInChildren<Selectable>();
		foreach (Selectable sel in guis)
			sel.interactable = interactable;
		
		if (interactable && interactivity)
		{
			if (!IsActive() || !IsInteractable())
				return;
			
			if (m_Counter == 0 && arrowLeft)
				arrowLeft.GetComponent<Selectable>().interactable = false;
			else if (arrowLeft)
				arrowLeft.GetComponent<Selectable>().interactable = true;
			
			if (resolutionsList != null)
			{
				if (m_Counter == resolutionsList.Length - 1 && arrowRight)
					arrowRight.GetComponent<Selectable>().interactable = false;
				else if (arrowRight)
					arrowRight.GetComponent<Selectable>().interactable = true;
			}
		}

		m_Label.text = resolutionsList[index];
	}

	public void Backward ()
	{
		if (!IsActive() || !IsInteractable())
			return;
		
		if (m_Counter > 0)
		{
			--m_Counter;
			m_OnValueChanged.Invoke();
		}
	}
	
	public void Forward ()
	{
		if (!IsActive() || !IsInteractable())
			return;
		
		if (m_Counter < resolutionsList.Length - 1)
		{
			++m_Counter;
			m_OnValueChanged.Invoke();
		}
	}
}
