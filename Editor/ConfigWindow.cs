using LibGit2Sharp;
using UnityEditor;
using UnityEngine;

namespace Exodrifter.Yggdrasil
{
	public class ConfigWindow : EditorWindow
	{
		private bool connected = false;

		void OnGUI()
		{
			var config = Config.Load();
			var oldStyle = GUI.skin.label.fontStyle;

			// Commit Settings
			{
				GUI.skin.label.fontStyle = FontStyle.Bold;
				GUILayout.Label("Commit Settings");
				GUI.skin.label.fontStyle = oldStyle;

				// name
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.PrefixLabel("Name");
				config.Name = EditorGUILayout.TextField(config.Name);
				EditorGUILayout.EndHorizontal();

				// email
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.PrefixLabel("Email");
				config.Email = EditorGUILayout.TextField(config.Email);
				EditorGUILayout.EndHorizontal();
			}

			EditorGUILayout.Space();

			// Authentication Settings
			{
				GUI.skin.label.fontStyle = FontStyle.Bold;
				GUILayout.Label("Authentication Settings");
				GUI.skin.label.fontStyle = oldStyle;

				// id_rsa path
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.PrefixLabel("id_rsa");
				config.IdRsa = EditorGUILayout.TextField(config.IdRsa);
				EditorGUILayout.EndHorizontal();

				// id_rsa.pub path
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.PrefixLabel("id_rsa.pub");
				config.IdRsaPub = EditorGUILayout.TextField(config.IdRsaPub);
				EditorGUILayout.EndHorizontal();
			}

			EditorGUILayout.Space();

			// Repository Settings
			{
				GUI.skin.label.fontStyle = FontStyle.Bold;
				GUILayout.Label("Repository Settings");
				GUI.skin.label.fontStyle = oldStyle;

				// Repository Path
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.PrefixLabel("Repository Path");
				config.Path = EditorGUILayout.TextField(config.Path);
				EditorGUILayout.EndHorizontal();
			}

			// Check if the repository path is valid
			try
			{
				new Repository(config.FullPath);

				EditorGUILayout.HelpBox(
					"Connected to repository at " + config.FullPath,
					MessageType.Info);

				if (!connected)
				{
					State.UpdateCache();
				}
				connected = true;
			}
			catch (RepositoryNotFoundException)
			{
				EditorGUILayout.HelpBox(
					"There is no repository at " + config.FullPath,
					MessageType.Error);

				if (connected)
				{
					State.UpdateCache();
				}
				connected = false;
			}

			Config.Save();
		}

		#region Static

		[MenuItem("Window/Yggdrasil/Config", priority = 100)]
		public static void Open()
		{
			var window = GetWindow<ConfigWindow>(true, "Yggdrasil Config");
			window.Show();
		}

		#endregion
	}
}