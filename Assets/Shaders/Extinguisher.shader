// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "AseCustom/Extinguisher"
{
	Properties
	{
		_BaseTex("BaseTex", 2D) = "white" {}
		_TransColor("TransColor", Color) = (0,0.7957666,1,0)
		_clipY("clipY", Float) = 0
		_Metallic("Metallic", Float) = 0
		_Opacity("Opacity", Range( 0 , 1)) = 0.45
		_FresnelBias("FresnelBias", Float) = 0
		_FresnelScale("FresnelScale", Float) = 1
		_FreshnelPower("FreshnelPower", Float) = 5
		_EmmiEdgeLen("EmmiEdgeLen", Range( 0 , 0.2)) = 0.009566019
		[HDR]_EmmisionColor("EmmisionColor", Color) = (1,0,0.009047508,0)
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "TransparentCutout"  "Queue" = "Transparent+0" "IsEmissive" = "true"  }
		Cull Back
		Blend SrcAlpha OneMinusSrcAlpha
		
		CGINCLUDE
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		#ifdef UNITY_PASS_SHADOWCASTER
			#undef INTERNAL_DATA
			#undef WorldReflectionVector
			#undef WorldNormalVector
			#define INTERNAL_DATA half3 internalSurfaceTtoW0; half3 internalSurfaceTtoW1; half3 internalSurfaceTtoW2;
			#define WorldReflectionVector(data,normal) reflect (data.worldRefl, half3(dot(data.internalSurfaceTtoW0,normal), dot(data.internalSurfaceTtoW1,normal), dot(data.internalSurfaceTtoW2,normal)))
			#define WorldNormalVector(data,normal) half3(dot(data.internalSurfaceTtoW0,normal), dot(data.internalSurfaceTtoW1,normal), dot(data.internalSurfaceTtoW2,normal))
		#endif
		struct Input
		{
			float3 worldPos;
			float2 uv_texcoord;
			float3 viewDir;
			float3 worldNormal;
			INTERNAL_DATA
		};

		uniform float _clipY;
		uniform float4 _TransColor;
		uniform sampler2D _BaseTex;
		uniform float4 _BaseTex_ST;
		uniform float _EmmiEdgeLen;
		uniform float4 _EmmisionColor;
		uniform float _Metallic;
		uniform float _Opacity;
		uniform float _FresnelBias;
		uniform float _FresnelScale;
		uniform float _FreshnelPower;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float clipY4 = _clipY;
			float3 ase_worldPos = i.worldPos;
			float worldY6 = ase_worldPos.y;
			float2 uv_BaseTex = i.uv_texcoord * _BaseTex_ST.xy + _BaseTex_ST.zw;
			float4 ifLocalVar12 = 0;
			if( clipY4 >= worldY6 )
				ifLocalVar12 = _TransColor;
			else
				ifLocalVar12 = tex2D( _BaseTex, uv_BaseTex );
			float4 Albedo16 = ifLocalVar12;
			o.Albedo = Albedo16.rgb;
			float temp_output_43_0 = ( clipY4 - worldY6 );
			float4 color47 = IsGammaSpace() ? float4(0,0,0,0) : float4(0,0,0,0);
			float4 ifLocalVar48 = 0;
			if( temp_output_43_0 >= _EmmiEdgeLen )
				ifLocalVar48 = color47;
			else
				ifLocalVar48 = _EmmisionColor;
			float4 ifLocalVar51 = 0;
			if( temp_output_43_0 >= 0.0 )
				ifLocalVar51 = ifLocalVar48;
			else
				ifLocalVar51 = color47;
			float4 Emmision49 = ifLocalVar51;
			o.Emission = Emmision49.rgb;
			o.Metallic = _Metallic;
			o.Smoothness = 0.0;
			float3 ase_worldNormal = i.worldNormal;
			float fresnelNdotV33 = dot( ase_worldNormal, i.viewDir );
			float fresnelNode33 = ( _FresnelBias + _FresnelScale * pow( 1.0 - fresnelNdotV33, _FreshnelPower ) );
			float FresnelOpacity36 = ( _Opacity * fresnelNode33 );
			float ifLocalVar20 = 0;
			if( clipY4 >= worldY6 )
				ifLocalVar20 = FresnelOpacity36;
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
				float4 tSpace0 : TEXCOORD2;
				float4 tSpace1 : TEXCOORD3;
				float4 tSpace2 : TEXCOORD4;
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
				half3 worldTangent = UnityObjectToWorldDir( v.tangent.xyz );
				half tangentSign = v.tangent.w * unity_WorldTransformParams.w;
				half3 worldBinormal = cross( worldNormal, worldTangent ) * tangentSign;
				o.tSpace0 = float4( worldTangent.x, worldBinormal.x, worldNormal.x, worldPos.x );
				o.tSpace1 = float4( worldTangent.y, worldBinormal.y, worldNormal.y, worldPos.y );
				o.tSpace2 = float4( worldTangent.z, worldBinormal.z, worldNormal.z, worldPos.z );
				o.customPack1.xy = customInputData.uv_texcoord;
				o.customPack1.xy = v.texcoord;
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
				float3 worldPos = float3( IN.tSpace0.w, IN.tSpace1.w, IN.tSpace2.w );
				half3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				surfIN.viewDir = worldViewDir;
				surfIN.worldPos = worldPos;
				surfIN.worldNormal = float3( IN.tSpace0.z, IN.tSpace1.z, IN.tSpace2.z );
				surfIN.internalSurfaceTtoW0 = IN.tSpace0.xyz;
				surfIN.internalSurfaceTtoW1 = IN.tSpace1.xyz;
				surfIN.internalSurfaceTtoW2 = IN.tSpace2.xyz;
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
88.8;116;1449;738;3357.746;952.8466;3.814152;True;False
Node;AmplifyShaderEditor.WorldNormalVector;34;-1646.169,1191.581;Inherit;False;False;1;0;FLOAT3;0,0,1;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.ViewDirInputsCoordNode;37;-1640.518,1325.062;Inherit;False;World;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RangedFloatNode;38;-1442.518,1421.974;Inherit;False;Property;_FresnelBias;FresnelBias;7;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;40;-1439.518,1582.974;Inherit;False;Property;_FreshnelPower;FreshnelPower;9;0;Create;True;0;0;False;0;5;5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;39;-1441.518,1510.974;Inherit;False;Property;_FresnelScale;FresnelScale;8;0;Create;True;0;0;False;0;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.WorldPosInputsNode;2;-851.4902,-382.409;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RangedFloatNode;3;-819.6773,-486.1383;Inherit;False;Property;_clipY;clipY;4;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;6;-599.3977,-383.5822;Inherit;False;worldY;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;4;-608.1654,-486.403;Inherit;False;clipY;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.FresnelNode;33;-1231.286,1232.468;Inherit;False;Standard;WorldNormal;ViewDir;False;5;0;FLOAT3;0,0,1;False;4;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;5;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;26;-1376.658,1125.078;Inherit;False;Property;_Opacity;Opacity;6;0;Create;True;0;0;False;0;0.45;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;41;-812.6027,422.6824;Inherit;False;6;worldY;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;32;-989.277,1131.855;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;42;-808.6731,366.4364;Inherit;False;4;clipY;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;5;-591.1838,567.7737;Inherit;False;Property;_EmmiEdgeLen;EmmiEdgeLen;10;0;Create;True;0;0;False;0;0.009566019;0.009566019;0;0.2;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;43;-548.986,392.5647;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;47;-531.9539,654.3998;Inherit;False;Constant;_Color0;Color 0;11;0;Create;True;0;0;False;0;0,0,0,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;46;-535.1995,848.2755;Inherit;False;Property;_EmmisionColor;EmmisionColor;11;1;[HDR];Create;True;0;0;False;0;1,0,0.009047508,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RegisterLocalVarNode;36;-816.9021,1125.415;Inherit;False;FresnelOpacity;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;14;-554.2853,-54.16673;Inherit;False;Property;_TransColor;TransColor;3;0;Create;True;0;0;False;0;0,0.7957666,1,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ConditionalIfNode;48;-140.658,494.1661;Inherit;False;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;4;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;10;-314.8091,-427.3563;Inherit;False;4;clipY;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;52;58.62288,495.7011;Inherit;False;Constant;_Float1;Float 1;12;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;11;-322.6093,-337.6564;Inherit;False;6;worldY;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;25;-488.6627,1490.655;Inherit;False;Constant;_Float0;Float 0;6;0;Create;True;0;0;False;0;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;1;-401.2907,156.8556;Inherit;True;Property;_BaseTex;BaseTex;1;0;Create;True;0;0;False;0;-1;145f2f1039b9bd94d96f7c5014c5af53;145f2f1039b9bd94d96f7c5014c5af53;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;18;-563.4627,1196.32;Inherit;False;6;worldY;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;35;-524.3477,1306.443;Inherit;False;36;FresnelOpacity;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;19;-555.6625,1106.62;Inherit;False;4;clipY;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.ConditionalIfNode;20;-112.1683,1148.45;Inherit;False;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ConditionalIfNode;51;282.8033,462.9695;Inherit;False;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;4;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ConditionalIfNode;12;151.2514,-289.6193;Inherit;False;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;4;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;49;484.5862,510.1527;Inherit;False;Emmision;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;16;381.551,-293.173;Inherit;False;Albedo;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;21;162.0033,1114.534;Inherit;False;Opacity;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;17;815.5442,-223.1749;Inherit;False;16;Albedo;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;50;806.5477,-118.4207;Inherit;False;49;Emmision;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;23;852.9054,4.800781;Inherit;False;Property;_Metallic;Metallic;5;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;22;847.7668,187.6772;Inherit;False;21;Opacity;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;8;-649.8351,-241.9289;Inherit;True;Property;_stripe;stripe;2;0;Create;True;0;0;False;0;-1;839455f7c881f134fb0ff1fc9ef0d45c;839455f7c881f134fb0ff1fc9ef0d45c;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;15;-299.0671,-238.3717;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;24;841.9054,93.80078;Inherit;False;Constant;_Smoothness;Smoothness;5;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;1104.421,-196.0485;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;AseCustom/Extinguisher;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Custom;0.5;True;True;0;True;TransparentCutout;;Transparent;ForwardOnly;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;0;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;6;0;2;2
WireConnection;4;0;3;0
WireConnection;33;0;34;0
WireConnection;33;4;37;0
WireConnection;33;1;38;0
WireConnection;33;2;39;0
WireConnection;33;3;40;0
WireConnection;32;0;26;0
WireConnection;32;1;33;0
WireConnection;43;0;42;0
WireConnection;43;1;41;0
WireConnection;36;0;32;0
WireConnection;48;0;43;0
WireConnection;48;1;5;0
WireConnection;48;2;47;0
WireConnection;48;3;47;0
WireConnection;48;4;46;0
WireConnection;20;0;19;0
WireConnection;20;1;18;0
WireConnection;20;2;35;0
WireConnection;20;3;35;0
WireConnection;20;4;25;0
WireConnection;51;0;43;0
WireConnection;51;1;52;0
WireConnection;51;2;48;0
WireConnection;51;3;48;0
WireConnection;51;4;47;0
WireConnection;12;0;10;0
WireConnection;12;1;11;0
WireConnection;12;2;14;0
WireConnection;12;3;14;0
WireConnection;12;4;1;0
WireConnection;49;0;51;0
WireConnection;16;0;12;0
WireConnection;21;0;20;0
WireConnection;15;0;8;0
WireConnection;15;1;14;0
WireConnection;0;0;17;0
WireConnection;0;2;50;0
WireConnection;0;3;23;0
WireConnection;0;4;24;0
WireConnection;0;9;22;0
ASEEND*/
//CHKSM=7A60493481F43F53D632ACEFB3618687D532AC82