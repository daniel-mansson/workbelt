using UnityEngine;
using System.Collections;

public class Belt : MonoBehaviour
{
	public Material m_material;
	public SurfaceEffector2D m_effector;

	public float m_scrollAdjust = 1f;
	public float m_extraForce = 1f;

	public float m_velScale = 0.2f;
	public float m_velScaleYOff = 0.62f;
	public float m_velScaleYScale = 0.05f;

	float m_scrollSpeed;

	void Start ()
	{
	}
	
	void Update ()
	{
		m_scrollSpeed = m_material.mainTextureScale.x * m_scrollAdjust * m_effector.speed / transform.localScale.x;

		m_material.mainTextureOffset = m_material.mainTextureOffset 
			+ Vector2.right * m_scrollSpeed * Time.deltaTime;
	}

	private void OnCollisionStay2D(Collision2D collision)
	{
		float dx = m_effector.speed - collision.rigidbody.velocity.x;
		if (dx > 0f)
		{
			collision.rigidbody.AddForce(Vector2.right * dx * m_extraForce);
			collision.rigidbody.velocity += Vector2.right * dx * m_velScale * Mathf.Clamp01(1f - (collision.rigidbody.position.y - m_velScaleYOff) / m_velScaleYScale);
		}
		else
		{
			collision.rigidbody.velocity += Vector2.right * dx * m_velScale * Mathf.Clamp01(1f - (collision.rigidbody.position.y - m_velScaleYOff) / m_velScaleYScale);
		}
	}
}
