// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Extinguisher"
{
	Properties
	{
		_BaseTex("BaseTex", 2D) = "white" {}
		_clipY("clipY", Float) = 0
		_stripe("stripe", 2D) = "white" {}
		_StripeColor("StripeColor", Color) = (0,0.7957666,1,0)
		_Metallic("Metallic", Float) = 0
		_Opacity("Opacity", Range( 0 , 1)) = 0.45
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "TransparentCutout"  "Queue" = "Transparent+0" }
		Cull Back
		Blend SrcAlpha OneMinusSrcAlpha
		CGINCLUDE
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		struct Input
		{
			float3 worldPos;
			float2 uv_texcoord;
		};

		uniform float _clipY;
		uniform sampler2D _stripe;
		uniform float4 _stripe_ST;
		uniform float4 _StripeColor;
		uniform sampler2D _BaseTex;
		uniform float4 _BaseTex_ST;
		uniform float _Metallic;
		uniform float _Opacity;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float clipY4 = _clipY;
			float3 ase_worldPos = i.worldPos;
			float worldY6 = ase_worldPos.y;
			float2 uv_stripe = i.uv_texcoord * _stripe_ST.xy + _stripe_ST.zw;
			float4 temp_output_15_0 = ( tex2D( _stripe, uv_stripe ) * _StripeColor );
			float2 uv_BaseTex = i.uv_texcoord * _BaseTex_ST.xy + _BaseTex_ST.zw;
			float4 ifLocalVar12 = 0;
			if( clipY4 >= worldY6 )
				ifLocalVar12 = temp_output_15_0;
			else
				ifLocalVar12 = tex2D( _BaseTex, uv_BaseTex );
			float4 Albedo16 = ifLocalVar12;
			o.Albedo = Albedo16.rgb;
			o.Metallic = _Metallic;
			o.Smoothness = 0.0;
			float ifLocalVar20 = 0;
			if( clipY4 >= worldY6 )
				ifLocalVar20 = _Opacity;
			else
				ifLocalVar20 = 1.0;
			float Opacity21 = ifLocalVar20;
			o.Alpha = Opacity21;
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Standard keepalpha fullforwardshadows exclude_path:deferred 

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
			sampler3D _DitherMaskLOD;
			struct v2f
			{
				V2F_SHADOW_CASTER;
				float2 customPack1 : TEXCOORD1;
				float3 worldPos : TEXCOORD2;
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
				SurfaceOutputStandard o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutputStandard, o )
				surf( surfIN, o );
				#if defined( CAN_SKIP_VPOS )
				float2 vpos = IN.pos;
				#endif
				half alphaRef = tex3D( _DitherMaskLOD, float3( vpos.xy * 0.25, o.Alpha * 0.9375 ) ).a;
				clip( alphaRef - 0.01 );
				SHADOW_CASTER_FRAGMENT( IN )
			}
			ENDCG
		}
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=17500
83.2;38.4;1449;738;2118.002;585.6532;1.9;True;False
Node;AmplifyShaderEditor.WorldPosInputsNode;2;-851.4902,-382.409;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RangedFloatNode;3;-819.6773,-486.1383;Inherit;False;Property;_clipY;clipY;2;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;4;-608.1654,-486.403;Inherit;False;clipY;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;6;-599.3977,-383.5822;Inherit;False;worldY;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;14;-500.7804,-31.56505;Inherit;False;Property;_StripeColor;StripeColor;4;0;Create;True;0;0;False;0;0,0.7957666,1,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;8;-541.8351,-238.9289;Inherit;True;Property;_stripe;stripe;3;0;Create;True;0;0;False;0;-1;839455f7c881f134fb0ff1fc9ef0d45c;839455f7c881f134fb0ff1fc9ef0d45c;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;1;-401.2907,156.8556;Inherit;True;Property;_BaseTex;BaseTex;1;0;Create;True;0;0;False;0;-1;145f2f1039b9bd94d96f7c5014c5af53;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;10;-314.8091,-427.3563;Inherit;False;4;clipY;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;11;-322.6093,-337.6564;Inherit;False;6;worldY;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;15;-249.0671,-235.9462;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;19;-332.8167,442.5502;Inherit;False;4;clipY;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;18;-340.6169,532.2501;Inherit;False;6;worldY;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;25;-273.3185,739.0649;Inherit;False;Constant;_Float0;Float 0;6;0;Create;True;0;0;False;0;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;26;-329.3185,647.0649;Inherit;False;Property;_Opacity;Opacity;6;0;Create;True;0;0;False;0;0.45;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.ConditionalIfNode;12;151.2514,-289.6193;Inherit;False;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;4;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ConditionalIfNode;20;110.6775,484.38;Inherit;False;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;21;384.8491,450.4642;Inherit;False;Opacity;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;16;381.551,-293.173;Inherit;False;Albedo;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;24;824.9054,-2.199219;Inherit;False;Constant;_Smoothness;Smoothness;5;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;23;835.9054,-91.19922;Inherit;False;Property;_Metallic;Metallic;5;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;22;830.7668,91.6772;Inherit;False;21;Opacity;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;5;-825.1342,741.8578;Inherit;False;Constant;_EmmiEdgeLen;EmmiEdgeLen;3;0;Create;True;0;0;False;0;0.01;0;0;0.2;0;1;FLOAT;0
Node;AmplifyShaderEditor.TexturePropertyNode;28;-1145.13,-93.14841;Inherit;True;Property;_Texture0;Texture 0;7;0;Create;True;0;0;False;0;839455f7c881f134fb0ff1fc9ef0d45c;None;False;white;Auto;Texture2D;-1;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.ScreenPosInputsNode;7;-882.2853,149.5437;Inherit;False;0;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;17;815.5442,-223.1749;Inherit;False;16;Albedo;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.Vector2Node;29;-1122.13,120.8516;Inherit;False;Constant;_Vector0;Vector 0;8;0;Create;True;0;0;False;0;0,0;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;1107.84,-193.7216;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;Extinguisher;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Custom;0.5;True;True;0;False;TransparentCutout;;Transparent;ForwardOnly;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;0;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;4;0;3;0
WireConnection;6;0;2;2
WireConnection;15;0;8;0
WireConnection;15;1;14;0
WireConnection;12;0;10;0
WireConnection;12;1;11;0
WireConnection;12;2;15;0
WireConnection;12;3;15;0
WireConnection;12;4;1;0
WireConnection;20;0;19;0
WireConnection;20;1;18;0
WireConnection;20;2;26;0
WireConnection;20;3;26;0
WireConnection;20;4;25;0
WireConnection;21;0;20;0
WireConnection;16;0;12;0
WireConnection;0;0;17;0
WireConnection;0;3;23;0
WireConnection;0;4;24;0
WireConnection;0;9;22;0
ASEEND*/
//CHKSM=9B19F389750DF82ACA4209B4E3B00487E6175E1F