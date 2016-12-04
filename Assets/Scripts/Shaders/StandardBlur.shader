﻿Shader "Custom/StandardBlur"
	{
		Properties
		{
			_MainTex("Texture", 2D) = "white" {}
		_BlurTex("Blur Texture", 2D) = "white" {}
		//_InvertBlur("Invert", float) = 1
			//_Position("Blur Position", float3 xyz) 
			//_DisplaceTex("Displacement Texture", 2D) = "white" {}
			//_Magnitude("Magnitude", Range(0,0.1)) = 1
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
			Blend SrcAlpha OneMinusSrcAlpha
			//Blend One One

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
			//sampler2D _DisplaceTex;
			//float _Magnitude;
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
				//float2 distuv = float2(i.uv.x + _Time.x * 2, i.uv.y + _Time.x * 2);

				//float2 disp = tex2D(_DisplaceTex, distuv).xy;
				//disp = ((disp * 2) - 1) * _Magnitude;

				fixed4 col = tex2D(_MainTex, i.uv);
			//fixed bg = col.a;

			float4 color1 = box(_MainTex, i.uv, _MainTex_TexelSize);
			/*color1.a *= tex2D(_BlurTex, i.uv).x;

			if (tex2D(_BlurTex, i.uv).x <= 0.25)
			{
				color1 = col;
			}*/
			//col = lerp(color1, col, 0.5);
			//float4 color2 = tex2D(_MainTex, i.uv);
			//float4 color1 = box(_MainTex, i.uv, _MainTex_TexelSize) * tex2D(_BlurTex, i.uv).a;


			//float4 color = lerp(color1, color2, 0.75);
			//color
			//tex2D(_MainTex, i.uv) * _Color;
			//float4 color = float4(i.uv.r,i.uv.g,1,1);
			return color1;
			}

				ENDCG
			}
			}
	}