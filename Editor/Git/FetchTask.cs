using LibGit2Sharp;

namespace Exodrifter.Yggdrasil
{
	public class FetchTask : Task
	{
		private Config config;

		public FetchTask(Config config)
			: base("Fetch")
		{
			this.config = config;
		}

		protected override void Perform()
		{
			var opts = new FetchOptions();
			opts.CredentialsProvider = Git.GetCredentials;
			opts.Prune = true;

			using (var repo = new Repository(config.FullPath))
			{
				float numRemotes = 0f;
				foreach (Remote remote in repo.Network.Remotes)
				{
					numRemotes++;
				}

				float index = 0f;
				foreach (var remote in repo.Network.Remotes)
				{
					index++;
					Progress = index / numRemotes;

					opts.OnTransferProgress = (status) =>
					{
						var t = status.ReceivedObjects / status.TotalObjects;
						Progress = (index + t) / numRemotes;
						return true;
					};

					var refspecs = Git.GetRefSpecs(remote);
					Commands.Fetch(repo, remote.Name, refspecs, opts, "");
				}
			}
		}
	}
}
