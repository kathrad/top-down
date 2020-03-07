using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
	// TODO: Proper input controller

	public static InputController inst;

	private void Awake()
	{
		inst = this;
	}

	public Keybind stopMovement;
	public Keybind onPoint;

	private void Update()
	{
		Handle(stopMovement);
		Handle(onPoint);
	}

	private void Handle(Keybind kb)
	{
		switch (kb.primaryMode)
		{
			case Keybind.Mode.none:
				return;
			case Keybind.Mode.mouseDown:
				if (Input.GetMouseButtonDown(kb.mouseButton))
					kb.InvokePrimary();
				break;
			case Keybind.Mode.mouseHeld:
				if (Input.GetMouseButton(kb.mouseButton))
					kb.InvokePrimary();
				break;
			case Keybind.Mode.mouseUp:
				if (Input.GetMouseButtonUp(kb.mouseButton))
					kb.InvokePrimary();
				break;
			case Keybind.Mode.keyDown:
				if (Input.GetKeyDown(kb.key))
					kb.InvokePrimary();
				break;
			case Keybind.Mode.keyHeld:
				if (Input.GetKey(kb.key))
					kb.InvokePrimary();
				break;
			case Keybind.Mode.keyUp:
				if (Input.GetKeyUp(kb.key))
					kb.InvokePrimary();
				break;
		}
		if (kb.secondaryMode == kb.primaryMode)
			return;

		switch (kb.secondaryMode)
		{
			case Keybind.Mode.none:
				return;
			case Keybind.Mode.mouseDown:
				if (Input.GetMouseButtonDown(kb.mouseButton))
					kb.InvokeSecondary();
				break;
			case Keybind.Mode.mouseHeld:
				if (Input.GetMouseButton(kb.mouseButton))
					kb.InvokeSecondary();
				break;
			case Keybind.Mode.mouseUp:
				if (Input.GetMouseButtonUp(kb.mouseButton))
					kb.InvokeSecondary();
				break;
			case Keybind.Mode.keyDown:
				if (Input.GetKeyDown(kb.key))
					kb.InvokeSecondary();
				break;
			case Keybind.Mode.keyHeld:
				if (Input.GetKey(kb.key))
					kb.InvokeSecondary();
				break;
			case Keybind.Mode.keyUp:
				if (Input.GetKeyUp(kb.key))
					kb.InvokeSecondary();
				break;
		}
	}
}

[System.Serializable]
public struct Keybind
{
	public int mouseButton;
	public KeyCode key;
	public Mode primaryMode;
	public Mode secondaryMode;

	public event System.EventHandler OnPrimary;
	public event System.EventHandler OnSecondary;


	public void InvokePrimary() => OnPrimary?.Invoke(this, System.EventArgs.Empty);
	public void InvokeSecondary() => OnSecondary?.Invoke(this, System.EventArgs.Empty);

	public enum Mode { none, mouseDown, mouseHeld, mouseUp, keyDown, keyHeld, keyUp }
}
