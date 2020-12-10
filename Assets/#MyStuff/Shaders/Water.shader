// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Water"
{
	Properties
	{
		_PosterizeScale("Posterize Scale", Float) = 0
		_DepthScale("Depth Scale", Range( 0 , 1)) = 0
		_FoamSpeed("Foam Speed", Range( 0 , 1)) = 0
		_NormalSpeed("Normal Speed", Range( 0 , 1)) = 0
		_NormalSpeed2("Normal Speed 2", Range( 0 , 1)) = 0
		_Texture0("Texture 0", 2D) = "white" {}
		_Normal2("Normal 2", 2D) = "white" {}
		_Normal1Tiling("Normal 1 Tiling", Vector) = (0,0,0,0)
		_Normal2Tiling("Normal 2 Tiling", Vector) = (0,0,0,0)
		_SpecularIntensity("Specular Intensity", Range( 0 , 1)) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" }
		Cull Back
		CGPROGRAM
		#include "UnityPBSLighting.cginc"
		#include "UnityShaderVariables.cginc"
		#include "UnityCG.cginc"
		#pragma target 3.0
		#pragma surface surf StandardCustomLighting keepalpha noshadow 
		struct Input
		{
			float3 worldNormal;
			INTERNAL_DATA
			float2 uv_texcoord;
			float4 screenPos;
			float3 worldPos;
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

		uniform sampler2D _Texture0;
		uniform float2 _Normal1Tiling;
		uniform float _NormalSpeed;
		uniform sampler2D _Normal2;
		uniform float2 _Normal2Tiling;
		uniform float _NormalSpeed2;
		UNITY_DECLARE_DEPTH_TEXTURE( _CameraDepthTexture );
		uniform float4 _CameraDepthTexture_TexelSize;
		uniform float _FoamSpeed;
		uniform float _PosterizeScale;
		uniform float _DepthScale;
		uniform float _SpecularIntensity;


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


		float3 mod2D289( float3 x ) { return x - floor( x * ( 1.0 / 289.0 ) ) * 289.0; }

		float2 mod2D289( float2 x ) { return x - floor( x * ( 1.0 / 289.0 ) ) * 289.0; }

		float3 permute( float3 x ) { return mod2D289( ( ( x * 34.0 ) + 1.0 ) * x ); }

		float snoise( float2 v )
		{
			const float4 C = float4( 0.211324865405187, 0.366025403784439, -0.577350269189626, 0.024390243902439 );
			float2 i = floor( v + dot( v, C.yy ) );
			float2 x0 = v - i + dot( i, C.xx );
			float2 i1;
			i1 = ( x0.x > x0.y ) ? float2( 1.0, 0.0 ) : float2( 0.0, 1.0 );
			float4 x12 = x0.xyxy + C.xxzz;
			x12.xy -= i1;
			i = mod2D289( i );
			float3 p = permute( permute( i.y + float3( 0.0, i1.y, 1.0 ) ) + i.x + float3( 0.0, i1.x, 1.0 ) );
			float3 m = max( 0.5 - float3( dot( x0, x0 ), dot( x12.xy, x12.xy ), dot( x12.zw, x12.zw ) ), 0.0 );
			m = m * m;
			m = m * m;
			float3 x = 2.0 * frac( p * C.www ) - 1.0;
			float3 h = abs( x ) - 0.5;
			float3 ox = floor( x + 0.5 );
			float3 a0 = x - ox;
			m *= 1.79284291400159 - 0.85373472095314 * ( a0 * a0 + h * h );
			float3 g;
			g.x = a0.x * x0.x + h.x * x0.y;
			g.yz = a0.yz * x12.xz + h.yz * x12.yw;
			return 130.0 * dot( m, g );
		}


		float2 UnStereo( float2 UV )
		{
			#if UNITY_SINGLE_PASS_STEREO
			float4 scaleOffset = unity_StereoScaleOffset[ unity_StereoEyeIndex ];
			UV.xy = (UV.xy - scaleOffset.zw) / scaleOffset.xy;
			#endif
			return UV;
		}


		float3 InvertDepthDir72_g1( float3 In )
		{
			float3 result = In;
			#if !defined(ASE_SRP_VERSION) || ASE_SRP_VERSION <= 70301
			result *= float3(1,1,-1);
			#endif
			return result;
		}


		inline half4 LightingStandardCustomLighting( inout SurfaceOutputCustomLightingCustom s, half3 viewDir, UnityGI gi )
		{
			UnityGIInput data = s.GIData;
			Input i = s.SurfInput;
			half4 c = 0;
			SurfaceOutputStandard s56 = (SurfaceOutputStandard ) 0;
			s56.Albedo = float3( 0,0,0 );
			float mulTime73 = _Time.y * _NormalSpeed;
			float2 temp_cast_0 = (mulTime73).xx;
			float2 uv_TexCoord72 = i.uv_texcoord * _Normal1Tiling + temp_cast_0;
			float mulTime80 = _Time.y * _NormalSpeed2;
			float2 temp_cast_1 = (( 1.0 - mulTime80 )).xx;
			float2 uv_TexCoord78 = i.uv_texcoord * _Normal2Tiling + temp_cast_1;
			float4 Normals99 = ( ( tex2D( _Texture0, uv_TexCoord72 ) + tex2D( _Normal2, uv_TexCoord78 ) ) * 0.1 );
			s56.Normal = WorldNormalVector( i , Normals99.rgb );
			Gradient gradient5 = NewGradient( 0, 2, 2, float4( 1, 1, 1, 0 ), float4( 0, 0, 0, 0.60589 ), 0, 0, 0, 0, 0, 0, float2( 1, 0 ), float2( 1, 1 ), 0, 0, 0, 0, 0, 0 );
			float4 ase_screenPos = float4( i.screenPos.xyz , i.screenPos.w + 0.00000000001 );
			float4 ase_screenPosNorm = ase_screenPos / ase_screenPos.w;
			ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
			float screenDepth3 = LinearEyeDepth(SAMPLE_DEPTH_TEXTURE( _CameraDepthTexture, ase_screenPosNorm.xy ));
			float distanceDepth3 = abs( ( screenDepth3 - LinearEyeDepth( ase_screenPosNorm.z ) ) / ( 10.93 ) );
			float4 FoamDepthFade26 = SampleGradient( gradient5, distanceDepth3 );
			float mulTime46 = _Time.y * _FoamSpeed;
			float2 temp_cast_3 = (mulTime46).xx;
			float2 uv_TexCoord7 = i.uv_texcoord + temp_cast_3;
			float simplePerlin2D10 = snoise( uv_TexCoord7*-1.0 );
			simplePerlin2D10 = simplePerlin2D10*0.5 + 0.5;
			float4 temp_cast_4 = (simplePerlin2D10).xxxx;
			float4 FoamNoise31 = step( temp_cast_4 , FoamDepthFade26 );
			float4 Foam61 = ( FoamDepthFade26 * FoamNoise31 );
			Gradient gradient37 = NewGradient( 0, 2, 2, float4( 0.627451, 1, 0.7813289, 0 ), float4( 0, 0.4762425, 0.7264151, 1 ), 0, 0, 0, 0, 0, 0, float2( 1, 0 ), float2( 1, 1 ), 0, 0, 0, 0, 0, 0 );
			Gradient gradient42 = NewGradient( 0, 2, 2, float4( 1, 1, 1, 0 ), float4( 0, 0, 0, 0.60589 ), 0, 0, 0, 0, 0, 0, float2( 1, 0 ), float2( 1, 1 ), 0, 0, 0, 0, 0, 0 );
			float3 ase_worldPos = i.worldPos;
			float2 UV22_g3 = ase_screenPosNorm.xy;
			float2 localUnStereo22_g3 = UnStereo( UV22_g3 );
			float2 break64_g1 = localUnStereo22_g3;
			float clampDepth69_g1 = SAMPLE_DEPTH_TEXTURE( _CameraDepthTexture, ase_screenPosNorm.xy );
			#ifdef UNITY_REVERSED_Z
				float staticSwitch38_g1 = ( 1.0 - clampDepth69_g1 );
			#else
				float staticSwitch38_g1 = clampDepth69_g1;
			#endif
			float3 appendResult39_g1 = (float3(break64_g1.x , break64_g1.y , staticSwitch38_g1));
			float4 appendResult42_g1 = (float4((appendResult39_g1*2.0 + -1.0) , 1.0));
			float4 temp_output_43_0_g1 = mul( unity_CameraInvProjection, appendResult42_g1 );
			float3 In72_g1 = ( (temp_output_43_0_g1).xyz / (temp_output_43_0_g1).w );
			float3 localInvertDepthDir72_g1 = InvertDepthDir72_g1( In72_g1 );
			float4 appendResult49_g1 = (float4(localInvertDepthDir72_g1 , 1.0));
			float4 WaterDepthFade62 = SampleGradient( gradient42, ( distance( ase_worldPos.y , (mul( unity_CameraToWorld, appendResult49_g1 )).y ) * _DepthScale ) );
			float div24=256.0/float((int)_PosterizeScale);
			float4 posterize24 = ( floor( SampleGradient( gradient37, ( 1.0 - WaterDepthFade62 ).r ) * div24 ) / div24 );
			float4 WaterColor67 = posterize24;
			float3 ase_worldViewDir = normalize( UnityWorldSpaceViewDir( ase_worldPos ) );
			#if defined(LIGHTMAP_ON) && UNITY_VERSION < 560 //aseld
			float3 ase_worldlightDir = 0;
			#else //aseld
			float3 ase_worldlightDir = normalize( UnityWorldSpaceLightDir( ase_worldPos ) );
			#endif //aseld
			float3 normalizeResult104 = normalize( ( ase_worldViewDir + ase_worldlightDir ) );
			float dotResult105 = dot( float4( normalizeResult104 , 0.0 ) , Normals99 );
			float temp_output_3_0_g4 = ( dotResult105 - ( 1.0 - _SpecularIntensity ) );
			float Specular109 = saturate( ( temp_output_3_0_g4 / fwidth( temp_output_3_0_g4 ) ) );
			s56.Emission = ( Foam61 + WaterColor67 + Specular109 ).rgb;
			s56.Metallic = 0.0;
			s56.Smoothness = 0.0;
			s56.Occlusion = 1.0;

			data.light = gi.light;

			UnityGI gi56 = gi;
			#ifdef UNITY_PASS_FORWARDBASE
			Unity_GlossyEnvironmentData g56 = UnityGlossyEnvironmentSetup( s56.Smoothness, data.worldViewDir, s56.Normal, float3(0,0,0));
			gi56 = UnityGlobalIllumination( data, s56.Occlusion, s56.Normal, g56 );
			#endif

			float3 surfResult56 = LightingStandard ( s56, viewDir, gi56 ).rgb;
			surfResult56 += s56.Emission;

			#ifdef UNITY_PASS_FORWARDADD//56
			surfResult56 -= s56.Emission;
			#endif//56
			c.rgb = surfResult56;
			c.a = 1;
			return c;
		}

		inline void LightingStandardCustomLighting_GI( inout SurfaceOutputCustomLightingCustom s, UnityGIInput data, inout UnityGI gi )
		{
			s.GIData = data;
		}

		void surf( Input i , inout SurfaceOutputCustomLightingCustom o )
		{
			o.SurfInput = i;
			o.Normal = float3(0,0,1);
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18712
0;0;1920;1029;1028.383;-647.213;1;True;True
Node;AmplifyShaderEditor.CommentaryNode;97;-2290,2270;Inherit;False;1683.592;820.845;;17;88;87;82;81;80;83;79;78;77;84;69;86;74;73;72;75;99;Normals;0.82692,0,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;63;-2306,23;Inherit;False;1464.361;514.9651;;9;62;44;42;41;95;94;93;92;90;Water Depth Fade;0,0.2905326,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;81;-2240,2976;Inherit;False;Property;_NormalSpeed2;Normal Speed 2;4;0;Create;True;0;0;0;False;0;False;0;0.017;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;90;-2272,224;Inherit;False;Reconstruct World Position From Depth;-1;;1;e7094bcbcc80eb140b2a3dbe6a861de8;0;0;1;FLOAT4;0
Node;AmplifyShaderEditor.RangedFloatNode;74;-2112,2608;Inherit;False;Property;_NormalSpeed;Normal Speed;3;0;Create;True;0;0;0;False;0;False;0;0.024;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;80;-1968,2976;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;25;-2304,1696;Inherit;False;971.3657;411.5245;;4;4;3;5;26;Foam Depth Fade;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;30;-2304,1072;Inherit;False;1327.662;536.3482;Comment;8;31;11;10;29;7;46;47;58;Foam Noise;1,1,1,1;0;0
Node;AmplifyShaderEditor.ComponentMaskNode;92;-1936,224;Inherit;False;False;True;False;False;1;0;FLOAT4;0,0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WorldPosInputsNode;94;-1904,80;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.OneMinusNode;83;-1808,2976;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;75;-1840,2464;Inherit;False;Property;_Normal1Tiling;Normal 1 Tiling;7;0;Create;True;0;0;0;False;0;False;0,0;0.12,0.07;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.Vector2Node;79;-1840,2848;Inherit;False;Property;_Normal2Tiling;Normal 2 Tiling;8;0;Create;True;0;0;0;False;0;False;0,0;0.05,0.05;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.SimpleTimeNode;73;-1840,2592;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.GradientNode;5;-2240,1744;Inherit;False;0;2;2;1,1,1,0;0,0,0,0.60589;1,0;1,1;0;1;OBJECT;0
Node;AmplifyShaderEditor.RangedFloatNode;41;-2016,320;Inherit;False;Property;_DepthScale;Depth Scale;1;0;Create;True;0;0;0;False;0;False;0;0.071;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.DistanceOpNode;93;-1712,176;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DepthFade;3;-2256,1872;Inherit;False;True;False;True;2;1;FLOAT3;0,0,0;False;0;FLOAT;10.93;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;47;-2288,1408;Inherit;False;Property;_FoamSpeed;Foam Speed;2;0;Create;True;0;0;0;False;0;False;0;0.5764706;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.TexturePropertyNode;69;-1664,2320;Inherit;True;Property;_Texture0;Texture 0;5;0;Create;True;0;0;0;False;0;False;None;2c362c87a25a10e45958cc9990d38d17;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.TexturePropertyNode;77;-1648,2704;Inherit;True;Property;_Normal2;Normal 2;6;0;Create;True;0;0;0;False;0;False;None;65af04b714704594b8329f3d4e8adcaf;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.TextureCoordinatesNode;72;-1648,2528;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;0.1,0.1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;78;-1648,2912;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;0.1,0.1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GradientNode;42;-1568,128;Inherit;False;0;2;2;1,1,1,0;0,0,0,0.60589;1,0;1,1;0;1;OBJECT;0
Node;AmplifyShaderEditor.SamplerNode;86;-1408,2528;Inherit;True;Property;_TextureSample0;Texture Sample 0;10;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;95;-1552,224;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;46;-2160,1232;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.GradientSampleNode;4;-1936,1824;Inherit;True;2;0;OBJECT;;False;1;FLOAT;0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;84;-1408,2768;Inherit;True;Property;_TextureSample3;Texture Sample 3;11;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.CommentaryNode;111;-2288,3248;Inherit;False;1234;454;;10;96;102;103;108;104;101;105;107;106;109;Specular;0,0.9638114,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;88;-1280,2960;Inherit;False;Constant;_NormalScale;Normal Scale;9;0;Create;True;0;0;0;False;0;False;0.1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;82;-1072,2640;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;58;-2000,1312;Inherit;False;Constant;_FoamScale;Foam Scale;3;0;Create;True;0;0;0;False;0;False;-1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;7;-1984,1152;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GradientSampleNode;44;-1360,208;Inherit;True;2;0;OBJECT;;False;1;FLOAT;0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RegisterLocalVarNode;26;-1600,1792;Inherit;False;FoamDepthFade;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;62;-1056,208;Inherit;False;WaterDepthFade;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;10;-1760,1136;Inherit;True;Simplex2D;True;False;2;0;FLOAT2;0,0;False;1;FLOAT;-1.44;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;87;-944,2640;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ViewDirInputsCoordNode;96;-2240,3328;Inherit;False;World;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.GetLocalVarNode;29;-1664,1408;Inherit;False;26;FoamDepthFade;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.WorldSpaceLightDirHlpNode;102;-2240,3504;Inherit;False;False;1;0;FLOAT;0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.CommentaryNode;65;-2304,560;Inherit;False;1364.55;403.7665;;7;35;24;36;55;37;64;67;Water Color;0,0.2941177,1,1;0;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;99;-816,2640;Inherit;False;Normals;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StepOpNode;11;-1424,1152;Inherit;True;2;0;FLOAT;0;False;1;COLOR;0.37,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;64;-2272,704;Inherit;False;62;WaterDepthFade;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;103;-1984,3408;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.OneMinusNode;55;-2064,704;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GradientNode;37;-2064,608;Inherit;False;0;2;2;0.627451,1,0.7813289,0;0,0.4762425,0.7264151,1;1,0;1,1;0;1;OBJECT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;31;-1200,1152;Inherit;False;FoamNoise;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.NormalizeNode;104;-1856,3408;Inherit;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.GetLocalVarNode;101;-1872,3600;Inherit;False;99;Normals;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;108;-1840,3312;Inherit;False;Property;_SpecularIntensity;Specular Intensity;9;0;Create;True;0;0;0;False;0;False;0;0.863;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;59;-1296,1696;Inherit;False;691.002;263.2081;;4;61;12;27;32;Foam;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;35;-1728,816;Inherit;False;Property;_PosterizeScale;Posterize Scale;0;0;Create;True;0;0;0;False;0;False;0;16.46;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;107;-1648,3360;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DotProductOpNode;105;-1648,3520;Inherit;False;2;0;FLOAT3;0,0,0;False;1;COLOR;0,0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GradientSampleNode;36;-1840,608;Inherit;True;2;0;OBJECT;;False;1;FLOAT;0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;32;-1232,1840;Inherit;False;31;FoamNoise;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;27;-1232,1744;Inherit;False;26;FoamDepthFade;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;12;-992,1792;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.PosterizeNode;24;-1472,608;Inherit;True;81;2;1;COLOR;0,0,0,0;False;0;INT;81;False;1;COLOR;0
Node;AmplifyShaderEditor.FunctionNode;106;-1472,3456;Inherit;False;Step Antialiasing;-1;;4;2a825e80dfb3290468194f83380797bd;0;2;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;109;-1280,3456;Inherit;False;Specular;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;67;-1200,608;Inherit;False;WaterColor;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;61;-822.9359,1785.022;Inherit;False;Foam;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;66;-352,1136;Inherit;False;67;WaterColor;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;110;-352,1232;Inherit;False;109;Specular;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;60;-352,1056;Inherit;False;61;Foam;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;21;0,1136;Inherit;False;3;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;112;-61.38342,986.213;Inherit;False;99;Normals;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.CustomStandardSurface;56;159.6002,1025.652;Inherit;False;Metallic;Tangent;6;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,1;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;1;False;1;FLOAT3;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;2;496,784;Float;False;True;-1;2;ASEMaterialInspector;0;0;CustomLighting;Water;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Translucent;0.5;True;False;0;False;Opaque;;Transparent;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;False;0;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;False;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT3;0,0,0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;80;0;81;0
WireConnection;92;0;90;0
WireConnection;83;0;80;0
WireConnection;73;0;74;0
WireConnection;93;0;94;2
WireConnection;93;1;92;0
WireConnection;72;0;75;0
WireConnection;72;1;73;0
WireConnection;78;0;79;0
WireConnection;78;1;83;0
WireConnection;86;0;69;0
WireConnection;86;1;72;0
WireConnection;95;0;93;0
WireConnection;95;1;41;0
WireConnection;46;0;47;0
WireConnection;4;0;5;0
WireConnection;4;1;3;0
WireConnection;84;0;77;0
WireConnection;84;1;78;0
WireConnection;82;0;86;0
WireConnection;82;1;84;0
WireConnection;7;1;46;0
WireConnection;44;0;42;0
WireConnection;44;1;95;0
WireConnection;26;0;4;0
WireConnection;62;0;44;0
WireConnection;10;0;7;0
WireConnection;10;1;58;0
WireConnection;87;0;82;0
WireConnection;87;1;88;0
WireConnection;99;0;87;0
WireConnection;11;0;10;0
WireConnection;11;1;29;0
WireConnection;103;0;96;0
WireConnection;103;1;102;0
WireConnection;55;0;64;0
WireConnection;31;0;11;0
WireConnection;104;0;103;0
WireConnection;107;0;108;0
WireConnection;105;0;104;0
WireConnection;105;1;101;0
WireConnection;36;0;37;0
WireConnection;36;1;55;0
WireConnection;12;0;27;0
WireConnection;12;1;32;0
WireConnection;24;1;36;0
WireConnection;24;0;35;0
WireConnection;106;1;107;0
WireConnection;106;2;105;0
WireConnection;109;0;106;0
WireConnection;67;0;24;0
WireConnection;61;0;12;0
WireConnection;21;0;60;0
WireConnection;21;1;66;0
WireConnection;21;2;110;0
WireConnection;56;1;112;0
WireConnection;56;2;21;0
WireConnection;2;13;56;0
ASEEND*/
//CHKSM=97359E91AF6A45ED1E90038043F9025E68AA207F