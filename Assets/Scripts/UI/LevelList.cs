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
using System.Collections.Generic;

[ExecuteInEditMode]

public class LevelList : Selectable 
{
	[System.Serializable] public class LevelListValueChangedEvent : UnityEvent {}
	
	public Graphic arrowLeft;
	public Graphic arrowRight;
	public Graphic scrollRect;
	public RectTransform content;
	public RectTransform pagination;
	public GameObject puzzleGroupPrefab;
	public GameObject puzzleButtonPrefab;
	[Space(8)]
	public bool interactivity = false;
	public float damping = 8;
	public int totalLevels;
	public int totalPuzzlesByLevels;
	public GameObject[] levels;
	[Space(8)]
	// Event delegates triggered on value changed.
	[SerializeField] private LevelListValueChangedEvent m_OnValueChanged = new LevelListValueChangedEvent();
	
	private Transform[] m_Points;
	private int m_Counter;
	private float m_Displacement;
	
	protected LevelList ()
	{}
	
	public int index
	{
		get { return m_Counter; }
		set { 
			m_Counter = value; 
			Displace(); 
		}
	}
	
	public GameObject currentLevel
	{
		get { return levels[index]; }
	}

	protected override void Awake ()
	{
		base.Awake();

		m_Points = new Transform[pagination.childCount];

		for (int i = 0; i < pagination.childCount; i++)
			m_Points[i] = pagination.GetChild(i);
		
//		if (Application.isPlaying)
//			StartCoroutine(Process());
	}
	
	protected override void Start () 
	{
		base.Start();

		if (Application.isPlaying)
		{
			m_Counter = -1;
			Forward();
		}
	}

	protected override void OnEnable ()
	{
		base.OnEnable();

		RectTransform transContent = content.GetComponent<RectTransform>();
		transContent.position = new Vector3(981.5809f, transContent.position.y, transContent.position.z);
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
			
			if (levels != null)
			{
				if (m_Counter == levels.Length - 1 && arrowRight)
					arrowRight.GetComponent<Selectable>().interactable = false;
				else if (arrowRight)
					arrowRight.GetComponent<Selectable>().interactable = true;
			}
		}
		
		if (content)
		{
			Vector3 pos = content.localPosition;
			pos.x = Mathf.Lerp(pos.x, -m_Displacement, Time.deltaTime * damping);
			content.localPosition = pos;
		}
	}

	public void Backward ()
	{
		if (!IsActive() || !IsInteractable())
			return;
		
		if (m_Counter > 0)
		{
			--m_Counter;

			Displace();
			Pagination();

			m_OnValueChanged.Invoke();
		}
	}
	
	public void Forward ()
	{
		if (!IsActive() || !IsInteractable())
			return;
		
		if (m_Counter < levels.Length - 1)
		{
			++m_Counter;

			Displace();
			Pagination();

			m_OnValueChanged.Invoke();
		}
	}

	private void Displace ()
	{
		float top = content.GetComponent<HorizontalLayoutGroup>().padding.top;
		float spacing = content.GetComponent<HorizontalLayoutGroup>().spacing;
		float width = scrollRect.GetComponent<RectTransform>().sizeDelta.x;
		
		m_Displacement = ((width + spacing) * m_Counter) + ((width * .5f) + top);
	}

	private void Pagination ()
	{
		foreach (Transform p in m_Points)
			p.gameObject.SetActive(false);

		for (int i = 0; i < index + 1; i++)
			m_Points[i].gameObject.SetActive(true);
	}

//	private IEnumerator Process ()
//	{
//		foreach (Transform child in content.transform)
//			DestroyImmediate(child.gameObject);
//
//		List<GameObject> list = new List<GameObject>();
//
//		for (int i = 0; i < totalLevels; i++)
//		{
//			GameObject goPuzzleGroup = (GameObject) Instantiate(puzzleGroupPrefab);
//			goPuzzleGroup.name = "PuzzleGroup " + i;
//			goPuzzleGroup.transform.SetParent(content);
//			goPuzzleGroup.SetActive(true);
//			goPuzzleGroup.transform.localScale = Vector3.one;
//
//			for (int j = 0; j < totalPuzzlesByLevels; j++)
//			{
//				GameObject goPuzzle = (GameObject) Instantiate(puzzleButtonPrefab);
//				goPuzzle.name = "Puzzle " + (i * 10 + j + 1);
//				goPuzzle.transform.SetParent(goPuzzleGroup.transform);
//				goPuzzle.transform.localScale = Vector3.one;
//
//				PuzzleButton puzzleButton = goPuzzle.GetComponent<PuzzleButton>();
//				puzzleButton.numLevel = PuzzleCache.current.puzzles[i * 10 + j].num;
//
//				goPuzzle.SetActive(true);
//			}
//
//			list.Add(goPuzzleGroup);
//		}
//
//		levels = new GameObject[list.Count];
//		list.CopyTo(levels);
//
//		yield return null;
//	}
}
