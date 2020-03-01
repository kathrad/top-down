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
		switch (kb.mode)
		{
			case Keybind.Mode.mouseDown:
				if (Input.GetMouseButtonDown(kb.mouseButton))
					kb.Invoke();
				break;
			case Keybind.Mode.mouseHeld:
				if (Input.GetMouseButton(kb.mouseButton))
					kb.Invoke();
				break;
			case Keybind.Mode.mouseUp:
				if (Input.GetMouseButtonUp(kb.mouseButton))
					kb.Invoke();
				break;
			case Keybind.Mode.keyDown:
				if (Input.GetKeyDown(kb.key))
					kb.Invoke();
				break;
			case Keybind.Mode.keyHeld:
				if (Input.GetKey(kb.key))
					kb.Invoke();
				break;
			case Keybind.Mode.keyUp:
				if (Input.GetKeyUp(kb.key))
					kb.Invoke();
				break;
		}
	}
}

[System.Serializable]
public struct Keybind
{
	public int mouseButton;
	public KeyCode key;
	public Mode mode;

	public event System.EventHandler OnInvoked;

	public void Invoke() => OnInvoked?.Invoke(this, System.EventArgs.Empty);

	public enum Mode { mouseDown, mouseHeld, mouseUp, keyDown, keyHeld, keyUp }
}
