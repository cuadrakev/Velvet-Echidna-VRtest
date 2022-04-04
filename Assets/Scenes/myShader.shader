// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Unlit/myShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            Tags { "LightMode" = "ForwardBase" }
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"
            #include "UnityLightingCommon.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
                float4 worldVertex : TEXCOORD1;
                float3 fragNormal : TEXCOORD2;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.worldVertex = mul(unity_ObjectToWorld, v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.fragNormal = UnityObjectToWorldNormal(v.normal);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);

                float3 pointLight = float3(unity_4LightPosX0.x, unity_4LightPosY0.x, unity_4LightPosZ0.x);
                float3 L = normalize(pointLight - i.worldVertex.xyz);
                float NdotL = max(0.0, dot(normalize(i.fragNormal), L));
                NdotL = floor(NdotL / 0.3);
                float4 pointColor = float4(unity_LightColor[0].rgb * NdotL, 1.);

                if (_WorldSpaceLightPos0.w == 0.0)
                {
                    L = normalize(_WorldSpaceLightPos0.xyz);
                }
                else
                {
                    L = normalize(_WorldSpaceLightPos0.xyz - i.worldVertex.xyz);
                }
                NdotL = max(0.0, dot(normalize(i.fragNormal), L));
                NdotL = floor(NdotL / 0.3);
                float4 otherColor = _LightColor0 * NdotL + pointColor;

                return col * otherColor;
            }
            ENDCG
        }
    }
}
