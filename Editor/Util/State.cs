using LibGit2Sharp;
using System.Collections.Generic;
using UnityEditor;

namespace Exodrifter.Yggdrasil
{
	/// <summary>
	/// Caches the state of the repository.
	/// </summary>
	public class State : AssetPostprocessor
	{
		/// <summary>
		/// True if the repository path in the config is invalid
		/// </summary>
		public static bool InvalidRepository
		{
			get { return invalidRepository; }
		}
		private static bool invalidRepository;

		/// <summary>
		/// A list of status entries, or an empty list if the repository path
		/// is invalid.
		/// </summary>
		public static List<StatusEntry> Files
		{
			get { return files ?? new List<StatusEntry>(); }
		}
		private static List<StatusEntry> files;

		/// <summary>
		/// Updates the cached state.
		/// </summary>
		private static void UpdateCache_Internal()
		{
			var repoPath = ConfigWindow.Config.GetPath();
			try
			{
				var repo = new Repository(repoPath);
				invalidRepository = false;

				files = files ?? new List<StatusEntry>();
				files.Clear();
				foreach (var file in repo.RetrieveStatus())
				{
					files.Add(file);
				}
			}
			catch (RepositoryNotFoundException)
			{
				invalidRepository = true;
			}
		}

		#region Callbacks

		[InitializeOnLoadMethod]
		[UnityEditor.Callbacks.DidReloadScripts(100)]
		public static void UpdateCache()
		{
			EditorApplication.delayCall += UpdateCache_Internal;
		}

		private static void OnPostprocessAllAssets
			(string[] imported, string[] deleted, string[] moved, string[] from)
		{
			EditorApplication.delayCall += UpdateCache_Internal;
		}

		#endregion
	}
}
