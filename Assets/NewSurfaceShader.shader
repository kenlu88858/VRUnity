Shader "Custom/HighlightOutline"
{
    Properties
    {
        _Color ("Outline Color", Color) = (1,1,0,1)
        _Width ("Outline Width", Range(0.001, 0.03)) = 0.01
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" "Queue"="Geometry" } // 確保是 Opaque
        Pass
        {
            Name "OUTLINE"
            Tags { "LightMode"="ForwardBase" }
            
            Cull Front     // 繪製反向面
            ZWrite On      // 確保深度測試啟用
            ZTest LEqual   // 避免被其他物件覆蓋
            Blend One Zero // 防止透明效果

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float4 pos : POSITION;
            };

            float _Width;
            float4 _Color;

            v2f vert(appdata_t v)
            {
                v2f o;
                float3 norm = mul((float3x3)UNITY_MATRIX_IT_MV, v.normal);
                float2 offset = TransformViewToProjection(norm.xy);
                o.pos = UnityObjectToClipPos(v.vertex);
                o.pos.xy += offset * _Width;
                return o;
            }

            half4 frag(v2f i) : SV_Target
            {
                return _Color;
            }
            ENDCG
        }
    }
}
