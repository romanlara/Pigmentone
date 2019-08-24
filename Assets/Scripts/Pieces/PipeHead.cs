// -----------------------------------------------------
//                      Pigmentone
//                          by 
//                 Sliverbroom Studios
//       Copyright 2015 Roman Lara & Humberto Lara
// -----------------------------------------------------

using UnityEngine;
using System.Collections;
using ParticlePlayground;

public class PipeHead : MonoBehaviour 
{
	public enum PipeType
	{
		Entry,
		Exit
	}

	public PipeType type = PipeType.Entry;
	public PipeHead otherPipeHead;
	public SpriteRenderer theSprite;
	public float laserDistance = 25f;
	public LayerMask WhatIsReceiver;					// Layer to send the color.
	public Color colorToEmit;
	[HideInInspector] public bool emit;
	[HideInInspector] public bool mirror;
	public bool onActivated;

	private Transform m_Raycast;						// Reference of the ray.
	private LineRenderer m_LineRenderer;				// Reference of line renderer.
	private PlaygroundParticlesC m_Particles;			// Reference of particles.
	private Transform m_ManipulatorTransform;			// Reference to relocate the particles manipulator.
	private ManipulatorObjectC m_ManipulatorObstruction;// Reference to modify the obstruction manipulator.
	private ManipulatorObjectC m_ManipulatorColor;		// Reference to modify the color manipulator.
	
	private float m_Multiplier;
//	private bool m_OnActivated;

	public bool Entry
	{
		get { return onActivated;/*m_OnActivated;*/ }
		set { 
//			if (value &&/* !m_OnActivated && */!onActivated)
//				onActivated =/* m_OnActivated = */value;
//			else if (!value &&/* m_OnActivated && */onActivated)
//				/*m_OnActivated */onActivated = value;
			onActivated = value;
		}
	}

	public void Awake ()
	{
		switch (type)
		{
		case PipeType.Entry: 
//			m_Particles = GetComponentInChildren<PlaygroundParticlesC>();
			theSprite.color = new Color(0, 0, 0, 0);
			break;
		case PipeType.Exit:
			m_Raycast = transform.FindChild("Raycast");
			m_LineRenderer = m_Raycast.GetComponentInChildren<LineRenderer>();
			m_Particles = GetComponentInChildren<PlaygroundParticlesC>();
			m_ManipulatorTransform = m_Particles.transform.FindChild("Manipulator");
			
			m_ManipulatorObstruction = PlaygroundC.GetManipulator(0, m_Particles);
			m_ManipulatorColor = PlaygroundC.GetManipulator(1, m_Particles);
			break;
		}
	}

	public void Start () 
	{
		switch (type)
		{
		case PipeType.Entry: 
//			if (mirror)
//			{
//				Vector3 angles =  m_Particles.transform.eulerAngles;
//				angles.z = -90;
//				m_Particles.transform.eulerAngles = angles;
//			}
//			else
//			{
//				Vector3 angles =  m_Particles.transform.eulerAngles;
//				angles.z = 90;
//				m_Particles.transform.eulerAngles = angles;
//			}
			break;
		case PipeType.Exit:
			EnableManipulator(false);

//			if (amount)
//			{
//				m_Multiplier = m_Particles.particleCount / laserDistance;
//				m_Particles.overflowOffset.y = laserDistance / m_Particles.particleCount;
//			}
			break;
		}
	}

	public void Update () 
	{
		if (Pause.current && Pause.current.IsPaused) return;

		switch (type)
		{
		case PipeType.Entry: EntryBehaviour(); break;
		case PipeType.Exit: ExitBehaviour(); break;
		}
	}

	public void EntryBehaviour ()
	{
		otherPipeHead.colorToEmit = colorToEmit;
//		m_Particles.lifetimeColor = GetGradientEntry(colorToEmit);

//		if (onActivated)
//		{
//			m_Particles.emit = true;
//			theSprite.color = Color.Lerp(theSprite.color, Color.white, 0.5f);
//		}
//		else
//		{
//			m_Particles.emit = false;
//			theSprite.color = Color.Lerp(theSprite.color, new Color(0, 0, 0, 0), 0.5f);
//		}

//		Entry = false;
	}

