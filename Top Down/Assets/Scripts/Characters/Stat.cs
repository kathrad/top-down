using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stat
{
    public int Value { get; private set; }
	public int MaxValue { get; private set; }

	public void SetValue(int newValue)
	{
		Value = Mathf.Clamp(Value + newValue, 0, MaxValue);
	}

	public void SetFraction(float fraction)
	{
		Value = (int)(MaxValue * fraction);
	}

	public void AddFraction(float fraction)
	{
		Value = Mathf.Clamp(Value + (int)(MaxValue * fraction), 0, MaxValue);
	}

	public void SetMaxValue(int newMaxValue)
	{
		MaxValue = Mathf.Max(0, newMaxValue);
	}

	public void ModifyValue(int modifier)
	{
		Value = Mathf.Clamp(Value + modifier, 0, MaxValue);
	}

	public Stat(int value, int maxValue)
	{
		MaxValue = Mathf.Max(0, maxValue);
		Value = Mathf.Clamp(value, 0, MaxValue);
	}
}
