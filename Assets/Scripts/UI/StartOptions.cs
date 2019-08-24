// -----------------------------------------------------
//                      Pigmentone
//                          by 
//                 Sliverbroom Studios
//       Copyright 2015 Roman Lara & Humberto Lara
// -----------------------------------------------------

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using System.Collections;

public class StartOptions : MonoBehaviour 
{
	public string sceneToGo = "Level 01";							// Index number in build settings of scene to load if changeScenes is true
	public bool changeScenes;											// If true, load a new scene when Start is pressed, if false, fade out UI and continue in single scene
	public bool changeMusicOnStart;										// Choose whether to continue playing menu music or start a new music clip
//	public int musicToChangeTo = 0;										// Array index in array MusicClips to change to if changeMusicOnStart is true.

	[HideInInspector] public bool inMainMenu = true;					// If true, pause button disabled in main menu (Cancel in input manager, default escape key)
	[HideInInspector] public Animator animColorFade; 					// Reference to animator which will fade to and from black when starting game.
	[HideInInspector] public Animator animMenuAlpha;					// Reference to animator that will fade out alpha of MenuPanel canvas group
	[HideInInspector] public AnimationClip fadeColorAnimationClip;		// Animation clip fading to color (black default) when changing scenes
	[HideInInspector] public AnimationClip fadeAlphaAnimationClip;		// Animation clip fading out UI elements alpha
	[HideInInspector] public GameTimer gameTimer;
	[HideInInspector] public HintScript hintScript;
	[HideInInspector] public Scrollbar progressbar;
	[HideInInspector] public ReceiverProgress receiverProgress;
	
	private PlayMusic playMusic;										// Reference to PlayMusic script
	private float fastFadeIn = .01f;									// Very short fade time (10 milliseconds) to start playing music immediately without a click/glitch
	private ShowPanels showPanels;										// Reference to ShowPanels script on UI GameObject, to show and hide panels
	private int m_levelToPlay;
	private bool m_bThanksPanel;

//	public float TriggerFadeColorAnim
//	{
//		get {
//			animColorFade.SetTrigger("fade");
//			return fadeColorAnimationClip.length;
//		}
//	}

	public void Awake ()
	{
		// Get a reference to ShowPanels attached to UI object
		showPanels = GetComponent<ShowPanels>();
		// Get a reference to PlayMusic attached to UI object
		playMusic = GetComponent<PlayMusic>();
	}

	public void MenuButtonClicked ()
	{
		sceneToGo = "Menu";

		// Pause button now works if escape is pressed since we are no longer in Main menu.
		inMainMenu = true;
		
		// If changeMusicOnStart is true, fade out volume of music group of AudioMixer by calling FadeDown function of PlayMusic, using length of fadeColorAnimationClip as time. 
		// To change fade time, change length of animation "FadeToColor"
		if (changeMusicOnStart) 
		{
			playMusic.FadeDown(fadeColorAnimationClip.length);
			Invoke("PlayMenuMusic", fadeAlphaAnimationClip.length);
		}
		
		// If changeScenes is true, start fading and change scenes halfway through animation when screen is blocked by FadeImage
		if (changeScenes) 
		{
			// Use invoke to delay calling of LoadDelayed by half the length of fadeColorAnimationClip
			Invoke("LoadDelayedToMenu", fadeColorAnimationClip.length * .5f);
			// Set the trigger of Animator animColorFade to start transition to the FadeToOpaque state.
			animColorFade.SetTrigger("fadeIn");
		} 
		// If changeScenes is false, call StartGameInScene
		else 
		{
			// Call the StartGameInScene function to start game without loading a new scene.
			StartGameInScene();
		}
	}

	public void GoThanks ()
	{
		sceneToGo = "Menu";
		
		// Pause button now works if escape is pressed since we are no longer in Main menu.
		inMainMenu = true;
		
		// If changeMusicOnStart is true, fade out volume of music group of AudioMixer by calling FadeDown function of PlayMusic, using length of fadeColorAnimationClip as time. 
		// To change fade time, change length of animation "FadeToColor"
		if (changeMusicOnStart) 
		{
			playMusic.FadeDown(fadeColorAnimationClip.length);
			Invoke("PlayMenuMusic", fadeAlphaAnimationClip.length);
		}
		
		// If changeScenes is true, start fading and change scenes halfway through animation when screen is blocked by FadeImage
		if (changeScenes) 
		{
			// Use invoke to delay calling of LoadDelayed by half the length of fadeColorAnimationClip
			Invoke("LoadDelayedToThanks", fadeColorAnimationClip.length * .5f);
			// Set the trigger of Animator animColorFade to start transition to the FadeToOpaque state.
			animColorFade.SetTrigger("fadeIn");
		} 
		// If changeScenes is false, call StartGameInScene
		else 
		{
			// Call the StartGameInScene function to start game without loading a new scene.
			StartGameInScene();
		}
	}

