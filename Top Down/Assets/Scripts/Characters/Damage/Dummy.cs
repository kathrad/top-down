using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dummy : Interactable, IDamageable
{
	public void Damage(int amount, DamageType damageType)
	{
		Debug.Log("Dummy takes " + amount + " damage");
	}
}
