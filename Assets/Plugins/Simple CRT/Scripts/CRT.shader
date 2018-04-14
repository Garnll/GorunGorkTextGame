Shader "Hidden/CRT"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
	}
	SubShader
	{
		// No culling or depth
		Cull Off ZWrite Off ZTest Always

		Pass
		{
			CGPROGRAM
			#include "CRT.cginc"
			#pragma vertex vert
			#pragma fragment fragSharpen
			ENDCG
		}

		Pass
		{
			CGPROGRAM
			#include "CRT.cginc"
			#pragma vertex vert
			#pragma fragment fragMix
			ENDCG
		}

		Pass
		{
			CGPROGRAM
			#include "CRT.cginc"
			#pragma vertex vert
			#pragma fragment fragFinal
			ENDCG
		}
	}
}
