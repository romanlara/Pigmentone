// -----------------------------------------------------
//                      Pigmentone
//                          by 
//                 Sliverbroom Studios
//       Copyright 2015 Roman Lara & Humberto Lara
// -----------------------------------------------------

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ParticlePlayground;

public class Receiver : MonoBehaviour 
{
	public float speed = 5;						// Speed of rotation.
	public float speedInking = 4;				// Speed inking.
	public float laserDistance = 20f;			// Distance of ray.
	public float hintDuration = 5f;
	public LayerMask WhatIsReceiver;			// Layer to send the color.
	public Color colorToReceive;				// Color to recieve.
	public Color currentColor;					// Current color.
	[Space(8)]
	public Color[] receivedColors;				// A color list received.
	[HideInInspector] public bool ready;

	private SpriteRenderer m_PigmentCircle;
	private SpriteRenderer m_PigmentRing;
	private SpriteRenderer m_CromaticRing;
	private Transform m_Raycast;						// Reference of the ray.
	private LineRenderer m_LineRenderer;				// Reference of line renderer.
	private SpriteRenderer m_Output;					// Reference of the direction indicator.
	private PlaygroundParticlesC m_Particles;			// Reference of particles.
	private Transform m_ManipulatorTransform;			// Reference to relocate the particles manipulator.
	private ManipulatorObjectC m_ManipulatorObstruction;// Reference to modify the obstruction manipulator.
	private ManipulatorObjectC m_ManipulatorColor;		// Reference to modify the color manipulator.
	private ParticleSystem m_ParticleRing;
	private AudioSource m_SoundSource;

	private List<Color> m_ColorList = new List<Color>();// A color list temp.
	private Stack<Color> m_ColorStack = new Stack<Color>();
	private bool m_bMouseOver;
	private int m_count;
//	private float m_Multiplier;

	public SpriteRenderer Circle
	{
		get { return m_PigmentCircle; }
	}

	public virtual void Awake ()
	{
		m_PigmentCircle = transform.FindChild("Pigment Circle").GetComponent<SpriteRenderer>();
		m_PigmentRing = transform.FindChild("Pigment Ring").GetComponent<SpriteRenderer>();
		m_CromaticRing = transform.FindChild("Cromatic Ring").GetComponent<SpriteRenderer>();
		m_Raycast = transform.FindChild("Raycast");
		m_Particles = m_Raycast.GetComponentInChildren<PlaygroundParticlesC>();
		m_ManipulatorTransform = m_Particles.transform.FindChild("Manipulator");
		m_LineRenderer = m_Raycast.GetComponentInChildren<LineRenderer>();
		m_Output = m_Raycast.GetComponentInChildren<SpriteRenderer>();

		m_ManipulatorObstruction = PlaygroundC.GetManipulator(0, m_Particles);
		m_ManipulatorColor = PlaygroundC.GetManipulator(1, m_Particles);

		m_ParticleRing = GetComponentInChildren<ParticleSystem>();
		m_SoundSource = GetComponent<AudioSource>();
	}

	public virtual void Start ()
	{
		EnableManipulator(false);
//		m_ParticleRing.emit = false;
//		m_ParticleRing.loop = false;

//		if (amount)
//		{
//			m_Multiplier = m_Particles.particleCount / laserDistance;
//			m_Particles.overflowOffset.y = laserDistance / m_Particles.particleCount;
//		}
	}

	public void OnDisable ()
	{
		m_bMouseOver = false;
	}