	public void ExitBehaviour ()
	{
		m_Particles.lifetimeColor = GetGradientExit(colorToEmit);
		
		if (otherPipeHead.onActivated)
		{
			theSprite.color = Color.Lerp(theSprite.color, Color.white, 0.5f);

			m_LineRenderer.SetColors(colorToEmit, colorToEmit);
			m_Particles.emit = true;
		}
		else
		{
			Color nothing = new Color(0, 0, 0, 0);
//			theSprite.color = Color.Lerp(theSprite.color, nothing, 0.5f);

			m_LineRenderer.SetColors(nothing, nothing);
			m_Particles.emit = false;
		}

		// target - origin
		Vector2 dir = m_Raycast.position - transform.position;
		RaycastHit2D hit = Physics2D.Raycast(m_Raycast.position, dir, laserDistance, WhatIsReceiver);
		if (hit)
		{
//			if (amount)
//				m_Particles.particleCount = Mathf.FloorToInt(Vector3.Distance(transform.position, hit.point) * m_Multiplier);
//			else
			// Set overflow offset y to hit distance (divide by particle count which by default is 1000)
			m_Particles.overflowOffset.y = Vector3.Distance(transform.position, hit.point) / m_Particles.particleCount;
			m_LineRenderer.SetPosition(1, new Vector3(0, hit.distance, 0));
			
			if (otherPipeHead.onActivated/* && emit*/)
			{
				if (hit.collider.tag == "Receiver")
				{
					Receiver cacheReceiver = hit.collider.GetComponent<Receiver>();
					cacheReceiver.AddColor(colorToEmit);
					
					UpdateManipulator(cacheReceiver.transform.position, cacheReceiver.currentColor);
					EnableManipulator(true);
				}
				else if (hit.collider.tag == "Obstacle")
				{
					Transform cacheObstacle = hit.collider.transform;
					UpdateManipulator(cacheObstacle.position, colorToEmit);
					EnableManipulator(true);
				}
				else if (hit.collider.tag == "Pipeline")
				{
					PipeHead cachePipeHead = hit.collider.GetComponent<PipeHead>();
					cachePipeHead.colorToEmit = colorToEmit;
					cachePipeHead.Entry = true;
					
					UpdateManipulator(cachePipeHead.transform.position, colorToEmit);
					EnableManipulator(true);
				}
			}
		}
		else
		{
//			if (amount) 
//				m_Particles.particleCount = Mathf.FloorToInt(laserDistance * m_Multiplier);
//			else
			// Render laser to laserMaxDistance on clear sight
			m_Particles.overflowOffset.y = laserDistance / m_Particles.particleCount;
			m_LineRenderer.SetPosition(1, new Vector3(0, laserDistance, 0));
			EnableManipulator(false);
		}
		Debug.DrawRay(m_Raycast.position, dir, Color.magenta);

		otherPipeHead.Entry = false;
	}

	public Gradient GetGradientEntry (Color c)
	{
		Gradient g = new Gradient();
		GradientColorKey[] gck = new GradientColorKey[2];
		GradientAlphaKey[] gak = new GradientAlphaKey[3];
		
		gck[0].color = c;
		gck[0].time = 0.0f;
		gck[1].color = c;
		gck[1].time = 1.0f;
		
		gak[0].alpha = 0.0f;
		gak[0].time = 0.0f;
		gak[0].alpha = 1.0f;
		gak[0].time = 0.5f;
		gak[2].alpha = 0.0f;
		gak[2].time = 1.0f;
		
		g.SetKeys(gck, gak);
		
		return g;
	}

	public Gradient GetGradientExit (Color c)
	{
		Gradient g = new Gradient();
		GradientColorKey[] gck = new GradientColorKey[2];
		GradientAlphaKey[] gak = new GradientAlphaKey[2];
		
		gck[0].color = c;
		gck[0].time = 0.0f;
		gck[1].color = c;
		gck[1].time = 1.0f;
		
		gak[0].alpha = 1.0f;
		gak[0].time = 0.0f;
		gak[1].alpha = 0.0f;
		gak[1].time = 1.0f;
		
		g.SetKeys(gck, gak);
		
		return g;
	}

	private void UpdateManipulator (Vector3 position, Color color)
	{
		m_ManipulatorTransform.position = position;
		m_ManipulatorColor.property.lifetimeColor = GetGradientExit(color);
	}

	private void EnableManipulator (bool b)
	{
		m_ManipulatorObstruction.enabled = b;
		m_ManipulatorColor.enabled = b;
	}
}
