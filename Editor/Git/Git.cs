using LibGit2Sharp;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Exodrifter.Yggdrasil
{
	/// <summary>
	/// Porcelain Git commands.
	/// </summary>
	public static class Git
	{
		public static Task Fetch()
		{
			var config = Config.Load();
			return new FetchTask(config);
		}

		#region Util

		/// <summary>
		/// Git credentials handler.
		/// </summary>
		/// <param name="url">The url being connected to.</param>
		/// <param name="username">The username being used.</param>
		/// <param name="types">The type of credentials requested.</param>
		/// <returns>The requested credentials.</returns>
		internal static Credentials GetCredentials
			(string url, string username, SupportedCredentialTypes types)
		{
			var config = Config.Load();

			return new SshUserKeyCredentials()
			{
				Username = username,
				Passphrase = string.Empty,
				PublicKey = config.IdRsaPub,
				PrivateKey = config.IdRsa,
			};
		}

		/// <summary>
		/// Returns the RefSpecs defined for the specified remote.
		/// </summary>
		/// <param name="remote">The remote to get the RefSpecs of.</param>
		/// <returns>The RefSpecs.</returns>
		internal static IEnumerable<string> GetRefSpecs(Remote remote)
		{
			return remote.FetchRefSpecs.Select(x => x.Specification);
		}

		/// <summary>
		/// Returns the signature to use for the current user.
		/// </summary>
		/// <param name="config">
		/// The config to generate the signature with.
		/// </param>
		/// <returns>The signature to use.</returns>
		internal static Signature GetSignature(Config config = null)
		{
			config = config ?? Config.Load();

			var name = config.Name;
			var email = config.Email;
			var timestamp = DateTime.Now;
			return new Signature(name, email, timestamp);
		}

		/// <summary>
		/// Formats a commit message properly.
		/// </summary>
		/// <param name="title">The title of the commit message.</param>
		/// <param name="message">The message of the commit.</param>
		/// <returns>The formatted commit message.</returns>
		internal static string MakeMessage(string title, string message)
		{
			return title.Trim() + "\n\n" + Wrap(message, 72);
		}

		/// <summary>
		/// Wraps a string at a specified length.
		/// </summary>
		/// <param name="str">The string to wrap.</param>
		/// <param name="maxLength">
		/// The maximum number of characters in each line.
		/// </param>
		/// <returns>The wrapped string.</returns>
		private static string Wrap(string str, int maxLength)
		{
			string ret = "", line = "";
			foreach (string word in str.Split(' '))
			{
				if ((line + word).Length > maxLength)
				{
					ret += line + '\n';
					line = "";
				}

				if (string.IsNullOrEmpty(line))
				{
					line = word;
				}
				else
				{
					line += ' ' + word;
				}
			}

			if (string.IsNullOrEmpty(line))
			{
				return ret;
			}
			return ret + line + '\n';
		}

		#endregion
	}
}
