using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct CharacterTagFilter
{
	public bool player;
	public bool friendly;
	public bool hostile;

	public bool CompareTag(GameObject gm)
	{
		return (player && gm.CompareTag("Player")) || (hostile && gm.CompareTag("Hostile")) || (friendly && gm.CompareTag("Friendly"));
	}
}
