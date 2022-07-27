// Unity built-in shader source. Copyright (c) 2016 Unity Technologies. MIT license (see license.txt)

Shader "Endless/Sprites/Outline"
{
	Properties
	{
		[PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
		//_OutlineColor ("Outline Color", Color) = (1,1,1,1)
		[MaterialToggle] PixelSnap ("Pixel snap", Float) = 0
		[HideInInspector] _RendererColor ("RendererColor", Color) = (1,1,1,1)
		[HideInInspector] _Flip ("Flip", Vector) = (1,1,1,1)
		[PerRendererData] _AlphaTex ("External Alpha", 2D) = "white" {}
		[PerRendererData] _EnableExternalAlpha ("Enable External Alpha", Float) = 0

		// Add values to determine if outlining is enabled and outline color.
		_Outline("Outline", Float) = 0
		_OutlineColor("Outline Color", Color) = (1,1,1,1)
		_OutlineSize("Outline Size", int) = 10
		_OutlineThickSize("Outline Thick Size", int) = 5
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

			float _Outline;
			fixed4 _OutlineColor;
			int _OutlineSize;
			int _OutlineThickSize;
			bool _OutlineBorderNotInternal = true; //added by Chris Garcia (thespinforce@gmail.com)

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

				// If outline is enabled and there is a pixel, try to draw an outline.
				if (_Outline > 0)
				{
					bool stop = false;
					for (int i = 1; i < _OutlineSize + 1; i++)
					{
						fixed4 currentPixel = tex2D(_MainTex, IN.texcoord + fixed2(0, 0));
						fixed4 pixelUp = tex2D(_MainTex, IN.texcoord + fixed2(0, i * _MainTex_TexelSize.y));
						fixed4 pixelDown = tex2D(_MainTex, IN.texcoord - fixed2(0, i *  _MainTex_TexelSize.y));
						fixed4 pixelRight = tex2D(_MainTex, IN.texcoord + fixed2(i * _MainTex_TexelSize.x, 0));
						fixed4 pixelLeft = tex2D(_MainTex, IN.texcoord - fixed2(i * _MainTex_TexelSize.x, 0));

						fixed4 pixelDownLeft = tex2D(_MainTex, IN.texcoord + fixed2(-i *  _MainTex_TexelSize.x, -i *  _MainTex_TexelSize.y));
						fixed4 pixelDownRight = tex2D(_MainTex, IN.texcoord + fixed2(+i *  _MainTex_TexelSize.x, -i *  _MainTex_TexelSize.y));
						fixed4 pixelUpLeft = tex2D(_MainTex, IN.texcoord + fixed2(-i *  _MainTex_TexelSize.x, +i *  _MainTex_TexelSize.y));
						fixed4 pixelUpRight = tex2D(_MainTex, IN.texcoord + fixed2(+i *  _MainTex_TexelSize.x, +i *  _MainTex_TexelSize.y));

						/*if (currentPixel.a <= 0.5f)
						{
							if (pixelUp.a > 0 || pixelDown.a > 0 || pixelRight.a > 0 || pixelLeft.a > 0) {
								c.rgba = fixed4(1, 1, 1, 1) * _OutlineColor;
								//break;
							}
						}*/
						if (!stop) {
							if (pixelUp.a <= 0 || pixelDown.a <= 0 || pixelRight.a <= 0 || pixelLeft.a <= 0 || 
								pixelDownLeft.a <= 0 || pixelDownRight.a <= 0 || pixelUpLeft.a <= 0 || pixelUpRight.a <= 0) {
								fixed4 tmpOutlineColor = _OutlineColor;
								float alphaRatio = (i - _OutlineThickSize) * 1.0f / (_OutlineSize - _OutlineThickSize + 1);
								//tmpOutlineColor.a = 1 - i * 1.0f / (_OutlineSize + 1);
								if (i <= _OutlineThickSize) {
									c.rgb = c.rgb + (tmpOutlineColor.rgb - c.rgb);// *(1 - alphaRatio);
								} else {
									c.rgb = c.rgb + (tmpOutlineColor.rgb - c.rgb) * (1 - alphaRatio);
								}
								stop = true;
							}
							if (IN.texcoord.y + i * _MainTex_TexelSize.y >= 1.0f ||
								IN.texcoord.y - i * _MainTex_TexelSize.y <= 0.0f ||
								IN.texcoord.x + i * _MainTex_TexelSize.x >= 1.0f ||
								IN.texcoord.x - i * _MainTex_TexelSize.x <= 0.0f) {
								//c.rgb += _OutlineColor;
								//c.rgb = fixed4(1, 1, 1, 1) * _OutlineColor;
								fixed4 tmpOutlineColor = _OutlineColor;
								float alphaRatio = (i - _OutlineThickSize) * 1.0f / (_OutlineSize - _OutlineThickSize + 1);
								if (i <= _OutlineThickSize) {
									c.rgb = c.rgb + (tmpOutlineColor.rgb - c.rgb);// *(1 - alphaRatio);
								}
								else {
									c.rgb = c.rgb + (tmpOutlineColor.rgb - c.rgb) * (1 - alphaRatio);
								}
								stop = true;
							}
						}

						/*if (IN.texcoord.x <= .01f || IN.texcoord.x >= .99f || IN.texcoord.y <= .01f || IN.texcoord.y >= .99f) {
							c.rgb = _OutlineColor;
						}*/
					}
					/*{
						// old code uses edge pixels as outlines, outline size grows internally
						float totalAlpha = 1.0;
						[unroll(16)]
						for (int i = 1; i < _OutlineSize + 1; i++) {
							fixed4 pixelUp = tex2D(_MainTex, IN.texcoord + fixed2(0, i * _MainTex_TexelSize.y));
							fixed4 pixelDown = tex2D(_MainTex, IN.texcoord - fixed2(0, i *  _MainTex_TexelSize.y));
							fixed4 pixelRight = tex2D(_MainTex, IN.texcoord + fixed2(i * _MainTex_TexelSize.x, 0));
							fixed4 pixelLeft = tex2D(_MainTex, IN.texcoord - fixed2(i * _MainTex_TexelSize.x, 0));

							totalAlpha = totalAlpha * pixelUp.a * pixelDown.a * pixelRight.a * pixelLeft.a;
						}

						if (totalAlpha == 0) {
							c.rgba = fixed4(1, 1, 1, 1) * _OutlineColor;
						}
					}*/
				}
				c.rgb *= c.a;
				return c;
			}
		ENDCG
		}
	}
}
