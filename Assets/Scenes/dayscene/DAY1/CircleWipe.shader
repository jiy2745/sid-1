// CircleWipe.shader

Shader "Unlit/CircleWipe"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color ("Color", Color) = (0,0,0,1) // 검은색 바탕
        _Cutoff ("Cutoff", Range(0.0, 1.0)) = 0.5 // 원의 크기를 조절 (0~1)
        _Smoothness ("Smoothness", Range(0.0, 1.0)) = 0.01 // 원의 경계 부드러움
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        LOD 100
        Blend SrcAlpha OneMinusSrcAlpha // 알파 블렌딩 활성화
        Cull Off ZWrite Off ZTest Always

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
            fixed4 _Color;
            float _Cutoff;
            float _Smoothness;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // UV 좌표를 화면 중앙이 (0,0)이 되도록 변환 (-0.5 ~ 0.5)
                float2 centered_uv = i.uv - 0.5;
                
                // 화면 중앙으로부터의 거리 계산
                float dist = length(centered_uv * float2(1, _ScreenParams.y / _ScreenParams.x));

                // 거리를 바탕으로 알파 값 계산
                float alpha = smoothstep(_Cutoff - _Smoothness, _Cutoff + _Smoothness, dist);

                fixed4 col = _Color;
                col.a = alpha; // 계산된 알파 값을 적용
                return col;
            }
            ENDCG
        }
    }
}