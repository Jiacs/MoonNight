Shader "Custom/Bloom"
{
	CGINCLUDE
	#include "UnityCG.cginc"

	sampler2D _MainTex;
	float4 _MainTex_TexelSize;
	float _Threshold;
	sampler2D _BloomTex;
	float _Intensity;

float3 ApplyBloomThreshold (float3 color)
{
	float br = max(max(color.r, color.g), color.b);
		br = max(0.0f, (br - _Threshold)) / max(br,0.00001f);
		color.rgb *= br;
	return color;
}
	
	half4 frag_PreFilter(v2f_img i) : SV_Target
	{

		float3 color = 0;
		float weightSum = 0.0;
		
		float2 offsets[] = {
			float2(0.0, 0.0),
			float2(-1.0, -1.0), float2(-1.0, 1.0), float2(1.0, -1.0), float2(1.0, 1.0)
		};
		
		
		for (int j = 0; j < 5; j++)
		{
			float3 c = tex2D(_MainTex, i.uv + _MainTex_TexelSize.xy * offsets[j]);
			c = ApplyBloomThreshold(c);
			
			float w = 1.0 / (Luminance(c) + 1.0);
			color += c * w;
			weightSum += w;
		}
		color /= weightSum;
		return float4(color, 1);
	}

	half4 frag_DownsampleBox(v2f_img i) : SV_Target
	{
		half4 d = _MainTex_TexelSize.xyxy * half4(-1,-1,1,1);
		half4 s = 0;
		s += tex2D(_MainTex, i.uv + d.xy);
		s += tex2D(_MainTex, i.uv + d.zy);
		s += tex2D(_MainTex, i.uv + d.xw);
		s += tex2D(_MainTex, i.uv + d.zw);
		s *= 0.25;
		return s;
	}

	half4 frag_UpsampleBox(v2f_img i) : SV_Target
	{
		half4 d = _MainTex_TexelSize.xyxy * half4(-1,-1,1,1);
		half4 color = 0;
		color += max(tex2D(_MainTex, i.uv + d.xy), 0.0001);
		color += max(tex2D(_MainTex, i.uv + d.zy), 0.0001);
		color += max(tex2D(_MainTex, i.uv + d.xw), 0.0001);
		color += max(tex2D(_MainTex, i.uv + d.zw), 0.0001);
		color *= 0.25;

		half4 color2 = tex2D(_BloomTex, i.uv);
		return color + color2;
	}

	half4 frag_Combine(v2f_img i) : SV_Target
	{
		half4 d = _MainTex_TexelSize.xyxy * half4(-1,-1,1,1);
		half4 base_color = tex2D(_MainTex, i.uv);
		half4 bloom_color = tex2D(_BloomTex, i.uv);

		half3 final_color = base_color.rgb + bloom_color.rgb * _Intensity;

		return half4(final_color,1.0);
	}

	ENDCG

	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_BlurOffset("BlurOffset",Float) = 1 
	}
	SubShader
	{
		Cull Off ZWrite Off ZTest Always
		Pass
		{
			CGPROGRAM
			#pragma vertex vert_img
			#pragma fragment frag_PreFilter
			ENDCG
		}
		Pass
		{
			CGPROGRAM
			#pragma vertex vert_img
			#pragma fragment frag_DownsampleBox
			ENDCG
		}
		Pass
		{
			CGPROGRAM
			#pragma vertex vert_img
			#pragma fragment frag_UpsampleBox
			ENDCG
		}
		Pass
		{
			CGPROGRAM
			#pragma vertex vert_img
			#pragma fragment frag_Combine
			ENDCG
		}
	}
}

