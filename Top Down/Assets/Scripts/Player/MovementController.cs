using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MovementController : MonoBehaviour
{
	public NavMeshAgent _agent;
	public Transform _body;
	public CameraController _cameraController;
	public Transform _pointer;
	[Space]
	public LayerMask rayMask;
	public float pointerMinDistance;
	public float pointerDelay;
	public float rayMaxDistance;
	public float pointerNormalThreshold;
	public float pointerOffset;
	[Space]
	public float runSpeed;
	public float walkSpeed;
	public float walkThreshold;
	[Space]
	public float lockedAngularSpeed;
	public int lockedRayUpdateRate;

	// Determines how the controller handles rotation.
	// Direction: rotates towards the next waypoint
	// Locked: towards cursor
	// None: rotation is disabled
	public RotationMode RotationMode { get; private set; }

	private NavMeshAgent body;
	private Transform pointer;
	private Coroutine cPointer;
	private bool pointerActive;
	private bool releasePointer;

	private Coroutine manualRot;
	private Coroutine hidePointer;

	private void Start()
	{
		// assign private variables, instantiate pointer and hide it
		body = _agent;
		pointer = Instantiate(_pointer, transform, true);
		pointer.name = "pointer";
		pointer.gameObject.SetActive(false);

		// input
		var ic = InputController.inst;
		ic.stopMovement.OnPrimary += OnStopMovement;
		ic.onPoint.OnPrimary += OnSetDestination;
		ic.onPoint.OnSecondary += OnReleasePointer;
	}

	private void OnSetDestination(object sender, System.EventArgs e)
	{
		if (!_cameraController.Pointer.HasInteractable())
			cPointer = StartCoroutine(ControlPointer());
		
	}

	private void OnReleasePointer(object sender, System.EventArgs e)
	{
		Debug.Log("Pointer released");
		releasePointer = true;
	}

	private void OnStopMovement(object sender, System.EventArgs e)
	{
		StopMovement();
	}

	private void Update()
	{
		// Hide pointer upon reaching the destination
		if (pointerActive && !body.hasPath)
		{
			HidePointer();
		}

		// Switch between walking and running speeds
		if (body.remainingDistance > walkThreshold)
		{
			body.speed = runSpeed;
		}
		else
		{
			body.speed = walkSpeed;
		}
	}

	public void SetRotationMode(RotationMode mode)
	{
		RotationMode = mode;

		switch (mode)
		{
			case RotationMode.direction:
				_agent.updateRotation = true;
				if (manualRot != null)
					StopCoroutine(manualRot);
				break;
			case RotationMode.locked:
				// Switch to manual rotation to 
				if (manualRot == null)
				{
					_agent.updatePosition = false;
					manualRot = StartCoroutine(ManualRotation());
				}
				break;
			case RotationMode.none:
				// Stop rotation through navmesh agent
				_agent.updateRotation = false;
				if (manualRot != null)
					StopCoroutine(manualRot);
				break;
		}
	}

	private IEnumerator ManualRotation()
	{
		Vector3 point = _cameraController.GetPointOnPlaneFromView(_body.transform.position.y);
		Quaternion target = Quaternion.LookRotation(point - _body.transform.position, Vector3.up);
		while (true)
		{
			if (Time.frameCount % lockedRayUpdateRate == 0)
			{
				point = _cameraController.GetPointOnPlaneFromView(_body.transform.position.y);
				target = Quaternion.LookRotation(point - _body.transform.position, Vector3.up);
				Debug.DrawRay(_body.transform.position, point - _body.transform.position);
			}

			_body.transform.rotation = Quaternion.Lerp(_body.transform.rotation, target, lockedAngularSpeed);

			Debug.Log("1");

			yield return null;
		}
	}

	public void StopMovement()
	{
		// Reset path
		_agent.ResetPath();

		HidePointer();

		if (cPointer != null)
			StopCoroutine(cPointer);
	}

	private IEnumerator ControlPointer()
	{
		// Cancel hiding the pointer
		if (hidePointer != null)
			StopCoroutine(hidePointer);

		// Not to be confused with pointer the object
		Pointer viewPointer = _cameraController.GetPointer(rayMask);

		// Find initial pointer position. Reset path and return if:
		// 1. No surface found
		// 2. Surface not horizontal
		// 3. Point not far enough away
		if (viewPointer.active && viewPointer.hit.normal.y > pointerNormalThreshold && Vector3.Distance(body.transform.position, viewPointer.hit.point) >= pointerMinDistance)
		{
			pointer.transform.position = viewPointer.hit.point;
			pointer.gameObject.SetActive(true);
			pointerActive = true;
		}
		else
		{
			_agent.ResetPath();
			yield break;
		}

		// Hide pointer after delay
		hidePointer = StartCoroutine(HidePointerAfterDelay());

		// Continuosly update destination until pointer button is released. Hide pointer object after delay
		do
		{
			if (Time.frameCount % _cameraController.pointerUpdateRate == 0)
			{
				viewPointer = _cameraController.GetPointer(rayMask);

				if (viewPointer.active && viewPointer.hit.normal.y > pointerNormalThreshold)
				{
					SetDestination(_cameraController.Pointer.hit.point);
				}
			}

			yield return null;
		}
		while (!releasePointer);

		releasePointer = false;
	}

	private IEnumerator HidePointerAfterDelay()
	{
		for (float t = 0; t < pointerDelay; t += Time.deltaTime) { yield return null; }
		HidePointer();
	}

	private void HidePointer()
	{
		pointerActive = false;
		pointer.gameObject.SetActive(false);
		pointer.position = Vector3.zero;
	}

	private void SetDestination(Vector3 point)
	{
		body.SetDestination(point);
	}

}

public enum RotationMode { direction, locked, none }