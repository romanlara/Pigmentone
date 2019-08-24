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

public class ColorSplitter : MonoBehaviour 
{
	public float laserDistance = 20f;
	public LayerMask WhatIsReceiver;
	public Color colorLeft;
	public Color colorRight;
	public Color[] receivedColors;

	private Transform m_RaycastLeft;
	private Transform m_RaycastRight;
	private LineRenderer m_LineRendererLeft;				// Reference of line renderer left.
	private LineRenderer m_LineRendererRight;				// Reference of line renderer right.
	private PlaygroundParticlesC m_ParticleLeft;
	private PlaygroundParticlesC m_ParticleRight;
	private Transform m_ManipulatorLeft;
	private Transform m_ManipulatorRight;
	private ManipulatorObjectC m_ManipulatorObstuctionLeft;
	private ManipulatorObjectC m_ManipulatorObstuctionRight;
	private ManipulatorObjectC m_ManipulatorColorLeft;
	private ManipulatorObjectC m_ManipulatorColorRight;

	private List<Color> m_ColorList = new List<Color>();	// A color list temp.
	private Color m_ColorLeft;
	private Color m_ColorRight;

	public void Awake ()
	{
		m_RaycastLeft = transform.FindChild("RaycastLeft");
		m_RaycastRight = transform.FindChild("RaycastRight");

		m_LineRendererLeft = m_RaycastLeft.GetComponentInChildren<LineRenderer>();
		m_LineRendererRight = m_RaycastRight.GetComponentInChildren<LineRenderer>();

		m_ParticleLeft = m_RaycastLeft.GetComponentInChildren<PlaygroundParticlesC>();
		m_ParticleRight = m_RaycastRight.GetComponentInChildren<PlaygroundParticlesC>();

		m_ManipulatorLeft = m_ParticleLeft.transform.FindChild("Manipulator");
		m_ManipulatorRight = m_ParticleRight.transform.FindChild("Manipulator");

		m_ManipulatorObstuctionLeft = PlaygroundC.GetManipulator(0, m_ParticleLeft);
		m_ManipulatorObstuctionRight = PlaygroundC.GetManipulator(0, m_ParticleRight);

		m_ManipulatorColorLeft = PlaygroundC.GetManipulator(1, m_ParticleLeft);
		m_ManipulatorColorRight = PlaygroundC.GetManipulator(1, m_ParticleRight);
	}

	public void Start () 
	{
		EnableManipulatorLeft(false);
		EnableManipulatorRight(false);
	}

	public void Update () 
	{
		if (Pause.current && Pause.current.IsPaused) return;

		receivedColors = GetColors();

		if (receivedColors.Length > 1)
		{
			m_ColorLeft = (Compare(receivedColors[0], colorLeft)) ? colorLeft : receivedColors[1];
			m_ColorRight = (Compare(receivedColors[1], colorRight)) ? colorRight : receivedColors[0];
//			colorLeft = receivedColors[0];
//			colorRight = receivedColors[1];
		}

		m_ParticleLeft.lifetimeColor = GetGradient(m_ColorLeft);
		m_ParticleRight.lifetimeColor = GetGradient(m_ColorRight);

		// Ray left
		Vector3 targetLeft = m_RaycastLeft.position + m_RaycastLeft.rotation * Vector3.up;
		Vector3 posLeft = m_RaycastLeft.position;
		Vector2 dirLeft = targetLeft - posLeft;

		TriggerRay(posLeft, dirLeft, m_ParticleLeft, true, m_ColorLeft);

		// Ray Right
		Vector3 targetRight = m_RaycastRight.position + m_RaycastRight.rotation * Vector3.up;
		Vector3 posRight = m_RaycastRight.position;
		Vector2 dirRight = targetRight - posRight;

		TriggerRay(posRight, dirRight, m_ParticleRight, false, m_ColorRight);

		if (receivedColors.Length < 2)
		{
			Color nothing = new Color(0, 0, 0, 0);
			m_LineRendererLeft.SetColors(nothing, nothing);
			m_LineRendererRight.SetColors(nothing, nothing);

			m_ParticleLeft.emit = false;
			m_ParticleRight.emit = false;
		}
		else
		{
			m_LineRendererLeft.SetColors(m_ColorLeft, m_ColorLeft);
			m_LineRendererRight.SetColors(m_ColorRight, m_ColorRight);

			m_ParticleLeft.emit = true;
			m_ParticleRight.emit = true;
		}
	}