	public void GoLevel (int level)
	{
		m_levelToPlay = level;
		sceneToGo = "Level " + ((level < 10) ? "0" + level.ToString() : level.ToString());
		
//		PuzzleManager.puzzleToPlay = puzzle;

		// Pause button now works if escape is pressed since we are no longer in Main menu.
		inMainMenu = false;
		
		// If changeMusicOnStart is true, fade out volume of music group of AudioMixer by calling FadeDown function of PlayMusic, using length of fadeColorAnimationClip as time. 
		// To change fade time, change length of animation "FadeToColor"
		if (changeMusicOnStart) 
		{
			playMusic.FadeDown(fadeColorAnimationClip.length);
			Invoke("PlayNewMusic", fadeAlphaAnimationClip.length);
		}
		
		// If changeScenes is true, start fading and change scenes halfway through animation when screen is blocked by FadeImage
		if (changeScenes) 
		{
			// Use invoke to delay calling of LoadDelayed by half the length of fadeColorAnimationClip
			Invoke("LoadDelayedGame", fadeColorAnimationClip.length * .5f);
			// Set the trigger of Animator animColorFade to start transition to the FadeToOpaque state.
			animColorFade.SetTrigger("fadeIn");
		} 
		// If changeScenes is false, call StartGameInScene
		else 
		{
			// Call the StartGameInScene function to start game without loading a new scene.
			StartGameInScene();
		}
	}

	public void StartCongratulate ()
	{
		playMusic.FadeDown(fadeColorAnimationClip.length);
		Invoke("PlayCongratulateMusic", fadeAlphaAnimationClip.length);

		Invoke("LoadCongratulatePanel", fadeColorAnimationClip.length * .5f);
//		animColorFade.SetTrigger("fade");
	}

	private void LoadCongratulatePanel ()
	{
		showPanels.ShowCongratulationsPanel();
	}

	private void LoadDelayedToMenu ()
	{
		// Hide the Congratulations panel UI element and
		// show the HUD panel UI element
		Pause.current.UnCongratulation();
		// Hide the HUD panel UI element
		showPanels.HideHudPanel();
		// Show the main menu UI element
		showPanels.ShowMenu();
		showPanels.ShowVersion();
		m_bThanksPanel = false;
		// Load the selected scene, by scene index number in build settings
		StartCoroutine(AsyncLoading(sceneToGo));
	}

	private void LoadDelayedToThanks ()
	{
		// Hide the Congratulations panel UI element and
		// show the HUD panel UI element
		Pause.current.UnCongratulation();
		// Hide the HUD panel UI element
		showPanels.HideHudPanel();
		// Show the main menu UI element
//		showPanels.ShowMenu();
//		showPanels.ShowVersion();
		showPanels.ShowThanksPanel();
		m_bThanksPanel = true;
		// Load the selected scene, by scene index number in build settings
		StartCoroutine(AsyncLoading(sceneToGo));
	}

	private void LoadDelayedGame ()
	{
		// Hide the main menu UI element
		showPanels.HideMenu();
		showPanels.HideVersion();
		// Hide the Level panel UI element
		showPanels.HideLevelsPanel();
		m_bThanksPanel = false;
		// Hide the Congratulations panel UI element and
		// show the HUD panel UI element
		Pause.current.UnCongratulation();
		// Load the selected scene, by scene index number in build settings
		StartCoroutine(AsyncLoading(sceneToGo));
	}

	public void StartGameInScene ()
	{
		// Pause button now works if escape is pressed since we are no longer in Main menu.
		inMainMenu = false;

		// If changeMusicOnStart is true, fade out volume of music group of AudioMixer by calling FadeDown function of PlayMusic, using length of fadeColorAnimationClip as time. 
		// To change fade time, change length of animation "FadeToColor"
		if (changeMusicOnStart) 
		{
			// Wait until game has started, then play new music
			Invoke("PlayNewMusic", fadeAlphaAnimationClip.length);
		}
		// Set trigger for animator to start animation fading out Menu UI
		animMenuAlpha.SetTrigger ("fade");
		Debug.Log("Game started in same scene! Put your game starting stuff here.");
	}

	private void PlayMenuMusic ()
	{
		// Fade up music nearly instantly without a click 
		playMusic.FadeUp(fastFadeIn);
		// Play menu music clip assigned in PlayMusic script
		playMusic.PlayMenuMusic();
	}
	
	private void PlayNewMusic ()
	{
		// Fade up music nearly instantly without a click 
		playMusic.FadeUp(fastFadeIn);
		// Play music clip assigned to mainMusic in PlayMusic script
		playMusic.PlayGameMusic();
//		playMusic.PlaySelectedMusic(musicToChangeTo);
	}

	private void PlayCongratulateMusic ()
	{
		// Fade up music nearly insantly without a click
		playMusic.FadeUp(fastFadeIn);
		// Play congratulate music clip assigned in PlayMusic script
		playMusic.PlayCongratulateMusic();
	}

	private IEnumerator AsyncLoading (string level)
	{
		// ... Show Screen.
		showPanels.ShowLoadingPanel();

//		int loadProgress = 0;
		progressbar.size = 0;
		receiverProgress.UnComplete();
		
		AsyncOperation async;
		async = Application.LoadLevelAsync(level);
		
//		if (typeof(int).Equals(((int) level).GetType()))
//			async = Application.LoadLevelAsync((int) level);
//		else
//			async = Application.LoadLevelAsync((string) level);
		
		while (!async.isDone)
		{
			// ... Progress Screen.
//			loadProgress = (int) (async.progress * 100);
			progressbar.size = async.progress;

			if ((int) (async.progress * 100) > 95)
				receiverProgress.Complete();

			yield return null;
		}

		if (!inMainMenu)
		{
			PuzzleManager.puzzleToPlay = m_levelToPlay;
			gameTimer.Start();
			hintScript.Start();
//			PuzzleManager.current.PlayPuzzle(PuzzleManager.puzzleToPlay);
		}

		if (m_bThanksPanel)
		{
			showPanels.HideMenu();
		}

		// ... Hide Screen.
		showPanels.HideLoadingPanel();
		animColorFade.SetTrigger("fadeOut");
	}
}
