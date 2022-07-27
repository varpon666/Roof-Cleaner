// Unity built-in shader source. Copyright (c) 2016 Unity Technologies. MIT license (see license.txt)

Shader "Endless/Sprites/Shadow"
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
		_ShadowAlpha("Shadow Alpha", Range(0,1)) = 0.3
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

			float4 _MainTex_TexelSize;
			uniform float _MonthRatio = 1.0f;
			float _ShadowAlpha;

			v2f vert(appdata_t IN)
			{
				v2f OUT;

				UNITY_SETUP_INSTANCE_ID(IN);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);

#ifdef UNITY_INSTANCING_ENABLED
				IN.vertex.xy *= _Flip.xy;
#endif

				//OUT.vertex = UnityObjectToClipPos(IN.vertex);
				//OUT.vertex = IN.vertex;// UnityObjectToClipPos(IN.vertex + float4(IN.texcoord.y * 40, _MainTex_TexelSize.w * .1, 0, 10000));
				//OUT.vertex = float4(IN.texcoord.x * 100, IN.texcoord.y * 100, 0, 0);
				//OUT.vertex = UnityObjectToClipPos(float3(IN.texcoord.x * 100, IN.texcoord.y * 100, 0));
				//IN.vertex.x -= 1 * IN.vertex.y;
				//IN.vertex.y *= _SinTime.w;
				IN.vertex.y *= .4f;
				OUT.vertex = UnityObjectToClipPos(IN.vertex + float4(IN.vertex.y * .1 * (.5f - _MonthRatio) * 25, 0, 0, 0));
				//OUT.vertex = UnityObjectToClipPos(IN.vertex + float4(IN.vertex.y * .1 * _SinTime.w * 20, 0, 0, 0));
				//OUT.vertex = UnityObjectToClipPos(IN.vertex);// +float4(0, -IN.vertex.y * _SinTime.w * 2, 0, 0));
				//OUT.vertex += float4(IN.texcoord.y * .1,0,0,0);
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
				c.r = 0;
				c.g = 0;
				c.b = 0;
				c.a *= _ShadowAlpha;
				//c.a = .3f;
				//c.rgb *= .3f;
				//c.a = .3f;
				//c.rgb *= c.a;
				return c;
			}
		ENDCG
		}
	}
}