	private void TriggerRay (Vector3 pos, Vector2 dir, PlaygroundParticlesC particles, bool left, Color color)
	{
		RaycastHit2D hit = Physics2D.Raycast(pos, dir, laserDistance, WhatIsReceiver);
		if (hit)
		{
			particles.overflowOffset.y = Vector3.Distance(transform.position, hit.point) / particles.particleCount;

			if (left)
				m_LineRendererLeft.SetPosition(1, new Vector3(0, hit.distance, 0));
			else
				m_LineRendererRight.SetPosition(1, new Vector3(0, hit.distance, 0));

			if (receivedColors.Length > 1)
			{
				if (hit.collider.tag == "Receiver")
				{
					Receiver cacheReceiver = hit.collider.GetComponent<Receiver>();
					cacheReceiver.AddColor(color);

					if (left)
					{
						UpdateManipulatorLeft(cacheReceiver.transform.position, cacheReceiver.currentColor);
						EnableManipulatorLeft(true);
					}
					else
					{
						UpdateManipulatorRight(cacheReceiver.transform.position, cacheReceiver.currentColor);
						EnableManipulatorRight(true);
					}
				}
				else if (hit.collider.tag == "Obstacle")
				{
					Transform cacheObstacle = hit.collider.transform;

					if (left)
					{
						UpdateManipulatorLeft(cacheObstacle.position, color);
						EnableManipulatorLeft(true);
					}
					else
					{
						UpdateManipulatorRight(cacheObstacle.position, color);
						EnableManipulatorRight(true);
					}
				}
				else if (hit.collider.tag == "Pipeline")
				{
					PipeHead cachePipeHead = hit.collider.GetComponent<PipeHead>();
					cachePipeHead.colorToEmit = color;
					cachePipeHead.Entry = true;

					if (left)
					{
						UpdateManipulatorLeft(cachePipeHead.transform.position, color);
						EnableManipulatorLeft(true);
					}
					else
					{
						UpdateManipulatorRight(cachePipeHead.transform.position, color);
						EnableManipulatorRight(true);
					}
				}
			}
		}
		else
		{
			particles.overflowOffset.y = laserDistance / particles.particleCount;

			if (left)
			{
				m_LineRendererLeft.SetPosition(1, new Vector3(0, hit.distance, 0));
				EnableManipulatorLeft(false);
			}
			else
			{
				m_LineRendererRight.SetPosition(1, new Vector3(0, hit.distance, 0));
				EnableManipulatorRight(false);
			}
		}
		Debug.DrawRay(pos, dir);
	}

	public void AddColors (Color[] colors)
	{
		foreach (Color c in colors)
			if (!m_ColorList.Contains(c))
				m_ColorList.Add(c);
	}

	public Color[] GetColors ()
	{
		Color[] colors = new Color[m_ColorList.Count];
		m_ColorList.CopyTo(colors);
		m_ColorList.Clear();
		
		return colors;
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

	private void UpdateManipulatorLeft (Vector3 position, Color color)
	{
		m_ManipulatorLeft.position = position;
		m_ManipulatorColorLeft.property.lifetimeColor = GetGradient(color);
	}

	private void UpdateManipulatorRight (Vector3 position, Color color)
	{
		m_ManipulatorRight.position = position;
		m_ManipulatorColorRight.property.lifetimeColor = GetGradient(color);
	}

	private void EnableManipulatorLeft (bool b)
	{
		m_ManipulatorObstuctionLeft.enabled = b;
		m_ManipulatorColorLeft.enabled = b;
	}

	private void EnableManipulatorRight (bool b)
	{
		m_ManipulatorObstuctionRight.enabled = b;
		m_ManipulatorColorRight.enabled = b;
	}
}
