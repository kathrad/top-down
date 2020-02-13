using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stat
{
    public int Value { get; private set; }
	public int MaxValue { get; private set; }

	// Plain set value
	public void SetValue(int newValue)
	{
		Value = Mathf.Clamp(Value + newValue, 0, MaxValue);
	}

	// Set value to max multiplied by fraction
	public void SetFraction(float fraction)
	{
		Value = (int)(MaxValue * fraction);
	}

	// Add max value multiplied by fraction
	public int AddFraction(float fraction)
	{
		int lastValue = Value;
		Value = Mathf.Clamp(Value + (int)(MaxValue * fraction), 0, MaxValue);
		return Value - lastValue;
	}

	// Plain set max value
	public void SetMaxValue(int newMaxValue)
	{
		MaxValue = Mathf.Max(0, newMaxValue);
	}

	// Add int modifier to value clampled between 0 and max value
	public int ModifyValue(int modifier)
	{
		int lastValue = Value;
		Value = Mathf.Clamp(Value + modifier, 0, MaxValue);
		return Value - lastValue;
	}

	// Min max value = 1, clamp value between 0 and max
	public Stat(int value, int maxValue)
	{
		MaxValue = Mathf.Max(1, maxValue);
		Value = Mathf.Clamp(value, 0, MaxValue);
	}
}
