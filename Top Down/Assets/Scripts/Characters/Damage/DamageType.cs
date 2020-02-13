using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New damage type", menuName = "Characters/Damage Type")]
public class DamageType : ScriptableObject
{
	public new string name;
	public string hexColor;
}
