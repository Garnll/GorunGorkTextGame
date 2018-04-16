using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Gorun Gork/Equip/Traje")]
public class Outfit : Equip {

	public enum OutfitType { NeutralOutfit, SystemOutfit, LibraryOutfit, GipsyOutfit}
	public OutfitType type;

	public override void put() {
		throw new NotImplementedException();
	}

	public override void remove() {
		throw new NotImplementedException();
	}
}
