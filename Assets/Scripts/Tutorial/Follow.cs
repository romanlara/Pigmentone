// -----------------------------------------------------
//                      Pigmentone
//                          by 
//                 Sliverbroom Studios
//       Copyright 2015 Roman Lara & Humberto Lara
// -----------------------------------------------------

using UnityEngine;
using System.Collections;

public class Follow : MonoBehaviour 
{
	public Transform target;
	public Vector3 offset;

	public void Update () 
	{
		Vector3 npos = target.position + offset;
		npos.z = -1f;
		transform.position = npos;
	}
}
