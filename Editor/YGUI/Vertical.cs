using System;
using UnityEngine;

namespace Exodrifter.Yggdrasil.Internal
{
	public class Vertical : IDisposable
	{
		public Vertical()
		{
			GUILayout.BeginVertical();
		}

		public void Dispose()
		{
			GUILayout.EndVertical();
		}
	}
}
