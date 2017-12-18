using LibGit2Sharp;
using System.IO;
using UnityEditor;

namespace Exodrifter.Yggdrasil
{
	public class ConfigWindow : EditorWindow
	{
		private bool connected = false;

		void OnGUI()
		{
			LoadConfig();

			// Name
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.PrefixLabel("Name");
			config.name = EditorGUILayout.TextField(config.name);
			EditorGUILayout.EndHorizontal();

			// Email
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.PrefixLabel("Email");
			config.email = EditorGUILayout.TextField(config.email);
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.Space();

			// id_rsa path
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.PrefixLabel("id_rsa");
			config.idrsa = EditorGUILayout.TextField(config.idrsa);
			EditorGUILayout.EndHorizontal();

			// id_rsa.pub path
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.PrefixLabel("id_rsa.pub");
			config.idrsa_pub = EditorGUILayout.TextField(config.idrsa_pub);
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.Space();

			// Repository Path
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.PrefixLabel("Repository Path");
			config.path = EditorGUILayout.TextField(config.path);
			EditorGUILayout.EndHorizontal();

			// Check if valid
			try
			{
				new Repository(config.GetPath());

				EditorGUILayout.HelpBox(
					"Connected to repository at " + config.GetPath(),
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
					"There is no repository at " + config.GetPath(),
					MessageType.Error);

				if (connected)
				{
					State.UpdateCache();
				}
				connected = false;
			}

			SaveConfig();
		}

		
		#region Static

		public static Config Config
		{
			get { return config; }
		}
		private static Config config = new Config();

		[InitializeOnLoadMethod]
		[UnityEditor.Callbacks.DidReloadScripts(50)]
		private static void InitConfig()
		{
			EditorApplication.delayCall += () =>
			{
				LoadConfig();
				SaveConfig();
			};
		}

		private static bool LoadConfig()
		{
			var path = GetConfigPath();
			if (File.Exists(path))
			{
				var json = File.ReadAllText(path);
				EditorJsonUtility.FromJsonOverwrite(json, config);
				return true;
			}

			return false;
		}

		public static void SaveConfig()
		{
			var path = GetConfigPath();
			var json = EditorJsonUtility.ToJson(config, true);
			File.WriteAllText(path, json);
		}

		private static string GetConfigPath()
		{
			return Path.GetFullPath(Path.Combine(
				PathCache.DataPath, "../ProjectSettings/Yggdrasil.json"));
		}

		[MenuItem("Window/Yggdrasil/Config", priority = 100)]
		public static void Open()
		{
			var window = GetWindow<ConfigWindow>(true, "Yggdrasil Config");
			window.Show();
		}

		#endregion
	}
}