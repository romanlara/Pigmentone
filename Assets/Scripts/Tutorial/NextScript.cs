// -----------------------------------------------------
//                      Pigmentone
//                          by 
//                 Sliverbroom Studios
//       Copyright 2015 Roman Lara & Humberto Lara
// -----------------------------------------------------

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class NextScript : MonoBehaviour 
{
	public Text text;
	public TutorialManager manager;
	public float delay = 1f;
	[HideInInspector] public bool levelExceeded;
	public Receiver[] receivers;

	public void Start ()
	{
		text.text = "Next Rule";
		levelExceeded = false;
	}

	public void Update () 
	{
		if (manager.index == manager.tutorials.Length - 1)
		{
			if (Truth(levelExceeded, receivers.Length - 1, receivers))
			{
				levelExceeded = true;
				text.text = "Completed";
			}
		}
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
}
