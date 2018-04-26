using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Equip : InteractableObject {

	[Space(10)]
	public MainBuff[] intBuffs;
	public OtherBuff[] floatBuffs;


	public abstract void put();
	public abstract void remove();

}
