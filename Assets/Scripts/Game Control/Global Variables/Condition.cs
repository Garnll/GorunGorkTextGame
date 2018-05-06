using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Condition {
	public enum ConditionType { Equal, Smaller, SmallerOrEqual, Greater, GreaterOrEqual, Different }

	public string variable;
	public ConditionType condition;
	public int value;

	public void createGlobalVariable() {
		if (!GlobalVariables.ContainsVariable(variable)) {
			GlobalVariables.AddNewAs(variable, 0); //Cambiar este 0 por Value para que por defecto aparezca la opción.
		}
	}

	public void createLocalVariable(string name) {
		if (!GlobalVariables.ContainsVariable(name + "_" + variable)) {
			GlobalVariables.AddNewAs(name + "_" + variable, 0); //Cambiar este 0 por Value para que por defecto aparezca la opción.
		}
	}

	public bool isTrue() {
		bool t = false;

		switch (condition) {
			case ConditionType.Equal:
				if (GlobalVariables.GetValueOf(variable) == value) {
					t = true;
				}
				break;
			case ConditionType.Smaller:
				if (GlobalVariables.GetValueOf(variable) < value) {
					t = true;
				}
				break;
			case ConditionType.SmallerOrEqual:
				if (GlobalVariables.GetValueOf(variable) <= value) {
					t = true;
				}
				break;
			case ConditionType.Greater:
				if (GlobalVariables.GetValueOf(variable) > value) {
					t = true;
				}
				break;
			case ConditionType.GreaterOrEqual:
				if (GlobalVariables.GetValueOf(variable) >= value) {
					t = true;
				}
				break;
			case ConditionType.Different:
				if (GlobalVariables.GetValueOf(variable) != value) {
					t = true;
				}
				break;
		}

		return t;
	}

}
