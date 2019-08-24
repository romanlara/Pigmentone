// -----------------------------------------------------
//                      Pigmentone
//                          by 
//                 Sliverbroom Studios
//       Copyright 2015 Roman Lara & Humberto Lara
// -----------------------------------------------------

using UnityEngine;
using System.Collections;
using ParticlePlayground;
// 15-Jul-2015
public class Emitter : MonoBehaviour 
{
	public enum Orientation
	{
		Horizontal,
		Vertical
	};

	public enum Location
	{
		Negative,
		Positive
	};

	public Orientation orient = Orientation.Horizontal;	// To orient the movement.
	public Location locate = Location.Negative;			// To locate the position.
	public float offset = 0.7f;							// Space between screen's borders.
	public float speed = 20f;							// Speed of movement.
	public float laserDistance = 20f;					// Distance of ray.
	public LayerMask WhatIsReceiver;					// Layer to send the color.
	public Color colorToEmit;							// Color to emit.
	[HideInInspector] public float iniOffset = 0f;

	private SpriteRenderer m_Sprite;					// Reference of the sprite.
	private LineRenderer m_LineRenderer;				// Reference of line renderer.
	private PlaygroundParticlesC m_Particles;			// Reference of particles.
	private Transform m_ManipulatorTransform;			// Reference to relocate the particles manipulator.
	private ManipulatorObjectC m_ManipulatorObstruction;// Reference to modify the obstruction manipulator.
	private ManipulatorObjectC m_ManipulatorColor;		// Reference to modify the color manipulator.

	private Vector2 m_Min;		// Store the screen's minimum limits.
	private Vector2 m_Max;		// Store the screen's maximum limits.
	private Vector2 m_Pos;		// Store the emitter's position.
	private bool m_Start;
//	private float m_Multiplier;

	public void Awake ()
	{
		m_Sprite = transform.GetComponentInChildren<SpriteRenderer>();
		m_LineRenderer = transform.GetComponentInChildren<LineRenderer>();
		m_Particles = transform.GetComponentInChildren<PlaygroundParticlesC>();
		m_ManipulatorTransform = m_Particles.transform.FindChild("Manipulator");

		m_ManipulatorObstruction = PlaygroundC.GetManipulator(0, m_Particles);
		m_ManipulatorColor = PlaygroundC.GetManipulator(1, m_Particles);
	}

	public void Start ()
	{
		EnableManipulator(false);
		m_Sprite.color = colorToEmit;

		// Find the screen limits to the emitter's movement.
		m_Min = Camera.main.ViewportToWorldPoint(new Vector2(0, 0));
		m_Max = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));

		m_Pos = transform.position;
		switch (orient)
		{
		case Orientation.Horizontal: 
			if (iniOffset > 0)
				m_Pos.x = (m_Max.x - offset) * iniOffset; 
			else if (iniOffset < 0)
				m_Pos.x = (m_Min.x + offset) * -iniOffset;
			break;
		case Orientation.Vertical: 
			if (iniOffset > 0)
				m_Pos.y = (m_Max.y - offset) * iniOffset; 
			else if (iniOffset < 0)
				m_Pos.y = (m_Min.y + offset) * -iniOffset;
			break;
		}
		transform.position = m_Pos;

