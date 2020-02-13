using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MovementController : MonoBehaviour
{
	public NavMeshAgent _body;
	public Camera _camera;
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

	private NavMeshAgent body;
	private Camera cam;
	private Transform pointer;
	private Coroutine pCoroutine;
	private bool pointerActive;

	private void Start()
	{
		// assign private variables, instantiate pointer and hide it
		body = _body;
		cam = _camera;
		pointer = Instantiate(_pointer, transform, true);
		pointer.name = "pointer";
		pointer.gameObject.SetActive(false);
	}

	private void Update()
	{
		// TODO: proper input system
		// Reset path
		if (Input.GetKeyDown(KeyCode.S))
			StopMovement();

		// Hide pointer at destination
		if (pointerActive && !body.hasPath)
		{
			pointerActive = false;
			pointer.gameObject.SetActive(false);
			pointer.position = Vector3.zero;
		}

		// Find destination
		if (Input.GetMouseButtonDown(0))
		{
			pCoroutine = StartCoroutine(ControlPointer());
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

	public void StopMovement()
	{
		// Reset path
		_body.ResetPath();

		pointerActive = false;
		pointer.gameObject.SetActive(false);
		pointer.position = Vector3.zero;
	}

	private IEnumerator ControlPointer()
	{
		// Find initial pointer position. Reset path and return if:
		// 1. No surface found
		// 2. Surface not horizontal
		// 3. Point not far enough away
		if (Physics.Raycast(cam.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, rayMaxDistance, rayMask) && hit.normal.y > pointerNormalThreshold && Vector3.Distance(body.transform.position, hit.point) >= pointerMinDistance)
		{
			pointer.gameObject.SetActive(true);
			pointerActive = true;
		}
		else
		{
			_body.ResetPath();
			yield break;
		}

		// Continuosly update pointer position until left mouse button is released, same return conditions, excluding distance check
		do
		{
			if (Physics.Raycast(cam.ScreenPointToRay(Input.mousePosition), out hit, rayMaxDistance, rayMask) && hit.normal.y > pointerNormalThreshold)
			{
				pointer.position = hit.point + Vector3.up * pointerOffset;
				SetDestination(hit.point);
			}
			yield return null;
		}
		while (Input.GetMouseButtonUp(0) == false);
	}

	private void SetDestination(Vector3 point)
	{
		body.SetDestination(point);
	}
}
