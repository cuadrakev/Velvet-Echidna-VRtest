Shader "MyShaders/Outline"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _NormalTex("Normals", 2D) = "bump" {}
    }
    SubShader
    {
        Tags { "RenderType" = "Opaque" }
        LOD 100

        Pass
        {
            Tags {"LightMode" = "Always"}

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
            };

            uniform sampler2D _MainTex;
            uniform sampler2D _NormalTex;
            uniform sampler2D_float _LastCameraDepthTexture;

            v2f vert(appdata v, out float4 vertexOut : SV_POSITION)
            {
                v2f o;
                o.uv = v.uv;
                vertexOut = UnityObjectToClipPos(v.vertex);
                return o;
            }

            static float3x3 SobelX = float3x3(float3(-1, 0, 1), float3(-2, 0, 2), float3(-1, 0, 1));
            static float3x3 SobelY = float3x3(float3(1, 2, 1), float3(0, 0, 0), float3(-1, -2, -1));

            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 color = tex2D(_MainTex, i.uv);

                float xStep = 1.0 / _ScreenParams.x;
                float yStep = 1.0 / _ScreenParams.y;
                float3 normalGrid[9];//row order
                normalGrid[0] = tex2D(_NormalTex, i.uv + float2(-xStep, yStep));
                normalGrid[1] = tex2D(_NormalTex, i.uv + float2(0, yStep));
                normalGrid[2] = tex2D(_NormalTex, i.uv + float2(xStep, yStep));
                normalGrid[3] = tex2D(_NormalTex, i.uv + float2(-xStep, 0));
                normalGrid[4] = tex2D(_NormalTex, i.uv + float2(0, 0));
                normalGrid[5] = tex2D(_NormalTex, i.uv + float2(xStep, 0));
                normalGrid[6] = tex2D(_NormalTex, i.uv + float2(-xStep, -yStep));
                normalGrid[7] = tex2D(_NormalTex, i.uv + float2(0, -yStep));
                normalGrid[8] = tex2D(_NormalTex, i.uv + float2(xStep, -yStep));

                float3 Gx = float3(0, 0, 0);
                float3 Gy = float3(0, 0, 0);
                for (uint i = 0; i < 9; i++)
                {
                    Gx += normalGrid[i] * SobelX[i / 3][i % 3];
                    Gy += normalGrid[i] * SobelY[i / 3][i % 3];
                }
                float3 colOut = sqrt(Gx * Gx + Gy * Gy);
                float tmp = colOut.x + colOut.y + colOut.z;

                if (tmp == 0)
                    return color;
                else
                    return fixed4(tmp, tmp, tmp, 1.0);
            }
            ENDCG
        }
    }
}
