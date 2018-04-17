using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Equip : InteractableObject {

	[Space(10)]
	public IntBuff[] intBuffs;
	public FloatBuff[] floatBuffs;


	public abstract void put();
	public abstract void remove();

}
