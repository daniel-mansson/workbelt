using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using UnityEngine;

public class Command : MonoBehaviour
{
	private static Command s_instance;

	private void Awake()
	{
		if (s_instance != null)
		{
			Destroy(this.gameObject);
		}
		else
		{
			s_instance = this;
		}
	}

	public static void PerformCommand(string command, Action<string> callback)
	{
		ProcessStartInfo startInfo = new ProcessStartInfo();
		Process p = new Process();

		startInfo.CreateNoWindow = true;
		startInfo.RedirectStandardOutput = true;
		startInfo.RedirectStandardInput = false;

		startInfo.UseShellExecute = false;

#if OSX
		startInfo.Arguments = "-c \"" + command + "\"";
		startInfo.FileName = "/bin/bash";
#else
		startInfo.Arguments = "/C \"" + command + "\"";
		startInfo.FileName = "cmd.exe";
#endif

		p.StartInfo = startInfo;
		p.Start();

		s_instance.StartCoroutine(WaitForProcess(p, callback));
	}

	static IEnumerator WaitForProcess(Process p, Action<string> callback)
	{
		while (!p.HasExited)
			yield return null;

		string output = p.StandardOutput.ReadToEnd();
		callback(output);
	}

}
