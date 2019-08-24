// -----------------------------------------------------
//                      Pigmentone
//                          by 
//                 Sliverbroom Studios
//       Copyright 2015 Roman Lara & Humberto Lara
// -----------------------------------------------------

using UnityEngine;
using System.Collections;

public class Pipeline : MonoBehaviour 
{
	public enum LocationState
	{
		Drag,
		Drop
	};

	public LocationState state = LocationState.Drag;
	public bool mirror = false;
	public float speed = 5f;
	[HideInInspector] public PipeHead entryPipeHead;
	[HideInInspector] public PipeHead exitPipeHead;
	[HideInInspector] public PipelineHUD hud;
	
	private SpriteRenderer m_Sprite;

	private float m_Angle;

	public void Awake ()
	{
		m_Sprite = GetComponentInChildren<SpriteRenderer>();
	}

	public void Start () 
	{
		switch (state)
		{
		case LocationState.Drag:
			Color c = m_Sprite.color;
			c.a = 0.8f;
			m_Sprite.color = c;

			if (mirror)
				entryPipeHead.mirror = true;
			else
				entryPipeHead.mirror = false;

			exitPipeHead.emit = false;
			break;
		case LocationState.Drop: break;
		}

		m_Angle = 0;
	}

	public void Update () 
	{
		if (Pause.current && Pause.current.IsPaused) return;

		Mirror();

		switch (state)
		{
		case LocationState.Drag: 
			Translate();
			Rotate();
			break;
		case LocationState.Drop: 
			Rotate(); 
			break;
		}
	}

	public void OnClick (int button)
	{
		switch (state)
		{
		case LocationState.Drag: 
			if (button == 0)
			{
				Color c = m_Sprite.color;
				c.a = 1.0f;
				m_Sprite.color = c;

				exitPipeHead.emit = true;

				state = LocationState.Drop;
			}
			else if (button == 1)
			{
				ApplyAngle();
			}
			break;
		case LocationState.Drop: 
			if (button == 0)
			{
				ApplyAngle();
			}
			else if (button == 1)
			{
				if (mirror)
					++hud.pipeMirror.amount;
				else
					++hud.pipeNormal.amount;

				Destroy(gameObject);
			}
			break;
		}
	}

	public void ApplyAngle ()
	{
		if (m_Angle >= 360)
			m_Angle = 0;

		m_Angle += 90;
	}

	public void Mirror ()
	{
		Vector3 scale = transform.localScale;
		
		if (mirror)
			scale.x = -1;
		else
			scale.x = 1;
		
		transform.localScale = scale;
	}

	public void Translate ()
	{
		Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		mousePosition.z = 0;
		transform.position = mousePosition;
	}

	public void Rotate ()
	{
		Quaternion rotation = Quaternion.Euler(0, 0, m_Angle);
		transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, speed);
	}
}
