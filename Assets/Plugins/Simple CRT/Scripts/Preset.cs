using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
[System.Serializable]
public class Preset : ScriptableObject {

	public float downscale;
	public float screenCurve;
	public float horizontalScanlineStrength;
	public float verticalScanlineStrength;
	public int scanlineSize;
	public int scanlineSpacing;
	public float movScanlineStrength;
	public float movScanlineSize;
	public float movScanlineSpeed;
	public float sharpenStrength;
	public float brightness;
	public float contrast;
	public float saturation;
	public float vignetteStrength;
	public float chromatic;
	public Color replacementColor;
	public float noiseStrength;
	public float noiseDistortion;
	public float jitter;
	public float wobble;

}
