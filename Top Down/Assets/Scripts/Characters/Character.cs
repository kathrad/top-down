using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
	public Vector2Int _health;

	public Stat Health { get; private set; }

	private void Start()
	{
		Health = new Stat(_health.x, _health.y);
	}
}
