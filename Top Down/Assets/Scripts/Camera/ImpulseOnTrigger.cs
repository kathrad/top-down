using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

[RequireComponent(typeof(CinemachineImpulseSource))]
public class ImpulseOnTrigger : MonoBehaviour
{
    public CinemachineImpulseSource source;

    private void OnTriggerEnter(Collider other)
    {
		Debug.Log("Triggered");
		if (other.CompareTag("Player"))
			source.GenerateImpulse();
    }
}
