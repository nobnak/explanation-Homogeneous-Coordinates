Shader "Unlit/Projector" {
    Properties {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader {
        Tags { "RenderType"="Opaque" }

		Blend SrcAlpha One
		ZWrite Off
		Offset -1, -1

        Pass {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f {
                float4 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

			float4x4 unity_Projector;

            v2f vert (appdata v) {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = mul(unity_Projector, v.vertex);
                return o;
            }

			float4 frag (v2f i) : SV_Target {
				float2 uv = i.uv.xy / i.uv.w;
				clip(1 - abs(uv * 2 - 1));

                float4 col = tex2Dproj(_MainTex, i.uv);
                return col;
            }
            ENDCG
        }
    }
}
