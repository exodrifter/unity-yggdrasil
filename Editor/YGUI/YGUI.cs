using Exodrifter.Yggdrasil.Internal;
using System;
using UnityEditor;
using UnityEngine;

namespace Exodrifter.Yggdrasil
{
	/// <summary>
	/// Utility GUI functions for Yggdrasil.
	/// </summary>
	public static class YGUI
	{
		#region GUI Elements

		public static void ErrBox(string str, params object[] opts)
		{
			var message = string.Format(str, opts);
			EditorGUILayout.HelpBox(message, MessageType.Error);
		}

		public static void InfoBox(string str, params object[] opts)
		{
			var message = string.Format(str, opts);
			EditorGUILayout.HelpBox(message, MessageType.Info);
		}

		public static void WarnBox(string str, params object[] opts)
		{
			var message = string.Format(str, opts);
			EditorGUILayout.HelpBox(message, MessageType.Warning);
		}

		public static bool Button(string label)
		{
			return GUILayout.Button(label);
		}

		public static void Label(string label, FontStyle? style = null)
		{
			var oldStyle = GUI.skin.label.fontStyle;
			GUI.skin.label.fontStyle = style ?? FontStyle.Normal;

			GUILayout.Label(label);

			GUI.skin.label.fontStyle = oldStyle;
		}

		public static void Prefix(string label)
		{
			EditorGUILayout.PrefixLabel(label);
		}

		public static void Space(float? pixels = null)
		{
			if (pixels.HasValue)
			{
				GUILayout.Space(pixels.Value);
			}
			else
			{
				EditorGUILayout.Space();
			}
		}

		public static void TextField(ref string value)
		{
			value = EditorGUILayout.TextField(value);
		}

		public static string TextField(string value)
		{
			return EditorGUILayout.TextField(value);
		}

		#endregion

		#region Begin/End

		public static Horizontal Horizontal()
		{
			return new Horizontal();
		}

		public static ScrollView ScrollView(ref Vector2 pos)
		{
			return new ScrollView(ref pos);
		}

		public static Vertical Vertical()
		{
			return new Vertical();
		}

		#endregion
	}
}
