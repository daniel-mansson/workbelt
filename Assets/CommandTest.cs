using UnityEngine;
using System.Collections;

public class CommandTest : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (Input.GetKeyDown(KeyCode.Alpha2))
		{
			Command.PerformCommand("svn help", cb =>
			{
				Debug.Log(cb);
			});
		}
		if (Input.GetKeyDown(KeyCode.Alpha3))
		{
			Command.PerformCommand("svn log https://svn.alfresco.com/repos/alfresco-open-mirror/alfresco/HEAD -l 5 --xml", cb =>
			{
				Debug.Log(cb);
			});
		}
	}
}
