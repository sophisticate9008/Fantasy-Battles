Shader "Custom/UIGradientWithAxisSelection"
{
    Properties
    {
        _StartColor ("Start Color", Color) = (1,1,1,1)   // 起始颜色
        _EndColor ("End Color", Color) = (1,0,0,1)       // 结束颜色
        _GradientDirection ("Gradient Direction (0: X, 1: Y, 2: XY)", Range(0,2)) = 1 // 渐变方向选择
    }
    SubShader
    {
        Tags {"Queue" = "Transparent" "RenderType"="Transparent" "IgnoreProjector"="True" "CanUseSpriteAtlas"="True"}
        Blend SrcAlpha OneMinusSrcAlpha   // 混合模式，考虑透明度
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

            float4 _StartColor;      // 起始颜色
            float4 _EndColor;        // 结束颜色
            float _GradientDirection; // 渐变方向选择

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float gradientFactor;

                // 根据用户选择的渐变方向计算渐变因子
                if (_GradientDirection == 0)
                {
                    // 基于 X 轴渐变
                    gradientFactor = i.uv.x;
                }
                else if (_GradientDirection == 1)
                {
                    // 基于 Y 轴渐变
                    gradientFactor = i.uv.y;
                }
                else
                {
                    // 同时基于 X 和 Y 轴渐变
                    gradientFactor = (i.uv.x + i.uv.y) * 0.5;
                }

                // 从起始颜色渐变到结束颜色
                fixed4 color = lerp(_StartColor, _EndColor, gradientFactor);
                return color;
            }
            ENDCG
        }
    }
    FallBack "Transparent"
}
