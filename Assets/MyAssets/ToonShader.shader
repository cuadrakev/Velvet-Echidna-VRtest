Shader "MyShaders/ToonShader"
{
    Properties
    {
        _Color ("Diffuse color", COLOR) = (1,1,1)
        _MainTex ("Texture", 2D) = "white" {}
        _ShadingRange ("Shading range", Range(0.0, 10.0)) = 3.0
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
            #include "Lighting.cginc"
            #include "AutoLight.cginc"

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
                SHADOW_COORDS(3)
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            fixed4 _Color;
            float _ShadingRange;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.worldVertex = mul(unity_ObjectToWorld, v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.fragNormal = UnityObjectToWorldNormal(v.normal);
                TRANSFER_SHADOW(o);
                return o;
            }

            fixed4 frag (v2f i): SV_Target
            {
                fixed4 col = _Color * tex2D(_MainTex, i.uv);
                fixed4 light;
                float specular;
                fixed shadow = SHADOW_ATTENUATION(i);
                float3 V = normalize(_WorldSpaceCameraPos - i.worldVertex.xyz);

                float3 L = normalize(_WorldSpaceLightPos0.xyz);
                float3 H = normalize(L + V);
                float NdotL = max(0.0, dot(i.fragNormal, L));
                NdotL = floor(NdotL * 10.0 / _ShadingRange) / 10.0;
                light = _LightColor0 * NdotL;
                specular = pow(max(0.0, dot(i.fragNormal, H)), 10);

                for(int l = 0; l < 4; l++)
                {
                    float3 lightPosition = float3(unity_4LightPosX0[l], unity_4LightPosY0[l], unity_4LightPosZ0[l]);
                    L = normalize(lightPosition.xyz - i.worldVertex.xyz);
                    H = normalize(L + V);
                    NdotL = max(0.0, dot(i.fragNormal, L));
                    NdotL = floor(NdotL * 10.0 / _ShadingRange) / 10.0;
                    light += float4(unity_LightColor[l].xyz * NdotL, 1.);
                    specular += floor(pow(max(0.0, dot(i.fragNormal, H)), 200) * 10.0 / 5) / 10.0;
                }

                return col * light * shadow + fixed4(1, 1, 1, 1) * specular + col * fixed4(ShadeSH9(half4(i.fragNormal, 1)), 1);
            }
            ENDCG
        }

        Pass
        {
          Tags {"LightMode"="ShadowCaster"}
          CGPROGRAM
          #pragma vertex vert
          #pragma fragment frag
          #pragma multi_compile_shadowcaster
          #include "UnityCG.cginc"

          struct v2f
          {
                V2F_SHADOW_CASTER;
          };

          v2f vert(appdata_base v)
          {
            v2f o;
            TRANSFER_SHADOW_CASTER_NORMALOFFSET(o);
            return o;
          }

          float4 frag(v2f i): SV_TARGET
          {
            SHADOW_CASTER_FRAGMENT(i);
          }
          ENDCG
        }

        UsePass "Legacy Shaders/VertexLit/SHADOWCASTER"
    }
}
