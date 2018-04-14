using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class SimpleCRT : MonoBehaviour {

	public Shader shader;
	public Material mat;

	private float[] sharpenKernel = new float[]{0f, -1f, 0f,
												-1f, 5f, -1f,
												0f, -1f, 0f};

	public string[] presetNames;
	public int presetIndex = 0;	
	public Preset curPreset;

	void Awake(){
		if(shader == null){
			shader = Shader.Find("Hidden/CRT");
		}
		if(mat == null){
			mat = new Material(shader);
		}
		//mat = new Material(Shader.Find("Hidden/CRT") );
	}

	void Start(){
		if (!SystemInfo.supportsImageEffects || !mat.shader.isSupported){
			enabled = false;
		}
	}

	public void SetVariables(){
		mat.SetInt("_ScanlineSize", curPreset.scanlineSize);
		mat.SetFloat("_HorizontalScanlineStrength", curPreset.horizontalScanlineStrength);
		mat.SetFloat("_VerticalScanlineStrength", curPreset.verticalScanlineStrength);
		mat.SetInt("_ScanlineSpacing", curPreset.scanlineSpacing);
		mat.SetFloat("_MovScanlineSize", curPreset.movScanlineSize);
		mat.SetFloat("_MovScanlineSpeed", curPreset.movScanlineSpeed);
		mat.SetFloat("_MovScanlineStrength", curPreset.movScanlineStrength);
		mat.SetFloatArray("_SharpenKernel", sharpenKernel);
		mat.SetFloat("_SharpenStrength", curPreset.sharpenStrength);
		mat.SetFloat("_Brightness", curPreset.brightness);
		mat.SetFloat("_Contrast", curPreset.contrast);
		mat.SetFloat("_Saturation", curPreset.saturation);
		mat.SetFloat("_VignetteStrength", curPreset.vignetteStrength);
		mat.SetFloat("_Chromatic", curPreset.chromatic);
		mat.SetFloat("_NoiseStrength", curPreset.noiseStrength);
		mat.SetFloat("_NoiseDistortion", curPreset.noiseDistortion);
		mat.SetFloat("_Jitter", curPreset.jitter);
		mat.SetFloat("_Wobble", curPreset.wobble);
		mat.SetColor("_ReplacementColor", curPreset.replacementColor);
		mat.SetFloat("_ScreenCurve", curPreset.screenCurve);
	}

	void OnRenderImage(RenderTexture source, RenderTexture destination){
		SetVariables();

		var downSampled = RenderTexture.GetTemporary((int)(source.width/curPreset.downscale), (int)(source.height/curPreset.downscale));
		downSampled.filterMode = FilterMode.Point;
		Graphics.Blit(source, downSampled);

		var sharpened = RenderTexture.GetTemporary(downSampled.width, downSampled.height);
		Graphics.Blit(downSampled, sharpened, mat, 0);

		var mix = RenderTexture.GetTemporary(downSampled.width, downSampled.height);
		Graphics.Blit(sharpened, mix, mat, 1);

		var final = RenderTexture.GetTemporary(downSampled.width, downSampled.height);
		Graphics.Blit(mix, final, mat, 2);

		Graphics.Blit(final, destination);

		RenderTexture.ReleaseTemporary(downSampled);
		RenderTexture.ReleaseTemporary(sharpened);
		RenderTexture.ReleaseTemporary(mix);
		RenderTexture.ReleaseTemporary(final);
	}
}
