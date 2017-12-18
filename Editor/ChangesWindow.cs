using LibGit2Sharp;
using System;
using UnityEditor;
using UnityEngine;

namespace Exodrifter.Yggdrasil
{
	public class ChangesWindow : EditorWindow
	{
		[SerializeField, HideInInspector]
		private ToggledDictionary toggled;
		[SerializeField, HideInInspector]
		private string commitSummary = "";
		[SerializeField, HideInInspector]
		private string commitMessage = "";
		[SerializeField, HideInInspector]
		private bool all = false;
		[SerializeField, HideInInspector]
		private Vector2 scrollPos = Vector2.zero;

		void OnInspectorUpdate()
		{
			Repaint();
		}

		void OnGUI()
		{
			// Start scroll view
			scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

			// Repository path is invalid error message
			if (State.InvalidRepository)
			{
				EditorGUILayout.HelpBox(
					"Cannot find repository; invalid Yggdrasil config!",
					MessageType.Error);

				if (GUILayout.Button("Edit Config"))
				{
					ConfigWindow.Open();
				}
			}
			// Check if user info is set
			else if (string.IsNullOrEmpty(ConfigWindow.Config.name)
				|| string.IsNullOrEmpty(ConfigWindow.Config.email))
			{
				EditorGUILayout.HelpBox(
					"Please set your name and email!",
					MessageType.Error);

				if (GUILayout.Button("Edit Config"))
				{
					ConfigWindow.Open();
				}
			}
			// No changes info message
			else if (State.Files.Count == 0)
			{
				EditorGUILayout.HelpBox(
					"No changes detected.",
					MessageType.Info);
			}
			// Show changes
			else
			{
				DrawChanges();

				EditorGUILayout.Space();

				// Change Summary
				EditorGUILayout.PrefixLabel("Change Summary");
				commitSummary = EditorGUILayout.TextField(commitSummary);

				// Change Message
				var oldWrap = EditorStyles.textField.wordWrap;
				EditorStyles.textField.wordWrap = true;
				var size = EditorGUIUtility.singleLineHeight;
				EditorGUILayout.PrefixLabel("Change Message");
				commitMessage = GUILayout.TextArea(
					commitMessage, GUILayout.MinHeight(size * 3));
				EditorStyles.textField.wordWrap = oldWrap;

				// Push button
				GUI.enabled = IsAnyToggled() && HasCommitMessage();
				if (GUILayout.Button("Push"))
				{
					CommitAndPushToggled();
				}
				GUI.enabled = true;

				// No changes message
				if (!IsAnyToggled())
				{
					EditorGUILayout.HelpBox(
						"Cannot push; no changes selected",
						MessageType.Error);
				}
				// No summary message
				else if (!HasCommitMessage())
				{
					EditorGUILayout.HelpBox(
						"Cannot push; missing change summary",
						MessageType.Error);
				}
				// Long summary message
				else if (commitSummary.Length > 50)
				{
					EditorGUILayout.HelpBox(
						"Summary is too long",
						MessageType.Warning);
				}
			}

			// End scroll view
			EditorGUILayout.EndScrollView();
		}

