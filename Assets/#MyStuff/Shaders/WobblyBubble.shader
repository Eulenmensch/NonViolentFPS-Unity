// Upgrade NOTE: upgraded instancing buffer 'WobblyBubble' to new syntax.

// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "WobblyBubble"
{
	Properties
	{
		_UnlitColor("Unlit Color", Color) = (0,0,0,0)
		_LitColor("Lit Color", Color) = (0,0,0,0)
		_StippleNoiseAmount("Stipple Noise Amount", Float) = 0.48
		_DisplacementAmount("Displacement Amount", Float) = 0
		_DisplacementScrollSpeed("Displacement Scroll Speed", Float) = 0.2
		_Opacity("Opacity", Range( 0 , 1)) = 0
		_FresnelPower("Fresnel Power", Float) = 0
		_FresnelScale("Fresnel Scale", Float) = 0
		_FresnelBias("Fresnel Bias", Range( -1 , 1)) = 0
		_PosterizePower("Posterize Power", Float) = 0
		_FresnelLightDirInfluence("Fresnel Light Dir Influence", Range( 0 , 1)) = 0
		_SpecularIntensity("Specular Intensity", Float) = 0
		_SpecularColor("Specular Color", Color) = (0,0,0,0)
		_Ice("Ice", Float) = 0
		[HDR]_IceColor("Ice Color", Color) = (0.8160377,1,0.9818605,0)
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Back
		CGPROGRAM
		#include "UnityPBSLighting.cginc"
		#include "UnityShaderVariables.cginc"
		#include "UnityCG.cginc"
		#pragma target 3.0
		#pragma multi_compile_instancing
		#pragma surface surf StandardCustomLighting alpha:fade keepalpha noshadow vertex:vertexDataFunc 
		struct Input
		{
			float3 worldPos;
			float3 worldNormal;
			float2 uv_texcoord;
			float3 viewDir;
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
		uniform float4 _IceColor;
		uniform float _StippleNoiseAmount;
		uniform float _SpecularIntensity;
		uniform float4 _SpecularColor;
		uniform float _Opacity;
		uniform float _PosterizePower;
		uniform float _FresnelLightDirInfluence;
		uniform float _FresnelBias;
		uniform float _FresnelScale;
		uniform float _FresnelPower;
		uniform float4 _UnlitColor;
		uniform float4 _LitColor;

		UNITY_INSTANCING_BUFFER_START(WobblyBubble)
			UNITY_DEFINE_INSTANCED_PROP(float, _Ice)
#define _Ice_arr WobblyBubble
		UNITY_INSTANCING_BUFFER_END(WobblyBubble)


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


		float2 voronoihash181( float2 p )
		{
			p = p - 8 * floor( p / 8 );
			p = float2( dot( p, float2( 127.1, 311.7 ) ), dot( p, float2( 269.5, 183.3 ) ) );
			return frac( sin( p ) *43758.5453);
		}


		float voronoi181( float2 v, float time, inout float2 id, inout float2 mr, float smoothness )
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
			 		float2 o = voronoihash181( n + g );
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
			
F1 = 8.0;
for ( int j = -2; j <= 2; j++ )
{
for ( int i = -2; i <= 2; i++ )
{
float2 g = mg + float2( i, j );
float2 o = voronoihash181( n + g );
		o = ( sin( time + o * 6.2831 ) * 0.5 + 0.5 ); float2 r = f - g - o;
float d = dot( 0.5 * ( r + mr ), normalize( r - mr ) );
F1 = min( F1, d );
}
}
return F1;
		}


		float4 SampleGradient( Gradient gradient, float time )
		{
			float3 color = gradient.colors[0].rgb;
			UNITY_UNROLL
			for (int c = 1; c < 8; c++)
			{
			float colorPos = saturate((time - gradient.colors[c-1].w) / ( 0.00001 + (gradient.colors[c].w - gradient.colors[c-1].w)) * step(c, (float)gradient.colorsLength-1));
			color = lerp(color, gradient.colors[c].rgb, lerp(colorPos, step(0.01, colorPos), gradient.type));
			}
			#ifndef UNITY_COLORSPACE_GAMMA
			color = half3(GammaToLinearSpaceExact(color.r), GammaToLinearSpaceExact(color.g), GammaToLinearSpaceExact(color.b));
			#endif
			float alpha = gradient.alphas[0].x;
			UNITY_UNROLL
			for (int a = 1; a < 8; a++)
			{
			float alphaPos = saturate((time - gradient.alphas[a-1].y) / ( 0.00001 + (gradient.alphas[a].y - gradient.alphas[a-1].y)) * step(a, (float)gradient.alphasLength-1));
			alpha = lerp(alpha, gradient.alphas[a].x, lerp(alphaPos, step(0.01, alphaPos), gradient.type));
			}
			return float4(color, alpha);
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
			float3 Distortion127 = ( ase_vertexNormal * voroi117 * _DisplacementAmount );
			v.vertex.xyz += Distortion127;
			v.vertex.w = 1;
		}

		inline half4 LightingStandardCustomLighting( inout SurfaceOutputCustomLightingCustom s, half3 viewDir, UnityGI gi )
		{
			UnityGIInput data = s.GIData;
			Input i = s.SurfInput;
			half4 c = 0;
			float3 ase_worldPos = i.worldPos;
			#if defined(LIGHTMAP_ON) && UNITY_VERSION < 560 //aseld
			float3 ase_worldlightDir = 0;
			#else //aseld
			float3 ase_worldlightDir = normalize( UnityWorldSpaceLightDir( ase_worldPos ) );
			#endif //aseld
			float3 normalizeResult163 = normalize( ( i.viewDir + ase_worldlightDir ) );
			float3 ase_worldNormal = i.worldNormal;
			float dotResult166 = dot( normalizeResult163 , ase_worldNormal );
			float temp_output_3_0_g1 = ( dotResult166 - ( 1.0 - _SpecularIntensity ) );
			float temp_output_168_0 = saturate( ( temp_output_3_0_g1 / fwidth( temp_output_3_0_g1 ) ) );
			float4 Specular171 = ( temp_output_168_0 * _SpecularColor );
			float fresnelNdotV124 = dot( ase_worldNormal, ( ( _FresnelLightDirInfluence * ase_worldlightDir ) + i.viewDir ) );
			float fresnelNode124 = ( _FresnelBias + _FresnelScale * pow( 1.0 - fresnelNdotV124, _FresnelPower ) );
			float4 temp_cast_2 = (fresnelNode124).xxxx;
			float div125=256.0/float((int)_PosterizePower);
			float4 posterize125 = ( floor( temp_cast_2 * div125 ) / div125 );
			float4 Fresnel132 = posterize125;
			float4 temp_cast_3 = (0.1).xxxx;
			float4 temp_cast_4 = (0.8).xxxx;
			float4 clampResult156 = clamp( ( _Opacity + Fresnel132 ) , temp_cast_3 , temp_cast_4 );
			SurfaceOutputStandard s89 = (SurfaceOutputStandard ) 0;
			s89.Albedo = float3( 0,0,0 );
			s89.Normal = ase_worldNormal;
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
			float Stipples135 = ( voroi69 * _StippleNoiseAmount );
			float time181 = 3.85;
			float2 coords181 = i.uv_texcoord * 8.0;
			float2 id181 = 0;
			float2 uv181 = 0;
			float voroi181 = voronoi181( coords181, time181, id181, uv181, 0 );
			float smoothstepResult183 = smoothstep( 0.21 , -0.04 , voroi181);
			float _Ice_Instance = UNITY_ACCESS_INSTANCED_PROP(_Ice_arr, _Ice);
			float IceCracks200 = ( smoothstepResult183 * _Ice_Instance );
			float4 lerpResult56 = lerp( _UnlitColor , _LitColor , SampleGradient( gradient52, ( dotResult51 + Stipples135 + IceCracks200 ) ));
			float4 Albedo57 = lerpResult56;
			s89.Emission = ( Albedo57 + Specular171 ).rgb;
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
			c.a = ( Specular171 + clampResult156 ).r;
			return c;
		}

		inline void LightingStandardCustomLighting_GI( inout SurfaceOutputCustomLightingCustom s, UnityGIInput data, inout UnityGI gi )
		{
			s.GIData = data;
		}

		void surf( Input i , inout SurfaceOutputCustomLightingCustom o )
		{
			o.SurfInput = i;
			Gradient gradient52 = NewGradient( 1, 2, 2, float4( 0, 0, 0, 0.5000076 ), float4( 1, 1, 1, 1 ), 0, 0, 0, 0, 0, 0, float2( 1, 0 ), float2( 1, 1 ), 0, 0, 0, 0, 0, 0 );
			float4 ase_vertex4Pos = mul( unity_WorldToObject, float4( i.worldPos , 1 ) );
			float3 ase_objectlightDir = normalize( ObjSpaceLightDir( ase_vertex4Pos ) );
			float3 normalizeResult50 = normalize( ase_objectlightDir );
			float3 ase_worldNormal = i.worldNormal;
			float3 ase_vertexNormal = mul( unity_WorldToObject, float4( ase_worldNormal, 0 ) );
			float3 normalizeResult49 = normalize( ase_vertexNormal );
			float dotResult51 = dot( normalizeResult50 , normalizeResult49 );
			float time69 = 0.0;
			float2 coords69 = i.uv_texcoord * -46.5;
			float2 id69 = 0;
			float2 uv69 = 0;
			float voroi69 = voronoi69( coords69, time69, id69, uv69, 0 );
			float Stipples135 = ( voroi69 * _StippleNoiseAmount );
			float time181 = 3.85;
			float2 coords181 = i.uv_texcoord * 8.0;
			float2 id181 = 0;
			float2 uv181 = 0;
			float voroi181 = voronoi181( coords181, time181, id181, uv181, 0 );
			float smoothstepResult183 = smoothstep( 0.21 , -0.04 , voroi181);
			float _Ice_Instance = UNITY_ACCESS_INSTANCED_PROP(_Ice_arr, _Ice);
			float IceCracks200 = ( smoothstepResult183 * _Ice_Instance );
			o.Emission = ( _IceColor * SampleGradient( gradient52, ( dotResult51 + Stipples135 + IceCracks200 ) ) * _Ice_Instance ).rgb;
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18712
0;6;1920;1023;2989.881;1242.565;2.332068;True;True
Node;AmplifyShaderEditor.CommentaryNode;134;-1024,688;Inherit;False;856.0496;546.5852;;5;135;71;72;70;69;Stipples;1,0,0.09470558,1;0;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;197;-1830.167,0.1241505;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;70;-976,752;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.VoronoiNode;181;-1566.843,-0.01978755;Inherit;True;0;0;1;4;1;True;8;False;False;4;0;FLOAT2;0,0;False;1;FLOAT;3.85;False;2;FLOAT;8;False;3;FLOAT;0;False;3;FLOAT;0;FLOAT2;1;FLOAT2;2
Node;AmplifyShaderEditor.CommentaryNode;159;53.54516,-2596.557;Inherit;False;1581.426;651.6493;;14;171;174;175;168;169;170;166;165;163;162;161;160;179;180;Specular;1,1,1,1;0;0
Node;AmplifyShaderEditor.SmoothstepOpNode;183;-1394.925,3.321422;Inherit;True;3;0;FLOAT;0;False;1;FLOAT;0.21;False;2;FLOAT;-0.04;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;131;-1824,-2800;Inherit;False;1693.844;849.3478;;12;152;132;125;140;124;139;138;137;151;146;144;153;Fresnel;0.1797888,1,0,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;202;-1302.035,225.3756;Inherit;False;InstancedProperty;_Ice;Ice;17;0;Create;True;0;0;0;False;0;False;0;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;72;-800,1040;Inherit;False;Property;_StippleNoiseAmount;Stipple Noise Amount;6;0;Create;True;0;0;0;False;0;False;0.48;0.48;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;46;-1824,-1808;Inherit;False;1895.545;919.4859;;14;57;56;52;55;54;53;51;136;73;49;50;48;47;203;Toon Colors;0,1,0.7750711,1;0;0
Node;AmplifyShaderEditor.VoronoiNode;69;-752,752;Inherit;True;0;0;1;0;1;False;1;False;False;4;0;FLOAT2;0,0;False;1;FLOAT;0;False;2;FLOAT;-46.5;False;3;FLOAT;0;False;3;FLOAT;0;FLOAT2;1;FLOAT2;2
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;201;-1075.045,-3.126452;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ObjSpaceLightDirHlpNode;48;-1776,-1328;Inherit;False;1;0;FLOAT;0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.ViewDirInputsCoordNode;160;128,-2496;Inherit;False;World;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;71;-512,896;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WorldSpaceLightDirHlpNode;146;-1760,-2576;Inherit;False;False;1;0;FLOAT;0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.WorldSpaceLightDirHlpNode;161;128,-2304;Inherit;False;False;1;0;FLOAT;0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RangedFloatNode;153;-1760,-2672;Inherit;False;Property;_FresnelLightDirInfluence;Fresnel Light Dir Influence;14;0;Create;True;0;0;0;False;0;False;0;0.367;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.NormalVertexDataNode;47;-1728,-1168;Inherit;False;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;162;400,-2384;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;135;-364.4829,945.7006;Inherit;False;Stipples;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;152;-1549,-2560;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.NormalizeNode;49;-1536,-1168;Inherit;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.ViewDirInputsCoordNode;144;-1760,-2432;Inherit;False;World;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.NormalizeNode;50;-1536,-1328;Inherit;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;200;-876.8086,-4.639705;Inherit;False;IceCracks;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;169;544,-2496;Inherit;False;Property;_SpecularIntensity;Specular Intensity;15;0;Create;True;0;0;0;False;0;False;0;0.05;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DotProductOpNode;51;-1344,-1328;Inherit;True;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;151;-1416,-2463;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;137;-1664,-2096;Inherit;False;Property;_FresnelPower;Fresnel Power;10;0;Create;True;0;0;0;False;0;False;0;2.01;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;139;-1664,-2192;Inherit;False;Property;_FresnelScale;Fresnel Scale;11;0;Create;True;0;0;0;False;0;False;0;0.97;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;136;-1312,-1088;Inherit;False;135;Stipples;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;126;-1808,1344;Inherit;False;1325.162;609.9359;;10;127;123;113;121;118;122;117;116;115;114;Distortion;0.0005874634,0,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;138;-1760,-2288;Inherit;False;Property;_FresnelBias;Fresnel Bias;12;0;Create;True;0;0;0;False;0;False;0;-0.33;-1;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;203;-1307.494,-977.9551;Inherit;False;200;IceCracks;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.NormalizeNode;163;515.5265,-2384;Inherit;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.WorldNormalVector;180;430.5964,-2090.873;Inherit;False;False;1;0;FLOAT3;0,0,1;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.FresnelNode;124;-1152,-2432;Inherit;True;Standard;WorldNormal;ViewDir;False;False;5;0;FLOAT3;0,0,1;False;4;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;5;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;73;-1104,-1168;Inherit;True;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GradientNode;52;-1104,-1264;Inherit;False;1;2;2;0,0,0,0.5000076;1,1,1,1;1,0;1,1;0;1;OBJECT;0
Node;AmplifyShaderEditor.RangedFloatNode;123;-1760,1728;Inherit;False;Property;_DisplacementScrollSpeed;Displacement Scroll Speed;8;0;Create;True;0;0;0;False;0;False;0.2;0.28;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;170;784,-2416;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DotProductOpNode;166;704,-2304;Inherit;True;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PosVertexDataNode;113;-1760,1392;Inherit;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;140;-1040,-2208;Inherit;False;Property;_PosterizePower;Posterize Power;13;0;Create;True;0;0;0;False;0;False;0;66.38;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.GradientSampleNode;55;-816,-1264;Inherit;True;2;0;OBJECT;;False;1;FLOAT;0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PosterizeNode;125;-784,-2432;Inherit;True;2;2;1;COLOR;0,0,0,0;False;0;INT;2;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleTimeNode;115;-1488,1664;Inherit;False;1;0;FLOAT;0.13;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;53;-816,-1440;Inherit;False;Property;_LitColor;Lit Color;1;0;Create;True;0;0;0;False;0;False;0,0,0,0;1,1,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.WorldToObjectTransfNode;114;-1568,1392;Inherit;False;1;0;FLOAT4;0,0,0,1;False;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;54;-816,-1616;Inherit;False;Property;_UnlitColor;Unlit Color;0;0;Create;True;0;0;0;False;0;False;0,0,0,0;0.5235848,0.863175,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.FunctionNode;168;960,-2368;Inherit;True;Step Antialiasing;-1;;1;2a825e80dfb3290468194f83380797bd;0;2;1;FLOAT;0.9;False;2;FLOAT;0.69;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;175;960,-2144;Inherit;False;Property;_SpecularColor;Specular Color;16;0;Create;True;0;0;0;False;0;False;0,0,0,0;0.8301887,0.7679786,0.7244571,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;116;-1280,1552;Inherit;False;2;2;0;FLOAT4;0,0,0,0;False;1;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;174;1216,-2352;Inherit;True;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.CommentaryNode;66;416,-320;Inherit;False;384;199;Remains here for now since I might want Opacity;1;28;;1,1,1,1;0;0
Node;AmplifyShaderEditor.LerpOp;56;-448,-1456;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;132;-480,-2432;Inherit;False;Fresnel;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;57;-192,-1456;Inherit;False;Albedo;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.VoronoiNode;117;-1088,1552;Inherit;True;0;0;1;0;1;False;1;False;False;4;0;FLOAT2;0,0;False;1;FLOAT;0;False;2;FLOAT;0.55;False;3;FLOAT;0;False;3;FLOAT;0;FLOAT2;1;FLOAT2;2
Node;AmplifyShaderEditor.GetLocalVarNode;133;704,-64;Inherit;False;132;Fresnel;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;122;-1136,1824;Inherit;False;Property;_DisplacementAmount;Displacement Amount;7;0;Create;True;0;0;0;False;0;False;0;0.1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.NormalVertexDataNode;118;-1088,1408;Inherit;False;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RegisterLocalVarNode;171;1424,-2352;Inherit;False;Specular;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;28;512,-240;Inherit;False;Property;_Opacity;Opacity;9;0;Create;True;0;0;0;False;0;False;0;0.9896376;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;86;224,256;Inherit;True;57;Albedo;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;142;896,-80;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;121;-848,1536;Inherit;False;3;3;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;157;880,32;Inherit;False;Constant;_Float0;Float 0;16;0;Create;True;0;0;0;False;0;False;0.1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;158;880,112;Inherit;False;Constant;_Float2;Float 2;16;0;Create;True;0;0;0;False;0;False;0.8;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;172;480,448;Inherit;True;171;Specular;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;176;896,-192;Inherit;False;171;Specular;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;193;-185.2716,-254.6555;Inherit;False;Property;_IceColor;Ice Color;18;1;[HDR];Create;True;0;0;0;False;0;False;0.8160377,1,0.9818605,0;0.8160377,1,0.9818605,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;173;736,304;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.CommentaryNode;129;-736,-688;Inherit;False;456.3;117.9;for some weird reason the register variable doesnt work here;1;59;;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;58;-1824,-704;Inherit;False;1066.205;518.7313;;8;23;22;11;7;10;6;8;9;Depth;1,0.5562034,0,1;0;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;127;-704,1536;Inherit;False;Distortion;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.ClampOpNode;156;1057,16;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0.09411765;False;2;COLOR;1,0,0,0.9058824;False;1;COLOR;0
Node;AmplifyShaderEditor.CustomStandardSurface;89;960,304;Inherit;False;Metallic;Tangent;6;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,1;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;1;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleAddOpNode;177;1200,-48;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ClampOpNode;22;-1168,-560;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;179;1216,-2496;Inherit;False;SpecularAlpha;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;191;132.2189,-162.7249;Inherit;False;3;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;10;-1312,-560;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;8;-1776,-448;Inherit;False;Property;_Scale;Scale;3;0;Create;True;0;0;0;False;0;False;0;0.48;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;128;1232,432;Inherit;False;127;Distortion;1;0;OBJECT;;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;11;-1520,-480;Inherit;False;Property;_DepthDivisor;Depth Divisor;2;0;Create;True;0;0;0;False;0;False;0;4.01;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.NormalVertexDataNode;165;400,-2254;Inherit;False;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;9;-1776,-336;Inherit;False;Property;_Offset;Offset;4;0;Create;True;0;0;0;False;0;False;0;0.99;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;59;-624,-656;Inherit;False;Depth;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;23;-1328,-448;Inherit;False;Property;_MaxDepth;Max Depth;5;0;Create;True;0;0;0;False;0;False;0;1.58;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ScreenDepthNode;6;-1776,-560;Inherit;False;0;True;1;0;FLOAT4;0,0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ScaleAndOffsetNode;7;-1520,-384;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;1;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;1440,-64;Float;False;True;-1;2;ASEMaterialInspector;0;0;CustomLighting;WobblyBubble;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Back;2;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;0.5;True;False;0;False;Transparent;;Transparent;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;False;2;5;False;-1;10;False;-1;0;5;False;-1;10;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;False;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT3;0,0,0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;181;0;197;0
WireConnection;183;0;181;0
WireConnection;69;0;70;0
WireConnection;201;0;183;0
WireConnection;201;1;202;0
WireConnection;71;0;69;0
WireConnection;71;1;72;0
WireConnection;162;0;160;0
WireConnection;162;1;161;0
WireConnection;135;0;71;0
WireConnection;152;0;153;0
WireConnection;152;1;146;0
WireConnection;49;0;47;0
WireConnection;50;0;48;0
WireConnection;200;0;201;0
WireConnection;51;0;50;0
WireConnection;51;1;49;0
WireConnection;151;0;152;0
WireConnection;151;1;144;0
WireConnection;163;0;162;0
WireConnection;124;4;151;0
WireConnection;124;1;138;0
WireConnection;124;2;139;0
WireConnection;124;3;137;0
WireConnection;73;0;51;0
WireConnection;73;1;136;0
WireConnection;73;2;203;0
WireConnection;170;0;169;0
WireConnection;166;0;163;0
WireConnection;166;1;180;0
WireConnection;55;0;52;0
WireConnection;55;1;73;0
WireConnection;125;1;124;0
WireConnection;125;0;140;0
WireConnection;115;0;123;0
WireConnection;114;0;113;0
WireConnection;168;1;170;0
WireConnection;168;2;166;0
WireConnection;116;0;114;0
WireConnection;116;1;115;0
WireConnection;174;0;168;0
WireConnection;174;1;175;0
WireConnection;56;0;54;0
WireConnection;56;1;53;0
WireConnection;56;2;55;0
WireConnection;132;0;125;0
WireConnection;57;0;56;0
WireConnection;117;0;116;0
WireConnection;171;0;174;0
WireConnection;142;0;28;0
WireConnection;142;1;133;0
WireConnection;121;0;118;0
WireConnection;121;1;117;0
WireConnection;121;2;122;0
WireConnection;173;0;86;0
WireConnection;173;1;172;0
WireConnection;127;0;121;0
WireConnection;156;0;142;0
WireConnection;156;1;157;0
WireConnection;156;2;158;0
WireConnection;89;2;173;0
WireConnection;177;0;176;0
WireConnection;177;1;156;0
WireConnection;22;0;10;0
WireConnection;22;2;23;0
WireConnection;179;0;168;0
WireConnection;191;0;193;0
WireConnection;191;1;55;0
WireConnection;191;2;202;0
WireConnection;10;0;6;0
WireConnection;10;1;11;0
WireConnection;59;0;22;0
WireConnection;7;0;6;0
WireConnection;7;1;8;0
WireConnection;7;2;9;0
WireConnection;0;2;191;0
WireConnection;0;9;177;0
WireConnection;0;13;89;0
WireConnection;0;11;128;0
ASEEND*/
//CHKSM=90B05D4E9FAC7B1FCEC80288CB5F58E4F0D5BCF4