Shader "Hidden/Underwater"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _ApplyPercentage ("Cutoff", range(0, 1)) = 0
        _Distance ("Distance", range(0, 20)) = 10
        _DarkWater ("Dark Water Color", color) = (0.24609375, 0.31640625, 0.70703125, 0.9)
        
        _DistortTex ("Distortion Texture", 2D) = "white" {}
        _DistortAmount ("Distortion Amount", range(0, 0.05)) = 0
    }
    SubShader
    {
        Cull Off ZTest Always

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
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
                float4 screenPos : TEXCOORD2;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.screenPos = ComputeScreenPos(o.vertex);
                return o;
            }

            float4 shiftToBlue (float4 col)
            {
                col.rg *= 0.5;
                col.b *= 1.2;
                return saturate(col);
            }

            sampler2D _MainTex;
            float _ApplyPercentage;
            sampler2D _CameraDepthTexture;
            float _Distance;
            float4 _DarkWater;
            
            sampler2D _DistortTex;
            float4 _DistortTex_ST;
            
            float _DistortAmount;

            fixed4 frag (v2f i) : SV_Target
            {
                if (i.uv.y > _ApplyPercentage) {
                    return tex2D(_MainTex, i.uv);
                }
                
                float2 distortion = tex2D(_DistortTex, i.uv + _Time[1] / 10).xy;
                distortion = ((distortion * 2) - 1) * _DistortAmount;
                            
                fixed4 col = tex2D(_MainTex, i.uv + distortion);
                
                float depth = tex2Dproj(_CameraDepthTexture, i.screenPos).r;
                float linearDepth = LinearEyeDepth(depth);
                float depthDiff = (linearDepth - i.screenPos.w) / _Distance;
                depthDiff = saturate(depthDiff);
                
                col = lerp(col, _DarkWater, depthDiff);
                
                col = shiftToBlue(col);
                
                return col;
            }
            ENDCG
        }
    }
}
