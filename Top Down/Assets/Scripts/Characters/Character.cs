using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour, IDamageable
{
	public Vector2Int _health;

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
		// Don't modify health after character death
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

		// If health reached zero - dead
		if (HP.Value == 0)
			Death();
	}

	protected virtual void Death()
	{
		Dead = true;
		Debug.Log(name + " dies");
		OnDeath?.Invoke(this, System.EventArgs.Empty);
	}	

	protected virtual void Start()
	{
		HP = new Stat(_health.x, _health.y);
	}

	public enum State { idle, moving, attac}
}
