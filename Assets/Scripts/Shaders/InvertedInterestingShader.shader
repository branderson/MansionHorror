Shader "Custom/InvertedInterestingShader"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_BlurTex("Blur Texture", 2D) = "white" {}
		//_InvertBlur("Invert", float) = 1

	}
	SubShader
	{
		Tags
		{
			"Queue" = "Transparent"
			"RenderType" = "Transparent"
			"PreviewType" = "Plane"
		}
		Cull Off ZWrite Off ZTest Always
		Pass
		{
				Blend OneMinusSrcAlpha SrcAlpha

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

			};


			v2f vert(appdata v)
			{
				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.uv = v.uv;
				return o;
			}

				sampler2D _MainTex;
				sampler2D _BlurTex;
				//float _InvertBlur;
				float4 _MainTex_TexelSize;


			float4 box(sampler2D tex, float2 uv, float4 size)
			{

				float4 c = tex2D(tex, uv + float2(-size.x, size.y)) + tex2D(tex, uv + float2(0, size.y)) + tex2D(tex, uv + float2(size.x, size.y)) +
					tex2D(tex, uv + float2(-size.x, 0)) + tex2D(tex, uv + float2(0, 0)) + tex2D(tex, uv + float2(size.x, 0)) +
					tex2D(tex, uv + float2(-size.x, -size.y)) + tex2D(tex, uv + float2(0, -size.y)) + tex2D(tex, uv + float2(size.x, -size.y));

				return c / 9;

			}

			float4 frag(v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.uv);

				float4 color1 = box(_MainTex, i.uv, _MainTex_TexelSize);
				color1.a *= tex2D(_BlurTex, i.uv).x;

				if (tex2D(_BlurTex, i.uv).x <= 0.25)
				{
					color1 = col;
				}
					return color1;
			}

				ENDCG
			}
	}
}