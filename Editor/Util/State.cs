using LibGit2Sharp;
using System.Collections.Generic;
using System.Linq;
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
		/// The number of commits the current local branch is behind the remote
		/// branch.
		/// </summary>
		public static int BehindBy
		{
			get { return behindBy; }
		}
		private static int behindBy;

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
			var config = Config.Load();
			try
			{
				Git.Fetch();

				using (var repo = new Repository(config.FullPath))
				{
					invalidRepository = false;

					// Update server status cache
					var branch = repo.Branches.Where(b => b.IsCurrentRepositoryHead).First();
					behindBy = branch.TrackingDetails.BehindBy ?? 0;

					// Update status entry cache
					files = files ?? new List<StatusEntry>();
					files.Clear();
					foreach (var file in repo.RetrieveStatus())
					{
						files.Add(file);
					}
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
