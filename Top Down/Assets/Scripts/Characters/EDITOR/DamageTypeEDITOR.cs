
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(DamageType))]
public class DamageTypeEDITOR : Editor
{
	DamageType dt;
	Color c;

	private void OnEnable()
	{
		dt = target as DamageType;
		var c = HexToRGB(dt.hexColor);
		if (c.HasValue)
		{
			this.c = c.Value;
		}
		else
		{
			this.c = Color.black;
		}
	}

	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();
		EditorGUILayout.Space();
		c = EditorGUILayout.ColorField("Color Picker", c);
		EditorGUILayout.BeginHorizontal();
		if (GUILayout.Button("Save"))
		{
			dt.hexColor = RGBToHex(c);
			return;
		}
		if (GUILayout.Button("Show Color"))
		{
			var c = HexToRGB(dt.hexColor);
			if (c.HasValue)
			{
				this.c = c.Value;
			}
			else
			{
				Debug.LogError("Invalid hex value");
			}
			return;
		}
		EditorGUILayout.EndHorizontal();
	}

	private string RGBToHex(Color color)
	{
		return "#" + ColorUtility.ToHtmlStringRGB(color);
	}

	private Color? HexToRGB(string hex)
	{
		if (ColorUtility.TryParseHtmlString(hex, out Color c))
		{
			return c;
		}
		else
		{
			return null;
		}
	}
}
