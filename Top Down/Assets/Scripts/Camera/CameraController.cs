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

	public bool PointingAtCharacter { get; private set; }
	public bool PointingAtInteractable { get; private set; }

	private const float POINTER_MAX_DISTANCE = 100;

	public event System.Action<Character> OnPointingAtCharacter;
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
		Pointer = new Pointer()
		{
			active = false,
		};
	}

	private void Start()
	{
		OnPointingAtCharacter += (o) => Debug.Log("Looking at " + o.name);
		OnPointingAtInteractable += (o) => Debug.Log("Looking at " + o.name);
		OnPointerLost += (o, a) => Debug.Log("Pointer lost");
	}

	private void Update()
	{
		if (pointerUpdateRate == 0 || Time.frameCount % pointerUpdateRate == 0)
		{
			Pointer = GetPointer(pointerLayerMask);
			var c = Pointer.GetCharacter();
			if (c != null && c != PlayerCharacter.inst)
			{
				if (!PointingAtCharacter)
					OnPointingAtCharacter?.Invoke(c);
				PointingAtCharacter = true;
			}
			else
			{
				if (PointingAtCharacter)
					OnPointerLost?.Invoke(this, System.EventArgs.Empty);
				PointingAtCharacter = false;
			}

			var i = Pointer.GetInteractable();
			if (i != null)
			{
				if (!PointingAtInteractable)
					OnPointingAtInteractable?.Invoke(i);
				PointingAtInteractable = true;
			}
			else
			{
				if (PointingAtInteractable)
					OnPointerLost?.Invoke(this, System.EventArgs.Empty);
				PointingAtInteractable = false;
			}
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
	public Pointer GetPointer(LayerMask layerMask)
	{
		if (Physics.Raycast(cam.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, POINTER_MAX_DISTANCE, layerMask))
		{
			return new Pointer()
			{
				active = true,
				hit = hit,
			};
		}
		else
		{
			return new Pointer()
			{
				active = false,
			};
		}
	}
}

public struct Pointer
{
	public bool active;
	public RaycastHit hit;

	public Character GetCharacter() => active ? hit.transform.GetComponent<Character>() : null;
	public Interactable GetInteractable() => active ? hit.transform.GetComponent<Interactable>() : null;
}