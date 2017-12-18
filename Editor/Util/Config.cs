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

		/// <summary>
		/// The path to the id_rsa file.
		/// </summary>
		public string idrsa
		{
			get { return EditorPrefs.GetString("yggdrasil-idrsa"); }
			set { EditorPrefs.SetString("yggdrasil-idrsa", value); }
		}

		/// <summary>
		/// The path to the id_rsa.pub file.
		/// </summary>
		public string idrsa_pub
		{
			get { return EditorPrefs.GetString("yggdrasil-idrsa_pub"); }
			set { EditorPrefs.SetString("yggdrasil-idrsa_pub", value); }
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
