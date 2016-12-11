using UnityEngine;
using System.Collections;

public class BoxSpawner : MonoBehaviour
{
	public Box m_boxPrefab;
	public float m_radius;
	public Vector2 m_minSpeed;
	public Vector2 m_maxSpeed;
	public float m_minAngVel;
	public float m_maxAngVel;

	void Start ()
	{
	
	}

	private void OnGUI()
	{
		if (GUI.Button(new Rect(10, 10, 50, 50), "B"))
		{
			var box = (Box)Instantiate(
				m_boxPrefab, 
				transform.position + (Vector3)Random.insideUnitCircle * m_radius, 
				Quaternion.Euler(0, 0, Random.Range(0f, 360f)));

			box.Body.velocity =
				Vector3.right * Random.Range(m_minSpeed.x, m_maxSpeed.x) +
				Vector3.up * Random.Range(m_minSpeed.y, m_maxSpeed.y);

			box.Body.angularVelocity =
				Random.Range(m_minAngVel, m_maxAngVel);

			box.SetProject(Random.Range(0, 2) == 0 ? Project.ABBA : Project.HS);
		}
	}

	void Update ()
	{
		
	}

	private void OnDrawGizmos()
	{
		Gizmos.DrawWireSphere(transform.position, m_radius);
	}
}
