using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ChangeVarEffect : Effect{

	public enum Operation { Set, Add, Substract}

	public string variableName;
	public Operation operation;
	public int value;
	
	public void apply() {
		if (checkVar() == false) {
			addVar();
		}

		switch (operation) {
			case (Operation.Set):
				GlobalVariables.SetValue(variableName, value);
				break;

			case (Operation.Add):
				GlobalVariables.SetValue(variableName, GlobalVariables.GetValueOf(variableName) + value);
				break;

			case (Operation.Substract):
				GlobalVariables.SetValue(variableName, GlobalVariables.GetValueOf(variableName) - value);
				break;		
		}
	}

	public bool checkVar() {
		if (GlobalVariables.ContainsVariable(variableName)) {
			return true;
		}
		return false;
	}

	public void addVar() {
		GlobalVariables.AddNewAs(variableName, 0);
	}
}
