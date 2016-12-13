using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Xml;
using System.Linq;

[Serializable]
public class CommitEntry
{
	public string author;
	public int revision;
	public DateTime time;
	public string message;
}

public class SvnWatcher : MonoBehaviour
{
	public Project m_project;
	public BoxSpawner m_boxSpawner;
	public string m_repoUrl;
	public float m_refreshTimeInSeconds = 30f;
	public float m_refreshTimeStartOffset = 0f;
	public float m_timeVariance = 0f;

	List<CommitEntry> m_entries = new List<CommitEntry>();
	List<CommitEntry> m_boxesToSpawn = new List<CommitEntry>();
	float m_timePerBox;
	float m_spawnTimer;
	float m_refreshTimer;

	void Start ()
	{
		m_refreshTimer = m_refreshTimeStartOffset;
	}
	
	void Update ()
	{
		m_refreshTimer -= Time.deltaTime;
		if (m_refreshTimer < 0f)
		{
			StartCoroutine(FetchSequence());
			m_refreshTimer = m_refreshTimeInSeconds;
		}

		if (m_boxesToSpawn.Count > 0)
		{
			m_spawnTimer -= Time.deltaTime;
			if (m_spawnTimer < 0f)
			{
				m_boxSpawner.SpawnBox(m_project, m_boxesToSpawn[0]);
				m_boxesToSpawn.RemoveAt(0);

				m_spawnTimer = m_timePerBox * (1f + UnityEngine.Random.Range(-m_timeVariance, m_timeVariance));
			}
		}
	}

	bool m_isFetching = false;
	IEnumerator FetchSequence()
	{
		if (m_isFetching)
			yield break;
		m_isFetching = true;

		bool waiting = true;
		string xmlData = "";

		Command.PerformCommand("svn log " + m_repoUrl + " -l 10 --xml", data => 
		{
			waiting = false;
			xmlData = data;
		});

		while (waiting)
			yield return null;
		
		try
		{
			XmlDocument doc = new XmlDocument();
			doc.LoadXml(xmlData);

			var logEntries = doc.GetElementsByTagName("logentry");
			List<CommitEntry> newEntries = new List<CommitEntry>();

			foreach (XmlNode logEntry in logEntries)
			{
				var commit = new CommitEntry()
				{
					author = logEntry["author"].InnerText,
					message = logEntry["msg"].InnerXml,
					revision = int.Parse(logEntry.Attributes[0].InnerText),
					time = DateTime.Parse(logEntry["date"].InnerText)
				};
				newEntries.Add(commit);
			}

			UpdateList(newEntries);
		}
		catch (Exception e)
		{
			Debug.LogWarning("Failed to fetch svn log: " + m_repoUrl + "\r\n" + e.Message);
		}

		m_isFetching = false;
	}

	void UpdateList(List<CommitEntry> fetchedEntries)
	{
		foreach (var entry in fetchedEntries)
		{
			var old = m_entries.FirstOrDefault(e => e.revision == entry.revision);
			if (old != null)
				continue;

			m_boxesToSpawn.Add(entry);
			m_entries.Add(entry);
		}

		m_timePerBox = m_refreshTimeInSeconds / m_boxesToSpawn.Count;
		m_spawnTimer = 0f;
	}
}
