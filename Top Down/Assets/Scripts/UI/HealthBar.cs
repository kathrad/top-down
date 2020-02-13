using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
	public Slider slider;
	public Image fill;
	[Space]
	public Character target;
	public bool targetPlayer;
	public float transitionSpeed;
	[Space]
	public Color normalColor;
	public Color transitionColor;

	private float valueTarget;
	private bool trans;
	private Coroutine colorAnim;

	private void Start()
	{
		if (targetPlayer)
		{
			target = PlayerCharacter.inst;
			target.OnHPChanged += Target_OnHealthChanged;
		}
		slider.value = (float)target.HP.Value / target.HP.MaxValue;
		fill.color = normalColor;
	}

	private void OnEnable()
	{
		if (target != null && !targetPlayer)
		{
			target.OnHPChanged += Target_OnHealthChanged;
		}
	}

	private void OnDisable()
	{
		target.OnHPChanged -= Target_OnHealthChanged;
	}

	private void Update()
	{
		if (trans)
		{
			float i = Time.deltaTime * transitionSpeed;

			if (slider.value < valueTarget)
			{
				if (slider.value + i >= valueTarget)
				{
					slider.value = valueTarget;
					trans = false;
				}
				else
					slider.value += i;

			}
			else if (slider.value > valueTarget)
			{
				if (slider.value - i <= valueTarget)
				{
					slider.value = valueTarget;
					trans = false;
				}
				else
					slider.value -= i;
			}
			else
				trans = false;
		}
	}

	private void Target_OnHealthChanged(object sender, Character.StatEventArgs e)
	{
		if (e.delta == 0)
			return;

		valueTarget = (float)e.value / e.maxValue;
		trans = true;
		if (colorAnim == null)
			colorAnim = StartCoroutine(ColorAnim());
	}

	private IEnumerator ColorAnim()
	{
		fill.color = transitionColor;
		yield return new WaitUntil(() => trans == false);
		fill.color = normalColor;

		colorAnim = null;
	}
}
