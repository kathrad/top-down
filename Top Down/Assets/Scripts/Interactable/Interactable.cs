using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
	public event System.EventHandler OnActivated;

	protected virtual void Start()
	{

	}

	public virtual void Activate()
	{
		Debug.Log(name + " activated");
		OnActivated?.Invoke(this, System.EventArgs.Empty);
	}
}
