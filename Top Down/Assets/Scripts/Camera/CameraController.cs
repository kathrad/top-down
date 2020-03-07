using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
	public static CameraController inst;
	public Pointer Pointer { get; private set; }

	[Space]
	public int pointerUpdateRate;
	public LayerMask pointerLayerMask;

	private const float POINTER_MAX_DISTANCE = 100;

	public event System.Action<Interactable> OnPointingAtInteractable;
	public event System.EventHandler OnPointerLost;

	private void OnDrawGizmos()
	{
		if (Pointer.active)
			Gizmos.DrawWireSphere(Pointer.hit.point, 0.5f);
	}

	public Camera cam;

	private void Awake()
	{
		inst = this;
		Pointer = new Pointer(false);
	}

	private void Start()
	{
		OnPointingAtInteractable += (o) => Debug.Log("Looking at " + o.name);
		OnPointerLost += (o, a) => Debug.Log("Pointer lost");
	}

	private void Update()
	{
		if (pointerUpdateRate == 0 || Time.frameCount % pointerUpdateRate == 0)
		{
			bool hadInteractable = Pointer.HasInteractable();
			Pointer = GetPointer(pointerLayerMask);

			Debug.Log(hadInteractable + " " + Pointer.HasInteractable());

			if (hadInteractable && !Pointer.HasInteractable())
				OnPointerLost?.Invoke(this, System.EventArgs.Empty);
			else if (!hadInteractable && Pointer.HasInteractable())
				OnPointingAtInteractable?.Invoke(Pointer.GetInteractable());
		}
	}

	public Vector3 GetPointOnPlaneFromView(float planeY)
	{
		// Math
		Ray r = cam.ScreenPointToRay(Input.mousePosition);
		float rad = Vector3.Angle(Vector3.down, r.direction.normalized) * Mathf.Deg2Rad;
		return r.GetPoint((transform.position.y - planeY) / Mathf.Cos(rad));
	}

	// Cast a ray from camera to mouse cursor
	public Pointer GetPointer(LayerMask layerMask, bool getInteractable = false)
	{
		if (Physics.Raycast(cam.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, POINTER_MAX_DISTANCE, layerMask))
		{
			if (getInteractable)
				return new Pointer(true, hit, hit.transform.GetComponent<Interactable>());
			else
				return new Pointer(true, hit);
		}
		else
		{
			return new Pointer(false);
		}
	}
}

public struct Pointer
{
	public bool active;
	public RaycastHit hit;
	private readonly Interactable interactable;

	public Pointer(bool active, RaycastHit hit, Interactable interactable)
	{
		this.active = active;
		this.hit = hit;
		this.interactable = interactable;
	}

	public Pointer(bool active, RaycastHit hit)
	{
		this.active = active;
		this.hit = hit;
		this.interactable = null;
	}

	public Pointer(bool active) : this()
	{
		this.active = active;
	}

	public Interactable GetInteractable() => active ? interactable : null;
	public bool HasInteractable() => active && interactable != null;
}