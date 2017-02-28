// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Hidden/NewImageEffectShader"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_R("r", Range(0, 1)) = 1
		_G("g", Range(0, 1)) = 1
		_B("b", Range(0, 1)) = 1
		_Range("range", Range(0, 5)) = .1

	}
	SubShader
	{
		// No culling or depth
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

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.uv = v.uv;
				return o;
			}

			sampler2D _MainTex;
			float _R;
			float _G;
			float _B;
			float _Range;
			float4 _Ray;

			float4 frag (v2f i) : SV_Target
			{
				float _Alpha = min(max(0, (1 - abs(_Ray.y - i.uv.y) / _Range)),
				                   max(0, sign(i.uv.x - _Ray.x) * sign(_Ray.z - i.uv.x)));
				float white = min(max(0, 1 - sign((abs(_Ray.y - i.uv.y) / _Range) - 0.07)),
				      	          max(0, sign(i.uv.x - _Ray.x) * sign(_Ray.z - i.uv.x)));
				float4 col = float4(_R, _G, _B, 1) * _Alpha;
				col += tex2D(_MainTex, i.uv) * (1 - _Alpha);
				col += float4(1,1,1,1) * white;
				return col;
			}
			ENDCG
		}
	}
}
