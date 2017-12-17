using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ToggledDictionary : ISerializationCallbackReceiver
{
	public Dictionary<string, bool> Dictionary
	{
		get { return dictionary; }
	}
	private Dictionary<string, bool> dictionary = new Dictionary<string, bool>();

	[SerializeField]
	private List<string> keys;
	[SerializeField]
	private List<bool> values;

	public bool this[string key]
	{
		get { return dictionary[key]; }
		set { dictionary[key] = value; }
	}

	public void OnAfterDeserialize()
	{
		dictionary = new Dictionary<string, bool>();
		if (keys == null || values == null)
		{
			return;
		}

		var max = Mathf.Max(keys.Count, values.Count);
		for (int i = 0; i < max; ++i)
		{
			dictionary[keys[i]] = values[i];
		}
	}

	public void OnBeforeSerialize()
	{
		keys = new List<string>();
		values = new List<bool>();

		foreach (var kvp in dictionary)
		{
			keys.Add(kvp.Key);
			values.Add(kvp.Value);
		}
	}
}
