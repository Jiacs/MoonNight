Shader "Custom/ScreenShake"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _ShakeFrequency("Shake Frequency", float) = 10
        _ShakeAmount("Shake Amount", float) = 0.1
    }
    SubShader
    {
        Tags { "RenderType" = "Opaque" }
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

            // Shader properties
            sampler2D _MainTex;
            float _ShakeFrequency;
            float _ShakeAmount;

            // Generate a random value for the shake effect based on time
            float random(float2 uv)
            {
                return frac(sin(dot(uv, float2(12.9898, 78.233))) * 43758.5453);
            }

            v2f vert(appdata v)
            {
                v2f o;

                // Use time and randomness to calculate the shake offset
                float2 uv = v.uv;
                float randVal = random(uv + float2(_Time.y * _ShakeFrequency, _Time.y * _ShakeFrequency));
                
                // Apply the shake offset to the vertex positions
                v.vertex.x += (randVal - 0.5) * _ShakeAmount;
                v.vertex.y += (randVal - 0.5) * _ShakeAmount;

                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                if (i.uv.x < 0 || i.uv.x > 1 || i.uv.y < 0 || i.uv.y > 1)
                {
                    return fixed4(0, 0, 0, 1); // black for out of bounds
                }
                fixed4 col = tex2D(_MainTex, i.uv);
                return col;
            }

            ENDCG
        }
    }
}
