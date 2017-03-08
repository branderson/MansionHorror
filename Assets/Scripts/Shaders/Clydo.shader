Shader "Custom/Clydo" {
	Properties {
		_MainTex("Texture", 2D) = "white" {}
		_TransitionTex("Transition Texture", 2D) = "white" {}
		_Color("Screen Color", Color) = (1,1,1,1)
		_Cutoff("Cutoff", Range(0, 1)) = 0
		_Fade("Fade", Range(0, 1)) = 0
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
			float2 uv1 : TEXCOORD1;
			float4 vertex : SV_POSITION;
		};

		float4 _MainTex_TexelSize;

		v2f vert(appdata v)
		{
			v2f o;
			o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
			o.uv = v.uv;
			o.uv1 = v.uv;

#if UNITY_UV_STARTS_AT_TOP
			if (_MainTex_TexelSize.y < 0)
				o.uv1.y = 1 - o.uv1.y;
#endif

			return o;
		}

		sampler2D _TransitionTex;
		float _Fade;

		sampler2D _MainTex;
		float _Cutoff;
		fixed4 _Color;

		fixed4 frag(v2f i) : SV_Target
		{
			fixed4 transit = tex2D(_TransitionTex, i.uv1);

		//fixed2 direction = float2(0,0);
		//if(_Distort)
		//	direction = normalize(float2((transit.r - 0.5) * 2, (transit.g - 0.5) * 2));

		fixed4 col = tex2D(_MainTex, i.uv);
		fixed4 col2 = tex2D(_MainTex, fixed2(1, 1) - i.uv);
		//fixed4 topright = tex2D(_MainTex, i.uv + fixed2(0.5f,0.5f));
		//fixed4 botleft = tex2D(_MainTex, i.uv - fixed2(0.5f,0.5f));
		//if (i.uv.y < _Cutoff)
		//	return _Color;
		if ((i.uv.x < 0.5f && i.uv.y > 0.5f && i.uv.y >= -(2.0)*i.uv.x + (1.5f)) 
			|| (i.uv.x > 0.5f && i.uv.y < 0.5f && i.uv.y <= -(2.0f)*i.uv.x + (1.5f)))
			return col = lerp(col, col2, _Fade);
		//while we are in the bot left quad grab the topright coords
		else if (i.uv.x < 0.5f && i.uv.y < 0.5f && i.uv.y >= i.uv.x)
			return col = lerp(col, col2, _Fade);
		//while we are in the top right quad grab the botleft coords
		else if (i.uv.x > 0.5f && i.uv.y > 0.5f && i.uv.y <= i.uv.x)
			return col = lerp(col, col2, _Fade);
		
		//else if (i.uv.x > 0.5f && i.uv.y > 0.5f && i.uv.y <= i.uv.x)
			//return col = lerp(col, col2, _Fade);
		return col;
		}
			ENDCG
		}			
		}
}
