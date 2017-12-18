using System;
using UnityEngine;

namespace Exodrifter.Yggdrasil.Internal
{
	public class ScrollView : IDisposable
	{
		public ScrollView(ref Vector2 pos)
		{
			pos = GUILayout.BeginScrollView(pos);
		}

		public void Dispose()
		{
			GUILayout.EndScrollView();
		}
	}
}
