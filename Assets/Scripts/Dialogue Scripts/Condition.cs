using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Condition {
	public enum ConditionType { Equal, Smaller, SmallerOrEqual, Greater, GreaterOrEqual }

	public GlobalVariable variable;
	public ConditionType condition;
	public int value;

	public void createGlobalVariable() {
		if (!GlobalVariables.ContainsVariable(variable.name)) {
			GlobalVariables.AddNewAs(variable.name, variable.value);
		}
	}

	public bool isTrue() {
		bool t = false;

		switch (condition) {
			case ConditionType.Equal:
				if (GlobalVariables.GetValueOf(variable.name) == value) {
					t = true;
				}
				break;
			case ConditionType.Smaller:
				if (GlobalVariables.GetValueOf(variable.name) < value) {
					t = true;
				}
				break;
			case ConditionType.SmallerOrEqual:
				if (GlobalVariables.GetValueOf(variable.name) <= value) {
					t = true;
				}
				break;
			case ConditionType.Greater:
				if (GlobalVariables.GetValueOf(variable.name) > value) {
					t = true;
				}
				break;
			case ConditionType.GreaterOrEqual:
				if (GlobalVariables.GetValueOf(variable.name) >= value) {
					t = true;
				}
				break;
		}

		return t;
	}

}
