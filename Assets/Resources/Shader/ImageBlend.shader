Shader "Custom/ImageBlend"
{
    Properties
    {
        _MainTex("Texture",2D)="white"{}
        _ImageTex("ImageTexture", 2D)="white"{}
        _Alpha ("Alpha", Range(0,1))=0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

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

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed3 col = tex2D(_MainTex, i.uv);
                fixed3 imageCol = tex2D(_ImageTex, i.uv);
                return fixed4(col * (1 - _Alpha) + imageCol * _Alpha, 1.0);
            }
            ENDCG
        }
    }
}