	public virtual void Update () 
	{
		if (Pause.current && Pause.current.IsPaused) return;

		// Rotate
		if (m_bMouseOver)
		{
			if (Input.GetKey(KeyCode.Mouse0)) 
				Rotate(Vector3.forward * speed);
			else if (Input.GetKey(KeyCode.Mouse1)) 
				Rotate(Vector3.forward * -speed);
		}

		receivedColors = GetColors();
		currentColor = CombineColors(receivedColors);
		
		m_PigmentCircle.color = Color.Lerp(m_PigmentCircle.color, currentColor, 0.5f * speedInking * Time.deltaTime);

		if (Compare(currentColor, colorToReceive))
		{
			m_Output.color = Color.Lerp(m_Output.color, currentColor, 0.5f * speedInking * Time.deltaTime);
			m_PigmentRing.color = Color.Lerp(m_PigmentRing.color, currentColor, 0.5f * speedInking * Time.deltaTime);
			
			Color ccr = m_CromaticRing.color;
			ccr.a = 0f;
			m_CromaticRing.color = Color.Lerp(m_CromaticRing.color, ccr, 0.5f * speedInking * Time.deltaTime);

			ready = true;
		}
		else
		{
			if (!HintScript.showHint)
			{
				m_Output.color = Color.Lerp(m_Output.color, Color.white, 0.5f * speedInking * Time.deltaTime);

				Color cpr = m_PigmentRing.color;
				cpr.a = 0f;
				m_PigmentRing.color = Color.Lerp(m_PigmentRing.color, cpr, 0.5f * speedInking * Time.deltaTime);

				Color ccr = m_CromaticRing.color;
				ccr.a = 1f;
				m_CromaticRing.color = Color.Lerp(m_CromaticRing.color, ccr, 0.5f * speedInking * Time.deltaTime);
			}

			ready = false;
		}

		if (HintScript.showHint)
		{
			m_Output.color = Color.Lerp(m_Output.color, colorToReceive, 0.5f * speedInking * Time.deltaTime);
			m_PigmentRing.color = Color.Lerp(m_PigmentRing.color, colorToReceive, 0.5f * speedInking * Time.deltaTime);

			Color ccr = m_CromaticRing.color;
			ccr.a = 0f;
			m_CromaticRing.color = Color.Lerp(m_CromaticRing.color, ccr, 0.5f * speedInking * Time.deltaTime);
		}

		// Ray
		Vector3 pos = m_Raycast.position;
		Vector2 dir = m_Raycast.position - transform.position;//m_Raycast.position + rot * pos;

		RaycastHit2D hit = Physics2D.Raycast(pos, dir, laserDistance, WhatIsReceiver);
		if (hit)
		{
//			if (amount)
//				m_Particles.particleCount = Mathf.FloorToInt(Vector3.Distance(transform.position, hit.point) * m_Multiplier);
//			else
			// Set overflow offset y to hit distance (divide by particle count which by default is 1000)
			m_Particles.overflowOffset.y = Vector3.Distance(transform.position, hit.point) / m_Particles.particleCount;
			m_LineRenderer.SetPosition(1, new Vector3(0, hit.distance, 0));

			if (currentColor != new Color(0, 0, 0, 0))
			{
				if (hit.collider.tag == "Receiver")
				{
					Receiver cacheReceiver = hit.collider.GetComponent<Receiver>();
					cacheReceiver.AddColor(currentColor);

					UpdateManipulator(cacheReceiver.transform.position, cacheReceiver.currentColor);
					EnableManipulator(true);
				}
				else if (hit.collider.tag == "Obstacle")
				{
					Transform cacheObstacle = hit.collider.transform;
					UpdateManipulator(cacheObstacle.position, currentColor);
					EnableManipulator(true);
				}
				else if (hit.collider.tag == "Pipeline")
				{
					PipeHead cachePipeHead = hit.collider.GetComponent<PipeHead>();
					cachePipeHead.colorToEmit = currentColor;
					cachePipeHead.Entry = true;
					
					UpdateManipulator(cachePipeHead.transform.position, currentColor);
					EnableManipulator(true);
				}
				else if (hit.collider.tag == "Prism")
				{
					ColorSplitter cacheSplitter = hit.collider.GetComponent<ColorSplitter>();
					if (receivedColors.Length > 1) cacheSplitter.AddColors(receivedColors);

					UpdateManipulator(cacheSplitter.transform.position, currentColor);
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
		Debug.DrawRay(pos, dir);

		m_LineRenderer.SetColors(currentColor, currentColor);
		m_Particles.lifetimeColor = GetGradient(currentColor);

		if (currentColor == new Color(0, 0, 0, 0))
			m_Particles.emit = false;
		else
			m_Particles.emit = true;

		// Sparkle
		if (m_ColorStack.Count > m_count && !m_ParticleRing.isPlaying)
		{
//			m_ParticleRing.emit = true;
			m_ParticleRing.Play();
			m_SoundSource.Play();
		}

//		if (m_ColorStack.Count == m_count)
//			m_ParticleRing.emit = false;

		m_count = receivedColors.Length;

		RotateParticles();
	}

	public void OnMouseEnter ()
	{
		m_bMouseOver = true;
	}
	
	public void OnMouseExit ()
	{
		m_bMouseOver = false;
	}

//	public void OnMouseDrag ()
//	{
//		// Get the mouse position input.
//		Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
//		// Convert to a local position.
//		mousePosition = transform.InverseTransformPoint(mousePosition);
//		// Move the emitter.
//		Rotate(mousePosition.normalized);
//	}

	public void Rotate (Vector3 direction)
	{
		// Get the emitter's current rotation.
//		float rot = direction.x;
		// Calculate the proposed rotation.
//		rot *= speed * Time.deltaTime;
		// Update the emitter's rotation.
		transform.Rotate(direction);
	}

	public void RotateParticles ()
	{
		// Calculate the angle of particles is observable 
		// if the texture shape is different to a circle.
		int angle = Mathf.FloorToInt(360 - transform.eulerAngles.z);
		// Set the angle.
		m_Particles.initialRotationMin = angle;
		m_Particles.initialRotationMax = angle;
	}

	public void AddColor (Color c)
	{
		if (!m_ColorList.Contains(c))
			m_ColorList.Add(c);

		if (!m_ColorStack.Contains(c))
			m_ColorStack.Push(c);
	}

	public Color[] GetColors ()
	{
		Color[] colors = new Color[m_ColorStack.Count];

		if (m_ColorList.Count != m_ColorStack.Count)
		{
			colors = m_ColorStack.ToArray();
			m_ColorStack.Clear();
		}

		if (m_ColorStack.Count > 0)
			colors = m_ColorStack.ToArray();
		m_ColorList.Clear();

		return colors;
	}

	public Color CombineColors (params Color[] colors)
	{
		// Store the result color.
		Color result = new Color(0, 0, 0, 0);
		// Add all colors
		foreach (Color c in colors) result += c;
		// if they are top to 1, are divided by the length.
		if (result.r > 1f) result.r /= colors.Length;
		if (result.g > 1f) result.g /= colors.Length;
		if (result.b > 1f) result.b /= colors.Length;
		// Reset the alpha.
		result.a /= colors.Length;
		// Will be white when the colors are 0.
		if (colors.Length == 0) result = new Color(0, 0, 0, 0);
		// Return the new color.
		return result;
	}

	public bool Compare (Color c1, Color c2)
	{
		float c1_r = (float) System.Math.Round(c1.r * 255);
		float c1_g = (float) System.Math.Round(c1.g * 255);
		float c1_b = (float) System.Math.Round(c1.b * 255);

		float c2_r = (float) System.Math.Round(c2.r * 255);
		float c2_g = (float) System.Math.Round(c2.g * 255);
		float c2_b = (float) System.Math.Round(c2.b * 255);
		
		return (c1_r == c2_r) && (c1_g == c2_g) && (c1_b == c2_b);
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
