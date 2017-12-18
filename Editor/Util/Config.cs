using System;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Exodrifter.Yggdrasil
{
	[Serializable]
	public class Config
	{
		#region Project Settings

		/// <summary>
		/// The user-set path to the repository.
		/// </summary>
		public string Path
		{
			get { return path; }
			set { path = value; }
		}
		[SerializeField]
		private string path;

		/// <summary>
		/// The full path to the repository.
		/// </summary>
		public string FullPath
		{
			get
			{
				if (string.IsNullOrEmpty(path))
				{
					var p = System.IO.Path.Combine(Application.dataPath, "..");
					return System.IO.Path.GetFullPath(p);
				}

				return System.IO.Path.GetFullPath(path);
			}
		}

		#endregion

		#region User Settings

		/// <summary>
		/// The name of the user.
		/// </summary>
		public string Name
		{
			get { return EditorPrefs.GetString("yggdrasil-user"); }
			set { EditorPrefs.SetString("yggdrasil-user", value); }
		}

		/// <summary>
		/// The email of the user.
		/// </summary>
		public string Email
		{
			get { return EditorPrefs.GetString("yggdrasil-email"); }
			set { EditorPrefs.SetString("yggdrasil-email", value); }
		}

		/// <summary>
		/// The path to the id_rsa file.
		/// </summary>
		public string IdRsa
		{
			get { return EditorPrefs.GetString("yggdrasil-idrsa"); }
			set { EditorPrefs.SetString("yggdrasil-idrsa", value); }
		}

		/// <summary>
		/// The path to the id_rsa.pub file.
		/// </summary>
		public string IdRsaPub
		{
			get { return EditorPrefs.GetString("yggdrasil-idrsa_pub"); }
			set { EditorPrefs.SetString("yggdrasil-idrsa_pub", value); }
		}

		#endregion

		#region Static

		private static Config instance = new Config();

		/// <summary>
		/// Initializes the config file.
		/// </summary>
		[InitializeOnLoadMethod]
		[UnityEditor.Callbacks.DidReloadScripts(50)]
		private static void InitConfig()
		{
			EditorApplication.delayCall += () =>
			{
				Load();
				Save();
			};
		}

		/// <summary>
		/// Loads the config for this project.
		/// </summary>
		/// <returns>The config for this project.</returns>
		public static Config Load()
		{
			var path = GetConfigPath();
			if (File.Exists(path))
			{
				var json = File.ReadAllText(path);
				EditorJsonUtility.FromJsonOverwrite(json, instance);
				return instance;
			}

			return instance;
		}

		/// <summary>
		/// Saves the config for this project.
		/// </summary>
		public static void Save()
		{
			var path = GetConfigPath();
			var json = EditorJsonUtility.ToJson(instance, true);
			File.WriteAllText(path, json);
		}

		/// <summary>
		/// Returns the full path to the config file for this project.
		/// </summary>
		/// <returns>
		/// The full path to the config file for this project.
		/// </returns>
		private static string GetConfigPath()
		{
			var p = System.IO.Path.Combine(
				PathCache.DataPath, "../ProjectSettings/Yggdrasil.json");
			return System.IO.Path.GetFullPath(p);
		}

		#endregion
	}
}
