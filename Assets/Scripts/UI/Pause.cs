// -----------------------------------------------------
//                      Pigmentone
//                          by 
//                 Sliverbroom Studios
//       Copyright 2015 Roman Lara & Humberto Lara
// -----------------------------------------------------

using UnityEngine;
using System.Collections;

public class Pause : MonoBehaviour 
{
	public static Pause current;

	private ShowPanels m_ShowPanels;					// Reference to the ShowPanels script used to hide and show UI panels
	private StartOptions m_StartScript;					// Reference to the StartButton script
	private bool m_IsPaused;							// Boolean to check if the game is paused or not
	private bool m_IsCogratulating;						// Boolean to check if the game is congratulating or not

	public bool IsPaused
	{
		get { return m_IsPaused; }
	}

	public bool IsCongratulating
	{
		get { return m_IsCogratulating; }
	}
	
	// Awake is called before Start()
	public void Awake ()
	{
		if (current == null)
			current = this;
		else if (current != null)
			Destroy(gameObject);

		// Get a component reference to ShowPanels attached to this object, store in showPanels variable
		m_ShowPanels = GetComponent<ShowPanels>();
		// Get a component reference to StartButton attached to this object, store in startScript variable
		m_StartScript = GetComponent<StartOptions>();
	}

	// Update is called once per frame
	public void Update () 
	{
		// Check if the Cancel button in Input Manager is down this frame (default is Escape key) and that game is not paused, and that we're not in main menu
		if (Input.GetButtonDown("Cancel") && !m_IsPaused && !m_IsCogratulating && !m_StartScript.inMainMenu) 
		{
			// Call the DoPause function to pause the game
			DoPause();
		} 
		// If the button is pressed and the game is paused and not in main menu
		else if (Input.GetButtonDown("Cancel") && m_IsPaused && !m_IsCogratulating && !m_StartScript.inMainMenu) 
		{
			// Call the UnPause function to unpause the game
			UnPause();
		}
	}

	public void DoPause ()
	{
		// Set isPaused to true
		m_IsPaused = true;
		// Set time.timescale to 0, this will cause animations and physics to stop updating
		Time.timeScale = 0;
		// call the ShowPausePanel function of the ShowPanels script
		m_ShowPanels.ShowPausePanel();
	}
	
	public void UnPause ()
	{
		// Set isPaused to false
		m_IsPaused = false;
		// Set time.timescale to 1, this will cause animations and physics to continue updating at regular speed
		Time.timeScale = 1;
		// call the HidePausePanel function of the ShowPanels script
		m_ShowPanels.HidePausePanel();
	}

	public void DoCongratulation ()
	{
		// Set isCongratulating to true
		m_IsCogratulating = true;
		// Start congratulations
		m_StartScript.StartCongratulate();
		// call the ShowCongratulationsPanel function of the ShowPanels script
//		m_ShowPanels.ShowCongratulationsPanel();
	}

	public void UnCongratulation ()
	{
		// Set isCongratulating to false
		m_IsCogratulating = false;
		// Set isPaused to false
//		m_IsPaused = false;
		// Set time.timescale to 1, this will cause animations and physics to continue updating at regular speed
//		Time.timeScale = 1;
		// call the HideCongratulationsPanel function of the ShowPanels script
		m_ShowPanels.HideCongratulationsPanel();
	}
}
