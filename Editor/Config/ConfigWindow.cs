using LibGit2Sharp;
using UnityEditor;
using UnityEngine;

namespace Exodrifter.Yggdrasil
{
	public class ConfigWindow : EditorWindow
	{
		#region Unity

		[SerializeField]
		private Vector2 pos;

		void OnGUI()
		{
			var config = Config.Load();

			using (YGUI.ScrollView(ref pos))
			{
				// Commit Settings
				YGUI.Label("Commit Settings", FontStyle.Bold);
				using (YGUI.Horizontal())
				{
					YGUI.Prefix("Name");
					config.Name = YGUI.TextField(config.Name);
				}
				using (YGUI.Horizontal())
				{
					YGUI.Prefix("Email");
					config.Email = YGUI.TextField(config.Email);
				}
				YGUI.Space();

				// Authentication Settings
				YGUI.Label("Authentication Settings", FontStyle.Bold);
				using (YGUI.Horizontal())
				{
					YGUI.Prefix("id_rsa");
					config.IdRsa = YGUI.TextField(config.IdRsa);
				}
				using (YGUI.Horizontal())
				{
					YGUI.Prefix("id_rsa.pub");
					config.IdRsaPub = YGUI.TextField(config.IdRsaPub);
				}
				YGUI.Space();

				// Repository Settings
				YGUI.Label("Repository Settings", FontStyle.Bold);
				using (YGUI.Horizontal())
				{
					YGUI.Prefix("Repository Path");
					config.Path = YGUI.TextField(config.Path);
				}

				// Check if the repository path is valid
				if (IsRepository(config.FullPath))
				{
					YGUI.InfoBox(
						"Connected to repository at {0}.",
						config.FullPath
					);
				}
				else
				{
					YGUI.ErrBox(
						"There is no repository at {0}.",
						config.FullPath
					);
				}
			}

			Config.Save();
		}

		#endregion

		#region Static

		[MenuItem("Window/Yggdrasil/Config", priority = 100)]
		public static void Open()
		{
			var window = GetWindow<ConfigWindow>(true, "Yggdrasil Config");
			window.Show();
		}

		#endregion

		#region Util

		private static bool IsRepository(string path)
		{
			try
			{
				new Repository(path);
				return true;
			}
			catch (RepositoryNotFoundException)
			{
				return false;
			}
		}

		#endregion
	}
}