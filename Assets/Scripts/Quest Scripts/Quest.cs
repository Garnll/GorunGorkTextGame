using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Gorun Gork/Quest")]
public class Quest : ScriptableObject{

	public string questName;
	[Range(0, 10)] public int priority;
	public bool loopeable;

	[Space(5)]
	[TextArea] public string description;

	
	[Space(10)]
	public Step[] steps;
	[HideInInspector] public Step currentStep;

	private bool done;

	[Space(5)]
	public ChangeVarEffect[] vars;

	public void initialize() {

		done = false;
		foreach (Step s in steps) {
			s.initialize();
		}
		currentStep = steps[0];
		currentStep.applyEffects();
	}

	private bool isInFinalStep() {
		if (currentStep == steps[steps.Length - 1]) {
			return true;
		}
		return false;
	}

	private bool currentStepIsDone() {
		if (currentStep.isDone()) {
			return true;
		}
		return false;
	}

	public void score() {
		done = true;
	}

	public bool isDone() {
		if (done) {
			return true;
		}
		return false;
	}

	public void pass() {
		if (!isInFinalStep() && steps[getIndexOfCurrentStep() + 1].canBePassedTo() ) {
			currentStep.applyEffects();
			currentStep = steps[getIndexOfCurrentStep() + 1];
		} else {
			currentStep.applyEffects();
			applyEffects();
			score();
		}
	}

	public void applyEffects() {
		foreach (ChangeVarEffect v in vars) {
			v.apply();
		}
	}

	public int getIndexOfCurrentStep() {
		for (int i = 0; i < steps.Length; i++) {
			if (currentStep == steps[i]) {
				return i;
			}
		}
		return 0;
	}

	public void update() {
		if (currentStepIsDone()) {
			pass();
		}
	}

}
