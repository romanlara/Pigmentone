// -----------------------------------------------------
//                      Pigmentone
//                          by 
//                 Sliverbroom Studios
//       Copyright 2015 Roman Lara & Humberto Lara
// -----------------------------------------------------

using UnityEngine;
using System.Collections;

public class Line : MonoBehaviour 
{
	public Transform target;
	public Vector3 offset;

	private LineRenderer m_LineRenderer;				// Reference of line renderer.

	public void Start () 
	{
		m_LineRenderer = GetComponent<LineRenderer>();
	}

	public void Update () 
	{
		m_LineRenderer.SetPosition(0, transform.position);
		m_LineRenderer.SetPosition(1, target.position + offset);
	}
}
