using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EXPcontroller : MonoBehaviour
{
	public Animator anim;

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space))
			anim.SetTrigger("Move");
		else if (Input.GetKeyDown(KeyCode.F))
			anim.SetTrigger("Attack");
		else if (Input.GetKeyDown(KeyCode.E))
			anim.SetTrigger("Interact");
		else if (Input.GetKeyDown(KeyCode.H))
			anim.SetTrigger("Death");
		else if (Input.GetKeyDown(KeyCode.R))
			anim.SetTrigger("Ressurect");
	}
}
