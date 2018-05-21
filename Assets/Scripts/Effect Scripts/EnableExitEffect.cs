using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnableExitEffect : Effect {
	public RoomObject room;
	public DirectionKeyword direction;

	public bool enable = true;

	public void apply() {
		foreach (Exit e in room.exits) {
			if (e.myKeyword == direction) {
				if (enable) {
					e.isAble = true;
				} else {
					e.isAble = false;
				}
			}
		}
	}
}
