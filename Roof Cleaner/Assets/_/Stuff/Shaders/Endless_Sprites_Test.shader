// Unity built-in shader source. Copyright (c) 2016 Unity Technologies. MIT license (see license.txt)

Shader "Endless/Sprites/Test"
{
	Properties
	{
		[PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
		_Color ("Tint", Color) = (1,1,1,1)
		[MaterialToggle] PixelSnap ("Pixel snap", Float) = 0
		[HideInInspector] _RendererColor ("RendererColor", Color) = (1,1,1,1)
		[HideInInspector] _Flip ("Flip", Vector) = (1,1,1,1)
		[PerRendererData] _AlphaTex ("External Alpha", 2D) = "white" {}
		[PerRendererData] _EnableExternalAlpha ("Enable External Alpha", Float) = 0
			_Float ("Float", float) = 0
	}

	SubShader
	{
		Tags
		{ 
			"Queue"="Transparent" 
			"IgnoreProjector"="True" 
			"RenderType"="Transparent" 
			"PreviewType"="Plane"
			"CanUseSpriteAtlas"="True"
		}

		Cull Off
		Lighting Off
		ZWrite Off
		Blend One OneMinusSrcAlpha

		Pass
		{
		CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 2.0
			#pragma multi_compile_instancing
			#pragma multi_compile _ PIXELSNAP_ON
			#pragma multi_compile _ ETC1_EXTERNAL_ALPHA
			#include "UnitySprites.cginc"



			/*
			struct appdata_t
			{
				float4 vertex   : POSITION;
				float4 color    : COLOR;
				float2 texcoord : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct v2f
			{
				float4 vertex   : SV_POSITION;
				fixed4 color : COLOR;
				float2 texcoord : TEXCOORD0;
				UNITY_VERTEX_OUTPUT_STEREO
			};
			*/
			struct appdata_WithId
			{
				float4 vertex   : POSITION;
				float4 color    : COLOR;
				float2 texcoord : TEXCOORD0;
				uint   id       : SV_VertexID;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			float4 _MainTex_TexelSize;
			float _Float;

			v2f vert(appdata_WithId IN)
			{
				v2f OUT;

				UNITY_SETUP_INSTANCE_ID(IN);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);

#ifdef UNITY_INSTANCING_ENABLED
				IN.vertex.xy *= _Flip.xy;
#endif

				// Return Vertex space is the screen, -1 to +1

				// Fill entire screen
				// Last Parameter is ScreenSize
				//OUT.vertex = (float4(-1.0f + IN.texcoord.x * 2.0f, +1.0f + IN.texcoord.y * -2.0f, 0, 1.0f));
				//OUT.vertex = (float4(-0.5f + IN.texcoord.x * 1.0f, +0.5f + IN.texcoord.y * -1.0f, 0, 0.5f));
				// ___________________________

				// Expand Vertex Size
				//OUT.vertex = UnityObjectToClipPos(IN.vertex + float4((-0.5f + IN.texcoord.x) * 1, (-0.5f + IN.texcoord.y) * 1, 0, 0));
				// ___________________________


				if (IN.id <= 60) {
					IN.vertex.x += 10;
				}
				float4 clippedVertex = UnityObjectToClipPos(IN.vertex);
				OUT.vertex = UnityObjectToClipPos(IN.vertex);
				OUT.texcoord = IN.texcoord;
				OUT.color = IN.color * _Color * _RendererColor;

				return OUT;
			}

			//sampler2D _MainTex;
			//sampler2D _AlphaTex;

			/*fixed4 SampleSpriteTexture(float2 uv)
			{
				fixed4 color = tex2D(_MainTex, uv);

#if ETC1_EXTERNAL_ALPHA
				fixed4 alpha = tex2D(_AlphaTex, uv);
				color.a = lerp(color.a, alpha.r, _EnableExternalAlpha);
#endif

				return color;
			}*/

			fixed4 frag(v2f IN) : SV_Target
			{
				fixed4 c = SampleSpriteTexture(IN.texcoord) * IN.color;
				c.rgb *= c.a;
				return c;
			}
		ENDCG
		}
	}
}