		private void DrawChanges()
		{
			var size = EditorGUIUtility.singleLineHeight;
			var square = new GUILayoutOption[] {
				GUILayout.MinWidth(size), GUILayout.MinHeight(size),
				GUILayout.MaxWidth(size), GUILayout.MaxHeight(size)
			};
			var button = new GUILayoutOption[] {
				GUILayout.MinWidth(size * 2 - 4), GUILayout.MinHeight(size - 2),
				GUILayout.MaxWidth(size * 2 - 4), GUILayout.MaxHeight(size - 2)
			};

			EditorGUILayout.BeginHorizontal();
			all = EditorGUILayout.Toggle(all, square);
			var newAll = all;
			EditorGUILayout.LabelField(GUIContent.none, square);
			var oldFontStyle = GUI.skin.label.fontStyle;
			GUI.skin.label.fontStyle = FontStyle.Bold;
			GUILayout.Label("Path");
			GUI.skin.label.fontStyle = oldFontStyle;
			EditorGUILayout.EndHorizontal();

			toggled = new ToggledDictionary();
			foreach (var file in State.Files)
			{
				EditorGUILayout.BeginHorizontal();

				// Draw toggle
				if (all)
				{
					newAll = EditorGUILayout.Toggle(all, square);
				}
				else
				{
					if (!toggled.Dictionary.ContainsKey(file.FilePath))
					{
						toggled[file.FilePath] = false;
					}
					toggled[file.FilePath] =
						EditorGUILayout.Toggle(toggled[file.FilePath], square);
				}

				// Draw change type image
				switch (file.State)
				{
					case FileStatus.DeletedFromIndex:
					case FileStatus.DeletedFromWorkdir:
						GUILayout.Label(Content(ImageCache.Deleted, "This file has been deleted."), square);
						break;

					case FileStatus.ModifiedInIndex:
					case FileStatus.ModifiedInWorkdir:
					case FileStatus.RenamedInIndex:
					case FileStatus.RenamedInWorkdir:
					case FileStatus.TypeChangeInIndex:
					case FileStatus.TypeChangeInWorkdir:
						GUILayout.Label(Content(ImageCache.Modified, "This file has been modified."), square);
						break;

					case FileStatus.NewInIndex:
					case FileStatus.NewInWorkdir:
						GUILayout.Label(Content(ImageCache.Added, "This file has been added."), square);
						break;

					case FileStatus.Ignored:
						GUI.enabled = false;
						break;
				}

				// Draw file label
				EditorGUILayout.LabelField(file.FilePath);

				EditorGUILayout.Space();

				if (GUILayout.Button(Content(ImageCache.X, "Discard Changes"), button))
				{
					var msg = "Are you sure you want to discard the changes to " + file.FilePath + "?";
					if (EditorUtility.DisplayDialog("Discard Changes?", msg, "Yes", "No"))
					{
						CheckoutFile(file.FilePath);
					}
				}
				GUI.enabled = true;

				EditorGUILayout.EndHorizontal();
			}

			if (newAll != all)
			{
				all = newAll;
			}
		}

		/// <summary>
		/// Emulates the `git checkout -- file` command.
		/// </summary>
		/// <param name="path">The path with the changes to discard.</param>
		private void CheckoutFile(string path)
		{
			try
			{
				using (var repo = new Repository(ConfigWindow.Config.path))
				{
					var opts = new CheckoutOptions();
					opts.CheckoutModifiers = CheckoutModifiers.Force;
					repo.CheckoutPaths("HEAD", new string[] { path }, opts);
				}
			}
			finally
			{
				GUI.FocusControl(null);
				if (toggled.Dictionary.ContainsKey(path))
				{
					toggled.Dictionary.Remove(path);
				}

				State.UpdateCache();
			}
		}

		/// <summary>
		/// Emulates the `git add .`, `git commit`, and `git push` commands.
		/// </summary>
		private void CommitAndPushToggled()
		{
			try
			{
				using (var repo = new Repository(ConfigWindow.Config.path))
				{
					foreach (var kvp in toggled.Dictionary)
					{
						if (all || kvp.Value)
						{
							Commands.Stage(repo, kvp.Key);
						}
					}

					var name = ConfigWindow.Config.name;
					var email = ConfigWindow.Config.email;
					var timestamp = DateTime.Now;
					var author = new Signature(name, email, timestamp);
					var message = commitSummary.Trim()
						+ "\n\n" + Wrap(commitMessage, 72);
					repo.Commit(message, author, author);

					// TODO: push
				}
			}
			finally
			{
				GUI.FocusControl(null);
				toggled.Dictionary.Clear();
				commitSummary = "";
				commitMessage = "";

				State.UpdateCache();
			}
		}

		#region Static

		[MenuItem("Window/Yggdrasil/Changes", priority = 0)]
		public static void Open()
		{
			var window = GetWindow<ChangesWindow>(false, "Changes", true);
			window.Show();
		}

		#endregion

		#region Util

		private bool HasCommitMessage()
		{
			return !string.IsNullOrEmpty(commitSummary.Trim());
		}

		private bool IsAnyToggled()
		{
			if (all) return true;
			foreach (var value in toggled.Dictionary.Values)
			{
				if (value) return true;
			}
			return false;
		}

		private static GUIContent Content(string tooltip)
		{
			return new GUIContent("", null, tooltip);
		}

		private static GUIContent Content(Texture texture, string tooltip)
		{
			return new GUIContent("", texture, tooltip);
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
