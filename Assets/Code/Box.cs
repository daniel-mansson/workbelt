using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine.UI;

public enum Project
{
	ABBA,
	HS
}

public class Box : MonoBehaviour
{
	[SerializeField]
	List<Entry> m_entries;
	[SerializeField]
	public GameObject m_logoRoot;
	[SerializeField]
	public GameObject m_revisionRoot;
	[SerializeField]
	public Text m_descText;
	[SerializeField]
	public Text m_nameText;

	public Rigidbody2D Body { get; private set; }

	[System.Serializable]
	public class Entry
	{
		public Project project;
		public Material logoMaterial;
		public Material crateMaterial;
	}

	private void Awake()
	{
		Body = GetComponent<Rigidbody2D>();
	}

	public void SetProject(Project project)
	{
		var entry = m_entries.FirstOrDefault(e => e.project == project);

		GetComponent<MeshRenderer>().material = entry.crateMaterial;

		var logos = m_logoRoot.GetComponentsInChildren<MeshRenderer>();
		foreach (var logo in logos)
		{
			logo.material = entry.logoMaterial;
		}
	}

	public void SetCommit(CommitEntry commit)
	{
		m_nameText.text = commit.author;
		m_descText.text = commit.message;
		var revs = m_revisionRoot.GetComponentsInChildren<Text>();
		foreach (var revText in revs)
		{
			revText.text = "#" + commit.revision;
		}
	}

	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Body.position.y < -50f)
		{
			Destroy(this.gameObject);
		}
	}
}
