Shader "DropletMask"
{
	Properties
	{
		[Enum(Off,0,Front,1,Back,2)] _CullMode ("Culling", Float) = 2
		[Toggle] _ZWriteMode ("Write to Z-buffer", Float) = 1
		[Enum(Less,0,Greater,1,LEqual,2,GEqual,3,Equal,4,NotEqual,5,Always,6)] _ZTestMode ("Depth Test", Float) = 6
		_AlphaMask ("Alpha Mask", 2D) = "white" {}
		_Normal ("Normal", 2D) = "bump" {}
		_Intensity ("Intensity", Float) = 1
		_Fade ("Opacity", Range(0, 1)) = 1
		[HideInInspector] _Cutoff ("Alpha cutoff", Range(0, 1)) = 0.5
	}
	SubShader
	{
		Tags { "IGNOREPROJECTOR" = "true" "QUEUE" = "Transparent" "RenderType" = "Transparent" }
		GrabPass { }
		Pass
		{
			Tags { "IGNOREPROJECTOR" = "true" "QUEUE" = "Transparent" "RenderType" = "Transparent" "SHADOWSUPPORT" = "true" }
			Blend SrcAlpha OneMinusSrcAlpha, SrcAlpha OneMinusSrcAlpha
			ZWrite Off
			Cull Off
			GpuProgramID 28525

			HLSLPROGRAM

			// https://docs.unity3d.com/Manual/SL-PragmaDirectives.html
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 4.0
			#pragma shader_feature DIRECTIONAL
			#pragma multi_compile _ LIGHTPROBE_SH
			#pragma multi_compile _ SHADOWS_SCREEN
			#pragma multi_compile _ VERTEXLIGHT_ON


			#ifdef DIRECTIONAL
			#ifndef LIGHTPROBE_SH
			#ifndef SHADOWS_SCREEN
			#ifndef VERTEXLIGHT_ON
			#define ANY_SHADER_VARIANT_ACTIVE

			float4 _ProjectionParams;
			float4x4 unity_ObjectToWorld;
			float4x4 unity_MatrixV;
			float4x4 unity_MatrixVP;

			static float4 vertex_uniform_buffer_0[6];
			static float4 vertex_uniform_buffer_1[4];
			static float4 vertex_uniform_buffer_2[21];
			static float4 gl_Position;
			static float4 vertex_input_0;
			static float2 vertex_input_1;
			static float4 vertex_input_2;
			static float2 vertex_output_1;
			static float4 vertex_output_2;
			static float4 vertex_output_3;

			struct Vertex_Stage_Input
			{
				float4 vertex_input_0 : POSITION; // POSITION
				float2 vertex_input_1 : COLOR; // TEXCOORD
				float4 vertex_input_2 : TEXCOORD0; // COLOR
			};

			struct Vertex_Stage_Output
			{
				float2 vertex_output_1 : TEXCOORD; // TEXCOORD
				float4 vertex_output_2 : COLOR; // COLOR
				float4 vertex_output_3 : TEXCOORD1; // TEXCOORD_1
				float4 gl_Position : SV_Position;
			};

			void vert_main()
			{
				precise float vertex_unnamed_47 = vertex_input_0.y * vertex_uniform_buffer_1[1u].x;
				precise float vertex_unnamed_48 = vertex_input_0.y * vertex_uniform_buffer_1[1u].y;
				precise float vertex_unnamed_49 = vertex_input_0.y * vertex_uniform_buffer_1[1u].z;
				precise float vertex_unnamed_50 = vertex_input_0.y * vertex_uniform_buffer_1[1u].w;
				precise float vertex_unnamed_84 = mad(vertex_uniform_buffer_1[2u].x, vertex_input_0.z, mad(vertex_uniform_buffer_1[0u].x, vertex_input_0.x, vertex_unnamed_47)) + vertex_uniform_buffer_1[3u].x;
				precise float vertex_unnamed_85 = mad(vertex_uniform_buffer_1[2u].y, vertex_input_0.z, mad(vertex_uniform_buffer_1[0u].y, vertex_input_0.x, vertex_unnamed_48)) + vertex_uniform_buffer_1[3u].y;
				precise float vertex_unnamed_86 = mad(vertex_uniform_buffer_1[2u].z, vertex_input_0.z, mad(vertex_uniform_buffer_1[0u].z, vertex_input_0.x, vertex_unnamed_49)) + vertex_uniform_buffer_1[3u].z;
				precise float vertex_unnamed_87 = mad(vertex_uniform_buffer_1[2u].w, vertex_input_0.z, mad(vertex_uniform_buffer_1[0u].w, vertex_input_0.x, vertex_unnamed_50)) + vertex_uniform_buffer_1[3u].w;
				precise float vertex_unnamed_95 = vertex_unnamed_85 * vertex_uniform_buffer_2[18u].x;
				precise float vertex_unnamed_96 = vertex_unnamed_85 * vertex_uniform_buffer_2[18u].y;
				precise float vertex_unnamed_97 = vertex_unnamed_85 * vertex_uniform_buffer_2[18u].z;
				precise float vertex_unnamed_98 = vertex_unnamed_85 * vertex_uniform_buffer_2[18u].w;
				float vertex_unnamed_128 = mad(vertex_uniform_buffer_2[20u].x, vertex_unnamed_87, mad(vertex_uniform_buffer_2[19u].x, vertex_unnamed_86, mad(vertex_uniform_buffer_2[17u].x, vertex_unnamed_84, vertex_unnamed_95)));
				float vertex_unnamed_129 = mad(vertex_uniform_buffer_2[20u].y, vertex_unnamed_87, mad(vertex_uniform_buffer_2[19u].y, vertex_unnamed_86, mad(vertex_uniform_buffer_2[17u].y, vertex_unnamed_84, vertex_unnamed_96)));
				float vertex_unnamed_131 = mad(vertex_uniform_buffer_2[20u].w, vertex_unnamed_87, mad(vertex_uniform_buffer_2[19u].w, vertex_unnamed_86, mad(vertex_uniform_buffer_2[17u].w, vertex_unnamed_84, vertex_unnamed_98)));
				gl_Position.x = vertex_unnamed_128;
				gl_Position.y = vertex_unnamed_129;
				gl_Position.z = mad(vertex_uniform_buffer_2[20u].z, vertex_unnamed_87, mad(vertex_uniform_buffer_2[19u].z, vertex_unnamed_86, mad(vertex_uniform_buffer_2[17u].z, vertex_unnamed_84, vertex_unnamed_97)));
				gl_Position.w = vertex_unnamed_131;
				vertex_output_1.x = vertex_input_1.x;
				vertex_output_1.y = vertex_input_1.y;
				vertex_output_2.x = vertex_input_2.x;
				vertex_output_2.y = vertex_input_2.y;
				vertex_output_2.z = vertex_input_2.z;
				vertex_output_2.w = vertex_input_2.w;
				precise float vertex_unnamed_159 = vertex_unnamed_85 * vertex_uniform_buffer_2[10u].z;
				precise float vertex_unnamed_175 = (-0.0f) - mad(vertex_uniform_buffer_2[12u].z, vertex_unnamed_87, mad(vertex_uniform_buffer_2[11u].z, vertex_unnamed_86, mad(vertex_uniform_buffer_2[9u].z, vertex_unnamed_84, vertex_unnamed_159)));
				vertex_output_3.z = vertex_unnamed_175;
				precise float vertex_unnamed_182 = vertex_unnamed_129 * vertex_uniform_buffer_0[5u].x;
				precise float vertex_unnamed_183 = vertex_unnamed_182 * 0.5f;
				precise float vertex_unnamed_185 = vertex_unnamed_128 * 0.5f;
				precise float vertex_unnamed_186 = vertex_unnamed_131 * 0.5f;
				vertex_output_3.w = vertex_unnamed_131;
				precise float vertex_unnamed_188 = vertex_unnamed_186 + vertex_unnamed_185;
				precise float vertex_unnamed_189 = vertex_unnamed_186 + vertex_unnamed_183;
				vertex_output_3.x = vertex_unnamed_188;
				vertex_output_3.y = vertex_unnamed_189;
			}

			Vertex_Stage_Output vert(Vertex_Stage_Input stage_input)
			{
				vertex_uniform_buffer_0[5] = float4(_ProjectionParams[0], _ProjectionParams[1], _ProjectionParams[2], _ProjectionParams[3]);

				vertex_uniform_buffer_1[0] = float4(unity_ObjectToWorld[0][0], unity_ObjectToWorld[1][0], unity_ObjectToWorld[2][0], unity_ObjectToWorld[3][0]);
				vertex_uniform_buffer_1[1] = float4(unity_ObjectToWorld[0][1], unity_ObjectToWorld[1][1], unity_ObjectToWorld[2][1], unity_ObjectToWorld[3][1]);
				vertex_uniform_buffer_1[2] = float4(unity_ObjectToWorld[0][2], unity_ObjectToWorld[1][2], unity_ObjectToWorld[2][2], unity_ObjectToWorld[3][2]);
				vertex_uniform_buffer_1[3] = float4(unity_ObjectToWorld[0][3], unity_ObjectToWorld[1][3], unity_ObjectToWorld[2][3], unity_ObjectToWorld[3][3]);

				vertex_uniform_buffer_2[9] = float4(unity_MatrixV[0][0], unity_MatrixV[1][0], unity_MatrixV[2][0], unity_MatrixV[3][0]);
				vertex_uniform_buffer_2[10] = float4(unity_MatrixV[0][1], unity_MatrixV[1][1], unity_MatrixV[2][1], unity_MatrixV[3][1]);
				vertex_uniform_buffer_2[11] = float4(unity_MatrixV[0][2], unity_MatrixV[1][2], unity_MatrixV[2][2], unity_MatrixV[3][2]);
				vertex_uniform_buffer_2[12] = float4(unity_MatrixV[0][3], unity_MatrixV[1][3], unity_MatrixV[2][3], unity_MatrixV[3][3]);

				vertex_uniform_buffer_2[17] = float4(unity_MatrixVP[0][0], unity_MatrixVP[1][0], unity_MatrixVP[2][0], unity_MatrixVP[3][0]);
				vertex_uniform_buffer_2[18] = float4(unity_MatrixVP[0][1], unity_MatrixVP[1][1], unity_MatrixVP[2][1], unity_MatrixVP[3][1]);
				vertex_uniform_buffer_2[19] = float4(unity_MatrixVP[0][2], unity_MatrixVP[1][2], unity_MatrixVP[2][2], unity_MatrixVP[3][2]);
				vertex_uniform_buffer_2[20] = float4(unity_MatrixVP[0][3], unity_MatrixVP[1][3], unity_MatrixVP[2][3], unity_MatrixVP[3][3]);

				vertex_input_0 = stage_input.vertex_input_0;
				vertex_input_1 = stage_input.vertex_input_1;
				vertex_input_2 = stage_input.vertex_input_2;
				vert_main();
				Vertex_Stage_Output stage_output;
				stage_output.gl_Position = gl_Position;
				stage_output.vertex_output_1 = vertex_output_1;
				stage_output.vertex_output_2 = vertex_output_2;
				stage_output.vertex_output_3 = vertex_output_3;
				return stage_output;
			}

			#endif // DIRECTIONAL
			#endif // !LIGHTPROBE_SH
			#endif // !SHADOWS_SCREEN
			#endif // !VERTEXLIGHT_ON


			#ifdef DIRECTIONAL
			#ifdef LIGHTPROBE_SH
			#ifndef SHADOWS_SCREEN
			#ifndef VERTEXLIGHT_ON
			#define ANY_SHADER_VARIANT_ACTIVE

			float4 _ProjectionParams;
			float4x4 unity_ObjectToWorld;
			float4x4 unity_MatrixV;
			float4x4 unity_MatrixVP;

			static float4 vertex_uniform_buffer_0[6];
			static float4 vertex_uniform_buffer_1[4];
			static float4 vertex_uniform_buffer_2[21];
			static float4 gl_Position;
			static float4 vertex_input_0;
			static float2 vertex_input_1;
			static float4 vertex_input_2;
			static float2 vertex_output_1;
			static float4 vertex_output_2;
			static float4 vertex_output_3;

			struct Vertex_Stage_Input
			{
				float4 vertex_input_0 : POSITION; // POSITION
				float2 vertex_input_1 : COLOR; // TEXCOORD
				float4 vertex_input_2 : TEXCOORD0; // COLOR
			};

			struct Vertex_Stage_Output
			{
				float2 vertex_output_1 : TEXCOORD; // TEXCOORD
				float4 vertex_output_2 : COLOR; // COLOR
				float4 vertex_output_3 : TEXCOORD1; // TEXCOORD_1
				float4 gl_Position : SV_Position;
			};

			void vert_main()
			{
				precise float vertex_unnamed_47 = vertex_input_0.y * vertex_uniform_buffer_1[1u].x;
				precise float vertex_unnamed_48 = vertex_input_0.y * vertex_uniform_buffer_1[1u].y;
				precise float vertex_unnamed_49 = vertex_input_0.y * vertex_uniform_buffer_1[1u].z;
				precise float vertex_unnamed_50 = vertex_input_0.y * vertex_uniform_buffer_1[1u].w;
				precise float vertex_unnamed_84 = mad(vertex_uniform_buffer_1[2u].x, vertex_input_0.z, mad(vertex_uniform_buffer_1[0u].x, vertex_input_0.x, vertex_unnamed_47)) + vertex_uniform_buffer_1[3u].x;
				precise float vertex_unnamed_85 = mad(vertex_uniform_buffer_1[2u].y, vertex_input_0.z, mad(vertex_uniform_buffer_1[0u].y, vertex_input_0.x, vertex_unnamed_48)) + vertex_uniform_buffer_1[3u].y;
				precise float vertex_unnamed_86 = mad(vertex_uniform_buffer_1[2u].z, vertex_input_0.z, mad(vertex_uniform_buffer_1[0u].z, vertex_input_0.x, vertex_unnamed_49)) + vertex_uniform_buffer_1[3u].z;
				precise float vertex_unnamed_87 = mad(vertex_uniform_buffer_1[2u].w, vertex_input_0.z, mad(vertex_uniform_buffer_1[0u].w, vertex_input_0.x, vertex_unnamed_50)) + vertex_uniform_buffer_1[3u].w;
				precise float vertex_unnamed_95 = vertex_unnamed_85 * vertex_uniform_buffer_2[18u].x;
				precise float vertex_unnamed_96 = vertex_unnamed_85 * vertex_uniform_buffer_2[18u].y;
				precise float vertex_unnamed_97 = vertex_unnamed_85 * vertex_uniform_buffer_2[18u].z;
				precise float vertex_unnamed_98 = vertex_unnamed_85 * vertex_uniform_buffer_2[18u].w;
				float vertex_unnamed_128 = mad(vertex_uniform_buffer_2[20u].x, vertex_unnamed_87, mad(vertex_uniform_buffer_2[19u].x, vertex_unnamed_86, mad(vertex_uniform_buffer_2[17u].x, vertex_unnamed_84, vertex_unnamed_95)));
				float vertex_unnamed_129 = mad(vertex_uniform_buffer_2[20u].y, vertex_unnamed_87, mad(vertex_uniform_buffer_2[19u].y, vertex_unnamed_86, mad(vertex_uniform_buffer_2[17u].y, vertex_unnamed_84, vertex_unnamed_96)));
				float vertex_unnamed_131 = mad(vertex_uniform_buffer_2[20u].w, vertex_unnamed_87, mad(vertex_uniform_buffer_2[19u].w, vertex_unnamed_86, mad(vertex_uniform_buffer_2[17u].w, vertex_unnamed_84, vertex_unnamed_98)));
				gl_Position.x = vertex_unnamed_128;
				gl_Position.y = vertex_unnamed_129;
				gl_Position.z = mad(vertex_uniform_buffer_2[20u].z, vertex_unnamed_87, mad(vertex_uniform_buffer_2[19u].z, vertex_unnamed_86, mad(vertex_uniform_buffer_2[17u].z, vertex_unnamed_84, vertex_unnamed_97)));
				gl_Position.w = vertex_unnamed_131;
				vertex_output_1.x = vertex_input_1.x;
				vertex_output_1.y = vertex_input_1.y;
				vertex_output_2.x = vertex_input_2.x;
				vertex_output_2.y = vertex_input_2.y;
				vertex_output_2.z = vertex_input_2.z;
				vertex_output_2.w = vertex_input_2.w;
				precise float vertex_unnamed_159 = vertex_unnamed_85 * vertex_uniform_buffer_2[10u].z;
				precise float vertex_unnamed_175 = (-0.0f) - mad(vertex_uniform_buffer_2[12u].z, vertex_unnamed_87, mad(vertex_uniform_buffer_2[11u].z, vertex_unnamed_86, mad(vertex_uniform_buffer_2[9u].z, vertex_unnamed_84, vertex_unnamed_159)));
				vertex_output_3.z = vertex_unnamed_175;
				precise float vertex_unnamed_182 = vertex_unnamed_129 * vertex_uniform_buffer_0[5u].x;
				precise float vertex_unnamed_183 = vertex_unnamed_182 * 0.5f;
				precise float vertex_unnamed_185 = vertex_unnamed_128 * 0.5f;
				precise float vertex_unnamed_186 = vertex_unnamed_131 * 0.5f;
				vertex_output_3.w = vertex_unnamed_131;
				precise float vertex_unnamed_188 = vertex_unnamed_186 + vertex_unnamed_185;
				precise float vertex_unnamed_189 = vertex_unnamed_186 + vertex_unnamed_183;
				vertex_output_3.x = vertex_unnamed_188;
				vertex_output_3.y = vertex_unnamed_189;
			}

			Vertex_Stage_Output vert(Vertex_Stage_Input stage_input)
			{
				vertex_uniform_buffer_0[5] = float4(_ProjectionParams[0], _ProjectionParams[1], _ProjectionParams[2], _ProjectionParams[3]);

				vertex_uniform_buffer_1[0] = float4(unity_ObjectToWorld[0][0], unity_ObjectToWorld[1][0], unity_ObjectToWorld[2][0], unity_ObjectToWorld[3][0]);
				vertex_uniform_buffer_1[1] = float4(unity_ObjectToWorld[0][1], unity_ObjectToWorld[1][1], unity_ObjectToWorld[2][1], unity_ObjectToWorld[3][1]);
				vertex_uniform_buffer_1[2] = float4(unity_ObjectToWorld[0][2], unity_ObjectToWorld[1][2], unity_ObjectToWorld[2][2], unity_ObjectToWorld[3][2]);
				vertex_uniform_buffer_1[3] = float4(unity_ObjectToWorld[0][3], unity_ObjectToWorld[1][3], unity_ObjectToWorld[2][3], unity_ObjectToWorld[3][3]);

				vertex_uniform_buffer_2[9] = float4(unity_MatrixV[0][0], unity_MatrixV[1][0], unity_MatrixV[2][0], unity_MatrixV[3][0]);
				vertex_uniform_buffer_2[10] = float4(unity_MatrixV[0][1], unity_MatrixV[1][1], unity_MatrixV[2][1], unity_MatrixV[3][1]);
				vertex_uniform_buffer_2[11] = float4(unity_MatrixV[0][2], unity_MatrixV[1][2], unity_MatrixV[2][2], unity_MatrixV[3][2]);
				vertex_uniform_buffer_2[12] = float4(unity_MatrixV[0][3], unity_MatrixV[1][3], unity_MatrixV[2][3], unity_MatrixV[3][3]);

				vertex_uniform_buffer_2[17] = float4(unity_MatrixVP[0][0], unity_MatrixVP[1][0], unity_MatrixVP[2][0], unity_MatrixVP[3][0]);
				vertex_uniform_buffer_2[18] = float4(unity_MatrixVP[0][1], unity_MatrixVP[1][1], unity_MatrixVP[2][1], unity_MatrixVP[3][1]);
				vertex_uniform_buffer_2[19] = float4(unity_MatrixVP[0][2], unity_MatrixVP[1][2], unity_MatrixVP[2][2], unity_MatrixVP[3][2]);
				vertex_uniform_buffer_2[20] = float4(unity_MatrixVP[0][3], unity_MatrixVP[1][3], unity_MatrixVP[2][3], unity_MatrixVP[3][3]);

				vertex_input_0 = stage_input.vertex_input_0;
				vertex_input_1 = stage_input.vertex_input_1;
				vertex_input_2 = stage_input.vertex_input_2;
				vert_main();
				Vertex_Stage_Output stage_output;
				stage_output.gl_Position = gl_Position;
				stage_output.vertex_output_1 = vertex_output_1;
				stage_output.vertex_output_2 = vertex_output_2;
				stage_output.vertex_output_3 = vertex_output_3;
				return stage_output;
			}

			#endif // DIRECTIONAL
			#endif // LIGHTPROBE_SH
			#endif // !SHADOWS_SCREEN
			#endif // !VERTEXLIGHT_ON


			#ifdef DIRECTIONAL
			#ifdef SHADOWS_SCREEN
			#ifndef LIGHTPROBE_SH
			#ifndef VERTEXLIGHT_ON
			#define ANY_SHADER_VARIANT_ACTIVE

			float4 _ProjectionParams;
			float4x4 unity_ObjectToWorld;
			float4x4 unity_MatrixV;
			float4x4 unity_MatrixVP;

			static float4 vertex_uniform_buffer_0[6];
			static float4 vertex_uniform_buffer_1[4];
			static float4 vertex_uniform_buffer_2[21];
			static float4 gl_Position;
			static float4 vertex_input_0;
			static float2 vertex_input_1;
			static float4 vertex_input_2;
			static float2 vertex_output_1;
			static float4 vertex_output_2;
			static float4 vertex_output_3;

			struct Vertex_Stage_Input
			{
				float4 vertex_input_0 : POSITION; // POSITION
				float2 vertex_input_1 : COLOR; // TEXCOORD
				float4 vertex_input_2 : TEXCOORD0; // COLOR
			};

			struct Vertex_Stage_Output
			{
				float2 vertex_output_1 : TEXCOORD; // TEXCOORD
				float4 vertex_output_2 : COLOR; // COLOR
				float4 vertex_output_3 : TEXCOORD1; // TEXCOORD_1
				float4 gl_Position : SV_Position;
			};

			void vert_main()
			{
				precise float vertex_unnamed_47 = vertex_input_0.y * vertex_uniform_buffer_1[1u].x;
				precise float vertex_unnamed_48 = vertex_input_0.y * vertex_uniform_buffer_1[1u].y;
				precise float vertex_unnamed_49 = vertex_input_0.y * vertex_uniform_buffer_1[1u].z;
				precise float vertex_unnamed_50 = vertex_input_0.y * vertex_uniform_buffer_1[1u].w;
				precise float vertex_unnamed_84 = mad(vertex_uniform_buffer_1[2u].x, vertex_input_0.z, mad(vertex_uniform_buffer_1[0u].x, vertex_input_0.x, vertex_unnamed_47)) + vertex_uniform_buffer_1[3u].x;
				precise float vertex_unnamed_85 = mad(vertex_uniform_buffer_1[2u].y, vertex_input_0.z, mad(vertex_uniform_buffer_1[0u].y, vertex_input_0.x, vertex_unnamed_48)) + vertex_uniform_buffer_1[3u].y;
				precise float vertex_unnamed_86 = mad(vertex_uniform_buffer_1[2u].z, vertex_input_0.z, mad(vertex_uniform_buffer_1[0u].z, vertex_input_0.x, vertex_unnamed_49)) + vertex_uniform_buffer_1[3u].z;
				precise float vertex_unnamed_87 = mad(vertex_uniform_buffer_1[2u].w, vertex_input_0.z, mad(vertex_uniform_buffer_1[0u].w, vertex_input_0.x, vertex_unnamed_50)) + vertex_uniform_buffer_1[3u].w;
				precise float vertex_unnamed_95 = vertex_unnamed_85 * vertex_uniform_buffer_2[18u].x;
				precise float vertex_unnamed_96 = vertex_unnamed_85 * vertex_uniform_buffer_2[18u].y;
				precise float vertex_unnamed_97 = vertex_unnamed_85 * vertex_uniform_buffer_2[18u].z;
				precise float vertex_unnamed_98 = vertex_unnamed_85 * vertex_uniform_buffer_2[18u].w;
				float vertex_unnamed_128 = mad(vertex_uniform_buffer_2[20u].x, vertex_unnamed_87, mad(vertex_uniform_buffer_2[19u].x, vertex_unnamed_86, mad(vertex_uniform_buffer_2[17u].x, vertex_unnamed_84, vertex_unnamed_95)));
				float vertex_unnamed_129 = mad(vertex_uniform_buffer_2[20u].y, vertex_unnamed_87, mad(vertex_uniform_buffer_2[19u].y, vertex_unnamed_86, mad(vertex_uniform_buffer_2[17u].y, vertex_unnamed_84, vertex_unnamed_96)));
				float vertex_unnamed_131 = mad(vertex_uniform_buffer_2[20u].w, vertex_unnamed_87, mad(vertex_uniform_buffer_2[19u].w, vertex_unnamed_86, mad(vertex_uniform_buffer_2[17u].w, vertex_unnamed_84, vertex_unnamed_98)));
				gl_Position.x = vertex_unnamed_128;
				gl_Position.y = vertex_unnamed_129;
				gl_Position.z = mad(vertex_uniform_buffer_2[20u].z, vertex_unnamed_87, mad(vertex_uniform_buffer_2[19u].z, vertex_unnamed_86, mad(vertex_uniform_buffer_2[17u].z, vertex_unnamed_84, vertex_unnamed_97)));
				gl_Position.w = vertex_unnamed_131;
				vertex_output_1.x = vertex_input_1.x;
				vertex_output_1.y = vertex_input_1.y;
				vertex_output_2.x = vertex_input_2.x;
				vertex_output_2.y = vertex_input_2.y;
				vertex_output_2.z = vertex_input_2.z;
				vertex_output_2.w = vertex_input_2.w;
				precise float vertex_unnamed_159 = vertex_unnamed_85 * vertex_uniform_buffer_2[10u].z;
				precise float vertex_unnamed_175 = (-0.0f) - mad(vertex_uniform_buffer_2[12u].z, vertex_unnamed_87, mad(vertex_uniform_buffer_2[11u].z, vertex_unnamed_86, mad(vertex_uniform_buffer_2[9u].z, vertex_unnamed_84, vertex_unnamed_159)));
				vertex_output_3.z = vertex_unnamed_175;
				precise float vertex_unnamed_182 = vertex_unnamed_129 * vertex_uniform_buffer_0[5u].x;
				precise float vertex_unnamed_183 = vertex_unnamed_182 * 0.5f;
				precise float vertex_unnamed_185 = vertex_unnamed_128 * 0.5f;
				precise float vertex_unnamed_186 = vertex_unnamed_131 * 0.5f;
				vertex_output_3.w = vertex_unnamed_131;
				precise float vertex_unnamed_188 = vertex_unnamed_186 + vertex_unnamed_185;
				precise float vertex_unnamed_189 = vertex_unnamed_186 + vertex_unnamed_183;
				vertex_output_3.x = vertex_unnamed_188;
				vertex_output_3.y = vertex_unnamed_189;
			}

			Vertex_Stage_Output vert(Vertex_Stage_Input stage_input)
			{
				vertex_uniform_buffer_0[5] = float4(_ProjectionParams[0], _ProjectionParams[1], _ProjectionParams[2], _ProjectionParams[3]);

				vertex_uniform_buffer_1[0] = float4(unity_ObjectToWorld[0][0], unity_ObjectToWorld[1][0], unity_ObjectToWorld[2][0], unity_ObjectToWorld[3][0]);
				vertex_uniform_buffer_1[1] = float4(unity_ObjectToWorld[0][1], unity_ObjectToWorld[1][1], unity_ObjectToWorld[2][1], unity_ObjectToWorld[3][1]);
				vertex_uniform_buffer_1[2] = float4(unity_ObjectToWorld[0][2], unity_ObjectToWorld[1][2], unity_ObjectToWorld[2][2], unity_ObjectToWorld[3][2]);
				vertex_uniform_buffer_1[3] = float4(unity_ObjectToWorld[0][3], unity_ObjectToWorld[1][3], unity_ObjectToWorld[2][3], unity_ObjectToWorld[3][3]);

				vertex_uniform_buffer_2[9] = float4(unity_MatrixV[0][0], unity_MatrixV[1][0], unity_MatrixV[2][0], unity_MatrixV[3][0]);
				vertex_uniform_buffer_2[10] = float4(unity_MatrixV[0][1], unity_MatrixV[1][1], unity_MatrixV[2][1], unity_MatrixV[3][1]);
				vertex_uniform_buffer_2[11] = float4(unity_MatrixV[0][2], unity_MatrixV[1][2], unity_MatrixV[2][2], unity_MatrixV[3][2]);
				vertex_uniform_buffer_2[12] = float4(unity_MatrixV[0][3], unity_MatrixV[1][3], unity_MatrixV[2][3], unity_MatrixV[3][3]);

				vertex_uniform_buffer_2[17] = float4(unity_MatrixVP[0][0], unity_MatrixVP[1][0], unity_MatrixVP[2][0], unity_MatrixVP[3][0]);
				vertex_uniform_buffer_2[18] = float4(unity_MatrixVP[0][1], unity_MatrixVP[1][1], unity_MatrixVP[2][1], unity_MatrixVP[3][1]);
				vertex_uniform_buffer_2[19] = float4(unity_MatrixVP[0][2], unity_MatrixVP[1][2], unity_MatrixVP[2][2], unity_MatrixVP[3][2]);
				vertex_uniform_buffer_2[20] = float4(unity_MatrixVP[0][3], unity_MatrixVP[1][3], unity_MatrixVP[2][3], unity_MatrixVP[3][3]);

				vertex_input_0 = stage_input.vertex_input_0;
				vertex_input_1 = stage_input.vertex_input_1;
				vertex_input_2 = stage_input.vertex_input_2;
				vert_main();
				Vertex_Stage_Output stage_output;
				stage_output.gl_Position = gl_Position;
				stage_output.vertex_output_1 = vertex_output_1;
				stage_output.vertex_output_2 = vertex_output_2;
				stage_output.vertex_output_3 = vertex_output_3;
				return stage_output;
			}

			#endif // DIRECTIONAL
			#endif // SHADOWS_SCREEN
			#endif // !LIGHTPROBE_SH
			#endif // !VERTEXLIGHT_ON


			#ifdef DIRECTIONAL
			#ifdef LIGHTPROBE_SH
			#ifdef SHADOWS_SCREEN
			#ifndef VERTEXLIGHT_ON
			#define ANY_SHADER_VARIANT_ACTIVE

			float4 _ProjectionParams;
			float4x4 unity_ObjectToWorld;
			float4x4 unity_MatrixV;
			float4x4 unity_MatrixVP;

			static float4 vertex_uniform_buffer_0[6];
			static float4 vertex_uniform_buffer_1[4];
			static float4 vertex_uniform_buffer_2[21];
			static float4 gl_Position;
			static float4 vertex_input_0;
			static float2 vertex_input_1;
			static float4 vertex_input_2;
			static float2 vertex_output_1;
			static float4 vertex_output_2;
			static float4 vertex_output_3;

			struct Vertex_Stage_Input
			{
				float4 vertex_input_0 : POSITION; // POSITION
				float2 vertex_input_1 : COLOR; // TEXCOORD
				float4 vertex_input_2 : TEXCOORD0; // COLOR
			};

			struct Vertex_Stage_Output
			{
				float2 vertex_output_1 : TEXCOORD; // TEXCOORD
				float4 vertex_output_2 : COLOR; // COLOR
				float4 vertex_output_3 : TEXCOORD1; // TEXCOORD_1
				float4 gl_Position : SV_Position;
			};

			void vert_main()
			{
				precise float vertex_unnamed_47 = vertex_input_0.y * vertex_uniform_buffer_1[1u].x;
				precise float vertex_unnamed_48 = vertex_input_0.y * vertex_uniform_buffer_1[1u].y;
				precise float vertex_unnamed_49 = vertex_input_0.y * vertex_uniform_buffer_1[1u].z;
				precise float vertex_unnamed_50 = vertex_input_0.y * vertex_uniform_buffer_1[1u].w;
				precise float vertex_unnamed_84 = mad(vertex_uniform_buffer_1[2u].x, vertex_input_0.z, mad(vertex_uniform_buffer_1[0u].x, vertex_input_0.x, vertex_unnamed_47)) + vertex_uniform_buffer_1[3u].x;
				precise float vertex_unnamed_85 = mad(vertex_uniform_buffer_1[2u].y, vertex_input_0.z, mad(vertex_uniform_buffer_1[0u].y, vertex_input_0.x, vertex_unnamed_48)) + vertex_uniform_buffer_1[3u].y;
				precise float vertex_unnamed_86 = mad(vertex_uniform_buffer_1[2u].z, vertex_input_0.z, mad(vertex_uniform_buffer_1[0u].z, vertex_input_0.x, vertex_unnamed_49)) + vertex_uniform_buffer_1[3u].z;
				precise float vertex_unnamed_87 = mad(vertex_uniform_buffer_1[2u].w, vertex_input_0.z, mad(vertex_uniform_buffer_1[0u].w, vertex_input_0.x, vertex_unnamed_50)) + vertex_uniform_buffer_1[3u].w;
				precise float vertex_unnamed_95 = vertex_unnamed_85 * vertex_uniform_buffer_2[18u].x;
				precise float vertex_unnamed_96 = vertex_unnamed_85 * vertex_uniform_buffer_2[18u].y;
				precise float vertex_unnamed_97 = vertex_unnamed_85 * vertex_uniform_buffer_2[18u].z;
				precise float vertex_unnamed_98 = vertex_unnamed_85 * vertex_uniform_buffer_2[18u].w;
				float vertex_unnamed_128 = mad(vertex_uniform_buffer_2[20u].x, vertex_unnamed_87, mad(vertex_uniform_buffer_2[19u].x, vertex_unnamed_86, mad(vertex_uniform_buffer_2[17u].x, vertex_unnamed_84, vertex_unnamed_95)));
				float vertex_unnamed_129 = mad(vertex_uniform_buffer_2[20u].y, vertex_unnamed_87, mad(vertex_uniform_buffer_2[19u].y, vertex_unnamed_86, mad(vertex_uniform_buffer_2[17u].y, vertex_unnamed_84, vertex_unnamed_96)));
				float vertex_unnamed_131 = mad(vertex_uniform_buffer_2[20u].w, vertex_unnamed_87, mad(vertex_uniform_buffer_2[19u].w, vertex_unnamed_86, mad(vertex_uniform_buffer_2[17u].w, vertex_unnamed_84, vertex_unnamed_98)));
				gl_Position.x = vertex_unnamed_128;
				gl_Position.y = vertex_unnamed_129;
				gl_Position.z = mad(vertex_uniform_buffer_2[20u].z, vertex_unnamed_87, mad(vertex_uniform_buffer_2[19u].z, vertex_unnamed_86, mad(vertex_uniform_buffer_2[17u].z, vertex_unnamed_84, vertex_unnamed_97)));
				gl_Position.w = vertex_unnamed_131;
				vertex_output_1.x = vertex_input_1.x;
				vertex_output_1.y = vertex_input_1.y;
				vertex_output_2.x = vertex_input_2.x;
				vertex_output_2.y = vertex_input_2.y;
				vertex_output_2.z = vertex_input_2.z;
				vertex_output_2.w = vertex_input_2.w;
				precise float vertex_unnamed_159 = vertex_unnamed_85 * vertex_uniform_buffer_2[10u].z;
				precise float vertex_unnamed_175 = (-0.0f) - mad(vertex_uniform_buffer_2[12u].z, vertex_unnamed_87, mad(vertex_uniform_buffer_2[11u].z, vertex_unnamed_86, mad(vertex_uniform_buffer_2[9u].z, vertex_unnamed_84, vertex_unnamed_159)));
				vertex_output_3.z = vertex_unnamed_175;
				precise float vertex_unnamed_182 = vertex_unnamed_129 * vertex_uniform_buffer_0[5u].x;
				precise float vertex_unnamed_183 = vertex_unnamed_182 * 0.5f;
				precise float vertex_unnamed_185 = vertex_unnamed_128 * 0.5f;
				precise float vertex_unnamed_186 = vertex_unnamed_131 * 0.5f;
				vertex_output_3.w = vertex_unnamed_131;
				precise float vertex_unnamed_188 = vertex_unnamed_186 + vertex_unnamed_185;
				precise float vertex_unnamed_189 = vertex_unnamed_186 + vertex_unnamed_183;
				vertex_output_3.x = vertex_unnamed_188;
				vertex_output_3.y = vertex_unnamed_189;
			}

			Vertex_Stage_Output vert(Vertex_Stage_Input stage_input)
			{
				vertex_uniform_buffer_0[5] = float4(_ProjectionParams[0], _ProjectionParams[1], _ProjectionParams[2], _ProjectionParams[3]);

				vertex_uniform_buffer_1[0] = float4(unity_ObjectToWorld[0][0], unity_ObjectToWorld[1][0], unity_ObjectToWorld[2][0], unity_ObjectToWorld[3][0]);
				vertex_uniform_buffer_1[1] = float4(unity_ObjectToWorld[0][1], unity_ObjectToWorld[1][1], unity_ObjectToWorld[2][1], unity_ObjectToWorld[3][1]);
				vertex_uniform_buffer_1[2] = float4(unity_ObjectToWorld[0][2], unity_ObjectToWorld[1][2], unity_ObjectToWorld[2][2], unity_ObjectToWorld[3][2]);
				vertex_uniform_buffer_1[3] = float4(unity_ObjectToWorld[0][3], unity_ObjectToWorld[1][3], unity_ObjectToWorld[2][3], unity_ObjectToWorld[3][3]);

				vertex_uniform_buffer_2[9] = float4(unity_MatrixV[0][0], unity_MatrixV[1][0], unity_MatrixV[2][0], unity_MatrixV[3][0]);
				vertex_uniform_buffer_2[10] = float4(unity_MatrixV[0][1], unity_MatrixV[1][1], unity_MatrixV[2][1], unity_MatrixV[3][1]);
				vertex_uniform_buffer_2[11] = float4(unity_MatrixV[0][2], unity_MatrixV[1][2], unity_MatrixV[2][2], unity_MatrixV[3][2]);
				vertex_uniform_buffer_2[12] = float4(unity_MatrixV[0][3], unity_MatrixV[1][3], unity_MatrixV[2][3], unity_MatrixV[3][3]);

				vertex_uniform_buffer_2[17] = float4(unity_MatrixVP[0][0], unity_MatrixVP[1][0], unity_MatrixVP[2][0], unity_MatrixVP[3][0]);
				vertex_uniform_buffer_2[18] = float4(unity_MatrixVP[0][1], unity_MatrixVP[1][1], unity_MatrixVP[2][1], unity_MatrixVP[3][1]);
				vertex_uniform_buffer_2[19] = float4(unity_MatrixVP[0][2], unity_MatrixVP[1][2], unity_MatrixVP[2][2], unity_MatrixVP[3][2]);
				vertex_uniform_buffer_2[20] = float4(unity_MatrixVP[0][3], unity_MatrixVP[1][3], unity_MatrixVP[2][3], unity_MatrixVP[3][3]);

				vertex_input_0 = stage_input.vertex_input_0;
				vertex_input_1 = stage_input.vertex_input_1;
				vertex_input_2 = stage_input.vertex_input_2;
				vert_main();
				Vertex_Stage_Output stage_output;
				stage_output.gl_Position = gl_Position;
				stage_output.vertex_output_1 = vertex_output_1;
				stage_output.vertex_output_2 = vertex_output_2;
				stage_output.vertex_output_3 = vertex_output_3;
				return stage_output;
			}

			#endif // DIRECTIONAL
			#endif // LIGHTPROBE_SH
			#endif // SHADOWS_SCREEN
			#endif // !VERTEXLIGHT_ON


			#ifdef DIRECTIONAL
			#ifdef VERTEXLIGHT_ON
			#ifndef LIGHTPROBE_SH
			#ifndef SHADOWS_SCREEN
			#define ANY_SHADER_VARIANT_ACTIVE

			float4 _ProjectionParams;
			float4x4 unity_ObjectToWorld;
			float4x4 unity_MatrixV;
			float4x4 unity_MatrixVP;

			static float4 vertex_uniform_buffer_0[6];
			static float4 vertex_uniform_buffer_1[4];
			static float4 vertex_uniform_buffer_2[21];
			static float4 gl_Position;
			static float4 vertex_input_0;
			static float2 vertex_input_1;
			static float4 vertex_input_2;
			static float2 vertex_output_1;
			static float4 vertex_output_2;
			static float4 vertex_output_3;

			struct Vertex_Stage_Input
			{
				float4 vertex_input_0 : POSITION; // POSITION
				float2 vertex_input_1 : COLOR; // TEXCOORD
				float4 vertex_input_2 : TEXCOORD0; // COLOR
			};

			struct Vertex_Stage_Output
			{
				float2 vertex_output_1 : TEXCOORD; // TEXCOORD
				float4 vertex_output_2 : COLOR; // COLOR
				float4 vertex_output_3 : TEXCOORD1; // TEXCOORD_1
				float4 gl_Position : SV_Position;
			};

			void vert_main()
			{
				precise float vertex_unnamed_47 = vertex_input_0.y * vertex_uniform_buffer_1[1u].x;
				precise float vertex_unnamed_48 = vertex_input_0.y * vertex_uniform_buffer_1[1u].y;
				precise float vertex_unnamed_49 = vertex_input_0.y * vertex_uniform_buffer_1[1u].z;
				precise float vertex_unnamed_50 = vertex_input_0.y * vertex_uniform_buffer_1[1u].w;
				precise float vertex_unnamed_84 = mad(vertex_uniform_buffer_1[2u].x, vertex_input_0.z, mad(vertex_uniform_buffer_1[0u].x, vertex_input_0.x, vertex_unnamed_47)) + vertex_uniform_buffer_1[3u].x;
				precise float vertex_unnamed_85 = mad(vertex_uniform_buffer_1[2u].y, vertex_input_0.z, mad(vertex_uniform_buffer_1[0u].y, vertex_input_0.x, vertex_unnamed_48)) + vertex_uniform_buffer_1[3u].y;
				precise float vertex_unnamed_86 = mad(vertex_uniform_buffer_1[2u].z, vertex_input_0.z, mad(vertex_uniform_buffer_1[0u].z, vertex_input_0.x, vertex_unnamed_49)) + vertex_uniform_buffer_1[3u].z;
				precise float vertex_unnamed_87 = mad(vertex_uniform_buffer_1[2u].w, vertex_input_0.z, mad(vertex_uniform_buffer_1[0u].w, vertex_input_0.x, vertex_unnamed_50)) + vertex_uniform_buffer_1[3u].w;
				precise float vertex_unnamed_95 = vertex_unnamed_85 * vertex_uniform_buffer_2[18u].x;
				precise float vertex_unnamed_96 = vertex_unnamed_85 * vertex_uniform_buffer_2[18u].y;
				precise float vertex_unnamed_97 = vertex_unnamed_85 * vertex_uniform_buffer_2[18u].z;
				precise float vertex_unnamed_98 = vertex_unnamed_85 * vertex_uniform_buffer_2[18u].w;
				float vertex_unnamed_128 = mad(vertex_uniform_buffer_2[20u].x, vertex_unnamed_87, mad(vertex_uniform_buffer_2[19u].x, vertex_unnamed_86, mad(vertex_uniform_buffer_2[17u].x, vertex_unnamed_84, vertex_unnamed_95)));
				float vertex_unnamed_129 = mad(vertex_uniform_buffer_2[20u].y, vertex_unnamed_87, mad(vertex_uniform_buffer_2[19u].y, vertex_unnamed_86, mad(vertex_uniform_buffer_2[17u].y, vertex_unnamed_84, vertex_unnamed_96)));
				float vertex_unnamed_131 = mad(vertex_uniform_buffer_2[20u].w, vertex_unnamed_87, mad(vertex_uniform_buffer_2[19u].w, vertex_unnamed_86, mad(vertex_uniform_buffer_2[17u].w, vertex_unnamed_84, vertex_unnamed_98)));
				gl_Position.x = vertex_unnamed_128;
				gl_Position.y = vertex_unnamed_129;
				gl_Position.z = mad(vertex_uniform_buffer_2[20u].z, vertex_unnamed_87, mad(vertex_uniform_buffer_2[19u].z, vertex_unnamed_86, mad(vertex_uniform_buffer_2[17u].z, vertex_unnamed_84, vertex_unnamed_97)));
				gl_Position.w = vertex_unnamed_131;
				vertex_output_1.x = vertex_input_1.x;
				vertex_output_1.y = vertex_input_1.y;
				vertex_output_2.x = vertex_input_2.x;
				vertex_output_2.y = vertex_input_2.y;
				vertex_output_2.z = vertex_input_2.z;
				vertex_output_2.w = vertex_input_2.w;
				precise float vertex_unnamed_159 = vertex_unnamed_85 * vertex_uniform_buffer_2[10u].z;
				precise float vertex_unnamed_175 = (-0.0f) - mad(vertex_uniform_buffer_2[12u].z, vertex_unnamed_87, mad(vertex_uniform_buffer_2[11u].z, vertex_unnamed_86, mad(vertex_uniform_buffer_2[9u].z, vertex_unnamed_84, vertex_unnamed_159)));
				vertex_output_3.z = vertex_unnamed_175;
				precise float vertex_unnamed_182 = vertex_unnamed_129 * vertex_uniform_buffer_0[5u].x;
				precise float vertex_unnamed_183 = vertex_unnamed_182 * 0.5f;
				precise float vertex_unnamed_185 = vertex_unnamed_128 * 0.5f;
				precise float vertex_unnamed_186 = vertex_unnamed_131 * 0.5f;
				vertex_output_3.w = vertex_unnamed_131;
				precise float vertex_unnamed_188 = vertex_unnamed_186 + vertex_unnamed_185;
				precise float vertex_unnamed_189 = vertex_unnamed_186 + vertex_unnamed_183;
				vertex_output_3.x = vertex_unnamed_188;
				vertex_output_3.y = vertex_unnamed_189;
			}

			Vertex_Stage_Output vert(Vertex_Stage_Input stage_input)
			{
				vertex_uniform_buffer_0[5] = float4(_ProjectionParams[0], _ProjectionParams[1], _ProjectionParams[2], _ProjectionParams[3]);

				vertex_uniform_buffer_1[0] = float4(unity_ObjectToWorld[0][0], unity_ObjectToWorld[1][0], unity_ObjectToWorld[2][0], unity_ObjectToWorld[3][0]);
				vertex_uniform_buffer_1[1] = float4(unity_ObjectToWorld[0][1], unity_ObjectToWorld[1][1], unity_ObjectToWorld[2][1], unity_ObjectToWorld[3][1]);
				vertex_uniform_buffer_1[2] = float4(unity_ObjectToWorld[0][2], unity_ObjectToWorld[1][2], unity_ObjectToWorld[2][2], unity_ObjectToWorld[3][2]);
				vertex_uniform_buffer_1[3] = float4(unity_ObjectToWorld[0][3], unity_ObjectToWorld[1][3], unity_ObjectToWorld[2][3], unity_ObjectToWorld[3][3]);

				vertex_uniform_buffer_2[9] = float4(unity_MatrixV[0][0], unity_MatrixV[1][0], unity_MatrixV[2][0], unity_MatrixV[3][0]);
				vertex_uniform_buffer_2[10] = float4(unity_MatrixV[0][1], unity_MatrixV[1][1], unity_MatrixV[2][1], unity_MatrixV[3][1]);
				vertex_uniform_buffer_2[11] = float4(unity_MatrixV[0][2], unity_MatrixV[1][2], unity_MatrixV[2][2], unity_MatrixV[3][2]);
				vertex_uniform_buffer_2[12] = float4(unity_MatrixV[0][3], unity_MatrixV[1][3], unity_MatrixV[2][3], unity_MatrixV[3][3]);

				vertex_uniform_buffer_2[17] = float4(unity_MatrixVP[0][0], unity_MatrixVP[1][0], unity_MatrixVP[2][0], unity_MatrixVP[3][0]);
				vertex_uniform_buffer_2[18] = float4(unity_MatrixVP[0][1], unity_MatrixVP[1][1], unity_MatrixVP[2][1], unity_MatrixVP[3][1]);
				vertex_uniform_buffer_2[19] = float4(unity_MatrixVP[0][2], unity_MatrixVP[1][2], unity_MatrixVP[2][2], unity_MatrixVP[3][2]);
				vertex_uniform_buffer_2[20] = float4(unity_MatrixVP[0][3], unity_MatrixVP[1][3], unity_MatrixVP[2][3], unity_MatrixVP[3][3]);

				vertex_input_0 = stage_input.vertex_input_0;
				vertex_input_1 = stage_input.vertex_input_1;
				vertex_input_2 = stage_input.vertex_input_2;
				vert_main();
				Vertex_Stage_Output stage_output;
				stage_output.gl_Position = gl_Position;
				stage_output.vertex_output_1 = vertex_output_1;
				stage_output.vertex_output_2 = vertex_output_2;
				stage_output.vertex_output_3 = vertex_output_3;
				return stage_output;
			}

			#endif // DIRECTIONAL
			#endif // VERTEXLIGHT_ON
			#endif // !LIGHTPROBE_SH
			#endif // !SHADOWS_SCREEN


			#ifdef DIRECTIONAL
			#ifdef LIGHTPROBE_SH
			#ifdef VERTEXLIGHT_ON
			#ifndef SHADOWS_SCREEN
			#define ANY_SHADER_VARIANT_ACTIVE

			float4 _ProjectionParams;
			float4x4 unity_ObjectToWorld;
			float4x4 unity_MatrixV;
			float4x4 unity_MatrixVP;

			static float4 vertex_uniform_buffer_0[6];
			static float4 vertex_uniform_buffer_1[4];
			static float4 vertex_uniform_buffer_2[21];
			static float4 gl_Position;
			static float4 vertex_input_0;
			static float2 vertex_input_1;
			static float4 vertex_input_2;
			static float2 vertex_output_1;
			static float4 vertex_output_2;
			static float4 vertex_output_3;

			struct Vertex_Stage_Input
			{
				float4 vertex_input_0 : POSITION; // POSITION
				float2 vertex_input_1 : COLOR; // TEXCOORD
				float4 vertex_input_2 : TEXCOORD0; // COLOR
			};

			struct Vertex_Stage_Output
			{
				float2 vertex_output_1 : TEXCOORD; // TEXCOORD
				float4 vertex_output_2 : COLOR; // COLOR
				float4 vertex_output_3 : TEXCOORD1; // TEXCOORD_1
				float4 gl_Position : SV_Position;
			};

			void vert_main()
			{
				precise float vertex_unnamed_47 = vertex_input_0.y * vertex_uniform_buffer_1[1u].x;
				precise float vertex_unnamed_48 = vertex_input_0.y * vertex_uniform_buffer_1[1u].y;
				precise float vertex_unnamed_49 = vertex_input_0.y * vertex_uniform_buffer_1[1u].z;
				precise float vertex_unnamed_50 = vertex_input_0.y * vertex_uniform_buffer_1[1u].w;
				precise float vertex_unnamed_84 = mad(vertex_uniform_buffer_1[2u].x, vertex_input_0.z, mad(vertex_uniform_buffer_1[0u].x, vertex_input_0.x, vertex_unnamed_47)) + vertex_uniform_buffer_1[3u].x;
				precise float vertex_unnamed_85 = mad(vertex_uniform_buffer_1[2u].y, vertex_input_0.z, mad(vertex_uniform_buffer_1[0u].y, vertex_input_0.x, vertex_unnamed_48)) + vertex_uniform_buffer_1[3u].y;
				precise float vertex_unnamed_86 = mad(vertex_uniform_buffer_1[2u].z, vertex_input_0.z, mad(vertex_uniform_buffer_1[0u].z, vertex_input_0.x, vertex_unnamed_49)) + vertex_uniform_buffer_1[3u].z;
				precise float vertex_unnamed_87 = mad(vertex_uniform_buffer_1[2u].w, vertex_input_0.z, mad(vertex_uniform_buffer_1[0u].w, vertex_input_0.x, vertex_unnamed_50)) + vertex_uniform_buffer_1[3u].w;
				precise float vertex_unnamed_95 = vertex_unnamed_85 * vertex_uniform_buffer_2[18u].x;
				precise float vertex_unnamed_96 = vertex_unnamed_85 * vertex_uniform_buffer_2[18u].y;
				precise float vertex_unnamed_97 = vertex_unnamed_85 * vertex_uniform_buffer_2[18u].z;
				precise float vertex_unnamed_98 = vertex_unnamed_85 * vertex_uniform_buffer_2[18u].w;
				float vertex_unnamed_128 = mad(vertex_uniform_buffer_2[20u].x, vertex_unnamed_87, mad(vertex_uniform_buffer_2[19u].x, vertex_unnamed_86, mad(vertex_uniform_buffer_2[17u].x, vertex_unnamed_84, vertex_unnamed_95)));
				float vertex_unnamed_129 = mad(vertex_uniform_buffer_2[20u].y, vertex_unnamed_87, mad(vertex_uniform_buffer_2[19u].y, vertex_unnamed_86, mad(vertex_uniform_buffer_2[17u].y, vertex_unnamed_84, vertex_unnamed_96)));
				float vertex_unnamed_131 = mad(vertex_uniform_buffer_2[20u].w, vertex_unnamed_87, mad(vertex_uniform_buffer_2[19u].w, vertex_unnamed_86, mad(vertex_uniform_buffer_2[17u].w, vertex_unnamed_84, vertex_unnamed_98)));
				gl_Position.x = vertex_unnamed_128;
				gl_Position.y = vertex_unnamed_129;
				gl_Position.z = mad(vertex_uniform_buffer_2[20u].z, vertex_unnamed_87, mad(vertex_uniform_buffer_2[19u].z, vertex_unnamed_86, mad(vertex_uniform_buffer_2[17u].z, vertex_unnamed_84, vertex_unnamed_97)));
				gl_Position.w = vertex_unnamed_131;
				vertex_output_1.x = vertex_input_1.x;
				vertex_output_1.y = vertex_input_1.y;
				vertex_output_2.x = vertex_input_2.x;
				vertex_output_2.y = vertex_input_2.y;
				vertex_output_2.z = vertex_input_2.z;
				vertex_output_2.w = vertex_input_2.w;
				precise float vertex_unnamed_159 = vertex_unnamed_85 * vertex_uniform_buffer_2[10u].z;
				precise float vertex_unnamed_175 = (-0.0f) - mad(vertex_uniform_buffer_2[12u].z, vertex_unnamed_87, mad(vertex_uniform_buffer_2[11u].z, vertex_unnamed_86, mad(vertex_uniform_buffer_2[9u].z, vertex_unnamed_84, vertex_unnamed_159)));
				vertex_output_3.z = vertex_unnamed_175;
				precise float vertex_unnamed_182 = vertex_unnamed_129 * vertex_uniform_buffer_0[5u].x;
				precise float vertex_unnamed_183 = vertex_unnamed_182 * 0.5f;
				precise float vertex_unnamed_185 = vertex_unnamed_128 * 0.5f;
				precise float vertex_unnamed_186 = vertex_unnamed_131 * 0.5f;
				vertex_output_3.w = vertex_unnamed_131;
				precise float vertex_unnamed_188 = vertex_unnamed_186 + vertex_unnamed_185;
				precise float vertex_unnamed_189 = vertex_unnamed_186 + vertex_unnamed_183;
				vertex_output_3.x = vertex_unnamed_188;
				vertex_output_3.y = vertex_unnamed_189;
			}

			Vertex_Stage_Output vert(Vertex_Stage_Input stage_input)
			{
				vertex_uniform_buffer_0[5] = float4(_ProjectionParams[0], _ProjectionParams[1], _ProjectionParams[2], _ProjectionParams[3]);

				vertex_uniform_buffer_1[0] = float4(unity_ObjectToWorld[0][0], unity_ObjectToWorld[1][0], unity_ObjectToWorld[2][0], unity_ObjectToWorld[3][0]);
				vertex_uniform_buffer_1[1] = float4(unity_ObjectToWorld[0][1], unity_ObjectToWorld[1][1], unity_ObjectToWorld[2][1], unity_ObjectToWorld[3][1]);
				vertex_uniform_buffer_1[2] = float4(unity_ObjectToWorld[0][2], unity_ObjectToWorld[1][2], unity_ObjectToWorld[2][2], unity_ObjectToWorld[3][2]);
				vertex_uniform_buffer_1[3] = float4(unity_ObjectToWorld[0][3], unity_ObjectToWorld[1][3], unity_ObjectToWorld[2][3], unity_ObjectToWorld[3][3]);

				vertex_uniform_buffer_2[9] = float4(unity_MatrixV[0][0], unity_MatrixV[1][0], unity_MatrixV[2][0], unity_MatrixV[3][0]);
				vertex_uniform_buffer_2[10] = float4(unity_MatrixV[0][1], unity_MatrixV[1][1], unity_MatrixV[2][1], unity_MatrixV[3][1]);
				vertex_uniform_buffer_2[11] = float4(unity_MatrixV[0][2], unity_MatrixV[1][2], unity_MatrixV[2][2], unity_MatrixV[3][2]);
				vertex_uniform_buffer_2[12] = float4(unity_MatrixV[0][3], unity_MatrixV[1][3], unity_MatrixV[2][3], unity_MatrixV[3][3]);

				vertex_uniform_buffer_2[17] = float4(unity_MatrixVP[0][0], unity_MatrixVP[1][0], unity_MatrixVP[2][0], unity_MatrixVP[3][0]);
				vertex_uniform_buffer_2[18] = float4(unity_MatrixVP[0][1], unity_MatrixVP[1][1], unity_MatrixVP[2][1], unity_MatrixVP[3][1]);
				vertex_uniform_buffer_2[19] = float4(unity_MatrixVP[0][2], unity_MatrixVP[1][2], unity_MatrixVP[2][2], unity_MatrixVP[3][2]);
				vertex_uniform_buffer_2[20] = float4(unity_MatrixVP[0][3], unity_MatrixVP[1][3], unity_MatrixVP[2][3], unity_MatrixVP[3][3]);

				vertex_input_0 = stage_input.vertex_input_0;
				vertex_input_1 = stage_input.vertex_input_1;
				vertex_input_2 = stage_input.vertex_input_2;
				vert_main();
				Vertex_Stage_Output stage_output;
				stage_output.gl_Position = gl_Position;
				stage_output.vertex_output_1 = vertex_output_1;
				stage_output.vertex_output_2 = vertex_output_2;
				stage_output.vertex_output_3 = vertex_output_3;
				return stage_output;
			}

			#endif // DIRECTIONAL
			#endif // LIGHTPROBE_SH
			#endif // VERTEXLIGHT_ON
			#endif // !SHADOWS_SCREEN


			#ifdef DIRECTIONAL
			#ifdef SHADOWS_SCREEN
			#ifdef VERTEXLIGHT_ON
			#ifndef LIGHTPROBE_SH
			#define ANY_SHADER_VARIANT_ACTIVE

			float4 _ProjectionParams;
			float4x4 unity_ObjectToWorld;
			float4x4 unity_MatrixV;
			float4x4 unity_MatrixVP;

			static float4 vertex_uniform_buffer_0[6];
			static float4 vertex_uniform_buffer_1[4];
			static float4 vertex_uniform_buffer_2[21];
			static float4 gl_Position;
			static float4 vertex_input_0;
			static float2 vertex_input_1;
			static float4 vertex_input_2;
			static float2 vertex_output_1;
			static float4 vertex_output_2;
			static float4 vertex_output_3;

			struct Vertex_Stage_Input
			{
				float4 vertex_input_0 : POSITION; // POSITION
				float2 vertex_input_1 : COLOR; // TEXCOORD
				float4 vertex_input_2 : TEXCOORD0; // COLOR
			};

			struct Vertex_Stage_Output
			{
				float2 vertex_output_1 : TEXCOORD; // TEXCOORD
				float4 vertex_output_2 : COLOR; // COLOR
				float4 vertex_output_3 : TEXCOORD1; // TEXCOORD_1
				float4 gl_Position : SV_Position;
			};

			void vert_main()
			{
				precise float vertex_unnamed_47 = vertex_input_0.y * vertex_uniform_buffer_1[1u].x;
				precise float vertex_unnamed_48 = vertex_input_0.y * vertex_uniform_buffer_1[1u].y;
				precise float vertex_unnamed_49 = vertex_input_0.y * vertex_uniform_buffer_1[1u].z;
				precise float vertex_unnamed_50 = vertex_input_0.y * vertex_uniform_buffer_1[1u].w;
				precise float vertex_unnamed_84 = mad(vertex_uniform_buffer_1[2u].x, vertex_input_0.z, mad(vertex_uniform_buffer_1[0u].x, vertex_input_0.x, vertex_unnamed_47)) + vertex_uniform_buffer_1[3u].x;
				precise float vertex_unnamed_85 = mad(vertex_uniform_buffer_1[2u].y, vertex_input_0.z, mad(vertex_uniform_buffer_1[0u].y, vertex_input_0.x, vertex_unnamed_48)) + vertex_uniform_buffer_1[3u].y;
				precise float vertex_unnamed_86 = mad(vertex_uniform_buffer_1[2u].z, vertex_input_0.z, mad(vertex_uniform_buffer_1[0u].z, vertex_input_0.x, vertex_unnamed_49)) + vertex_uniform_buffer_1[3u].z;
				precise float vertex_unnamed_87 = mad(vertex_uniform_buffer_1[2u].w, vertex_input_0.z, mad(vertex_uniform_buffer_1[0u].w, vertex_input_0.x, vertex_unnamed_50)) + vertex_uniform_buffer_1[3u].w;
				precise float vertex_unnamed_95 = vertex_unnamed_85 * vertex_uniform_buffer_2[18u].x;
				precise float vertex_unnamed_96 = vertex_unnamed_85 * vertex_uniform_buffer_2[18u].y;
				precise float vertex_unnamed_97 = vertex_unnamed_85 * vertex_uniform_buffer_2[18u].z;
				precise float vertex_unnamed_98 = vertex_unnamed_85 * vertex_uniform_buffer_2[18u].w;
				float vertex_unnamed_128 = mad(vertex_uniform_buffer_2[20u].x, vertex_unnamed_87, mad(vertex_uniform_buffer_2[19u].x, vertex_unnamed_86, mad(vertex_uniform_buffer_2[17u].x, vertex_unnamed_84, vertex_unnamed_95)));
				float vertex_unnamed_129 = mad(vertex_uniform_buffer_2[20u].y, vertex_unnamed_87, mad(vertex_uniform_buffer_2[19u].y, vertex_unnamed_86, mad(vertex_uniform_buffer_2[17u].y, vertex_unnamed_84, vertex_unnamed_96)));
				float vertex_unnamed_131 = mad(vertex_uniform_buffer_2[20u].w, vertex_unnamed_87, mad(vertex_uniform_buffer_2[19u].w, vertex_unnamed_86, mad(vertex_uniform_buffer_2[17u].w, vertex_unnamed_84, vertex_unnamed_98)));
				gl_Position.x = vertex_unnamed_128;
				gl_Position.y = vertex_unnamed_129;
				gl_Position.z = mad(vertex_uniform_buffer_2[20u].z, vertex_unnamed_87, mad(vertex_uniform_buffer_2[19u].z, vertex_unnamed_86, mad(vertex_uniform_buffer_2[17u].z, vertex_unnamed_84, vertex_unnamed_97)));
				gl_Position.w = vertex_unnamed_131;
				vertex_output_1.x = vertex_input_1.x;
				vertex_output_1.y = vertex_input_1.y;
				vertex_output_2.x = vertex_input_2.x;
				vertex_output_2.y = vertex_input_2.y;
				vertex_output_2.z = vertex_input_2.z;
				vertex_output_2.w = vertex_input_2.w;
				precise float vertex_unnamed_159 = vertex_unnamed_85 * vertex_uniform_buffer_2[10u].z;
				precise float vertex_unnamed_175 = (-0.0f) - mad(vertex_uniform_buffer_2[12u].z, vertex_unnamed_87, mad(vertex_uniform_buffer_2[11u].z, vertex_unnamed_86, mad(vertex_uniform_buffer_2[9u].z, vertex_unnamed_84, vertex_unnamed_159)));
				vertex_output_3.z = vertex_unnamed_175;
				precise float vertex_unnamed_182 = vertex_unnamed_129 * vertex_uniform_buffer_0[5u].x;
				precise float vertex_unnamed_183 = vertex_unnamed_182 * 0.5f;
				precise float vertex_unnamed_185 = vertex_unnamed_128 * 0.5f;
				precise float vertex_unnamed_186 = vertex_unnamed_131 * 0.5f;
				vertex_output_3.w = vertex_unnamed_131;
				precise float vertex_unnamed_188 = vertex_unnamed_186 + vertex_unnamed_185;
				precise float vertex_unnamed_189 = vertex_unnamed_186 + vertex_unnamed_183;
				vertex_output_3.x = vertex_unnamed_188;
				vertex_output_3.y = vertex_unnamed_189;
			}

			Vertex_Stage_Output vert(Vertex_Stage_Input stage_input)
			{
				vertex_uniform_buffer_0[5] = float4(_ProjectionParams[0], _ProjectionParams[1], _ProjectionParams[2], _ProjectionParams[3]);

				vertex_uniform_buffer_1[0] = float4(unity_ObjectToWorld[0][0], unity_ObjectToWorld[1][0], unity_ObjectToWorld[2][0], unity_ObjectToWorld[3][0]);
				vertex_uniform_buffer_1[1] = float4(unity_ObjectToWorld[0][1], unity_ObjectToWorld[1][1], unity_ObjectToWorld[2][1], unity_ObjectToWorld[3][1]);
				vertex_uniform_buffer_1[2] = float4(unity_ObjectToWorld[0][2], unity_ObjectToWorld[1][2], unity_ObjectToWorld[2][2], unity_ObjectToWorld[3][2]);
				vertex_uniform_buffer_1[3] = float4(unity_ObjectToWorld[0][3], unity_ObjectToWorld[1][3], unity_ObjectToWorld[2][3], unity_ObjectToWorld[3][3]);

				vertex_uniform_buffer_2[9] = float4(unity_MatrixV[0][0], unity_MatrixV[1][0], unity_MatrixV[2][0], unity_MatrixV[3][0]);
				vertex_uniform_buffer_2[10] = float4(unity_MatrixV[0][1], unity_MatrixV[1][1], unity_MatrixV[2][1], unity_MatrixV[3][1]);
				vertex_uniform_buffer_2[11] = float4(unity_MatrixV[0][2], unity_MatrixV[1][2], unity_MatrixV[2][2], unity_MatrixV[3][2]);
				vertex_uniform_buffer_2[12] = float4(unity_MatrixV[0][3], unity_MatrixV[1][3], unity_MatrixV[2][3], unity_MatrixV[3][3]);

				vertex_uniform_buffer_2[17] = float4(unity_MatrixVP[0][0], unity_MatrixVP[1][0], unity_MatrixVP[2][0], unity_MatrixVP[3][0]);
				vertex_uniform_buffer_2[18] = float4(unity_MatrixVP[0][1], unity_MatrixVP[1][1], unity_MatrixVP[2][1], unity_MatrixVP[3][1]);
				vertex_uniform_buffer_2[19] = float4(unity_MatrixVP[0][2], unity_MatrixVP[1][2], unity_MatrixVP[2][2], unity_MatrixVP[3][2]);
				vertex_uniform_buffer_2[20] = float4(unity_MatrixVP[0][3], unity_MatrixVP[1][3], unity_MatrixVP[2][3], unity_MatrixVP[3][3]);

				vertex_input_0 = stage_input.vertex_input_0;
				vertex_input_1 = stage_input.vertex_input_1;
				vertex_input_2 = stage_input.vertex_input_2;
				vert_main();
				Vertex_Stage_Output stage_output;
				stage_output.gl_Position = gl_Position;
				stage_output.vertex_output_1 = vertex_output_1;
				stage_output.vertex_output_2 = vertex_output_2;
				stage_output.vertex_output_3 = vertex_output_3;
				return stage_output;
			}

			#endif // DIRECTIONAL
			#endif // SHADOWS_SCREEN
			#endif // VERTEXLIGHT_ON
			#endif // !LIGHTPROBE_SH


			#ifdef DIRECTIONAL
			#ifdef LIGHTPROBE_SH
			#ifdef SHADOWS_SCREEN
			#ifdef VERTEXLIGHT_ON
			#define ANY_SHADER_VARIANT_ACTIVE

			float4 _ProjectionParams;
			float4x4 unity_ObjectToWorld;
			float4x4 unity_MatrixV;
			float4x4 unity_MatrixVP;

			static float4 vertex_uniform_buffer_0[6];
			static float4 vertex_uniform_buffer_1[4];
			static float4 vertex_uniform_buffer_2[21];
			static float4 gl_Position;
			static float4 vertex_input_0;
			static float2 vertex_input_1;
			static float4 vertex_input_2;
			static float2 vertex_output_1;
			static float4 vertex_output_2;
			static float4 vertex_output_3;

			struct Vertex_Stage_Input
			{
				float4 vertex_input_0 : POSITION; // POSITION
				float2 vertex_input_1 : COLOR; // TEXCOORD
				float4 vertex_input_2 : TEXCOORD0; // COLOR
			};

			struct Vertex_Stage_Output
			{
				float2 vertex_output_1 : TEXCOORD; // TEXCOORD
				float4 vertex_output_2 : COLOR; // COLOR
				float4 vertex_output_3 : TEXCOORD1; // TEXCOORD_1
				float4 gl_Position : SV_Position;
			};

			void vert_main()
			{
				precise float vertex_unnamed_47 = vertex_input_0.y * vertex_uniform_buffer_1[1u].x;
				precise float vertex_unnamed_48 = vertex_input_0.y * vertex_uniform_buffer_1[1u].y;
				precise float vertex_unnamed_49 = vertex_input_0.y * vertex_uniform_buffer_1[1u].z;
				precise float vertex_unnamed_50 = vertex_input_0.y * vertex_uniform_buffer_1[1u].w;
				precise float vertex_unnamed_84 = mad(vertex_uniform_buffer_1[2u].x, vertex_input_0.z, mad(vertex_uniform_buffer_1[0u].x, vertex_input_0.x, vertex_unnamed_47)) + vertex_uniform_buffer_1[3u].x;
				precise float vertex_unnamed_85 = mad(vertex_uniform_buffer_1[2u].y, vertex_input_0.z, mad(vertex_uniform_buffer_1[0u].y, vertex_input_0.x, vertex_unnamed_48)) + vertex_uniform_buffer_1[3u].y;
				precise float vertex_unnamed_86 = mad(vertex_uniform_buffer_1[2u].z, vertex_input_0.z, mad(vertex_uniform_buffer_1[0u].z, vertex_input_0.x, vertex_unnamed_49)) + vertex_uniform_buffer_1[3u].z;
				precise float vertex_unnamed_87 = mad(vertex_uniform_buffer_1[2u].w, vertex_input_0.z, mad(vertex_uniform_buffer_1[0u].w, vertex_input_0.x, vertex_unnamed_50)) + vertex_uniform_buffer_1[3u].w;
				precise float vertex_unnamed_95 = vertex_unnamed_85 * vertex_uniform_buffer_2[18u].x;
				precise float vertex_unnamed_96 = vertex_unnamed_85 * vertex_uniform_buffer_2[18u].y;
				precise float vertex_unnamed_97 = vertex_unnamed_85 * vertex_uniform_buffer_2[18u].z;
				precise float vertex_unnamed_98 = vertex_unnamed_85 * vertex_uniform_buffer_2[18u].w;
				float vertex_unnamed_128 = mad(vertex_uniform_buffer_2[20u].x, vertex_unnamed_87, mad(vertex_uniform_buffer_2[19u].x, vertex_unnamed_86, mad(vertex_uniform_buffer_2[17u].x, vertex_unnamed_84, vertex_unnamed_95)));
				float vertex_unnamed_129 = mad(vertex_uniform_buffer_2[20u].y, vertex_unnamed_87, mad(vertex_uniform_buffer_2[19u].y, vertex_unnamed_86, mad(vertex_uniform_buffer_2[17u].y, vertex_unnamed_84, vertex_unnamed_96)));
				float vertex_unnamed_131 = mad(vertex_uniform_buffer_2[20u].w, vertex_unnamed_87, mad(vertex_uniform_buffer_2[19u].w, vertex_unnamed_86, mad(vertex_uniform_buffer_2[17u].w, vertex_unnamed_84, vertex_unnamed_98)));
				gl_Position.x = vertex_unnamed_128;
				gl_Position.y = vertex_unnamed_129;
				gl_Position.z = mad(vertex_uniform_buffer_2[20u].z, vertex_unnamed_87, mad(vertex_uniform_buffer_2[19u].z, vertex_unnamed_86, mad(vertex_uniform_buffer_2[17u].z, vertex_unnamed_84, vertex_unnamed_97)));
				gl_Position.w = vertex_unnamed_131;
				vertex_output_1.x = vertex_input_1.x;
				vertex_output_1.y = vertex_input_1.y;
				vertex_output_2.x = vertex_input_2.x;
				vertex_output_2.y = vertex_input_2.y;
				vertex_output_2.z = vertex_input_2.z;
				vertex_output_2.w = vertex_input_2.w;
				precise float vertex_unnamed_159 = vertex_unnamed_85 * vertex_uniform_buffer_2[10u].z;
				precise float vertex_unnamed_175 = (-0.0f) - mad(vertex_uniform_buffer_2[12u].z, vertex_unnamed_87, mad(vertex_uniform_buffer_2[11u].z, vertex_unnamed_86, mad(vertex_uniform_buffer_2[9u].z, vertex_unnamed_84, vertex_unnamed_159)));
				vertex_output_3.z = vertex_unnamed_175;
				precise float vertex_unnamed_182 = vertex_unnamed_129 * vertex_uniform_buffer_0[5u].x;
				precise float vertex_unnamed_183 = vertex_unnamed_182 * 0.5f;
				precise float vertex_unnamed_185 = vertex_unnamed_128 * 0.5f;
				precise float vertex_unnamed_186 = vertex_unnamed_131 * 0.5f;
				vertex_output_3.w = vertex_unnamed_131;
				precise float vertex_unnamed_188 = vertex_unnamed_186 + vertex_unnamed_185;
				precise float vertex_unnamed_189 = vertex_unnamed_186 + vertex_unnamed_183;
				vertex_output_3.x = vertex_unnamed_188;
				vertex_output_3.y = vertex_unnamed_189;
			}

			Vertex_Stage_Output vert(Vertex_Stage_Input stage_input)
			{
				vertex_uniform_buffer_0[5] = float4(_ProjectionParams[0], _ProjectionParams[1], _ProjectionParams[2], _ProjectionParams[3]);

				vertex_uniform_buffer_1[0] = float4(unity_ObjectToWorld[0][0], unity_ObjectToWorld[1][0], unity_ObjectToWorld[2][0], unity_ObjectToWorld[3][0]);
				vertex_uniform_buffer_1[1] = float4(unity_ObjectToWorld[0][1], unity_ObjectToWorld[1][1], unity_ObjectToWorld[2][1], unity_ObjectToWorld[3][1]);
				vertex_uniform_buffer_1[2] = float4(unity_ObjectToWorld[0][2], unity_ObjectToWorld[1][2], unity_ObjectToWorld[2][2], unity_ObjectToWorld[3][2]);
				vertex_uniform_buffer_1[3] = float4(unity_ObjectToWorld[0][3], unity_ObjectToWorld[1][3], unity_ObjectToWorld[2][3], unity_ObjectToWorld[3][3]);

				vertex_uniform_buffer_2[9] = float4(unity_MatrixV[0][0], unity_MatrixV[1][0], unity_MatrixV[2][0], unity_MatrixV[3][0]);
				vertex_uniform_buffer_2[10] = float4(unity_MatrixV[0][1], unity_MatrixV[1][1], unity_MatrixV[2][1], unity_MatrixV[3][1]);
				vertex_uniform_buffer_2[11] = float4(unity_MatrixV[0][2], unity_MatrixV[1][2], unity_MatrixV[2][2], unity_MatrixV[3][2]);
				vertex_uniform_buffer_2[12] = float4(unity_MatrixV[0][3], unity_MatrixV[1][3], unity_MatrixV[2][3], unity_MatrixV[3][3]);

				vertex_uniform_buffer_2[17] = float4(unity_MatrixVP[0][0], unity_MatrixVP[1][0], unity_MatrixVP[2][0], unity_MatrixVP[3][0]);
				vertex_uniform_buffer_2[18] = float4(unity_MatrixVP[0][1], unity_MatrixVP[1][1], unity_MatrixVP[2][1], unity_MatrixVP[3][1]);
				vertex_uniform_buffer_2[19] = float4(unity_MatrixVP[0][2], unity_MatrixVP[1][2], unity_MatrixVP[2][2], unity_MatrixVP[3][2]);
				vertex_uniform_buffer_2[20] = float4(unity_MatrixVP[0][3], unity_MatrixVP[1][3], unity_MatrixVP[2][3], unity_MatrixVP[3][3]);

				vertex_input_0 = stage_input.vertex_input_0;
				vertex_input_1 = stage_input.vertex_input_1;
				vertex_input_2 = stage_input.vertex_input_2;
				vert_main();
				Vertex_Stage_Output stage_output;
				stage_output.gl_Position = gl_Position;
				stage_output.vertex_output_1 = vertex_output_1;
				stage_output.vertex_output_2 = vertex_output_2;
				stage_output.vertex_output_3 = vertex_output_3;
				return stage_output;
			}

			#endif // DIRECTIONAL
			#endif // LIGHTPROBE_SH
			#endif // SHADOWS_SCREEN
			#endif // VERTEXLIGHT_ON


			#ifdef DIRECTIONAL
			#ifndef LIGHTPROBE_SH
			#ifndef SHADOWS_SCREEN
			#ifndef VERTEXLIGHT_ON
			#define ANY_SHADER_VARIANT_ACTIVE

			float4 _AlphaMask_ST;
			float4 _Normal_ST;
			float _Intensity;
			float _Fade;

			static float4 fragment_uniform_buffer_0[5];
			Texture2D<float4> _GrabTexture;
			Texture2D<float4> _Normal;
			Texture2D<float4> _AlphaMask;
			SamplerState sampler_GrabTexture;
			SamplerState sampler_AlphaMask;
			SamplerState sampler_Normal;

			static float2 fragment_input_1;
			static float4 fragment_input_2;
			static float4 fragment_input_3;
			static float4 fragment_output_0;

			struct Fragment_Stage_Input
			{
				float2 fragment_input_1 : TEXCOORD; // TEXCOORD
				float4 fragment_input_2 : COLOR; // COLOR
				float4 fragment_input_3 : TEXCOORD1; // TEXCOORD_1
			};

			struct Fragment_Stage_Output
			{
				float4 fragment_output_0 : SV_Target0;
			};

			void frag_main()
			{
				float4 fragment_unnamed_61 = _Normal.Sample(sampler_Normal, float2(mad(fragment_input_1.x, fragment_uniform_buffer_0[3u].x, fragment_uniform_buffer_0[3u].z), mad(fragment_input_1.y, fragment_uniform_buffer_0[3u].y, fragment_uniform_buffer_0[3u].w)));
				precise float fragment_unnamed_66 = fragment_unnamed_61.w * fragment_unnamed_61.x;
				precise float fragment_unnamed_75 = mad(fragment_unnamed_66, 2.0f, -1.0f) * fragment_uniform_buffer_0[4u].x;
				precise float fragment_unnamed_76 = mad(fragment_unnamed_61.y, 2.0f, -1.0f) * fragment_uniform_buffer_0[4u].x;
				precise float fragment_unnamed_83 = fragment_input_3.x / fragment_input_3.w;
				precise float fragment_unnamed_84 = fragment_input_3.y / fragment_input_3.w;
				float4 fragment_unnamed_92 = _GrabTexture.Sample(sampler_GrabTexture, float2(mad(fragment_unnamed_75, fragment_input_2.x, fragment_unnamed_83), mad(fragment_unnamed_76, fragment_input_2.y, fragment_unnamed_84)));
				fragment_output_0.x = fragment_unnamed_92.x;
				fragment_output_0.y = fragment_unnamed_92.y;
				fragment_output_0.z = fragment_unnamed_92.z;
				precise float fragment_unnamed_122 = _AlphaMask.Sample(sampler_AlphaMask, float2(mad(fragment_input_1.x, fragment_uniform_buffer_0[2u].x, fragment_uniform_buffer_0[2u].z), mad(fragment_input_1.y, fragment_uniform_buffer_0[2u].y, fragment_uniform_buffer_0[2u].w))).w * fragment_input_2.w;
				precise float fragment_unnamed_126 = fragment_unnamed_122 * fragment_uniform_buffer_0[4u].y;
				fragment_output_0.w = fragment_unnamed_126;
			}

			Fragment_Stage_Output frag(Fragment_Stage_Input stage_input)
			{
				fragment_uniform_buffer_0[2] = float4(_AlphaMask_ST[0], _AlphaMask_ST[1], _AlphaMask_ST[2], _AlphaMask_ST[3]);

				fragment_uniform_buffer_0[3] = float4(_Normal_ST[0], _Normal_ST[1], _Normal_ST[2], _Normal_ST[3]);

				fragment_uniform_buffer_0[4] = float4(_Intensity, fragment_uniform_buffer_0[4][1], fragment_uniform_buffer_0[4][2], fragment_uniform_buffer_0[4][3]);

				fragment_uniform_buffer_0[4] = float4(fragment_uniform_buffer_0[4][0], _Fade, fragment_uniform_buffer_0[4][2], fragment_uniform_buffer_0[4][3]);

				fragment_input_1 = stage_input.fragment_input_1;
				fragment_input_2 = stage_input.fragment_input_2;
				fragment_input_3 = stage_input.fragment_input_3;
				frag_main();
				Fragment_Stage_Output stage_output;
				stage_output.fragment_output_0 = fragment_output_0;
				return stage_output;
			}

			#endif // DIRECTIONAL
			#endif // !LIGHTPROBE_SH
			#endif // !SHADOWS_SCREEN
			#endif // !VERTEXLIGHT_ON


			#ifdef DIRECTIONAL
			#ifdef LIGHTPROBE_SH
			#ifndef SHADOWS_SCREEN
			#ifndef VERTEXLIGHT_ON
			#define ANY_SHADER_VARIANT_ACTIVE

			float4 _AlphaMask_ST;
			float4 _Normal_ST;
			float _Intensity;
			float _Fade;

			static float4 fragment_uniform_buffer_0[5];
			Texture2D<float4> _GrabTexture;
			Texture2D<float4> _Normal;
			Texture2D<float4> _AlphaMask;
			SamplerState sampler_GrabTexture;
			SamplerState sampler_AlphaMask;
			SamplerState sampler_Normal;

			static float2 fragment_input_1;
			static float4 fragment_input_2;
			static float4 fragment_input_3;
			static float4 fragment_output_0;

			struct Fragment_Stage_Input
			{
				float2 fragment_input_1 : TEXCOORD; // TEXCOORD
				float4 fragment_input_2 : COLOR; // COLOR
				float4 fragment_input_3 : TEXCOORD1; // TEXCOORD_1
			};

			struct Fragment_Stage_Output
			{
				float4 fragment_output_0 : SV_Target0;
			};

			void frag_main()
			{
				float4 fragment_unnamed_61 = _Normal.Sample(sampler_Normal, float2(mad(fragment_input_1.x, fragment_uniform_buffer_0[3u].x, fragment_uniform_buffer_0[3u].z), mad(fragment_input_1.y, fragment_uniform_buffer_0[3u].y, fragment_uniform_buffer_0[3u].w)));
				precise float fragment_unnamed_66 = fragment_unnamed_61.w * fragment_unnamed_61.x;
				precise float fragment_unnamed_75 = mad(fragment_unnamed_66, 2.0f, -1.0f) * fragment_uniform_buffer_0[4u].x;
				precise float fragment_unnamed_76 = mad(fragment_unnamed_61.y, 2.0f, -1.0f) * fragment_uniform_buffer_0[4u].x;
				precise float fragment_unnamed_83 = fragment_input_3.x / fragment_input_3.w;
				precise float fragment_unnamed_84 = fragment_input_3.y / fragment_input_3.w;
				float4 fragment_unnamed_92 = _GrabTexture.Sample(sampler_GrabTexture, float2(mad(fragment_unnamed_75, fragment_input_2.x, fragment_unnamed_83), mad(fragment_unnamed_76, fragment_input_2.y, fragment_unnamed_84)));
				fragment_output_0.x = fragment_unnamed_92.x;
				fragment_output_0.y = fragment_unnamed_92.y;
				fragment_output_0.z = fragment_unnamed_92.z;
				precise float fragment_unnamed_122 = _AlphaMask.Sample(sampler_AlphaMask, float2(mad(fragment_input_1.x, fragment_uniform_buffer_0[2u].x, fragment_uniform_buffer_0[2u].z), mad(fragment_input_1.y, fragment_uniform_buffer_0[2u].y, fragment_uniform_buffer_0[2u].w))).w * fragment_input_2.w;
				precise float fragment_unnamed_126 = fragment_unnamed_122 * fragment_uniform_buffer_0[4u].y;
				fragment_output_0.w = fragment_unnamed_126;
			}

			Fragment_Stage_Output frag(Fragment_Stage_Input stage_input)
			{
				fragment_uniform_buffer_0[2] = float4(_AlphaMask_ST[0], _AlphaMask_ST[1], _AlphaMask_ST[2], _AlphaMask_ST[3]);

				fragment_uniform_buffer_0[3] = float4(_Normal_ST[0], _Normal_ST[1], _Normal_ST[2], _Normal_ST[3]);

				fragment_uniform_buffer_0[4] = float4(_Intensity, fragment_uniform_buffer_0[4][1], fragment_uniform_buffer_0[4][2], fragment_uniform_buffer_0[4][3]);

				fragment_uniform_buffer_0[4] = float4(fragment_uniform_buffer_0[4][0], _Fade, fragment_uniform_buffer_0[4][2], fragment_uniform_buffer_0[4][3]);

				fragment_input_1 = stage_input.fragment_input_1;
				fragment_input_2 = stage_input.fragment_input_2;
				fragment_input_3 = stage_input.fragment_input_3;
				frag_main();
				Fragment_Stage_Output stage_output;
				stage_output.fragment_output_0 = fragment_output_0;
				return stage_output;
			}

			#endif // DIRECTIONAL
			#endif // LIGHTPROBE_SH
			#endif // !SHADOWS_SCREEN
			#endif // !VERTEXLIGHT_ON


			#ifdef DIRECTIONAL
			#ifdef SHADOWS_SCREEN
			#ifndef LIGHTPROBE_SH
			#ifndef VERTEXLIGHT_ON
			#define ANY_SHADER_VARIANT_ACTIVE

			float4 _AlphaMask_ST;
			float4 _Normal_ST;
			float _Intensity;
			float _Fade;

			static float4 fragment_uniform_buffer_0[5];
			Texture2D<float4> _GrabTexture;
			Texture2D<float4> _Normal;
			Texture2D<float4> _AlphaMask;
			SamplerState sampler_GrabTexture;
			SamplerState sampler_AlphaMask;
			SamplerState sampler_Normal;

			static float2 fragment_input_1;
			static float4 fragment_input_2;
			static float4 fragment_input_3;
			static float4 fragment_output_0;

			struct Fragment_Stage_Input
			{
				float2 fragment_input_1 : TEXCOORD; // TEXCOORD
				float4 fragment_input_2 : COLOR; // COLOR
				float4 fragment_input_3 : TEXCOORD1; // TEXCOORD_1
			};

			struct Fragment_Stage_Output
			{
				float4 fragment_output_0 : SV_Target0;
			};

			void frag_main()
			{
				float4 fragment_unnamed_61 = _Normal.Sample(sampler_Normal, float2(mad(fragment_input_1.x, fragment_uniform_buffer_0[3u].x, fragment_uniform_buffer_0[3u].z), mad(fragment_input_1.y, fragment_uniform_buffer_0[3u].y, fragment_uniform_buffer_0[3u].w)));
				precise float fragment_unnamed_66 = fragment_unnamed_61.w * fragment_unnamed_61.x;
				precise float fragment_unnamed_75 = mad(fragment_unnamed_66, 2.0f, -1.0f) * fragment_uniform_buffer_0[4u].x;
				precise float fragment_unnamed_76 = mad(fragment_unnamed_61.y, 2.0f, -1.0f) * fragment_uniform_buffer_0[4u].x;
				precise float fragment_unnamed_83 = fragment_input_3.x / fragment_input_3.w;
				precise float fragment_unnamed_84 = fragment_input_3.y / fragment_input_3.w;
				float4 fragment_unnamed_92 = _GrabTexture.Sample(sampler_GrabTexture, float2(mad(fragment_unnamed_75, fragment_input_2.x, fragment_unnamed_83), mad(fragment_unnamed_76, fragment_input_2.y, fragment_unnamed_84)));
				fragment_output_0.x = fragment_unnamed_92.x;
				fragment_output_0.y = fragment_unnamed_92.y;
				fragment_output_0.z = fragment_unnamed_92.z;
				precise float fragment_unnamed_122 = _AlphaMask.Sample(sampler_AlphaMask, float2(mad(fragment_input_1.x, fragment_uniform_buffer_0[2u].x, fragment_uniform_buffer_0[2u].z), mad(fragment_input_1.y, fragment_uniform_buffer_0[2u].y, fragment_uniform_buffer_0[2u].w))).w * fragment_input_2.w;
				precise float fragment_unnamed_126 = fragment_unnamed_122 * fragment_uniform_buffer_0[4u].y;
				fragment_output_0.w = fragment_unnamed_126;
			}

			Fragment_Stage_Output frag(Fragment_Stage_Input stage_input)
			{
				fragment_uniform_buffer_0[2] = float4(_AlphaMask_ST[0], _AlphaMask_ST[1], _AlphaMask_ST[2], _AlphaMask_ST[3]);

				fragment_uniform_buffer_0[3] = float4(_Normal_ST[0], _Normal_ST[1], _Normal_ST[2], _Normal_ST[3]);

				fragment_uniform_buffer_0[4] = float4(_Intensity, fragment_uniform_buffer_0[4][1], fragment_uniform_buffer_0[4][2], fragment_uniform_buffer_0[4][3]);

				fragment_uniform_buffer_0[4] = float4(fragment_uniform_buffer_0[4][0], _Fade, fragment_uniform_buffer_0[4][2], fragment_uniform_buffer_0[4][3]);

				fragment_input_1 = stage_input.fragment_input_1;
				fragment_input_2 = stage_input.fragment_input_2;
				fragment_input_3 = stage_input.fragment_input_3;
				frag_main();
				Fragment_Stage_Output stage_output;
				stage_output.fragment_output_0 = fragment_output_0;
				return stage_output;
			}

			#endif // DIRECTIONAL
			#endif // SHADOWS_SCREEN
			#endif // !LIGHTPROBE_SH
			#endif // !VERTEXLIGHT_ON


			#ifdef DIRECTIONAL
			#ifdef LIGHTPROBE_SH
			#ifdef SHADOWS_SCREEN
			#ifndef VERTEXLIGHT_ON
			#define ANY_SHADER_VARIANT_ACTIVE

			float4 _AlphaMask_ST;
			float4 _Normal_ST;
			float _Intensity;
			float _Fade;

			static float4 fragment_uniform_buffer_0[5];
			Texture2D<float4> _GrabTexture;
			Texture2D<float4> _Normal;
			Texture2D<float4> _AlphaMask;
			SamplerState sampler_GrabTexture;
			SamplerState sampler_AlphaMask;
			SamplerState sampler_Normal;

			static float2 fragment_input_1;
			static float4 fragment_input_2;
			static float4 fragment_input_3;
			static float4 fragment_output_0;

			struct Fragment_Stage_Input
			{
				float2 fragment_input_1 : TEXCOORD; // TEXCOORD
				float4 fragment_input_2 : COLOR; // COLOR
				float4 fragment_input_3 : TEXCOORD1; // TEXCOORD_1
			};

			struct Fragment_Stage_Output
			{
				float4 fragment_output_0 : SV_Target0;
			};

			void frag_main()
			{
				float4 fragment_unnamed_61 = _Normal.Sample(sampler_Normal, float2(mad(fragment_input_1.x, fragment_uniform_buffer_0[3u].x, fragment_uniform_buffer_0[3u].z), mad(fragment_input_1.y, fragment_uniform_buffer_0[3u].y, fragment_uniform_buffer_0[3u].w)));
				precise float fragment_unnamed_66 = fragment_unnamed_61.w * fragment_unnamed_61.x;
				precise float fragment_unnamed_75 = mad(fragment_unnamed_66, 2.0f, -1.0f) * fragment_uniform_buffer_0[4u].x;
				precise float fragment_unnamed_76 = mad(fragment_unnamed_61.y, 2.0f, -1.0f) * fragment_uniform_buffer_0[4u].x;
				precise float fragment_unnamed_83 = fragment_input_3.x / fragment_input_3.w;
				precise float fragment_unnamed_84 = fragment_input_3.y / fragment_input_3.w;
				float4 fragment_unnamed_92 = _GrabTexture.Sample(sampler_GrabTexture, float2(mad(fragment_unnamed_75, fragment_input_2.x, fragment_unnamed_83), mad(fragment_unnamed_76, fragment_input_2.y, fragment_unnamed_84)));
				fragment_output_0.x = fragment_unnamed_92.x;
				fragment_output_0.y = fragment_unnamed_92.y;
				fragment_output_0.z = fragment_unnamed_92.z;
				precise float fragment_unnamed_122 = _AlphaMask.Sample(sampler_AlphaMask, float2(mad(fragment_input_1.x, fragment_uniform_buffer_0[2u].x, fragment_uniform_buffer_0[2u].z), mad(fragment_input_1.y, fragment_uniform_buffer_0[2u].y, fragment_uniform_buffer_0[2u].w))).w * fragment_input_2.w;
				precise float fragment_unnamed_126 = fragment_unnamed_122 * fragment_uniform_buffer_0[4u].y;
				fragment_output_0.w = fragment_unnamed_126;
			}

			Fragment_Stage_Output frag(Fragment_Stage_Input stage_input)
			{
				fragment_uniform_buffer_0[2] = float4(_AlphaMask_ST[0], _AlphaMask_ST[1], _AlphaMask_ST[2], _AlphaMask_ST[3]);

				fragment_uniform_buffer_0[3] = float4(_Normal_ST[0], _Normal_ST[1], _Normal_ST[2], _Normal_ST[3]);

				fragment_uniform_buffer_0[4] = float4(_Intensity, fragment_uniform_buffer_0[4][1], fragment_uniform_buffer_0[4][2], fragment_uniform_buffer_0[4][3]);

				fragment_uniform_buffer_0[4] = float4(fragment_uniform_buffer_0[4][0], _Fade, fragment_uniform_buffer_0[4][2], fragment_uniform_buffer_0[4][3]);

				fragment_input_1 = stage_input.fragment_input_1;
				fragment_input_2 = stage_input.fragment_input_2;
				fragment_input_3 = stage_input.fragment_input_3;
				frag_main();
				Fragment_Stage_Output stage_output;
				stage_output.fragment_output_0 = fragment_output_0;
				return stage_output;
			}

			#endif // DIRECTIONAL
			#endif // LIGHTPROBE_SH
			#endif // SHADOWS_SCREEN
			#endif // !VERTEXLIGHT_ON


			// Fallback Shader Code
			#ifndef ANY_SHADER_VARIANT_ACTIVE

			// https://docs.unity3d.com/Manual/SL-UnityShaderVariables.html
			float4x4 unity_MatrixMVP;

			struct Vertex_Stage_Input
			{
				float3 pos : POSITION;
			};

			struct Vertex_Stage_Output
			{
				float4 pos : SV_POSITION;
			};

			Vertex_Stage_Output vert(Vertex_Stage_Input input)
			{
				Vertex_Stage_Output output;
				output.pos = mul(unity_MatrixMVP, float4(input.pos, 1.0));
				return output;
			}

			float4 frag(Vertex_Stage_Output input) : SV_TARGET
			{
				// Output solid grey color (e.g., 50% grey)
				return float4(0.5, 0.5, 0.5, 1.0); // RGBA
			}

			#endif // !ANY_SHADER_VARIANT_ACTIVE


			ENDHLSL
		}
	}
	FallBack "Diffuse"
	CustomEditor "ShaderForgeMaterialInspector"
}
