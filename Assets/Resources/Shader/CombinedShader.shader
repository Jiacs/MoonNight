﻿Shader "Custom/CombinedShader"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _Color("Color", Color) = (1, 1, 1, 1)
        _ImageTex("ImageTexture", 2D) = "white" {}
        _Alpha("Alpha", Range(0,1)) = 0
    }
        SubShader
        {
            Tags { "RenderType" = "Opaque" }
            LOD 100

            // Pass 1: Color Tint
            Pass
            {
                Name "ColorTintPass"
                CGPROGRAM
                #pragma vertex vert
                #pragma fragment fragColorTint

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

                sampler2D _MainTex;
                float4 _MainTex_ST;
                float4 _Color;

                v2f vert(appdata v)
                {
                    v2f o;
                    o.vertex = UnityObjectToClipPos(v.vertex);
                    o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                    return o;
                }

                fixed4 fragColorTint(v2f i) : SV_Target
                {
                    fixed4 col = tex2D(_MainTex, i.uv);
                    return col * _Color;
                }
                ENDCG
            }

            // Pass 2: Image Blend
            Pass
            {
                Name "ImageBlendPass"
                CGPROGRAM
                #pragma vertex vert
                #pragma fragment fragImageBlend

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

                sampler2D _MainTex;
                float4 _MainTex_ST;
                sampler2D _ImageTex;
                float4 _ImageTex_ST;
                float _Alpha;

                v2f vert(appdata v)
                {
                    v2f o;
                    o.vertex = UnityObjectToClipPos(v.vertex);
                    o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                    return o;
                }

                fixed4 fragImageBlend(v2f i) : SV_Target
                {
                    fixed4 col = tex2D(_MainTex, i.uv); 
                    fixed4 imageCol = tex2D(_ImageTex, i.uv);

                    if (imageCol.a == 0)
                    {
                        return col;
                    }
                    return col * (1 - _Alpha) + imageCol * _Alpha;
                }
                ENDCG
            }
        }
}
