using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnableExitEffect : Effect {
	public RoomObject room;
	public DirectionKeyword direction;

	public void apply() {
		foreach (Exit e in room.exits) {
			if (e.myKeyword == direction) {
				e.isAble = true;
			}
		}
	}
}
