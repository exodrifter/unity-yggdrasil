using LibGit2Sharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Exodrifter.Yggdrasil
{
	/// <summary>
	/// Porcelain Git commands.
	/// </summary>
	public static class Git
	{
		/// <summary>
		/// git add .
		/// </summary>
		/// <returns>True if operation was successful.</returns>
		public static bool AddAll()
		{
			try
			{
				using (var repo = new Repository(ConfigWindow.Config.path))
				{
					foreach (var file in repo.RetrieveStatus())
					{
						Commands.Stage(repo, file.FilePath);
					}
				}
			}
			catch (RepositoryNotFoundException)
			{
				return false;
			}

			return true;
		}

		/// <summary>
		/// git add file
		/// </summary>
		/// <returns>True if operation was successful.</returns>
		public static bool Add(List<string> files)
		{
			try
			{
				using (var repo = new Repository(ConfigWindow.Config.path))
				{
					foreach (var file in files)
					{
						Commands.Stage(repo, file);
					}
				}
			}
			catch (RepositoryNotFoundException)
			{
				return false;
			}

			return true;
		}

		/// <summary>
		/// git commit -m message
		/// </summary>
		/// <returns>True if operation was successful.</returns>
		public static bool Commit(string summary, string message)
		{
			try
			{
				using (var repo = new Repository(ConfigWindow.Config.path))
				{
					var name = ConfigWindow.Config.name;
					var email = ConfigWindow.Config.email;
					var timestamp = DateTime.Now;
					var author = new Signature(name, email, timestamp);
					var str = MakeMessage(summary, message);
					repo.Commit(str, author, author);
				}
			}
			catch (RepositoryNotFoundException)
			{
				return false;
			}

			return true;
		}

		/// <summary>
		/// git push origin
		/// </summary>
		/// <returns>True if operation was successful.</returns>
		public static bool Push()
		{
			try
			{
				using (var repo = new Repository(ConfigWindow.Config.path))
				{
					var branch = repo.Branches.Where(b => b.IsCurrentRepositoryHead).First();
					var opts = new PushOptions();
					opts.CredentialsProvider = MyCredentialsProvider;
					repo.Network.Push(branch, opts);
				}
			}
			catch (RepositoryNotFoundException)
			{
				return false;
			}

			return true;
		}

		#region Util

		private static Credentials MyCredentialsProvider(string url, string usernameFromUrl, SupportedCredentialTypes types)
		{
			return new SshUserKeyCredentials()
			{
				Username = usernameFromUrl,
				Passphrase = string.Empty,
				PublicKey = ConfigWindow.Config.idrsa_pub,
				PrivateKey = ConfigWindow.Config.idrsa,
			};
		}

		private static string MakeMessage(string summary, string message)
		{
			return summary.Trim() + "\n\n" + Wrap(message, 72);
		}

		private static string Wrap(string str, int maxLength)
		{
			string ret = "", line = "";
			foreach (string word in str.Split(' '))
			{
				if ((line + word).Length > maxLength)
				{
					ret += line + "\n";
					line = "";
				}

				line += word + ' ';
			}

			return ret + line;
		}

		#endregion
	}
}
