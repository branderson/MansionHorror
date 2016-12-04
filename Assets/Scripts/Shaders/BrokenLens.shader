Shader "Unlit/BrokenLens"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Triangles("Number of Triangles", float) = 1
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
		GrabPass{ }
		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			struct v2f
			{
				float4 vertex : TEXCOORD0;
				float4 uv : SV_POSITION;
			};

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			v2f vert(appdata v) {
				v2f o;
				// use UnityObjectToClipPos from UnityCG.cginc to calculate 
				// the clip-space of the vertex
				//o.vertex = UnityObjectToClipPos(v.vertex);
				// use ComputeGrabScreenPos function from UnityCG.cginc
				// to get the correct texture coordinate
				//o.vertex = ComputeGrabScreenPos(o.vertex);
				return o;
			}

			sampler2D _BackgroundTexture;
			float _Triangles;

			half4 frag(v2f i) : SV_Target
			{
				half4 bgcolor = tex2Dproj(_BackgroundTexture, i.vertex);
				return 1 - bgcolor;
			}
			ENDCG
		}
	}
}
