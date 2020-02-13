using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageOnTrigger : MonoBehaviour
{
	public Mode mode;
	public int damage;
	public DamageType damageType;
	[Space]
	public CharacterTagFilter filter;

	private float lastTrigger;

	private void OnTriggerEnter(Collider other)
	{
		if (mode == Mode.onEnter && filter.CompareTag(other.gameObject))
		{
			Trigger(other.gameObject);
		}
	}

	private void OnTriggerStay(Collider other)
	{
		if (mode == Mode.continuous && filter.CompareTag(other.gameObject) && Time.time - lastTrigger > 1)
		{
			Trigger(other.gameObject);
			lastTrigger = Time.time;
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (mode == Mode.onExit && filter.CompareTag(other.gameObject))
		{
			Trigger(other.gameObject);
		}
	}

	private void Trigger(GameObject gm)
	{
		Character c;
		if (gm.CompareTag("Player"))
		{
			c = PlayerCharacter.inst;
		}
		else
		{
			c = gm.GetComponent<Character>();
		}
		if (c != null)
		{
			c.Damage(damage, damageType);
		}
	}

	public enum Mode { continuous, onEnter, onExit }
}
