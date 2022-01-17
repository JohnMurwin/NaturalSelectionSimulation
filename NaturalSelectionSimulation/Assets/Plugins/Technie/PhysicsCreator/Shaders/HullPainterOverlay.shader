Shader "Hidden/HullPainterOverlay"
{
	Properties
	{
		_Tint("Tint", Color) = (1.0, 1.0, 1.0, 1.0)
	}
	SubShader
	{
		Tags
		{
			"Queue"="Transparent-1"
			"RenderType"="Transparent"
		}

		Pass
		{
			Blend SrcAlpha OneMinusSrcAlpha
			ZWrite On
			ZTest LEqual
			Offset -1.0, -1.0
			LOD 200

			CGPROGRAM
			#pragma vertex vert_surf
			#pragma fragment frag_surf
			#include "UnityCG.cginc"

			struct v2f_surf
			{
				float4 pos		: SV_POSITION;
				fixed4 color	: COLOR;
			};

			fixed4 _Tint;

			v2f_surf vert_surf (appdata_full v)
			{
				v2f_surf o;
				o.pos = UnityObjectToClipPos (v.vertex);
				o.color = v.color * _Tint;
				return o;
			}

			float4 frag_surf (v2f_surf IN) : COLOR
			{
				return IN.color;
			}
			ENDCG
		}
	}
}
