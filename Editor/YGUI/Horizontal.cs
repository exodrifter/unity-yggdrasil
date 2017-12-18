using System;
using UnityEngine;

namespace Exodrifter.Yggdrasil.Internal
{
	public class Horizontal : IDisposable
	{
		public Horizontal()
		{
			GUILayout.BeginHorizontal();
		}

		public void Dispose()
		{
			GUILayout.EndHorizontal();
		}
	}
}
