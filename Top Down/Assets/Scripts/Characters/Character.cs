using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : Interactable, IDamageable
{
	public Vector2Int _hp	;

	// Hit points or health
	public Stat HP { get; private set; }

	public bool Dead { get; private set; }

	//EVENTS
	public event System.EventHandler<StatEventArgs> OnHPChanged;
	public event System.EventHandler OnDeath;

	public class StatEventArgs : System.EventArgs
	{
		public int value, delta, maxValue;
	}

	// Derived from IDamageable. Negative value = damage, positive = healing
	public virtual void Damage(int amount, DamageType damageType)
	{
		// Don't modify hp after character death
		if (Dead)
			return;

		if (amount > 0)
			Debug.Log(name + " takes " + amount + " damage");
		else
			Debug.Log(name + " is healed by " + -amount);

		// TODO: damage resistances
		// pass the difference of previous and current hit points
		int deltaHealth = HP.ModifyValue(-amount);
		OnHPChanged?.Invoke(this, new StatEventArgs { value = HP.Value, delta = deltaHealth, maxValue = HP.MaxValue });

		// If hp reached zero - dead
		if (HP.Value == 0)
			Death();
	}

	protected virtual void Death()
	{
		Dead = true;
		Debug.Log(name + " dies");
		OnDeath?.Invoke(this, System.EventArgs.Empty);
	}	

	protected override void Start()
	{
		// Initialize hp
		HP = new Stat(_hp.x, _hp.y);
	}

	public enum State { idle, moving, attac}
}
