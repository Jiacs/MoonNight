Shader "Custom/ImageBlend"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _ImageTex("ImageTexture", 2D) = "white" {}
        _Alpha("Alpha", Range(0, 1)) = 0
        _ImagePos("Image Position", Vector) = (0.5, 0.5, 0, 0)
        _ImageScale("Image Scale", Vector) = (1, 1, 0, 0)
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

            sampler2D _MainTex;
            float4 _MainTex_ST;
            sampler2D _ImageTex;
            float4 _ImageTex_ST;
            float _Alpha;
            float4 _ImagePos;   // Image position in UV space (0 to 1)
            float4 _ImageScale; // Image scale in UV space (x for width, y for height)

            // Vertex Shader
            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                // Apply the main texture's scaling and translation
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            // Fragment Shader
            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);

                // Calculate image texture's UV with offset and scaling
                float2 imageUV = (i.uv - _ImagePos.xy) / _ImageScale.xy;

                // Use a custom edge handling method to avoid linear stretching at the edges
                // We will add a simple border to the texture sampling
                imageUV = clamp(imageUV, 0.0, 1.0);

                // Sample the image texture at the adjusted UV coordinates
                fixed4 imageCol = tex2D(_ImageTex, imageUV);

                // If the image texture is fully transparent, return the main texture
                if (imageCol.a == 0)
                {
                    return col;
                }

                // Blend the two textures based on alpha
                return col * (1 - _Alpha) + imageCol * _Alpha;
            }
            ENDCG
        }
    }
}
