using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour, IDamageable
{
	public Vector2Int _health;

	public Stat Health { get; private set; }

	public bool Dead { get; private set; }

	//EVENTS
	public event System.EventHandler<StatEventArgs> OnHealthChanged;
	public event System.EventHandler OnDeath;

	public class StatEventArgs : System.EventArgs
	{
		public int value, delta, maxValue;
	}

	public virtual void Damage(int amount, DamageType damageType)
	{
		if (Dead)
			return;

		if (amount < 0)
			Debug.Log(name + " takes " + -amount + " damage");
		else
			Debug.Log(name + " is healed by " + amount);
		// TODO: resistances
		int lastValue = Health.Value;
		Health.ModifyValue(amount);
		OnHealthChanged?.Invoke(this, new StatEventArgs { value = Health.Value, delta = lastValue - Health.Value, maxValue = Health.MaxValue });

		if (Health.Value == 0)
			Death();
	}

	protected virtual void Death()
	{
		Dead = true;
		Debug.Log(name + " dies");
		OnDeath?.Invoke(this, System.EventArgs.Empty);
	}	

	private void Start()
	{
		Health = new Stat(_health.x, _health.y);
	}
}
