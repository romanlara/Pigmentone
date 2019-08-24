// -----------------------------------------------------
//                      Pigmentone
//                          by 
//                 Sliverbroom Studios
//       Copyright 2015 Roman Lara & Humberto Lara
// -----------------------------------------------------

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PipelineHUD : MonoBehaviour 
{
	[System.Serializable]
	public class PipeHUD
	{
		public Text text;
		public int amount = 0;
	}

	public GameObject pipelinePrefab;
	public PipeHUD pipeNormal = new PipeHUD();
	public PipeHUD pipeMirror = new PipeHUD();

	public void Update () 
	{
		pipeNormal.text.text = pipeNormal.amount.ToString();
		pipeMirror.text.text = pipeMirror.amount.ToString();
	}

	public void CreatePipelineNormal ()
	{
		if (Pause.current && Pause.current.IsPaused) return;
		if (pipeNormal.amount == 0) return;

		GameObject go = Instantiate(pipelinePrefab);
		Pipeline pipe = go.GetComponent<Pipeline>();
		pipe.mirror = false;
		pipe.hud = this;
		go.SetActive(true);

		--pipeNormal.amount;
	}

	public void CreatePipelineMirror ()
	{
		if (Pause.current && Pause.current.IsPaused) return;
		if (pipeMirror.amount == 0) return;
		
		GameObject go = Instantiate(pipelinePrefab);
		Pipeline pipe = go.GetComponent<Pipeline>();
		pipe.mirror = true;
		pipe.hud = this;
		go.SetActive(true);

		--pipeMirror.amount;
	}
}