//		if (amount)
//		{
//			m_Multiplier = m_Particles.particleCount / laserDistance;
//			m_Particles.overflowOffset.y = laserDistance / m_Particles.particleCount;
//		}
	}

	public void Update ()
	{
		if (Pause.current && Pause.current.IsPaused) return;

		m_LineRenderer.SetColors(colorToEmit, colorToEmit);
		m_Particles.lifetimeColor = GetGradient(colorToEmit);

		// Find the screen limits to the emitter's movement.
		m_Min = Camera.main.ViewportToWorldPoint(new Vector2(0, 0));
		m_Max = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));
		// Get the emitter's current position.
		m_Pos = transform.position;
		// Direction of the ray.
		Vector2 dir = Vector2.zero;
		Vector3 rot = Vector3.zero;

		// Update the location according to its orientation.
		switch (orient)
		{
		case Orientation.Horizontal:
			// Set 'Y' location when the screen changes its horizontal size.
			switch (locate)
			{
			case Location.Negative: m_Pos.y = m_Min.y + offset; dir = Vector2.up; rot = new Vector3(0, 0, 0); break;
			case Location.Positive: m_Pos.y = m_Max.y - offset; dir = Vector2.down; rot = new Vector3(0, 0, 180); break;
			}
			// Set 'X' location if is outside of the screen.
			if (m_Pos.x < m_Min.x) m_Pos.x = m_Min.x + offset;
			if (m_Pos.x > m_Max.x) m_Pos.x = m_Max.x - offset;
			break;
		case Orientation.Vertical:
			// Set 'X' location when the screen changes its vertical size.
			switch (locate)
			{
			case Location.Negative: m_Pos.x = m_Min.x + offset; dir = Vector2.right; rot = new Vector3(0, 0, -90); break;
			case Location.Positive: m_Pos.x = m_Max.x - offset; dir = Vector2.left; rot = new Vector3(0, 0, 90); break;
			}
			// Set 'Y' location if is outside of the screen.
			if (m_Pos.y < m_Min.y) m_Pos.y = m_Min.y + offset;
			if (m_Pos.y > m_Max.y) m_Pos.y = m_Max.y - offset;
			break;
		}
		// Update the emitter's position.
		transform.position = Vector3.Lerp(transform.position, m_Pos, 0.5f);
		m_Particles.transform.eulerAngles = rot;
		m_LineRenderer.transform.eulerAngles = rot;

		// Ray
		RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, laserDistance, WhatIsReceiver);
		if (hit)
		{
//			if (amount)
//				m_Particles.particleCount = Mathf.FloorToInt(Vector3.Distance(transform.position, hit.point) * m_Multiplier);
//			else
			// Set overflow offset y to hit distance (divide by particle count which by default is 1000)
			m_Particles.overflowOffset.y = Vector3.Distance(transform.position, hit.point) / m_Particles.particleCount;
//			m_Particles.lifetime = ((Vector3.Distance(transform.position, hit.point) / m_Particles.particleCount) * 60) * 0.5f;

			m_LineRenderer.SetPosition(1, new Vector3(0, hit.distance, 0));

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
				cachePipeHead.Entry = true;
				cachePipeHead.colorToEmit = colorToEmit;
				
				UpdateManipulator(cachePipeHead.transform.position, colorToEmit);
				EnableManipulator(true);
			}
		}
		else
		{
//			if (amount) 
//				m_Particles.particleCount = Mathf.FloorToInt(laserDistance * m_Multiplier);
//			else
			// Render laser to laserMaxDistance on clear sight
			m_Particles.overflowOffset.y = laserDistance / m_Particles.particleCount;
//			m_Particles.lifetime = ((laserDistance / m_Particles.particleCount) * 60) * 0.5f;

			m_LineRenderer.SetPosition(1, new Vector3(0, laserDistance, 0));
			EnableManipulator(false);
		}
		Debug.DrawRay(transform.position, dir);
	}

	public void OnMouseDrag ()
	{
		if (Pause.current && Pause.current.IsPaused) return;

		// Get the mouse position input.
		Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		// Convert to a local position.
		mousePosition = transform.InverseTransformPoint(mousePosition);
		// Move the emitter.
		Move(mousePosition.normalized);
	}

	public void Move (Vector2 direction)
	{
		// Set the movement according to its orientation.
		switch (orient)
		{
		case Orientation.Horizontal: direction = new Vector2(direction.x, 0); break;
		case Orientation.Vertical: direction = new Vector2(0, direction.y); break;
		}

		// Calculate the proposed position.
		m_Pos += direction * speed * Time.deltaTime;
		// Ensure that the proposed position isn't outside of the limits.
		m_Pos.x = Mathf.Clamp(m_Pos.x, m_Min.x + offset, m_Max.x - offset);
		m_Pos.y = Mathf.Clamp(m_Pos.y, m_Min.y + offset, m_Max.y - offset);
		// Update the emitter's position.
		transform.position = m_Pos;
	}

	public Gradient GetGradient (Color c)
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
		m_ManipulatorColor.property.lifetimeColor = GetGradient(color);
	}

	private void EnableManipulator (bool b)
	{
		m_ManipulatorObstruction.enabled = b;
		m_ManipulatorColor.enabled = b;
	}
}
