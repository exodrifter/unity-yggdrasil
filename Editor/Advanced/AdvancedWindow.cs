using UnityEditor;
using UnityEngine;

namespace Exodrifter.Yggdrasil
{
	public class AdvancedWindow : EditorWindow
	{
		#region Unity

		[SerializeField]
		private Vector2 pos;

		private void OnGUI()
		{
			using (YGUI.ScrollView(ref pos))
			{
				if (YGUI.Button("Fetch"))
				{

				}
			}
		}

		#endregion

		#region Static

		[MenuItem("Window/Yggdrasil/Advanced", priority = 90)]
		public static void Open()
		{
			var window = GetWindow<AdvancedWindow>("Advanced");
			window.Show();
		}

		#endregion
	}
}