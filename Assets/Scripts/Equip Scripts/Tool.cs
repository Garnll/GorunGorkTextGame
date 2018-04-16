using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Gorun Gork/Equip/Herramienta")]
public class Tool : Equip {

	public enum ToolType { GeneralTool, DelegateTool, OperatorTool, PacificatorTool}
	public ToolType type;

	public override void put() {
		throw new NotImplementedException();
	}

	public override void remove() {
		throw new NotImplementedException();
	}
}
