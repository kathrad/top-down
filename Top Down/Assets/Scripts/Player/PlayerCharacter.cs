using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : Character
{
	public MovementController controller;
	public Animator animator;

	public static PlayerCharacter inst;

	private void Awake()
	{
		inst = this;
	}

	private void Update()
	{
		if (Input.GetMouseButtonDown(1))
		{
			animator.SetTrigger("Attack");
		}
	}

	public void Attack()
	{
		Debug.Log("Attacking");
	}

	protected override void Death()
	{
		base.Death();


		// TODO: death animation

		var rb = controller._body.GetComponent<Rigidbody>();
		rb.useGravity = true;
		rb.constraints = RigidbodyConstraints.None;
		rb.velocity = controller._body.velocity;
		rb.isKinematic = false;

		// Stop and disable movement controller to prevent movement after death
		controller.StopMovement();
		controller._body.enabled = false;
		controller.enabled = false;
	}
}
