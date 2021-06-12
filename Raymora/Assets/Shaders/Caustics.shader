// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Caustics"
{
	Properties
	{
		_CausticsSpeed("CausticsSpeed", Float) = 3
		_CausticsScale("CausticsScale", Float) = 5
		_CausticsColor("CausticsColor", Color) = (1,1,1,0)
		_TextureSample0("Texture Sample 0", 2D) = "white" {}
		_TextureSample1("Texture Sample 0", 2D) = "bump" {}
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" "IsEmissive" = "true"  }
		Cull Back
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 3.0
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows 
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform sampler2D _TextureSample1;
		uniform float4 _TextureSample1_ST;
		uniform sampler2D _TextureSample0;
		uniform float4 _TextureSample0_ST;
		uniform float _CausticsScale;
		uniform float _CausticsSpeed;
		uniform float4 _CausticsColor;


		float2 voronoihash1( float2 p )
		{
			
			p = float2( dot( p, float2( 127.1, 311.7 ) ), dot( p, float2( 269.5, 183.3 ) ) );
			return frac( sin( p ) *43758.5453);
		}


		float voronoi1( float2 v, float time, inout float2 id, inout float2 mr, float smoothness )
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
			 		float2 o = voronoihash1( n + g );
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
			return (F2 + F1) * 0.5;
		}


		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_TextureSample1 = i.uv_texcoord * _TextureSample1_ST.xy + _TextureSample1_ST.zw;
			o.Normal = UnpackNormal( tex2D( _TextureSample1, uv_TextureSample1 ) );
			float2 uv_TextureSample0 = i.uv_texcoord * _TextureSample0_ST.xy + _TextureSample0_ST.zw;
			o.Albedo = tex2D( _TextureSample0, uv_TextureSample0 ).rgb;
			float mulTime5 = _Time.y * _CausticsSpeed;
			float time1 = ( mulTime5 * 1.0 );
			float2 coords1 = i.uv_texcoord * _CausticsScale;
			float2 id1 = 0;
			float2 uv1 = 0;
			float fade1 = 0.5;
			float voroi1 = 0;
			float rest1 = 0;
			for( int it1 = 0; it1 <2; it1++ ){
			voroi1 += fade1 * voronoi1( coords1, time1, id1, uv1, 0 );
			rest1 += fade1;
			coords1 *= 2;
			fade1 *= 0.5;
			}//Voronoi1
			voroi1 /= rest1;
			float smoothstepResult8 = smoothstep( 0.16 , 0.76 , voroi1);
			o.Emission = ( saturate( smoothstepResult8 ) * _CausticsColor * 2.0 ).rgb;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18900
1920;0;1920;1007;1187.041;251.2785;1;True;False
Node;AmplifyShaderEditor.RangedFloatNode;6;-990,287.5;Inherit;False;Property;_CausticsSpeed;CausticsSpeed;0;0;Create;True;0;0;0;False;0;False;3;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;5;-717,257.5;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;3;-593,426.5;Inherit;False;Property;_CausticsScale;CausticsScale;1;0;Create;True;0;0;0;False;0;False;5;24.85;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;2;-616,78.5;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;7;-539,226.5;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.VoronoiNode;1;-330,62.5;Inherit;True;0;0;1;3;2;False;1;False;False;4;0;FLOAT2;0,0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0;False;3;FLOAT;0;FLOAT2;1;FLOAT2;2
Node;AmplifyShaderEditor.SmoothstepOpNode;8;-146,66.5;Inherit;True;3;0;FLOAT;0;False;1;FLOAT;0.16;False;2;FLOAT;0.76;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;11;70,243.5;Inherit;False;Property;_CausticsColor;CausticsColor;2;0;Create;True;0;0;0;False;0;False;1,1,1,0;1,1,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;12;110,470.5;Inherit;False;Constant;_Float0;Float 0;3;0;Create;True;0;0;0;False;0;False;2;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;9;142,17.5;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;10;402,134.5;Inherit;True;3;3;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;13;356,-243.5;Inherit;True;Property;_TextureSample0;Texture Sample 0;3;0;Create;True;0;0;0;False;0;False;-1;None;8b3135be4972eae4e98e95be997b4c94;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;14;361,-59.5;Inherit;True;Property;_TextureSample1;Texture Sample 0;4;0;Create;True;0;0;0;False;0;False;-1;None;fa41de56fb6e42845ae39aed2a2608cd;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;731,49;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;Caustics;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;False;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;5;0;6;0
WireConnection;7;0;5;0
WireConnection;1;0;2;0
WireConnection;1;1;7;0
WireConnection;1;2;3;0
WireConnection;8;0;1;0
WireConnection;9;0;8;0
WireConnection;10;0;9;0
WireConnection;10;1;11;0
WireConnection;10;2;12;0
WireConnection;0;0;13;0
WireConnection;0;1;14;0
WireConnection;0;2;10;0
ASEEND*/
//CHKSM=BA875C6E7249D44F76B9DDC34449A89FD31ED908