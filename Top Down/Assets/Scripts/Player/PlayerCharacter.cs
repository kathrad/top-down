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
			Damage(20, null);
		else if (Input.GetKeyDown(KeyCode.UpArrow))
			Damage(-20, null);
		else if (Input.GetKeyDown(KeyCode.Space))
			Damage(150, null);
	}

	protected override void Death()
	{
		base.Death();

		// Stop and disable movement controller to prevent movement after death
		controller.StopMovement();
		controller.enabled = false;

		// TODO: death animation

		var rb = controller._body.GetComponent<Rigidbody>();
		rb.useGravity = true;
		rb.constraints = RigidbodyConstraints.None;
		rb.velocity = controller._body.velocity;
		controller._body.enabled = false;
	}
}
