#pragma target 3.0
#include "UnityCG.cginc"

struct appdata
{
	float4 vertex : POSITION;
	float2 uv : TEXCOORD0;
};

struct v2f
{
	float2 uv : TEXCOORD0;
	float4 vertex : SV_POSITION;
};

v2f vert (appdata v)
{	
	v2f o;
	o.vertex = UnityObjectToClipPos(v.vertex);
	o.uv = v.uv;
	return o;
}

sampler2D _MainTex;
float2 _MainTex_TexelSize;
int _ScanlineSize;
half _HorizontalScanlineStrength;
half _VerticalScanlineStrength;
int _ScanlineSpacing;
half _MovScanlineSize;
half _MovScanlineSpeed;
half _MovScanlineStrength;
half _SharpenKernel[9];
half _SharpenStrength;
half _Brightness;
half _Contrast;
half _Saturation;
half _VignetteStrength;
half _Chromatic;
half _NoiseStrength;
half _NoiseDistortion;
half _Jitter;
half _Wobble;
half4 _ReplacementColor;
half _ScreenCurve;

half4 HorizontalScanlines(half4 col, float2 uv){
	if(floor((uv.y*_ScreenParams.y)/_ScanlineSize)%_ScanlineSpacing == 0){
		col.rgb *= 1 - _HorizontalScanlineStrength;
	}

	return col;
}

half4 VerticalScanlines(half4 col, float2 uv){
	if(floor((uv.x*_ScreenParams.x)/_ScanlineSize)%_ScanlineSpacing == 0){
		col.rgb *= 1 - _VerticalScanlineStrength;
	}

	return col;
}

half MovingScanline(float2 uv) {
	half size = (1-_MovScanlineSize)*0.495 + 0.005;
	return sin(_ScreenParams.y * uv.y * size - _Time.y * _MovScanlineSpeed);
}

half4 Sharpen(float2 uv)
{
	half4 col = fixed4(0, 0, 0, 1);
	[unroll(3)]for(int y = -1; y <= 1; y++){
		[unroll(3)]for(int x = -1; x <= 1; x++){
			float2 pos = uv + float2(x,y)*_MainTex_TexelSize.xy;
			if(pos.x >= 0 && pos.x <= 1 && pos.y >= 0 && pos.y <= 1){
				int index = (y+1)*3 + (x+1);
				col.rgb += _SharpenKernel[index] * tex2D(_MainTex, pos).rgb;
			}
		}	
	}

	return col;
}

half Poltergeist(float2 uv, float seed)
{
    return frac(sin(dot(uv*seed, float2(12.9898989898, 78.233333)))*43758.54535353);
}

float OnOff(float a, float b, float c)
{
	return step(c, sin(_Time.y + a*cos(_Time.y*b)));
}

float Distort(float2 uv)
{
	float window = 1./(1.+20.*(uv.y-(_Time.y/4%1))*(uv.y-(_Time.y/4%1)));
	return uv.x + sin(uv.y*10. + _Time.y)/50.*OnOff(4.,4.,.3)*(1.+cos(_Time.y*80.))*window;
}

float2 Curve(float2 uv)
{
	uv = (uv - 0.5) * 2.0;
	uv *= 1.1;	
	uv.x *= 1.0 + pow((abs(uv.y) / 5.0), 2.0);
	uv.y *= 1.0 + pow((abs(uv.x) / 4.0), 2.0);
	uv  = (uv / 2.0) + 0.5;
	uv =  uv *0.92 + 0.04;
	return uv;
}

half4 Chromatic(float2 uv)
{
	half dist = pow(uv.x*uv.x + uv.y*uv.y, 0.5);
	half mov = (_Chromatic*0.01) * dist;
	half2 uvR = half2(uv.x - mov, uv.y);
	half2 uvG = half2(uv.x + mov, uv.y);
	half2 uvB = half2(uv.x, uv.y - mov);
	half4 colR = tex2D(_MainTex, uvR);
	half4 colG = tex2D(_MainTex, uvG);
	half4 colB = tex2D(_MainTex, uvB);
	return half4(colR.r, colG.g, colB.b, 1);
}

