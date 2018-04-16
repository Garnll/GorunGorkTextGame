using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Equip : ScriptableObject {

	public bool equiped;
	public abstract void put();
	public abstract void remove();

}
