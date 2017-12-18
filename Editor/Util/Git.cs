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
			var config = Config.Load();

			try
			{
				using (var repo = new Repository(config.FullPath))
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
			var config = Config.Load();

			try
			{
				using (var repo = new Repository(config.FullPath))
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
		/// git checkout -- file
		/// </summary>
		/// <param name="path">
		/// The path of the file to discard the changes of.
		/// </param>
		/// <returns>True if the operation was successful.</returns>
		public static bool Checkout(string path)
		{
			var config = Config.Load();

			try
			{
				using (var repo = new Repository(config.FullPath))
				{
					var opts = new CheckoutOptions();
					opts.CheckoutModifiers = CheckoutModifiers.Force;
					repo.CheckoutPaths("HEAD", new string[] { path }, opts);
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
			var config = Config.Load();

			try
			{
				using (var repo = new Repository(config.FullPath))
				{
					var name = config.Name;
					var email = config.Email;
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
			var config = Config.Load();

			try
			{
				using (var repo = new Repository(config.FullPath))
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
			var config = Config.Load();

			return new SshUserKeyCredentials()
			{
				Username = usernameFromUrl,
				Passphrase = string.Empty,
				PublicKey = config.IdRsaPub,
				PrivateKey = config.IdRsa,
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
