// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Bubblegum/Invisible Shadow Receiver" 
{

	SubShader 
	{
		Pass 
		{
			Blend DstColor Zero Tags { "LightMode" = "ForwardBase" }

			CGPROGRAM
	
			#include "UnityCG.cginc"
			#include "AutoLight.cginc"

			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_fwdbase

			struct v2f 
			{
				float4 pos : SV_POSITION; 
				LIGHTING_COORDS(0,1)
			};

			v2f vert(appdata_base v)
			{
				v2f o; 
				o.pos = UnityObjectToClipPos(v.vertex); 
				TRANSFER_VERTEX_TO_FRAGMENT(o);

				return o; 
			}
	
			fixed4 frag(v2f i) : COLOR 
			{
				float attenuation = LIGHT_ATTENUATION(i);

				return attenuation;
			} 

			ENDCG 
		} 
	} 
	Fallback "VertexLit"
}