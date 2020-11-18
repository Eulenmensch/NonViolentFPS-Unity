// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Flat"
{
	Properties
	{
		_UnlitColor("Unlit Color", Color) = (0,0,0,0)
		_LitColor("Lit Color", Color) = (0,0,0,0)
		_ShadowColor("Shadow Color", Color) = (0.121957,0.2169811,0.05629227,0)
		_MainTexture("MainTexture", 2D) = "white" {}
		_Metallic("Metallic", Range( 0 , 1)) = 0
		_Smoothness("Smoothness", Range( 0 , 1)) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" }
		Cull Back
		Blend SrcAlpha OneMinusSrcAlpha
		BlendOp Add
		CGINCLUDE
		#include "UnityPBSLighting.cginc"
		#include "UnityShaderVariables.cginc"
		#include "UnityCG.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		struct Input
		{
			float3 worldNormal;
			float2 uv_texcoord;
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

		uniform sampler2D _MainTexture;
		uniform float4 _ShadowColor;
		uniform float4 _UnlitColor;
		uniform float4 _LitColor;
		uniform float _Metallic;
		uniform float _Smoothness;


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
			SurfaceOutputStandard s15 = (SurfaceOutputStandard ) 0;
			s15.Albedo = float3( 0,0,0 );
			float3 ase_worldNormal = i.worldNormal;
			s15.Normal = ase_worldNormal;
			float4 Texture84 = tex2D( _MainTexture, i.uv_texcoord );
			float4 color22 = IsGammaSpace() ? float4(0,0,0,0) : float4(0,0,0,0);
			Gradient gradient13 = NewGradient( 1, 2, 2, float4( 0, 0, 0, 0.602945 ), float4( 1, 1, 1, 1 ), 0, 0, 0, 0, 0, 0, float2( 1, 0 ), float2( 1, 1 ), 0, 0, 0, 0, 0, 0 );
			#if defined(LIGHTMAP_ON) && ( UNITY_VERSION < 560 || ( defined(LIGHTMAP_SHADOW_MIXING) && !defined(SHADOWS_SHADOWMASK) && defined(SHADOWS_SCREEN) ) )//aselc
			float4 ase_lightColor = 0;
			#else //aselc
			float4 ase_lightColor = _LightColor0;
			#endif //aselc
			float4 LightAttenuation19 = SampleGradient( gradient13, ( ase_lightAtten * ( ase_lightColor.rgb * ase_lightColor.a ) ).x );
			float4 lerpResult16 = lerp( _ShadowColor , color22 , LightAttenuation19);
			float4 Shadow23 = lerpResult16;
			Gradient gradient5 = NewGradient( 1, 2, 2, float4( 0, 0, 0, 0.5000076 ), float4( 1, 1, 1, 1 ), 0, 0, 0, 0, 0, 0, float2( 1, 0 ), float2( 1, 1 ), 0, 0, 0, 0, 0, 0 );
			float4 ase_vertex4Pos = mul( unity_WorldToObject, float4( i.worldPos , 1 ) );
			float3 ase_objectlightDir = normalize( ObjSpaceLightDir( ase_vertex4Pos ) );
			float3 normalizeResult30 = normalize( ase_objectlightDir );
			float3 ase_vertexNormal = mul( unity_WorldToObject, float4( ase_worldNormal, 0 ) );
			float3 normalizeResult31 = normalize( ase_vertexNormal );
			float dotResult9 = dot( normalizeResult30 , normalizeResult31 );
			float4 lerpResult6 = lerp( _UnlitColor , _LitColor , SampleGradient( gradient5, dotResult9 ));
			float4 Albedo18 = lerpResult6;
			float4 lerpResult28 = lerp( Shadow23 , Albedo18 , LightAttenuation19);
			s15.Emission = ( Texture84 * lerpResult28 ).rgb;
			s15.Metallic = _Metallic;
			s15.Smoothness = _Smoothness;
			s15.Occlusion = 1.0;

			data.light = gi.light;

			UnityGI gi15 = gi;
			#ifdef UNITY_PASS_FORWARDBASE
			Unity_GlossyEnvironmentData g15 = UnityGlossyEnvironmentSetup( s15.Smoothness, data.worldViewDir, s15.Normal, float3(0,0,0));
			gi15 = UnityGlobalIllumination( data, s15.Occlusion, s15.Normal, g15 );
			#endif

			float3 surfResult15 = LightingStandard ( s15, viewDir, gi15 ).rgb;
			surfResult15 += s15.Emission;

			#ifdef UNITY_PASS_FORWARDADD//15
			surfResult15 -= s15.Emission;
			#endif//15
			c.rgb = surfResult15;
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
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf StandardCustomLighting keepalpha fullforwardshadows 

		ENDCG
		Pass
		{
			Name "ShadowCaster"
			Tags{ "LightMode" = "ShadowCaster" }
			ZWrite On
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0
			#pragma multi_compile_shadowcaster
			#pragma multi_compile UNITY_PASS_SHADOWCASTER
			#pragma skip_variants FOG_LINEAR FOG_EXP FOG_EXP2
			#include "HLSLSupport.cginc"
			#if ( SHADER_API_D3D11 || SHADER_API_GLCORE || SHADER_API_GLES || SHADER_API_GLES3 || SHADER_API_METAL || SHADER_API_VULKAN )
				#define CAN_SKIP_VPOS
			#endif
			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "UnityPBSLighting.cginc"
			struct v2f
			{
				V2F_SHADOW_CASTER;
				float2 customPack1 : TEXCOORD1;
				float3 worldPos : TEXCOORD2;
				float3 worldNormal : TEXCOORD3;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};
			v2f vert( appdata_full v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID( v );
				UNITY_INITIALIZE_OUTPUT( v2f, o );
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO( o );
				UNITY_TRANSFER_INSTANCE_ID( v, o );
				Input customInputData;
				float3 worldPos = mul( unity_ObjectToWorld, v.vertex ).xyz;
				half3 worldNormal = UnityObjectToWorldNormal( v.normal );
				o.worldNormal = worldNormal;
				o.customPack1.xy = customInputData.uv_texcoord;
				o.customPack1.xy = v.texcoord;
				o.worldPos = worldPos;
				TRANSFER_SHADOW_CASTER_NORMALOFFSET( o )
				return o;
			}
			half4 frag( v2f IN
			#if !defined( CAN_SKIP_VPOS )
			, UNITY_VPOS_TYPE vpos : VPOS
			#endif
			) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				Input surfIN;
				UNITY_INITIALIZE_OUTPUT( Input, surfIN );
				surfIN.uv_texcoord = IN.customPack1.xy;
				float3 worldPos = IN.worldPos;
				half3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				surfIN.worldPos = worldPos;
				surfIN.worldNormal = IN.worldNormal;
				SurfaceOutputCustomLightingCustom o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutputCustomLightingCustom, o )
				surf( surfIN, o );
				#if defined( CAN_SKIP_VPOS )
				float2 vpos = IN.pos;
				#endif
				SHADOW_CASTER_FRAGMENT( IN )
			}
			ENDCG
		}
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18600
2572.286;155;1447;934;1028.29;-411.2029;1;True;False
Node;AmplifyShaderEditor.CommentaryNode;87;-688,480;Inherit;False;1059.71;357.744;;6;13;96;97;3;12;19;Light Attenuation;1,0,0.9104991,1;0;0
Node;AmplifyShaderEditor.LightColorNode;96;-640,704;Inherit;False;0;3;COLOR;0;FLOAT3;1;FLOAT;2
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;98;-472.2898,716.2029;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.CommentaryNode;88;-688,-448;Inherit;False;1714;824;;11;10;80;31;30;9;5;7;1;4;6;18;Toon Colors;0,1,0.7750711,1;0;0
Node;AmplifyShaderEditor.LightAttenuation;3;-640,624;Inherit;False;0;1;FLOAT;0
Node;AmplifyShaderEditor.GradientNode;13;-640,528;Inherit;False;1;2;2;0,0,0,0.602945;1,1,1,1;1,0;1,1;0;1;OBJECT;0
Node;AmplifyShaderEditor.NormalVertexDataNode;10;-592,192;Inherit;False;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ObjSpaceLightDirHlpNode;80;-640,32;Inherit;False;1;0;FLOAT;0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;97;-320,592;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.NormalizeNode;31;-400,192;Inherit;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.NormalizeNode;30;-400,32;Inherit;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.GradientSampleNode;12;-176,528;Inherit;True;2;0;OBJECT;;False;1;FLOAT;0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DotProductOpNode;9;-208,32;Inherit;True;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;19;144,528;Inherit;False;LightAttenuation;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GradientNode;5;-192,-48;Inherit;False;1;2;2;0,0,0,0.5000076;1,1,1,1;1,0;1,1;0;1;OBJECT;0
Node;AmplifyShaderEditor.CommentaryNode;90;-672,960;Inherit;False;706;550;;5;22;20;17;16;23;Shadows;0,0,0,1;0;0
Node;AmplifyShaderEditor.ColorNode;1;16,-400;Inherit;False;Property;_UnlitColor;Unlit Color;0;0;Create;True;0;0;False;0;False;0,0,0,0;0.2678229,0.3119999,0.2291681,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;17;-624,1008;Inherit;False;Property;_ShadowColor;Shadow Color;2;0;Create;True;0;0;False;0;False;0.121957,0.2169811,0.05629227,0;0.2452473,0.1492142,0.3396226,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;20;-624,1392;Inherit;False;19;LightAttenuation;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;7;16,-224;Inherit;False;Property;_LitColor;Lit Color;1;0;Create;True;0;0;False;0;False;0,0,0,0;0.5205875,0.5943396,0.362547,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.CommentaryNode;89;-688,-976;Inherit;False;895.9999;440;;4;82;81;83;84;Texture;1,1,1,1;0;0
Node;AmplifyShaderEditor.ColorNode;22;-624,1200;Inherit;False;Constant;_Black;Black;2;0;Create;True;0;0;False;0;False;0,0,0,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GradientSampleNode;4;16,-48;Inherit;True;2;0;OBJECT;;False;1;FLOAT;0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;16;-352,1184;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;81;-576,-704;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TexturePropertyNode;82;-640,-928;Inherit;True;Property;_MainTexture;MainTexture;6;0;Create;True;0;0;False;0;False;6344bf96fbda94141a525046d088fb23;None;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.LerpOp;6;384,-240;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;18;800,-240;Inherit;False;Albedo;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;83;-304,-784;Inherit;True;Property;_TextureSample0;Texture Sample 0;6;0;Create;True;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RegisterLocalVarNode;23;-192,1184;Inherit;False;Shadow;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;24;1536,384;Inherit;True;23;Shadow;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;84;-16,-800;Inherit;False;Texture;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;29;1536,800;Inherit;True;19;LightAttenuation;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;21;1536,592;Inherit;True;18;Albedo;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;86;1808,384;Inherit;False;84;Texture;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;28;1792,544;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;79;2080,704;Inherit;False;Property;_Smoothness;Smoothness;9;0;Create;True;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;78;2080,624;Inherit;False;Property;_Metallic;Metallic;8;0;Create;True;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;85;2080,384;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.CommentaryNode;91;-672,1664;Inherit;False;1778;1017;ATM show in different space than shadows;17;61;65;72;69;56;35;68;66;37;73;75;36;58;70;71;55;67;Stipples;1,1,1,1;0;0
Node;AmplifyShaderEditor.GetLocalVarNode;76;1536,1008;Inherit;True;61;Stipples;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.NormalVertexDataNode;72;-576,2496;Inherit;False;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;36;-528,2080;Inherit;False;Property;_StippleScale;Stipple Scale;3;0;Create;True;0;0;False;0;False;0;13.03;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;37;-128,1936;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;12.9;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;66;80,1936;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;56;368,1712;Inherit;False;Property;_StippleColor;Stipple Color;4;0;Create;True;0;0;False;0;False;0.6886792,0.1726339,0.1726339,0;0.1890352,0.6792453,0.6450446,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.FunctionNode;68;240,1936;Inherit;False;Step Antialiasing;-1;;1;2a825e80dfb3290468194f83380797bd;0;2;1;FLOAT;0;False;2;FLOAT;0.24;False;1;FLOAT;0
Node;AmplifyShaderEditor.VoronoiNode;35;-320,2080;Inherit;False;0;0;1;0;1;True;1;True;False;4;0;FLOAT2;0,0;False;1;FLOAT;16.46;False;2;FLOAT;2.6;False;3;FLOAT;0;False;3;FLOAT;0;FLOAT2;1;FLOAT2;2
Node;AmplifyShaderEditor.TextureCoordinatesNode;58;-576,1888;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;77;2048,1008;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;75;-368,1872;Inherit;True;Simplex2D;True;False;2;0;FLOAT2;0,0;False;1;FLOAT;-59.07;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;73;-144,2320;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DotProductOpNode;70;-320,2400;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CustomStandardSurface;15;2384,560;Inherit;False;Metallic;Tangent;6;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,1;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;1;False;1;FLOAT3;0
Node;AmplifyShaderEditor.LerpOp;55;624,1872;Inherit;True;3;0;COLOR;1,1,1,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;67;-352,2272;Inherit;False;Property;_StippleGap;Stipple Gap;7;0;Create;True;0;0;False;0;False;8.6;31.58;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;65;-528,2000;Inherit;False;Property;_StippleAngle;Stipple Angle;5;0;Create;True;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;69;448,1936;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;61;880,1872;Inherit;False;Stipples;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.WorldSpaceLightDirHlpNode;71;-624,2336;Inherit;False;False;1;0;FLOAT;0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;2608,320;Float;False;True;-1;2;ASEMaterialInspector;0;0;CustomLighting;Flat;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;2;5;False;-1;10;False;-1;0;1;False;-1;1;False;-1;1;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;False;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT3;0,0,0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;98;0;96;1
WireConnection;98;1;96;2
WireConnection;97;0;3;0
WireConnection;97;1;98;0
WireConnection;31;0;10;0
WireConnection;30;0;80;0
WireConnection;12;0;13;0
WireConnection;12;1;97;0
WireConnection;9;0;30;0
WireConnection;9;1;31;0
WireConnection;19;0;12;0
WireConnection;4;0;5;0
WireConnection;4;1;9;0
WireConnection;16;0;17;0
WireConnection;16;1;22;0
WireConnection;16;2;20;0
WireConnection;6;0;1;0
WireConnection;6;1;7;0
WireConnection;6;2;4;0
WireConnection;18;0;6;0
WireConnection;83;0;82;0
WireConnection;83;1;81;0
WireConnection;23;0;16;0
WireConnection;84;0;83;0
WireConnection;28;0;24;0
WireConnection;28;1;21;0
WireConnection;28;2;29;0
WireConnection;85;0;86;0
WireConnection;85;1;28;0
WireConnection;37;0;75;0
WireConnection;37;1;73;0
WireConnection;66;0;37;0
WireConnection;68;1;66;0
WireConnection;35;0;58;0
WireConnection;35;1;65;0
WireConnection;35;2;36;0
WireConnection;77;0;28;0
WireConnection;77;1;76;0
WireConnection;75;0;58;0
WireConnection;75;1;36;0
WireConnection;73;0;67;0
WireConnection;73;1;70;0
WireConnection;70;0;71;0
WireConnection;70;1;72;0
WireConnection;15;2;85;0
WireConnection;15;3;78;0
WireConnection;15;4;79;0
WireConnection;55;1;56;0
WireConnection;55;2;69;0
WireConnection;69;0;68;0
WireConnection;61;0;55;0
WireConnection;0;13;15;0
ASEEND*/
//CHKSM=EFFE32F22C38EF91B1C17F1ABF0A22DDE39FA2D4