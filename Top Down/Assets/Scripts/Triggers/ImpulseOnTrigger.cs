using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

[RequireComponent(typeof(CinemachineImpulseSource))]
public class ImpulseOnTrigger : MonoBehaviour
{
    public CinemachineImpulseSource source;
	[Space]
	public TriggerMode mode;

	private void Start()
	{
		if (source == null)
			source = GetComponent<CinemachineImpulseSource>();
	}

	private void OnTriggerStay(Collider other)
    {
		if (mode == TriggerMode.continuous && other.CompareTag("Player"))
			source.GenerateImpulse();
    }

	private void OnTriggerEnter(Collider other)
	{
		if (mode == TriggerMode.onEnter && other.CompareTag("Player"))
			source.GenerateImpulse();
	}

	private void OnTriggerExit(Collider other)
	{
		if (mode == TriggerMode.onExit && other.CompareTag("Player"))
			source.GenerateImpulse();
	}
}
