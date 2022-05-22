Shader "MyShaders/ToonShader"
{
    Properties
    {
        _Color ("Diffuse color", COLOR) = (1, 1, 1)
        _Specular ("Specular color", COLOR) = (1, 1, 1)
        _MainTex ("Texture", 2D) = "white" {}

        [Header(Lighting)]
        _DiffuseCutoff ("Diffuse cutoff", Range(0.0, 0.9)) = 0.2
        _DiffuseSmooth ("Diffuse smooth", Range(0.0, 0.5)) = 0.1
        _SpecularSize ("Specular size", Range(0.0, 1.0)) = 0.1
        _SpecularFalloff ("Specular falloff", Range(0.0, 5.0)) = 1.0

        [Header(Halftone)]
        _Halftone("Halftone texture", 2D) = "white" {}
        _HalftoneInputMin ("Input min", Range(0., 1.)) = 0.
        _HalftoneInputMax ("Input max", Range(0., 1.)) = 1.
        _HalftoneOutputMin ("Output min", Range(0., 1.)) = 0.
        _HalftoneOutputMax ("Output max", Range(0., 1.)) = 1.

        [Header(Outline)]
        _OutlineColor("Outline color", COLOR) = (0, 0, 0, 1)
        _OutlineThickness("Outline thickness", Range(0., 1.)) = 0.1

    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            Cull front

            CGPROGRAM

            #include "UnityCG.cginc"

            #pragma vertex vert
            #pragma fragment frag

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float4 position : SV_POSITION;
            };

            fixed4 _OutlineColor;
            float _OutlineThickness;

            v2f vert(appdata v)
            {
                v2f o;

                float3 normal = normalize(v.normal);
                float3 outlinePosition = v.vertex + normal * _OutlineThickness;

                o.position = UnityObjectToClipPos(outlinePosition);

                return o;
            }

            fixed4 frag(v2f i) : SV_TARGET
            {
                return _OutlineColor;
            }

            ENDCG
        }

        Pass
        {
            Tags { "LightMode" = "ForwardBase" }
            Cull back

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fwdbase nolightmap nodirlightmap nodynlightmap novertexlight

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
                float2 halftoneUv : TEXCOORD1;
                float4 worldVertex : TEXCOORD2;
                float3 fragNormal : TEXCOORD3;
                SHADOW_COORDS(4)
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            fixed4 _Color;
            fixed4 _Specular;

            float _DiffuseCutoff;
            float _DiffuseSmooth;
            float _SpecularSize;
            float _SpecularFalloff;

            sampler2D _Halftone;
            float4 _Halftone_ST;
            float _HalftoneInputMin;
            float _HalftoneInputMax;
            float _HalftoneOutputMin;
            float _HalftoneOutputMax;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.worldVertex = mul(unity_ObjectToWorld, v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.halftoneUv = TRANSFORM_TEX(v.uv, _Halftone);
                o.fragNormal = UnityObjectToWorldNormal(v.normal);
                TRANSFER_SHADOW(o);
                return o;
            }

            struct LightProperties
            {
                fixed4 color;
                float lightValue;
            };

            LightProperties getLight(fixed4 albedo, float3 fragPos, float3 normal, fixed shadowValue, float3 toLight, fixed3 lightColor)
            {
                float3 V = normalize(_WorldSpaceCameraPos - fragPos);
                float3 L = toLight;
                float3 H = normalize(L + V);

                float lightIntensity = max(0.0, dot(normal, L));
                lightIntensity = smoothstep(_DiffuseCutoff, _DiffuseCutoff + _DiffuseSmooth, lightIntensity) * shadowValue;

                float specularFalloff = pow(dot(V, normal), _SpecularFalloff);
                float specular = dot(H, normal) * specularFalloff;
                float specularChange = fwidth(specular);
                specular = smoothstep(1. - _SpecularSize, 1. - _SpecularSize + specularChange, specular);

                LightProperties o;
                o.color = lerp(albedo * fixed4(lightColor, 1.) * lightIntensity, _Specular * fixed4(lightColor, 1.), saturate(specular));
                o.lightValue = lightIntensity;
                return o;
            }

            fixed4 applyHalftone(LightProperties light, float2 uv)
            {
                float halftone = tex2D(_Halftone, uv).a;
                halftone = (halftone - _HalftoneInputMin) / (_HalftoneInputMax - _HalftoneInputMin);
                halftone = lerp(_HalftoneOutputMin, _HalftoneOutputMax, halftone);
                float lightIntensity = step(halftone, saturate(light.lightValue));
                float halftoneChange = fwidth(halftone) * 0.5;
                lightIntensity = smoothstep(halftone - halftoneChange, halftone + halftoneChange, lightIntensity);

                return light.color * lightIntensity;
            }

            fixed4 frag (v2f i): SV_Target
            {
                fixed4 col = _Color * tex2D(_MainTex, i.uv);
                fixed shadow = SHADOW_ATTENUATION(i);
                fixed4 outColor;

                float3 L = normalize(_WorldSpaceLightPos0.xyz);
                outColor = applyHalftone(getLight(col, i.worldVertex.xyz, i.fragNormal, shadow, L, _LightColor0), i.halftoneUv);

                for(int l = 0; l < 4; l++)
                {
                    float3 lightPosition = float3(unity_4LightPosX0[l], unity_4LightPosY0[l], unity_4LightPosZ0[l]);
                    L = normalize(lightPosition.xyz - i.worldVertex.xyz);
                    if (length(unity_LightColor[l].xyz) > 0.)
                    {
                        outColor += applyHalftone(getLight(col, i.worldVertex.xyz, i.fragNormal, shadow, L, unity_LightColor[l].xyz), i.halftoneUv);
                    }
                }

                return col * outColor + 0.01 * fixed4(ShadeSH9(float4(i.fragNormal, 1.0)), 1.0);
            }
            ENDCG
        }

        UsePass "Legacy Shaders/VertexLit/SHADOWCASTER"
    }
}
