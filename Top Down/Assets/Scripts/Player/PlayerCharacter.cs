using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : Character
{
	public MovementController _controller;
	public Animator _animator;
	public Transform _weaponParent;

	public Transform weaponTEMP;
	[Space]
	public float attackOriginOffset;
	public float attackDistance;
	public float attackRange;
	public int attackDamage;

	private Transform weapon;

	public static PlayerCharacter inst;

	private void Awake()
	{
		inst = this;
	}

	protected override void Start()
	{
		base.Start();

		// Input;
		var ic = InputController.inst;
		ic.onPoint.OnPrimary += OnPoint;
	}

	private void OnPoint(object sender, System.EventArgs e)
	{
		if (_controller._cameraController.Pointer.HasInteractable())
		{
			var i = _controller._cameraController.Pointer.GetInteractable();
			i.Activate();
			Debug.Log("Pointing at " + i.name);
		}
	}

	private void OnDrawGizmosSelected()
	{
		Vector3 pos = new Ray(transform.position + Vector3.up * attackOriginOffset, transform.forward).GetPoint(attackDistance);

		Gizmos.DrawWireSphere(pos, attackRange);
	}

	//private void Update()
	//{
	//	//TODO: combat input
	//	if (!Dead && false)
	//	{
	//		if (Input.GetMouseButtonDown(1))
	//		{
	//			StartAttack();
	//		}
	//		if (Input.GetKeyDown(KeyCode.Z))
	//		{
	//			_animator.SetTrigger("Sheathe");
	//		}
	//	}
	//}

	//private void StartAttack()
	//{
	//	_animator.SetTrigger("Attack");
	//	_controller.StopMovement();
	//	_controller.SetRotationMode(RotationMode.locked);
	//}

	//public void FinishAttack()
	//{
	//	_controller.SetRotationMode(RotationMode.direction);
	//	Debug.Log("2");
	//}

	//public void AttackPeak()
	//{
	//	if (Dead)
	//		return;

	//	Debug.Log("Attacking");

	//	Vector3 pos = new Ray(transform.position + Vector3.up * attackOriginOffset, transform.forward).GetPoint(attackDistance);

	//	Collider[] colliders = Physics.OverlapSphere(pos, attackRange);
	//	foreach (Collider c in colliders)
	//	{
	//		if (c.gameObject.Equals(gameObject))
	//			continue;

	//		IDamageable dm = c.GetComponent<IDamageable>();
	//		if (dm != null)
	//			dm.Damage(attackDamage, null);
	//	}
		
	//}

	//public void HideWeapon()
	//{
	//	if (Dead)
	//		return;

	//	if (weapon != null)
	//	{
	//		weapon.gameObject.SetActive(false);
	//	}
	//}

	//public void ShowWeapon()
	//{
	//	if (Dead)
	//		return;

	//	if (weapon == null)
	//	{
	//		weapon = Instantiate(weaponTEMP, _weaponParent);
	//		weapon.SetPositionAndRotation(_weaponParent.position, _weaponParent.rotation);
	//	}
	//	else
	//	{
	//		weapon.gameObject.SetActive(true);
	//	}
	//}

	protected override void Death()
	{
		base.Death();


		// TODO: death animation

		var rb = _controller._agent.GetComponent<Rigidbody>();
		rb.useGravity = true;
		rb.constraints = RigidbodyConstraints.None;
		rb.velocity = _controller._agent.velocity;
		rb.isKinematic = false;

		// Stop and disable movement controller to prevent movement after death
		_controller.StopMovement();
		_controller._agent.enabled = false;
		_controller.enabled = false;
	}
}
