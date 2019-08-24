// -----------------------------------------------------
//                      Pigmentone
//                          by 
//                 Sliverbroom Studios
//       Copyright 2015 Roman Lara & Humberto Lara
// -----------------------------------------------------

using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class EventSystemChecker : MonoBehaviour
{
	// OnLevelWasLoaded is called after a new scene has finished loading
	public void OnLevelWasLoaded ()
	{
		// If there is no EventSystem (needed for UI interactivity) present
		if(!FindObjectOfType<EventSystem>())
		{
			//T he following code instantiates a new object called EventSystem
			GameObject obj = new GameObject("EventSystem");

			// And adds the required components
			obj.AddComponent<EventSystem>();
			obj.AddComponent<StandaloneInputModule>().forceModuleActive = true;
			obj.AddComponent<TouchInputModule>();
		}
	}
}
