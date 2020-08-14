// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/Shockwave"
{
    Properties
    {
        //_MainTex ("Texture", 2D) = "" {}
        _Center ("Shockwave Origin", Vector) = (0, 0, 0, 0)
        _Force ("Distorion Intensity", Float) = 0.0
        _Size ("Shockwave Size", Float) = 0.0
        _Thickness ("Thickness", Float) = 0.0
    }
    SubShader
    {
        //Tags { "RenderType"="Opaque" }
        //Tags { "RenderType"="Transparent" "Queue"="Transparent"}
        Tags {"Queue"="Overlay"}

        Cull Off ZWrite Off ZTest Always

        GrabPass
        {
            "_BackgroundTexture"
        }

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

            //sampler2D _MainTex;
            float2 _Center;
            float _Force;
            float _Size;
            float _Thickness;
            sampler2D _BackgroundTexture;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                //o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                //o.uv = v.uv;
                o.uv = ComputeGrabScreenPos(o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float ratio = _ScreenParams.y / _ScreenParams.x;
	            float2 scaledUV = (i.uv - float2(0.5, 0.0)) / float2(ratio, 1.0) + float2(0.5, 0.0);
	            float mask = (1.0 - smoothstep(_Size - 0.1, _Size, length(scaledUV - _Center))) *
			                smoothstep(_Size - _Thickness - 0.1, _Size - _Thickness, length(scaledUV - _Center));
	            float2 disp = normalize(scaledUV - _Center) * _Force * mask;
	            //COLOR = texture(SCREEN_TEXTURE, SCREEN_UV - disp);
                fixed4 col = tex2D(_BackgroundTexture, i.uv - disp);
                return col;
            }
            ENDCG
        }
    }
}
