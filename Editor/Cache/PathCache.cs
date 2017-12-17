using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public static class PathCache
{
	public static string DataPath
	{
		get { return dataPath; }
	}
	private static string dataPath;

	static PathCache()
	{
		CachePaths();
	}

	[UnityEditor.Callbacks.DidReloadScripts(0)]
	private static void CachePaths()
	{
		dataPath = Application.dataPath;
	}
}
