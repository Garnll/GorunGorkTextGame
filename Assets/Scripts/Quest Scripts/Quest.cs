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
	public ChangeVarEffect[] varEffects;
	public EnableExitEffect[] exitEffects;

	public int getNumberOfSteps() {
		if (steps == null) {
			return 0;
		}
		int count = 0;
		for (int i = 0; i<steps.Length; i++) {
			count++;
		}
		return count;
	}

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
		return done;
	}

	public void pass() {
		if (!isInFinalStep() && currentStepIsDone()) {
			if (steps[getIndexOfCurrentStep() + 1].canBePassedTo()) {
				currentStep.applyEffects();
				currentStep = steps[getIndexOfCurrentStep() + 1];
			}
		} else {
			currentStep.applyEffects();
			applyEffects();
			score();
		}
	}

	public void applyEffects() {
		foreach (ChangeVarEffect v in varEffects) {
			v.apply();
		}

		foreach (EnableExitEffect e in exitEffects) {
			e.apply();
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
