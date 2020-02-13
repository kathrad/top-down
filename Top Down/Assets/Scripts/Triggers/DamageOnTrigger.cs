using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageOnTrigger : MonoBehaviour
{
	public TriggerMode mode;
	public int damage;
	public DamageType damageType;
	[Space]
	public CharacterTagFilter filter;

	private float lastTrigger;

	private void OnTriggerEnter(Collider other)
	{
		if (mode == TriggerMode.onEnter && filter.CompareTag(other.gameObject))
		{
			Trigger(other.gameObject);
		}
	}

	private void OnTriggerStay(Collider other)
	{
		if (mode == TriggerMode.continuous && filter.CompareTag(other.gameObject) && Time.time - lastTrigger > 1)
		{
			Trigger(other.gameObject);
			lastTrigger = Time.time;
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (mode == TriggerMode.onExit && filter.CompareTag(other.gameObject))
		{
			Trigger(other.gameObject);
		}
	}

	private void Trigger(GameObject gm)
	{
		Character c = gm.GetComponent<Character>();
		if (c != null)
		{
			c.Damage(damage, damageType);
		}
	}
}
