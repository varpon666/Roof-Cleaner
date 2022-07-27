// Unity built-in shader source. Copyright (c) 2016 Unity Technologies. MIT license (see license.txt)

Shader "Endless/Sprites/Hue"
{
	Properties
	{
		[PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
		//_Hue("Tint", Color) = (1,1,1,1)
		_Hue("Hue", Range(0, 1)) = 0.5
		_Brightness("Brightness", Range(0, 1)) = 0.5
		_Contrast("Contrast", Range(0, 1)) = 0.5
		_Saturation("Saturation", Range(0, 1)) = 0.5

		[MaterialToggle] PixelSnap ("Pixel snap", Float) = 0
		[HideInInspector] _RendererColor ("RendererColor", Color) = (1,1,1,1)
		[HideInInspector] _Flip ("Flip", Vector) = (1,1,1,1)
		[PerRendererData] _AlphaTex ("External Alpha", 2D) = "white" {}
		[PerRendererData] _EnableExternalAlpha ("Enable External Alpha", Float) = 0
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
			float _Hue;
			float _Brightness;
			float _Contrast;
			float _Saturation;


			inline float3 applyHue(float3 aColor, float aHue)
			{
				float angle = radians(aHue);
				float3 k = float3(0.57735, 0.57735, 0.57735);
				float cosAngle = cos(angle);
				//Rodrigues' rotation formula
				return aColor * cosAngle + cross(k, aColor) * sin(angle) + k * dot(k, aColor) * (1 - cosAngle);
			}


			inline float4 applyHSBEffect(float4 startColor, fixed4 hsbc)
			{
				float _Hue = 360 * hsbc.r;
				float _Brightness = hsbc.g * 2 - 1;
				float _Contrast = hsbc.b * 2;
				float _Saturation = hsbc.a * 2;

				float4 outputColor = startColor;
				outputColor.rgb = applyHue(outputColor.rgb, _Hue);
				outputColor.rgb = (outputColor.rgb - 0.5f) * (_Contrast)+0.5f;
				outputColor.rgb = outputColor.rgb + _Brightness;
				float3 intensity = dot(outputColor.rgb, float3(0.299, 0.587, 0.114));
				outputColor.rgb = lerp(intensity, outputColor.rgb, _Saturation);

				return outputColor;
			}
			inline float4 applyHSBEffect(float4 startColor, float hue, float brightness, float contrast, float saturation)
			{
				return applyHSBEffect(startColor, fixed4(hue, brightness, contrast, saturation));
			}


			v2f vert(appdata_t IN)
			{
				v2f OUT;

				UNITY_SETUP_INSTANCE_ID(IN);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);

#ifdef UNITY_INSTANCING_ENABLED
				IN.vertex.xy *= _Flip.xy;
#endif

				OUT.vertex = UnityObjectToClipPos(IN.vertex);
				OUT.texcoord = IN.texcoord;
				OUT.color = IN.color * _RendererColor;

				return OUT;
			}

			fixed4 frag(v2f IN) : SV_Target
			{
				fixed4 c = SampleSpriteTexture(IN.texcoord) * IN.color;// *_Color;
				c = applyHSBEffect(c, _Hue, _Brightness, _Contrast, _Saturation);
				//c.rgb = applyHue(c.rgb, _Hue);
				c.rgb *= c.a;
				return c;
			}




		ENDCG
		}
	}
}
