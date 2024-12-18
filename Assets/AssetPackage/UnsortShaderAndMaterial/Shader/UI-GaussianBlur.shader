Shader "Custom/UI-GaussianBlur"
{
    Properties
    {
        
        _MainTex ("Texture", 2D) = "white" {}
        _BlurSize ("Blur Size", Float) = 1.0
    }

    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Overlay" }
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float4 _MainTex_TexelSize; // Unity 自动传递的纹理信息
            float _BlurSize;

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            float4 frag (v2f i) : SV_Target
            {
                float2 uv = i.uv;
                float4 color = float4(0, 0, 0, 0);

                // 高斯模糊权重
                float weight[5];
                weight[0] = 0.227027;
                weight[1] = 0.1945946;
                weight[2] = 0.1216216;
                weight[3] = 0.054054;
                weight[4] = 0.016216;

                // 水平方向模糊
                for (int x = -4; x <= 4; x++)
                {
                    color += weight[abs(x)] * tex2D(_MainTex, uv + float2(x, 0) * _BlurSize * _MainTex_TexelSize.xy);
                }

                // 垂直方向模糊
                float4 finalColor = float4(0, 0, 0, 0);
                for (int y = -4; y <= 4; y++)
                {
                    finalColor += weight[abs(y)] * tex2D(_MainTex, uv + float2(0, y) * _BlurSize * _MainTex_TexelSize.xy);
                }

                return finalColor;
            }
            ENDCG
        }
    }

    FallBack "UI/Default"
}
