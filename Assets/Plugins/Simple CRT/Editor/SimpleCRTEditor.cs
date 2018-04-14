using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

[CustomEditor(typeof(SimpleCRT))]
public class SimpleCRTEditor : Editor {

	private SimpleCRT crt;
	private Object[] presets;

	private void OnEnable(){
		crt = (SimpleCRT)target;

		presets = Resources.LoadAll("", typeof(Preset));
		crt.presetNames = new string[presets.Length];
		for(int i = 0; i < presets.Length; i++){
			crt.presetNames[i] = presets[i].name;
		}
		if(crt.curPreset == null){
			crt.curPreset = (Preset)presets[crt.presetIndex];
		}
	}

	public override void OnInspectorGUI(){
		EditorGUILayout.Space();
		int prevIndex = crt.presetIndex;
		crt.presetIndex = EditorGUILayout.Popup("Preset", crt.presetIndex, crt.presetNames);
		if(prevIndex != crt.presetIndex){
			crt.curPreset = (Preset)presets[crt.presetIndex];
		}

		EditorGUILayout.Space();
		EditorGUILayout.LabelField("Screen Space", EditorStyles.boldLabel);
		crt.curPreset.downscale = EditorGUILayout.Slider("Downscale", crt.curPreset.downscale, 1f, 10f);
		crt.curPreset.screenCurve = EditorGUILayout.Slider("Screen Curve", crt.curPreset.screenCurve, 0f, 1f);
		EditorGUILayout.Space();
		EditorGUILayout.LabelField("Static Scanlines", EditorStyles.boldLabel);
		crt.curPreset.horizontalScanlineStrength = EditorGUILayout.Slider("Horizontal", crt.curPreset.horizontalScanlineStrength, 0f, 1f);
		crt.curPreset.verticalScanlineStrength = EditorGUILayout.Slider("Vertical", crt.curPreset.verticalScanlineStrength, 0f, 1f);
		crt.curPreset.scanlineSize = EditorGUILayout.IntSlider("Size", crt.curPreset.scanlineSize, 1, 20);
		crt.curPreset.scanlineSpacing = EditorGUILayout.IntSlider("Spacing", crt.curPreset.scanlineSpacing, 2, 6);
		EditorGUILayout.Space();
		EditorGUILayout.LabelField("Moving Scanlines", EditorStyles.boldLabel);
		crt.curPreset.movScanlineStrength = EditorGUILayout.Slider("Strength", crt.curPreset.movScanlineStrength, 0f, 1f);
		crt.curPreset.movScanlineSize = EditorGUILayout.Slider("Size", crt.curPreset.movScanlineSize, 0f, 1f);
		crt.curPreset.movScanlineSpeed = EditorGUILayout.Slider("Speed", crt.curPreset.movScanlineSpeed, 1f, 20f);
		EditorGUILayout.Space();
		EditorGUILayout.LabelField("Pixel Operations", EditorStyles.boldLabel);
		crt.curPreset.sharpenStrength = EditorGUILayout.Slider("Sharpen", crt.curPreset.sharpenStrength, 0f, 1f);
		crt.curPreset.brightness = EditorGUILayout.Slider("Brightness", crt.curPreset.brightness, -1f, 1f);
		crt.curPreset.contrast = EditorGUILayout.Slider("Contrast", crt.curPreset.contrast, -1f, 1f);
		crt.curPreset.saturation = EditorGUILayout.Slider("Saturation", crt.curPreset.saturation, -1, 1f);
		crt.curPreset.vignetteStrength = EditorGUILayout.Slider("Vignette", crt.curPreset.vignetteStrength, 0f, 1f);
		crt.curPreset.chromatic = EditorGUILayout.Slider("Chromatic Aberration", crt.curPreset.chromatic, 0f, 1f);
		crt.curPreset.replacementColor = EditorGUILayout.ColorField("Replacement Color", crt.curPreset.replacementColor);
		EditorGUILayout.Space();
		EditorGUILayout.LabelField("Noise", EditorStyles.boldLabel);
		crt.curPreset.noiseStrength = EditorGUILayout.Slider("Noise", crt.curPreset.noiseStrength, -1f, 1f);
		crt.curPreset.noiseDistortion = EditorGUILayout.Slider("Distortion", crt.curPreset.noiseDistortion, 0f, 1f);
		EditorGUILayout.Space();
		EditorGUILayout.LabelField("Glitches", EditorStyles.boldLabel);
		crt.curPreset.jitter = EditorGUILayout.Slider("Jitter", crt.curPreset.jitter, 0f, 1f);
		crt.curPreset.wobble = EditorGUILayout.Slider("Wobble", crt.curPreset.wobble, 0f, 1f);

		if(GUI.changed){
			crt.enabled = false;
			crt.enabled = true;
			EditorUtility.SetDirty(crt.curPreset);
			if(!Application.isPlaying){
				EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
			}
		}
	}
		
}
