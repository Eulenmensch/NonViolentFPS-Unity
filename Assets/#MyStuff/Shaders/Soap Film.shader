// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Soap Film"
{
	Properties
	{
		_Color("Color", Color) = (0.5333304,0.8018868,0.6416331,0)
		_PosterizePower("Posterize Power", Float) = 50
		_BigPatternEmissive("Big Pattern Emissive", Float) = 2.17
		_BorderPatternEmissive("Border Pattern Emissive", Float) = 2.17
		_CenterOffset("Center Offset", Vector) = (0,0,0,0)
		_BorderSmoothstepMinMax("Border Smoothstep MinMax", Vector) = (0.34,0.84,0,0)
		_StipplesSmoothstepMinMax("Stipples Smoothstep MinMax", Vector) = (0.3,0.58,0,0)
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Back
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 3.0
		#pragma surface surf Standard alpha:fade keepalpha noshadow 
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform float4 _Color;
		uniform float _PosterizePower;
		uniform float2 _CenterOffset;
		uniform float2 _BorderSmoothstepMinMax;
		uniform float _BigPatternEmissive;
		uniform float2 _StipplesSmoothstepMinMax;
		uniform float _BorderPatternEmissive;


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


		float3 mod3D289( float3 x ) { return x - floor( x / 289.0 ) * 289.0; }

		float4 mod3D289( float4 x ) { return x - floor( x / 289.0 ) * 289.0; }

		float4 permute( float4 x ) { return mod3D289( ( x * 34.0 + 1.0 ) * x ); }

		float4 taylorInvSqrt( float4 r ) { return 1.79284291400159 - r * 0.85373472095314; }

		float snoise( float3 v )
		{
			const float2 C = float2( 1.0 / 6.0, 1.0 / 3.0 );
			float3 i = floor( v + dot( v, C.yyy ) );
			float3 x0 = v - i + dot( i, C.xxx );
			float3 g = step( x0.yzx, x0.xyz );
			float3 l = 1.0 - g;
			float3 i1 = min( g.xyz, l.zxy );
			float3 i2 = max( g.xyz, l.zxy );
			float3 x1 = x0 - i1 + C.xxx;
			float3 x2 = x0 - i2 + C.yyy;
			float3 x3 = x0 - 0.5;
			i = mod3D289( i);
			float4 p = permute( permute( permute( i.z + float4( 0.0, i1.z, i2.z, 1.0 ) ) + i.y + float4( 0.0, i1.y, i2.y, 1.0 ) ) + i.x + float4( 0.0, i1.x, i2.x, 1.0 ) );
			float4 j = p - 49.0 * floor( p / 49.0 );  // mod(p,7*7)
			float4 x_ = floor( j / 7.0 );
			float4 y_ = floor( j - 7.0 * x_ );  // mod(j,N)
			float4 x = ( x_ * 2.0 + 0.5 ) / 7.0 - 1.0;
			float4 y = ( y_ * 2.0 + 0.5 ) / 7.0 - 1.0;
			float4 h = 1.0 - abs( x ) - abs( y );
			float4 b0 = float4( x.xy, y.xy );
			float4 b1 = float4( x.zw, y.zw );
			float4 s0 = floor( b0 ) * 2.0 + 1.0;
			float4 s1 = floor( b1 ) * 2.0 + 1.0;
			float4 sh = -step( h, 0.0 );
			float4 a0 = b0.xzyw + s0.xzyw * sh.xxyy;
			float4 a1 = b1.xzyw + s1.xzyw * sh.zzww;
			float3 g0 = float3( a0.xy, h.x );
			float3 g1 = float3( a0.zw, h.y );
			float3 g2 = float3( a1.xy, h.z );
			float3 g3 = float3( a1.zw, h.w );
			float4 norm = taylorInvSqrt( float4( dot( g0, g0 ), dot( g1, g1 ), dot( g2, g2 ), dot( g3, g3 ) ) );
			g0 *= norm.x;
			g1 *= norm.y;
			g2 *= norm.z;
			g3 *= norm.w;
			float4 m = max( 0.6 - float4( dot( x0, x0 ), dot( x1, x1 ), dot( x2, x2 ), dot( x3, x3 ) ), 0.0 );
			m = m* m;
			m = m* m;
			float4 px = float4( dot( x0, g0 ), dot( x1, g1 ), dot( x2, g2 ), dot( x3, g3 ) );
			return 42.0 * dot( m, px);
		}


		float2 voronoihash47( float2 p )
		{
			p = p - 15 * floor( p / 15 );
			p = float2( dot( p, float2( 127.1, 311.7 ) ), dot( p, float2( 269.5, 183.3 ) ) );
			return frac( sin( p ) *43758.5453);
		}


		float voronoi47( float2 v, float time, inout float2 id, inout float2 mr, float smoothness )
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
			 		float2 o = voronoihash47( n + g );
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


		void surf( Input i , inout SurfaceOutputStandard o )
		{
			Gradient gradient22 = NewGradient( 0, 2, 2, float4( 0.4433962, 0.4433962, 0.4433962, 0.1494163 ), float4( 1, 1, 1, 1 ), 0, 0, 0, 0, 0, 0, float2( 1, 0 ), float2( 1, 1 ), 0, 0, 0, 0, 0, 0 );
			float2 uv_TexCoord21 = i.uv_texcoord + _CenterOffset;
			float2 CenteredUV15_g1 = ( uv_TexCoord21 - float2( 0.5,0.5 ) );
			float2 break17_g1 = CenteredUV15_g1;
			float2 appendResult23_g1 = (float2(( length( CenteredUV15_g1 ) * 0.73 * 2.0 ) , ( atan2( break17_g1.x , break17_g1.y ) * ( 1.0 / 6.28318548202515 ) * 1.0 )));
			float2 temp_output_20_0 = appendResult23_g1;
			float4 ramp83 = SampleGradient( gradient22, temp_output_20_0.x );
			float div39=256.0/float((int)_PosterizePower);
			float4 posterize39 = ( floor( ramp83 * div39 ) / div39 );
			o.Albedo = ( _Color * posterize39 ).rgb;
			float4 temp_cast_3 = (_BorderSmoothstepMinMax.x).xxxx;
			float4 temp_cast_4 = (_BorderSmoothstepMinMax.y).xxxx;
			float4 smoothstepResult126 = smoothstep( temp_cast_3 , temp_cast_4 , ramp83);
			float2 _StreakSmoothstepMinMax = float2(0.18,0.66);
			float4 temp_cast_5 = (_StreakSmoothstepMinMax.x).xxxx;
			float4 temp_cast_6 = (_StreakSmoothstepMinMax.y).xxxx;
			float4 smoothstepResult119 = smoothstep( temp_cast_5 , temp_cast_6 , ramp83);
			float mulTime80 = _Time.y * 8E-06;
			float PanSpeed110 = mulTime80;
			float2 temp_cast_7 = (PanSpeed110).xx;
			float2 uv_TexCoord104 = i.uv_texcoord + temp_cast_7;
			float2 panner99 = ( 1.0 * _Time.y * float2( 0.18,0 ) + uv_TexCoord104);
			float simplePerlin3D100 = snoise( float3( panner99 ,  0.0 ) );
			simplePerlin3D100 = simplePerlin3D100*0.5 + 0.5;
			float2 temp_cast_9 = (( 2.38 * simplePerlin3D100 )).xx;
			float2 break16_g3 = ( i.uv_texcoord * temp_cast_9 );
			float2 appendResult7_g3 = (float2(( break16_g3.x + ( 0.0 * step( 1.0 , ( break16_g3.y % 2.0 ) ) ) ) , ( break16_g3.y + ( 0.0 * step( 1.0 , ( break16_g3.x % 2.0 ) ) ) )));
			float temp_output_2_0_g3 = 0.87;
			float2 appendResult11_g4 = (float2(temp_output_2_0_g3 , temp_output_2_0_g3));
			float temp_output_17_0_g4 = length( ( (frac( appendResult7_g3 )*2.0 + -1.0) / appendResult11_g4 ) );
			float4 temp_cast_10 = (_StipplesSmoothstepMinMax.x).xxxx;
			float4 temp_cast_11 = (_StipplesSmoothstepMinMax.y).xxxx;
			float4 smoothstepResult130 = smoothstep( temp_cast_10 , temp_cast_11 , ramp83);
			float2 temp_cast_12 = (mulTime80).xx;
			float2 uv_TexCoord79 = i.uv_texcoord + temp_cast_12;
			float mulTime67 = _Time.y * -0.5;
			float cos66 = cos( mulTime67 );
			float sin66 = sin( mulTime67 );
			float2 rotator66 = mul( uv_TexCoord79 - float2( 0.5,0.5 ) , float2x2( cos66 , -sin66 , sin66 , cos66 )) + float2( 0.5,0.5 );
			float simplePerlin3D63 = snoise( float3( rotator66 ,  0.0 )*1.02 );
			float time47 = 4.7;
			float mulTime54 = _Time.y * 0.25;
			float cos53 = cos( mulTime54 );
			float sin53 = sin( mulTime54 );
			float2 rotator53 = mul( i.uv_texcoord - float2( 0.5,0.5 ) , float2x2( cos53 , -sin53 , sin53 , cos53 )) + float2( 0.5,0.5 );
			float2 CenteredUV15_g2 = ( rotator53 - float2( 0.5,0.5 ) );
			float2 break17_g2 = CenteredUV15_g2;
			float2 appendResult23_g2 = (float2(( length( CenteredUV15_g2 ) * 1.0 * 2.0 ) , ( atan2( break17_g2.x , break17_g2.y ) * ( 1.0 / 6.28318548202515 ) * 1.0 )));
			float2 coords47 = appendResult23_g2 * 15.0;
			float2 id47 = 0;
			float2 uv47 = 0;
			float voroi47 = voronoi47( coords47, time47, id47, uv47, 0 );
			float4 blendOpSrc144 = ( smoothstepResult126 + ( smoothstepResult119 * ( 1.0 - saturate( ( ( 1.0 - temp_output_17_0_g4 ) / fwidth( temp_output_17_0_g4 ) ) ) ) * _BigPatternEmissive ) + ( smoothstepResult130 * step( simplePerlin3D63 , voroi47 ) * _BorderPatternEmissive ) );
			float4 blendOpDest144 = float4( 0,0,0,0 );
			o.Emission = ( saturate( ( 1.0 - ( 1.0 - blendOpSrc144 ) * ( 1.0 - blendOpDest144 ) ) )).rgb;
			float4 clampResult40 = clamp( ramp83 , float4( 0.27,0.27,0.27,0 ) , float4( 1,1,1,0 ) );
			o.Alpha = clampResult40.r;
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18712
0;6;1920;1023;2035.457;267.2535;2.842288;True;True
Node;AmplifyShaderEditor.CommentaryNode;92;-852.8337,415.9247;Inherit;False;1002;309;masks out voronoi dots;6;63;66;79;80;67;110;Panning, Rotating Simplex Mask;0.8396226,0.5109024,0.7888296,1;0;0
Node;AmplifyShaderEditor.SimpleTimeNode;80;-802.8337,513.9247;Inherit;False;1;0;FLOAT;8E-06;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;115;-1748.802,-167.1573;Inherit;False;1825.667;384.8;Looks Like Soap Streaking;8;111;104;99;98;100;97;96;113;Panning Simplex Dots;0.8679245,0.5199359,0.5199359,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;81;-1603,-768;Inherit;False;1330.437;462.8159;;7;94;83;23;20;22;21;149;Ramp;0.6037736,0.6037736,0.6037736,1;0;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;110;-818.8979,649.2776;Inherit;False;PanSpeed;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;111;-1698.802,7.974072;Inherit;False;110;PanSpeed;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;94;-1557.598,-533.3264;Inherit;False;Property;_CenterOffset;Center Offset;4;0;Create;True;0;0;0;False;0;False;0,0;0.04,0.022;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.CommentaryNode;93;-964.2491,738.6844;Inherit;False;1114;352;;5;47;55;53;56;54;Polar Voronoi Dots;0.3484336,0.8490566,0.8309398,1;0;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;21;-1360,-576;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;104;-1478.292,-34.99056;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PannerNode;99;-1169.417,-48.35745;Inherit;True;3;0;FLOAT2;0,0;False;2;FLOAT2;0.18,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.GradientNode;22;-1008,-720;Inherit;False;0;2;2;0.4433962,0.4433962,0.4433962,0.1494163;1,1,1,1;1,0;1,1;0;1;OBJECT;0
Node;AmplifyShaderEditor.SimpleTimeNode;54;-866.2494,916.6846;Inherit;False;1;0;FLOAT;0.25;False;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;20;-1120,-576;Inherit;True;Polar Coordinates;-1;;1;7dab8e02884cf104ebefaa2e788e4162;0;4;1;FLOAT2;0,0;False;2;FLOAT2;0.5,0.5;False;3;FLOAT;0.73;False;4;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;56;-914.2491,788.6846;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RotatorNode;53;-658.2492,788.6846;Inherit;True;3;0;FLOAT2;0,0;False;1;FLOAT2;0.5,0.5;False;2;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleTimeNode;67;-578.8337,593.9245;Inherit;False;1;0;FLOAT;-0.5;False;1;FLOAT;0
Node;AmplifyShaderEditor.GradientSampleNode;23;-784,-592;Inherit;True;2;0;OBJECT;;False;1;FLOAT;0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;79;-626.8337,465.9247;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.NoiseGeneratorNode;100;-892.7169,-41.35751;Inherit;True;Simplex3D;True;False;2;0;FLOAT3;0,0,0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;98;-813.8168,-117.1575;Inherit;False;Constant;_Float4;Float 4;4;0;Create;True;0;0;0;False;0;False;2.38;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;83;-480,-592;Inherit;False;ramp;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.CommentaryNode;124;27.36656,-743.6486;Inherit;False;553.9482;501.2422;So that the center of the ADS view stays clear of noise;3;121;119;101;Streak Mask;0.8113208,0.4452962,0.1569064,1;0;0
Node;AmplifyShaderEditor.RotatorNode;66;-370.8343,465.9247;Inherit;True;3;0;FLOAT2;0,0;False;1;FLOAT2;0.5,0.5;False;2;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.CommentaryNode;131;175.2883,5.673927;Inherit;False;633.1001;329.4;;3;128;129;130;Stipples Mask;0.1946422,0.509434,0.4832184,1;0;0
Node;AmplifyShaderEditor.FunctionNode;55;-370.2492,788.6846;Inherit;True;Polar Coordinates;-1;;2;7dab8e02884cf104ebefaa2e788e4162;0;4;1;FLOAT2;0,0;False;2;FLOAT2;0.5,0.5;False;3;FLOAT;1;False;4;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;97;-599.1699,-76.93307;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;121;45.71592,-501.6593;Inherit;False;Constant;_StreakSmoothstepMinMax;Streak Smoothstep MinMax;5;0;Create;True;0;0;0;False;0;False;0.18,0.66;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.GetLocalVarNode;101;80.06493,-691.91;Inherit;True;83;ramp;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;128;322.8882,55.67392;Inherit;False;83;ramp;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.Vector2Node;129;225.2883,140.4742;Inherit;False;Property;_StipplesSmoothstepMinMax;Stipples Smoothstep MinMax;6;0;Create;True;0;0;0;False;0;False;0.3,0.58;0.39,0.58;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.FunctionNode;96;-408.2503,-85.13454;Inherit;True;Dots Pattern;-1;;3;7d8d5e315fd9002418fb41741d3a59cb;1,22,0;5;21;FLOAT2;0,0;False;3;FLOAT2;2,2;False;2;FLOAT;0.87;False;4;FLOAT;0;False;5;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;63;-114.8341,465.9247;Inherit;True;Simplex3D;False;False;2;0;FLOAT3;0,0,0;False;1;FLOAT;1.02;False;1;FLOAT;0
Node;AmplifyShaderEditor.VoronoiNode;47;-50.24914,787.3846;Inherit;True;0;0;1;0;1;True;15;False;False;4;0;FLOAT2;0,0;False;1;FLOAT;4.7;False;2;FLOAT;15;False;3;FLOAT;0;False;3;FLOAT;0;FLOAT2;1;FLOAT2;2
Node;AmplifyShaderEditor.CommentaryNode;132;623.7941,-844.92;Inherit;False;625.4156;397.7212;;3;127;125;126;White Border;0,0,0,1;0;0
Node;AmplifyShaderEditor.GetLocalVarNode;125;753.0236,-794.92;Inherit;False;83;ramp;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;114;603.6627,615.6178;Inherit;False;Property;_BorderPatternEmissive;Border Pattern Emissive;3;0;Create;True;0;0;0;False;0;False;2.17;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SmoothstepOpNode;119;327.3148,-496.4064;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0.3490566,0.3490566,0.3490566,0;False;2;COLOR;0.8584906,0.8584906,0.8584906,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SmoothstepOpNode;130;554.3885,81.07387;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;1,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.Vector2Node;127;673.7941,-704.355;Inherit;False;Property;_BorderSmoothstepMinMax;Border Smoothstep MinMax;5;0;Create;True;0;0;0;False;0;False;0.34,0.84;0.58,0.61;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.RangedFloatNode;78;757.795,-132.5777;Inherit;False;Property;_BigPatternEmissive;Big Pattern Emissive;2;0;Create;True;0;0;0;False;0;False;2.17;1.34;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.StepOpNode;61;589.0945,373.8429;Inherit;True;2;0;FLOAT;0.11;False;1;FLOAT;0.06;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;113;-121.1353,-87.64188;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;82;498.8673,-1376.594;Inherit;False;716.5353;473.2688;;5;46;45;39;84;85;Color;0.3771412,0.7169812,0.3686365,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;112;875.085,865.312;Inherit;False;524.7379;304;;2;91;40;Opacity;1,1,1,1;0;0
Node;AmplifyShaderEditor.GetLocalVarNode;84;516.7936,-1152.594;Inherit;False;83;ramp;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;102;1027.75,174.6096;Inherit;True;3;3;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;85;516.7936,-1056.594;Inherit;False;Property;_PosterizePower;Posterize Power;1;0;Create;True;0;0;0;False;0;False;50;33.67;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SmoothstepOpNode;126;995.2097,-701.1989;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;1,1,1,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;75;1086.615,-265.6236;Inherit;True;3;3;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;91;925.0847,1021.052;Inherit;False;83;ramp;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;103;1399.086,-264.5988;Inherit;True;3;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;46;740.7936,-1328.594;Inherit;False;Property;_Color;Color;0;0;Create;True;0;0;0;False;0;False;0.5333304,0.8018868,0.6416331,0;0.5333304,0.8018868,0.6416331,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PosterizeNode;39;708.7936,-1152.594;Inherit;True;50;2;1;COLOR;0,0,0,0;False;0;INT;50;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;45;980.7944,-1168.594;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;149;-788.6439,-401.3972;Inherit;False;polarcoords;-1;True;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ClampOpNode;40;1145.822,915.3121;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0.27,0.27,0.27,0;False;2;COLOR;1,1,1,0;False;1;COLOR;0
Node;AmplifyShaderEditor.BlendOpsNode;144;1649.04,-200.132;Inherit;True;Screen;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;1;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;1940.6,-304.3999;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;Soap Film;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;0.5;True;False;0;False;Transparent;;Transparent;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;False;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;False;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;110;0;80;0
WireConnection;21;1;94;0
WireConnection;104;1;111;0
WireConnection;99;0;104;0
WireConnection;20;1;21;0
WireConnection;53;0;56;0
WireConnection;53;2;54;0
WireConnection;23;0;22;0
WireConnection;23;1;20;0
WireConnection;79;1;80;0
WireConnection;100;0;99;0
WireConnection;83;0;23;0
WireConnection;66;0;79;0
WireConnection;66;2;67;0
WireConnection;55;1;53;0
WireConnection;97;0;98;0
WireConnection;97;1;100;0
WireConnection;96;3;97;0
WireConnection;63;0;66;0
WireConnection;47;0;55;0
WireConnection;119;0;101;0
WireConnection;119;1;121;1
WireConnection;119;2;121;2
WireConnection;130;0;128;0
WireConnection;130;1;129;1
WireConnection;130;2;129;2
WireConnection;61;0;63;0
WireConnection;61;1;47;0
WireConnection;113;0;96;0
WireConnection;102;0;130;0
WireConnection;102;1;61;0
WireConnection;102;2;114;0
WireConnection;126;0;125;0
WireConnection;126;1;127;1
WireConnection;126;2;127;2
WireConnection;75;0;119;0
WireConnection;75;1;113;0
WireConnection;75;2;78;0
WireConnection;103;0;126;0
WireConnection;103;1;75;0
WireConnection;103;2;102;0
WireConnection;39;1;84;0
WireConnection;39;0;85;0
WireConnection;45;0;46;0
WireConnection;45;1;39;0
WireConnection;149;0;20;0
WireConnection;40;0;91;0
WireConnection;144;0;103;0
WireConnection;0;0;45;0
WireConnection;0;2;144;0
WireConnection;0;9;40;0
ASEEND*/
//CHKSM=4A7A9A06544B0F0D49F57CB81316E303C978C147