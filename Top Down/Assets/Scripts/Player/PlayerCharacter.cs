using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : Character
{
	public MovementController controller;

	public static PlayerCharacter inst;

	private void Awake()
	{
		inst = this;
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.DownArrow))
			Damage(-20, null);
		else if (Input.GetKeyDown(KeyCode.UpArrow))
			Damage(20, null);
		else if (Input.GetKeyDown(KeyCode.Space))
			Damage(-150, null);
	}

	protected override void Death()
	{
		base.Death();
		controller.StopMovement();
		controller.enabled = false;
	}
}
