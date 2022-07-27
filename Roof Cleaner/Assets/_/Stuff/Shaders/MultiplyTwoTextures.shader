Shader "Endless/MultiplyTwoTextures" {
	Properties {
		_MainTex ("Base Texture", 2D) = "white" {}
		_MultiplyTex ("Multiply Texture", 2D) = "white" {}
	}

	// CGVertexLit http://wiki.unity3d.com/index.php/CGVertexLit

	SubShader {
		Cull Off
		//Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
		Tags{ "RenderType" = "Opaque" }
		LOD 200

		Pass {

		CGPROGRAM
		#pragma vertex vert
		#pragma fragment frag

		#include "UnityCG.cginc"

		sampler2D _MainTex;
		sampler2D _MultiplyTex;

		struct v2f {
			float4 pos : SV_POSITION;
			float2 uv : TEXCOORD0;
		};

		float4 _MainTex_ST;

		v2f vert(appdata_base v) {
			v2f o;
			o.pos = UnityObjectToClipPos(v.vertex);
			o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
			return o;
		}

		fixed4 frag(v2f i) : SV_Target {
			fixed4 mainColor = tex2D(_MainTex, i.uv);
			fixed4 secondColor = tex2D(_MultiplyTex, i.uv);
			fixed4 finalColor = mainColor * secondColor;
			return finalColor;
		}
		ENDCG
		/*
			CGPROGRAM
			//#pragma vertex vert
			//#pragma surface surf Lambert alpha
			#pragma surface surf Lambert alpha

			//#include "UnityCG.cginc"

			sampler2D _MainTex;
			sampler2D _MultiplyTex;
			fixed4 _Color;

			struct Input {
				float2 uv_MainTex;
				float2 uv_MultiplyTex;
			};

			void surf(Input IN, inout SurfaceOutput o) {
				fixed4 mainColor = tex2D(_MainTex, IN.uv_MainTex) * _Color;
				//fixed4 secondColor = tex2D(_MultiplyTex, IN.uv_MultiplyTex) * _Color;
				//fixed4 finalColor = secondColor;// mainColor * secondColor;
				//o.Albedo = (c.r/10) + (c.b/10) + (c.g/10);
				//o.Albedo = c.rgb + (_Tint.rgb * _Tint.a);
				o.Albedo = mainColor.rgb;
				o.Alpha = mainColor.a;
			}

			ENDCG
				*/
		}
	}

	//Fallback "Transparent/Diffuse"
}

/*
Shader "Endless/MultiplyTwoTextures" {
	Properties {
		_MainTex("Background Texture", 2D) = "white" {}
		_SecondTexture("Multiply Texture", 2D) = "white" {}
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 100

		Pass {
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				// sample the texture
				fixed4 col = tex2D(_MainTex, i.uv);
				return col;
			}
			ENDCG
		}
	}
}
*/