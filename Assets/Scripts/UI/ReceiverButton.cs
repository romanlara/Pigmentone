// -----------------------------------------------------
//                      Pigmentone
//                          by 
//                 Sliverbroom Studios
//       Copyright 2015 Roman Lara & Humberto Lara
// -----------------------------------------------------

using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class ReceiverButton : MonoBehaviour 
{
	[System.Serializable] public class ReceiverClickedEvent : UnityEvent {}

	public enum ButtonType
	{
		Level,
		Option,
		Quit
	}

	public ButtonType button = ButtonType.Level;
	public AudioClip soundClip;

	private ShowPanels m_ShowPanels;
	private AudioSource m_SoundFx;
	private QuitApplication m_Quit;
	private Animator m_Anim;
	
	[SerializeField] private ReceiverClickedEvent m_OnClick = new ReceiverClickedEvent();

	public ReceiverClickedEvent onClick
	{
		get { return m_OnClick; }
		set { m_OnClick = value; }
	}
	
	public void Start () 
	{
		m_ShowPanels = GameObject.Find("Canvas").GetComponent<ShowPanels>();
		m_Quit = GameObject.Find("Canvas").GetComponent<QuitApplication>();
		m_SoundFx = GameObject.Find("Canvas/SoundFx").GetComponent<AudioSource>();
		m_Anim = GetComponent<Animator>();

		onClick.RemoveAllListeners();

		switch (button)
		{
		case ButtonType.Level:
			onClick.AddListener(() => {
				m_SoundFx.clip = soundClip;
				m_SoundFx.Play();
				m_ShowPanels.HideMenu();
				m_ShowPanels.ShowLevelsPanel();
			});
			break;
		case ButtonType.Option:
			onClick.AddListener(() => {
				m_SoundFx.clip = soundClip;
				m_SoundFx.Play();
				m_ShowPanels.ShowOptionsPanel();
				m_ShowPanels.HideMenu();
			});
			break;
		case ButtonType.Quit:
			onClick.AddListener(() => {
				m_SoundFx.clip = soundClip;
				m_SoundFx.Play();
				m_Quit.Quit();
			});
			break;
		}

		m_Anim.SetTrigger("Hide");
	}

	public void OnMouseEnter () 
	{
		m_Anim.SetTrigger("Highlight");
	}

	public void OnMouseExit ()
	{
		m_Anim.SetTrigger("Normal");
	}

	public void OnMouseDown ()
	{
		m_Anim.SetTrigger("Pressed");
		m_OnClick.Invoke();
	}
}
