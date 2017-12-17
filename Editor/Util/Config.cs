using System;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Exodrifter.Yggdrasil
{
	[Serializable]
	public class Config
	{
		/// <summary>
		/// Path to the repository
		/// </summary>
		public string path;

		/// <summary>
		/// The name of the user
		/// </summary>
		public string name
		{
			get { return EditorPrefs.GetString("yggdrasil-user"); }
			set { EditorPrefs.SetString("yggdrasil-user", value); }
		}

		/// <summary>
		/// The email of the user
		/// </summary>
		public string email
		{
			get { return EditorPrefs.GetString("yggdrasil-email"); }
			set { EditorPrefs.SetString("yggdrasil-email", value); }
		}

		public string GetPath()
		{
			if (string.IsNullOrEmpty(path))
			{
				return Path.GetFullPath(
					Path.Combine(Application.dataPath, ".."));
			}

			return Path.GetFullPath(path);
		}
	}
}
