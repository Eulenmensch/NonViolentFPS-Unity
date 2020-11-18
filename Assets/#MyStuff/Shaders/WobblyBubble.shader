// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "WobblyBubble"
{
	Properties
	{
		_UnlitColor("Unlit Color", Color) = (0,0,0,0)
		_LitColor("Lit Color", Color) = (0,0,0,0)
		_ShadowColor("Shadow Color", Color) = (0.121957,0.2169811,0.05629227,0)
		_Opacity("Opacity", Float) = 0
		_StippleNoiseAmount("Stipple Noise Amount", Float) = 0.48
		_DisplacementAmount("Displacement Amount", Float) = 0
		_DisplacementScrollSpeed("Displacement Scroll Speed", Float) = 0.2
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" }
		Cull Back
		CGPROGRAM
		#include "UnityPBSLighting.cginc"
		#include "UnityShaderVariables.cginc"
		#include "UnityCG.cginc"
		#pragma target 3.0
		#pragma surface surf StandardCustomLighting alpha:fade keepalpha noshadow vertex:vertexDataFunc 
		struct Input
		{
			float3 worldNormal;
			float3 worldPos;
			float2 uv_texcoord;
		};

		struct SurfaceOutputCustomLightingCustom
		{
			half3 Albedo;
			half3 Normal;
			half3 Emission;
			half Metallic;
			half Smoothness;
			half Occlusion;
			half Alpha;
			Input SurfInput;
			UnityGIInput GIData;
		};

		uniform float _DisplacementScrollSpeed;
		uniform float _DisplacementAmount;
		uniform float _Opacity;
		uniform float4 _ShadowColor;
		uniform float4 _UnlitColor;
		uniform float4 _LitColor;
		uniform float _StippleNoiseAmount;


		float2 voronoihash117( float2 p )
		{
			
			p = float2( dot( p, float2( 127.1, 311.7 ) ), dot( p, float2( 269.5, 183.3 ) ) );
			return frac( sin( p ) *43758.5453);
		}


		float voronoi117( float2 v, float time, inout float2 id, inout float2 mr, float smoothness )
		{
			float2 n = floor( v );
			float2 f = frac( v );
			float F1 = 8.0;
			float F2 = 8.0; float2 mg = 0;
			for ( int j = -1; j <= 1; j++ )
			{
				for ( int i = -1; i <= 1; i++ )
			 	{
			 		float2 g = float2( i, j );
			 		float2 o = voronoihash117( n + g );
					o = ( sin( time + o * 6.2831 ) * 0.5 + 0.5 ); float2 r = f - g - o;
					float d = 0.5 * dot( r, r );
			 		if( d<F1 ) {
			 			F2 = F1;
			 			F1 = d; mg = g; mr = r; id = o;
			 		} else if( d<F2 ) {
			 			F2 = d;
			 		}
			 	}
			}
			return F1;
		}


		struct Gradient
		{
			int type;
			int colorsLength;
			int alphasLength;
			float4 colors[8];
			float2 alphas[8];
		};


		Gradient NewGradient(int type, int colorsLength, int alphasLength, 
		float4 colors0, float4 colors1, float4 colors2, float4 colors3, float4 colors4, float4 colors5, float4 colors6, float4 colors7,
		float2 alphas0, float2 alphas1, float2 alphas2, float2 alphas3, float2 alphas4, float2 alphas5, float2 alphas6, float2 alphas7)
		{
			Gradient g;
			g.type = type;
			g.colorsLength = colorsLength;
			g.alphasLength = alphasLength;
			g.colors[ 0 ] = colors0;
			g.colors[ 1 ] = colors1;
			g.colors[ 2 ] = colors2;
			g.colors[ 3 ] = colors3;
			g.colors[ 4 ] = colors4;
			g.colors[ 5 ] = colors5;
			g.colors[ 6 ] = colors6;
			g.colors[ 7 ] = colors7;
			g.alphas[ 0 ] = alphas0;
			g.alphas[ 1 ] = alphas1;
			g.alphas[ 2 ] = alphas2;
			g.alphas[ 3 ] = alphas3;
			g.alphas[ 4 ] = alphas4;
			g.alphas[ 5 ] = alphas5;
			g.alphas[ 6 ] = alphas6;
			g.alphas[ 7 ] = alphas7;
			return g;
		}


		float4 SampleGradient( Gradient gradient, float time )
		{
			float3 color = gradient.colors[0].rgb;
			UNITY_UNROLL
			for (int c = 1; c < 8; c++)
			{
			float colorPos = saturate((time - gradient.colors[c-1].w) / (gradient.colors[c].w - gradient.colors[c-1].w)) * step(c, (float)gradient.colorsLength-1);
			color = lerp(color, gradient.colors[c].rgb, lerp(colorPos, step(0.01, colorPos), gradient.type));
			}
			#ifndef UNITY_COLORSPACE_GAMMA
			color = half3(GammaToLinearSpaceExact(color.r), GammaToLinearSpaceExact(color.g), GammaToLinearSpaceExact(color.b));
			#endif
			float alpha = gradient.alphas[0].x;
			UNITY_UNROLL
			for (int a = 1; a < 8; a++)
			{
			float alphaPos = saturate((time - gradient.alphas[a-1].y) / (gradient.alphas[a].y - gradient.alphas[a-1].y)) * step(a, (float)gradient.alphasLength-1);
			alpha = lerp(alpha, gradient.alphas[a].x, lerp(alphaPos, step(0.01, alphaPos), gradient.type));
			}
			return float4(color, alpha);
		}


		float2 voronoihash69( float2 p )
		{
			
			p = float2( dot( p, float2( 127.1, 311.7 ) ), dot( p, float2( 269.5, 183.3 ) ) );
			return frac( sin( p ) *43758.5453);
		}


		float voronoi69( float2 v, float time, inout float2 id, inout float2 mr, float smoothness )
		{
			float2 n = floor( v );
			float2 f = frac( v );
			float F1 = 8.0;
			float F2 = 8.0; float2 mg = 0;
			for ( int j = -1; j <= 1; j++ )
			{
				for ( int i = -1; i <= 1; i++ )
			 	{
			 		float2 g = float2( i, j );
			 		float2 o = voronoihash69( n + g );
					o = ( sin( time + o * 6.2831 ) * 0.5 + 0.5 ); float2 r = f - g - o;
					float d = 0.5 * dot( r, r );
			 		if( d<F1 ) {
			 			F2 = F1;
			 			F1 = d; mg = g; mr = r; id = o;
			 		} else if( d<F2 ) {
			 			F2 = d;
			 		}
			 	}
			}
			return F1;
		}


		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			float3 ase_vertexNormal = v.normal.xyz;
			float time117 = 0.0;
			float3 ase_vertex3Pos = v.vertex.xyz;
			float4 transform114 = mul(unity_WorldToObject,float4( ase_vertex3Pos , 0.0 ));
			float mulTime115 = _Time.y * _DisplacementScrollSpeed;
			float2 coords117 = ( transform114 + mulTime115 ).xy * 0.55;
			float2 id117 = 0;
			float2 uv117 = 0;
			float voroi117 = voronoi117( coords117, time117, id117, uv117, 0 );
			v.vertex.xyz += ( ase_vertexNormal * voroi117 * _DisplacementAmount );
			v.vertex.w = 1;
		}

		inline half4 LightingStandardCustomLighting( inout SurfaceOutputCustomLightingCustom s, half3 viewDir, UnityGI gi )
		{
			UnityGIInput data = s.GIData;
			Input i = s.SurfInput;
			half4 c = 0;
			#ifdef UNITY_PASS_FORWARDBASE
			float ase_lightAtten = data.atten;
			if( _LightColor0.a == 0)
			ase_lightAtten = 0;
			#else
			float3 ase_lightAttenRGB = gi.light.color / ( ( _LightColor0.rgb ) + 0.000001 );
			float ase_lightAtten = max( max( ase_lightAttenRGB.r, ase_lightAttenRGB.g ), ase_lightAttenRGB.b );
			#endif
			#if defined(HANDLE_SHADOWS_BLENDING_IN_GI)
			half bakedAtten = UnitySampleBakedOcclusion(data.lightmapUV.xy, data.worldPos);
			float zDist = dot(_WorldSpaceCameraPos - data.worldPos, UNITY_MATRIX_V[2].xyz);
			float fadeDist = UnityComputeShadowFadeDistance(data.worldPos, zDist);
			ase_lightAtten = UnityMixRealtimeAndBakedShadows(data.atten, bakedAtten, UnityComputeShadowFade(fadeDist));
			#endif
			SurfaceOutputStandard s89 = (SurfaceOutputStandard ) 0;
			s89.Albedo = float3( 0,0,0 );
			float3 ase_worldNormal = i.worldNormal;
			s89.Normal = ase_worldNormal;
			float4 color81 = IsGammaSpace() ? float4(0,0,0,0) : float4(0,0,0,0);
			Gradient gradient75 = NewGradient( 1, 2, 2, float4( 0, 0, 0, 0.602945 ), float4( 1, 1, 1, 1 ), 0, 0, 0, 0, 0, 0, float2( 1, 0 ), float2( 1, 1 ), 0, 0, 0, 0, 0, 0 );
			float4 LightAttenuation78 = SampleGradient( gradient75, ase_lightAtten );
			float4 lerpResult83 = lerp( _ShadowColor , color81 , LightAttenuation78);
			float4 Shadow84 = lerpResult83;
			Gradient gradient52 = NewGradient( 1, 2, 2, float4( 0, 0, 0, 0.5000076 ), float4( 1, 1, 1, 1 ), 0, 0, 0, 0, 0, 0, float2( 1, 0 ), float2( 1, 1 ), 0, 0, 0, 0, 0, 0 );
			float4 ase_vertex4Pos = mul( unity_WorldToObject, float4( i.worldPos , 1 ) );
			float3 ase_objectlightDir = normalize( ObjSpaceLightDir( ase_vertex4Pos ) );
			float3 normalizeResult50 = normalize( ase_objectlightDir );
			float3 ase_vertexNormal = mul( unity_WorldToObject, float4( ase_worldNormal, 0 ) );
			float3 normalizeResult49 = normalize( ase_vertexNormal );
			float dotResult51 = dot( normalizeResult50 , normalizeResult49 );
			float time69 = 0.0;
			float2 coords69 = i.uv_texcoord * -46.5;
			float2 id69 = 0;
			float2 uv69 = 0;
			float voroi69 = voronoi69( coords69, time69, id69, uv69, 0 );
			float4 lerpResult56 = lerp( _UnlitColor , _LitColor , SampleGradient( gradient52, ( dotResult51 + ( voroi69 * _StippleNoiseAmount ) ) ));
			float4 Albedo57 = lerpResult56;
			float4 lerpResult88 = lerp( Shadow84 , Albedo57 , LightAttenuation78);
			s89.Emission = lerpResult88.rgb;
			s89.Metallic = 0.0;
			s89.Smoothness = 0.0;
			s89.Occlusion = 1.0;

			data.light = gi.light;

			UnityGI gi89 = gi;
			#ifdef UNITY_PASS_FORWARDBASE
			Unity_GlossyEnvironmentData g89 = UnityGlossyEnvironmentSetup( s89.Smoothness, data.worldViewDir, s89.Normal, float3(0,0,0));
			gi89 = UnityGlobalIllumination( data, s89.Occlusion, s89.Normal, g89 );
			#endif

			float3 surfResult89 = LightingStandard ( s89, viewDir, gi89 ).rgb;
			surfResult89 += s89.Emission;

			#ifdef UNITY_PASS_FORWARDADD//89
			surfResult89 -= s89.Emission;
			#endif//89
			c.rgb = surfResult89;
			c.a = _Opacity;
			return c;
		}

		inline void LightingStandardCustomLighting_GI( inout SurfaceOutputCustomLightingCustom s, UnityGIInput data, inout UnityGI gi )
		{
			s.GIData = data;
		}

		void surf( Input i , inout SurfaceOutputCustomLightingCustom o )
		{
			o.SurfInput = i;
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18600
2326.286;66;1447;934;1324.589;-716.9473;1;True;False
Node;AmplifyShaderEditor.CommentaryNode;46;-1824,-1856;Inherit;False;1714;824;;13;57;56;55;54;53;52;51;50;49;48;47;71;73;Toon Colors;0,1,0.7750711,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;74;-1824,-48;Inherit;False;797;275;;4;78;77;76;75;Light Attenuation;1,0,0.9104991,1;0;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;70;-1618,-916;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.NormalVertexDataNode;47;-1728,-1216;Inherit;False;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ObjSpaceLightDirHlpNode;48;-1776,-1376;Inherit;False;1;0;FLOAT;0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.NormalizeNode;50;-1536,-1376;Inherit;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.NormalizeNode;49;-1536,-1216;Inherit;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.VoronoiNode;69;-1372.413,-972.8522;Inherit;True;0;0;1;0;1;False;1;False;False;4;0;FLOAT2;0,0;False;1;FLOAT;0;False;2;FLOAT;-46.5;False;3;FLOAT;0;False;3;FLOAT;0;FLOAT2;1;FLOAT2;2
Node;AmplifyShaderEditor.LightAttenuation;76;-1760,80;Inherit;False;0;1;FLOAT;0
Node;AmplifyShaderEditor.GradientNode;75;-1760,0;Inherit;False;1;2;2;0,0,0,0.602945;1,1,1,1;1,0;1,1;0;1;OBJECT;0
Node;AmplifyShaderEditor.RangedFloatNode;72;-1346.613,-1067.752;Inherit;False;Property;_StippleNoiseAmount;Stipple Noise Amount;8;0;Create;True;0;0;False;0;False;0.48;0.48;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.GradientSampleNode;77;-1552,0;Inherit;True;2;0;OBJECT;;False;1;FLOAT;0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;71;-1178.913,-1123.652;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DotProductOpNode;51;-1344,-1376;Inherit;True;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;79;-1808,688;Inherit;False;706;550;;5;84;83;82;81;80;Shadows;0,0,0,1;0;0
Node;AmplifyShaderEditor.GradientNode;52;-1328,-1456;Inherit;False;1;2;2;0,0,0,0.5000076;1,1,1,1;1,0;1,1;0;1;OBJECT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;78;-1248,0;Inherit;False;LightAttenuation;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;73;-1045.013,-1193.852;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;54;-1120,-1808;Inherit;False;Property;_UnlitColor;Unlit Color;1;0;Create;True;0;0;False;0;False;0,0,0,0;0.215735,0.45283,0.411419,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;82;-1760,1120;Inherit;False;78;LightAttenuation;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.GradientSampleNode;55;-1120,-1456;Inherit;True;2;0;OBJECT;;False;1;FLOAT;0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;81;-1760,928;Inherit;False;Constant;_Black;Black;2;0;Create;True;0;0;False;0;False;0,0,0,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;53;-1120,-1632;Inherit;False;Property;_LitColor;Lit Color;2;0;Create;True;0;0;False;0;False;0,0,0,0;0.4138927,0.6698113,0.6175831,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;80;-1760,736;Inherit;False;Property;_ShadowColor;Shadow Color;3;0;Create;True;0;0;False;0;False;0.121957,0.2169811,0.05629227,0;0.2549018,0.1725489,0.3490195,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;83;-1504,912;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;123;-1004.589,1047.947;Inherit;False;Property;_DisplacementScrollSpeed;Displacement Scroll Speed;10;0;Create;True;0;0;False;0;False;0.2;0.2;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.PosVertexDataNode;113;-949.6603,729.4712;Inherit;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;56;-752,-1648;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;57;-336,-1648;Inherit;False;Albedo;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.WorldToObjectTransfNode;114;-741.6602,765.8711;Inherit;False;1;0;FLOAT4;0,0,0,1;False;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RegisterLocalVarNode;84;-1328,912;Inherit;False;Shadow;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleTimeNode;115;-734.3727,994.9841;Inherit;False;1;0;FLOAT;0.13;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;86;-698.0563,-79.36491;Inherit;True;57;Albedo;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;87;-698.0563,128.6351;Inherit;True;78;LightAttenuation;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;116;-501.3729,855.9844;Inherit;False;2;2;0;FLOAT4;0,0,0,0;False;1;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.GetLocalVarNode;85;-698.0563,-287.3649;Inherit;True;84;Shadow;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.NormalVertexDataNode;118;-343.1336,743.093;Inherit;False;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.CommentaryNode;66;-323.2789,-728.8352;Inherit;False;384;199;Remains here for now since I might want Opacity;1;28;;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;58;-1824,-704;Inherit;False;1066.205;518.7313;;9;59;23;22;11;7;10;6;8;9;Depth;1,0.5562034,0,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;122;-308.3804,1158.216;Inherit;False;Property;_DisplacementAmount;Displacement Amount;9;0;Create;True;0;0;False;0;False;0;0.64;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;88;-442.0563,-127.3649;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.VoronoiNode;117;-345.3729,892.9844;Inherit;True;0;0;1;0;1;False;1;False;False;4;0;FLOAT2;0,0;False;1;FLOAT;0;False;2;FLOAT;0.55;False;3;FLOAT;0;False;3;FLOAT;0;FLOAT2;1;FLOAT2;2
Node;AmplifyShaderEditor.SimpleDivideOpNode;10;-1312,-560;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;59;-944,-320;Inherit;False;Depth;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;93;-1906.234,361.7025;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;22;-1168,-560;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;90;-2345.32,569.3548;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;94;-1772.334,291.5024;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ScreenDepthNode;6;-1776,-560;Inherit;False;0;True;1;0;FLOAT4;0,0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;28;-206.2787,-651.8354;Inherit;False;Property;_Opacity;Opacity;7;0;Create;True;0;0;False;0;False;0;0.92;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;9;-1776,-336;Inherit;False;Property;_Offset;Offset;5;0;Create;True;0;0;False;0;False;0;0.99;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ScaleAndOffsetNode;7;-1520,-384;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;1;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;67;-101.4956,-302.0659;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;23;-1328,-448;Inherit;False;Property;_MaxDepth;Max Depth;6;0;Create;True;0;0;False;0;False;0;1.58;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.VoronoiNode;91;-2099.733,512.5023;Inherit;True;0;0;1;0;1;False;1;False;False;4;0;FLOAT2;0,0;False;1;FLOAT;0;False;2;FLOAT;9.4;False;3;FLOAT;0;False;3;FLOAT;0;FLOAT2;1;FLOAT2;2
Node;AmplifyShaderEditor.RangedFloatNode;11;-1520,-480;Inherit;False;Property;_DepthDivisor;Depth Divisor;0;0;Create;True;0;0;False;0;False;0;4.01;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;8;-1776,-448;Inherit;False;Property;_Scale;Scale;4;0;Create;True;0;0;False;0;False;0;0.48;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.CustomStandardSurface;89;-62.00305,-88.52118;Inherit;False;Metallic;Tangent;6;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,1;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;1;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;121;-107.4795,843.231;Inherit;False;3;3;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;92;-2072.934,417.6025;Inherit;False;Constant;_Float1;Float 1;7;0;Create;True;0;0;False;0;False;-3.2;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;208,-336;Float;False;True;-1;2;ASEMaterialInspector;0;0;CustomLighting;WobblyBubble;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Back;2;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;0.5;True;False;0;True;Transparent;;Transparent;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;False;2;5;False;-1;10;False;-1;0;5;False;-1;10;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;8;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;False;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT3;0,0,0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;50;0;48;0
WireConnection;49;0;47;0
WireConnection;69;0;70;0
WireConnection;77;0;75;0
WireConnection;77;1;76;0
WireConnection;71;0;69;0
WireConnection;71;1;72;0
WireConnection;51;0;50;0
WireConnection;51;1;49;0
WireConnection;78;0;77;0
WireConnection;73;0;51;0
WireConnection;73;1;71;0
WireConnection;55;0;52;0
WireConnection;55;1;73;0
WireConnection;83;0;80;0
WireConnection;83;1;81;0
WireConnection;83;2;82;0
WireConnection;56;0;54;0
WireConnection;56;1;53;0
WireConnection;56;2;55;0
WireConnection;57;0;56;0
WireConnection;114;0;113;0
WireConnection;84;0;83;0
WireConnection;115;0;123;0
WireConnection;116;0;114;0
WireConnection;116;1;115;0
WireConnection;88;0;85;0
WireConnection;88;1;86;0
WireConnection;88;2;87;0
WireConnection;117;0;116;0
WireConnection;10;0;6;0
WireConnection;10;1;11;0
WireConnection;59;0;22;0
WireConnection;93;0;91;0
WireConnection;93;1;92;0
WireConnection;22;0;10;0
WireConnection;22;2;23;0
WireConnection;94;0;76;0
WireConnection;94;1;93;0
WireConnection;7;0;6;0
WireConnection;7;1;8;0
WireConnection;7;2;9;0
WireConnection;67;0;22;0
WireConnection;67;1;88;0
WireConnection;91;0;90;0
WireConnection;89;2;88;0
WireConnection;121;0;118;0
WireConnection;121;1;117;0
WireConnection;121;2;122;0
WireConnection;0;9;28;0
WireConnection;0;13;89;0
WireConnection;0;11;121;0
ASEEND*/
//CHKSM=2A923B8F2679944C4DCC7ED34CC9093CD814C2B0