half4 fragSharpen (v2f i) : SV_TARGET
{
	half4 col = fixed4(0, 0, 0, 1);
	if(i.uv.x <= _MainTex_TexelSize.x || i.uv.x >= 1-_MainTex_TexelSize.x || i.uv.y <= _MainTex_TexelSize.y || i.uv.y >= 1-_MainTex_TexelSize.y){
		col = tex2D(_MainTex, i.uv);
	}
	else{
		col = lerp(tex2D(_MainTex, i.uv), Sharpen(i.uv), _SharpenStrength);
	}
	return col;
}

half4 fragMix (v2f i) : SV_TARGET
{
	half4 col = tex2D(_MainTex, i.uv);

	//chromatic abberation
	if(_Chromatic > 0){
		col = lerp(col, Chromatic(i.uv), 0.8);
	}

	//saturation
	half lum = col.r*0.3 + col.g*0.59 + col.b*0.11;
	if(_Saturation > 0){
		_Saturation *= 3;
	}
	col.rgb = -lum * _Saturation + col.rgb * (1+_Saturation);

	//contrast
	half con = 0;
	if(_Contrast < 0.5){
		con = (_Contrast+1)*0.9 + 0.1;		// scales (-1 - 0) -> (0.1 - 1)
	}
	else{
		con = _Contrast + 1;		//scales (0 - 1) -> (1 - 2)
	}
	col.rgb = (col.rgb-0.5)*con + 0.5;

	//brightness
	col.rgb += _Brightness;

	//noise
	if(_NoiseStrength != 0){
		col.rgb += Poltergeist(i.uv, _Time.x/(_NoiseDistortion*300 + 1))*_NoiseStrength*3;
	}

	//color replacement
	if(_ReplacementColor.a > 0){
		lum = col.r*0.3 + col.g*0.59 + col.b*0.11;
		col.rgb = lerp(col.rgb, _ReplacementColor.rgb*lum, _ReplacementColor.a);
	}

	//scanlines
	if(_HorizontalScanlineStrength > 0){
		col = HorizontalScanlines(col, i.uv);
	}
	if(_VerticalScanlineStrength > 0){
		col = VerticalScanlines(col, i.uv);
	}
	if(_MovScanlineStrength > 0){
		col.rgb *= 1-(MovingScanline(i.uv)*_MovScanlineStrength);
	}

	//vignette
	if(_VignetteStrength > 0){
		i.uv *=  1 - i.uv.yx;   
   		half vig = i.uv.x*i.uv.y * 15; 
    	vig = pow(vig, 0.6); 
		half4 vignetteCol = col*vig; 
		col = lerp(col, vignetteCol, _VignetteStrength);
	}

	return col;
}

half4 fragFinal (v2f i) : SV_TARGET
{
	if(_ScreenCurve > 0){
		i.uv = lerp(i.uv, Curve(i.uv), _ScreenCurve);
		if (i.uv.x < 0.0 || i.uv.x > 1.0 || i.uv.y < 0.0 || i.uv.y > 1.0){
			return half4(0,0,0,1);
		}
	}

	if(_Jitter > 0){
		float2 jitterUV = i.uv;
		if(Poltergeist(jitterUV, _Time.y) > 0.6){
			jitterUV.x += cos(_Time.y * 10 + jitterUV.y * 1000) * 0.01;
		}
		i.uv = lerp(i.uv, jitterUV, _Jitter);
	}

	if(_Wobble > 0){
		float2 wobbleUV = i.uv;
		wobbleUV.x = Distort(wobbleUV);
		i.uv = lerp(i.uv, wobbleUV, _Wobble);
	}

	half4 col = tex2D(_MainTex, i.uv);
	return col;
}