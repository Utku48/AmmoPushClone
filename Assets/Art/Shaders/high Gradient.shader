// Upgrade NOTE: upgraded instancing buffer 'highGradient' to new syntax.

// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "high Gradient"
{
	Properties
	{
		_UpperColor1("Upper Color", Color) = (0.09546334,0.8396226,0.08317016,0)
		_BottomColor1("Bottom Color", Color) = (0.5283019,0.1569954,0.1569954,0)
		_GradientHeight1("Gradient Height", Float) = 1
		_HeightSize1("Height Size", Float) = 1
		_TextureSample0("Texture Sample 0", 2D) = "white" {}
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" }
		Cull Back
		CGPROGRAM
		#pragma target 3.0
		#pragma multi_compile_instancing
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows 
		struct Input
		{
			float2 uv_texcoord;
			float3 worldPos;
		};

		uniform sampler2D _TextureSample0;

		UNITY_INSTANCING_BUFFER_START(highGradient)
			UNITY_DEFINE_INSTANCED_PROP(float4, _TextureSample0_ST)
#define _TextureSample0_ST_arr highGradient
			UNITY_DEFINE_INSTANCED_PROP(float4, _UpperColor1)
#define _UpperColor1_arr highGradient
			UNITY_DEFINE_INSTANCED_PROP(float4, _BottomColor1)
#define _BottomColor1_arr highGradient
			UNITY_DEFINE_INSTANCED_PROP(float, _HeightSize1)
#define _HeightSize1_arr highGradient
			UNITY_DEFINE_INSTANCED_PROP(float, _GradientHeight1)
#define _GradientHeight1_arr highGradient
		UNITY_INSTANCING_BUFFER_END(highGradient)

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float4 _TextureSample0_ST_Instance = UNITY_ACCESS_INSTANCED_PROP(_TextureSample0_ST_arr, _TextureSample0_ST);
			float2 uv_TextureSample0 = i.uv_texcoord * _TextureSample0_ST_Instance.xy + _TextureSample0_ST_Instance.zw;
			float4 _UpperColor1_Instance = UNITY_ACCESS_INSTANCED_PROP(_UpperColor1_arr, _UpperColor1);
			float4 _BottomColor1_Instance = UNITY_ACCESS_INSTANCED_PROP(_BottomColor1_arr, _BottomColor1);
			float _HeightSize1_Instance = UNITY_ACCESS_INSTANCED_PROP(_HeightSize1_arr, _HeightSize1);
			float _GradientHeight1_Instance = UNITY_ACCESS_INSTANCED_PROP(_GradientHeight1_arr, _GradientHeight1);
			float3 ase_worldPos = i.worldPos;
			float smoothstepResult20 = smoothstep( 0.0 , _HeightSize1_Instance , ( _GradientHeight1_Instance - ase_worldPos.y ));
			float4 lerpResult21 = lerp( _UpperColor1_Instance , _BottomColor1_Instance , smoothstepResult20);
			o.Albedo = ( tex2D( _TextureSample0, uv_TextureSample0 ) * lerpResult21 ).rgb;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18910
-1680;242;1215;613;2420.716;1037.238;2.448602;True;True
Node;AmplifyShaderEditor.WorldPosInputsNode;14;-1920.608,-179.7814;Inherit;True;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RangedFloatNode;15;-1848.155,-313.8587;Inherit;False;InstancedProperty;_GradientHeight1;Gradient Height;2;0;Create;True;0;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;16;-1597.355,-328.8112;Inherit;False;InstancedProperty;_HeightSize1;Height Size;3;0;Create;True;0;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;17;-1600.138,-193.1622;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;18;-1309.608,-587.418;Inherit;False;InstancedProperty;_UpperColor1;Upper Color;0;0;Create;True;0;0;0;False;0;False;0.09546334,0.8396226,0.08317016,0;0.09546334,0.8396226,0.08317016,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;19;-1373.37,-409.0543;Inherit;False;InstancedProperty;_BottomColor1;Bottom Color;1;0;Create;True;0;0;0;False;0;False;0.5283019,0.1569954,0.1569954,0;0.5283019,0.1569954,0.1569954,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SmoothstepOpNode;20;-1331.093,-208.7261;Inherit;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;7;-518.467,-432.6199;Inherit;True;Property;_TextureSample0;Texture Sample 0;4;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;21;-966.5063,-256.8399;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;8;-242.405,-103.0356;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;0,0;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;high Gradient;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;16;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;False;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;17;0;15;0
WireConnection;17;1;14;2
WireConnection;20;0;17;0
WireConnection;20;2;16;0
WireConnection;21;0;18;0
WireConnection;21;1;19;0
WireConnection;21;2;20;0
WireConnection;8;0;7;0
WireConnection;8;1;21;0
WireConnection;0;0;8;0
ASEEND*/
//CHKSM=7C043DC2B120A112EB5579FB8771FA2CA125C70B