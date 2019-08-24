// -----------------------------------------------------
//                      Pigmentone
//                          by 
//                 Sliverbroom Studios
//       Copyright 2015 Roman Lara & Humberto Lara
// -----------------------------------------------------

using UnityEngine;
using System.Collections;

public class ShowPanels : MonoBehaviour 
{
	public GameObject optionsPanel;							// Store a reference to the Game Object OptionsPanel 
	public GameObject thanksPanel;							// Store a reference to the Game Object ThanksPanel 
//	public GameObject optionsTint;							// Store a reference to the Game Object OptionsTint 
//	public GameObject menuPanel;							// Store a reference to the Game Object MenuPanel 
	public GameObject creditsPanel;							// Store a reference to the Game Object CreditsPanel 
	public GameObject title;								// Store a reference to the Game Object MainMenu  
	public GameObject pausePanel;							// Store a reference to the Game Object PausePanel 
	public GameObject loadingPanel;							// Store a reference to the Game Object LoadingPanel 
	public GameObject hudPanel;								// Store a reference to the Game Object HUDPanel
	public GameObject levelsPanel;							// Store a reference to the Game Object LevelsPanel
	public GameObject congratPanel;							// Store a reference to the Game Object CongratulationsPanel
	public GameObject congratCamera;						// Store a reference to the Game Object CongratulationsCamera
	public ParticleSystem congratParticle;
	public GameObject versionText;

	// Call this function to activate and display the Options panel during the main menu
	public void ShowOptionsPanel ()
	{
		optionsPanel.SetActive(true);
//		optionsTint.SetActive(true);
	}

	// Call this function to deactivate and hide the Options panel during the main menu
	public void HideOptionsPanel ()
	{
		optionsPanel.SetActive(false);
//		optionsTint.SetActive(false);
	}

	// Call this function to activate and display the Thanks panel during the main menu
	public void ShowThanksPanel ()
	{
		thanksPanel.SetActive(true);
	}

	// Call this function to deactivate and hide the Thanks panel during the main menu
	public void HideThanksPanel ()
	{
		thanksPanel.SetActive(false);
	}

	// Call this function to activate and display the Credits panel during the main menu
	public void ShowCreditsPanel ()
	{
		creditsPanel.SetActive(true);
	}

	// Call this function to deactivate and hide the Credits panel during the main menu
	public void HideCreditsPanel ()
	{
		creditsPanel.SetActive(false);
	}

	// Call this function to activate and display the main menu panel during the main menu
	public void ShowMenu()
	{
		if (title == null)
			title = GameObject.Find("TitleObject");

//		menuPanel.SetActive(true);
		if (title != null) 
			title.SetActive(true);
	}

	// Call this function to deactivate and hide the main menu panel during the main menu
	public void HideMenu ()
	{
		if (title == null)
			title = GameObject.Find("TitleObject");

//		menuPanel.SetActive(false);
		if (title != null) 
			title.SetActive(false);
	}
	
	// Call this function to activate and display the Pause panel during game play
	public void ShowPausePanel ()
	{
		pausePanel.SetActive(true);
//		optionsTint.SetActive(true);
	}

	// Call this function to deactivate and hide the Pause panel during game play
	public void HidePausePanel ()
	{
		pausePanel.SetActive(false);
//		optionsTint.SetActive(false);
	}

	// Call this function to activate and display the HUD panel during game play
	public void ShowHudPanel ()
	{
		hudPanel.SetActive(true);
	}

	// Call this function to deactivate and hide the HUD panel during game play
	public void HideHudPanel ()
	{
		hudPanel.SetActive(false);
	}

	// Call this function to activate and display the Levels panel during main menu
	public void ShowLevelsPanel ()
	{
		levelsPanel.SetActive(true);
	}
	
	// Call this function to deactivate and hide the Levels panel during main menu
	public void HideLevelsPanel ()
	{
		levelsPanel.SetActive(false);
	}

	// Call this function to activate and display the Congratulations panel during main menu
	public void ShowCongratulationsPanel ()
	{
		congratPanel.SetActive(true);
		congratCamera.GetComponent<Camera>().enabled = true;
		congratParticle.Play(true);
		HideHudPanel();
	}

	// Call this function to deactivate and hide the Congratulations panel during main menu
	public void HideCongratulationsPanel ()
	{
		congratPanel.SetActive(false);
		congratCamera.GetComponent<Camera>().enabled = false;
		congratParticle.Play(false);
		ShowHudPanel();
	}

	// Call this function to activate and display the Loading panel during main menu
	public void ShowLoadingPanel ()
	{
		loadingPanel.SetActive(true);
	}

	// Call this function to deactivate and hide the Loading panel during main menu
	public void HideLoadingPanel ()
	{
		loadingPanel.SetActive(false);
	}

	public void ShowVersion ()
	{
		versionText.SetActive(true);
	}

	public void HideVersion ()
	{
		versionText.SetActive(false);
	}
}
