// Upgrade NOTE: commented out 'float4 unity_LightmapST', a built-in variable
// Upgrade NOTE: commented out 'sampler2D unity_Lightmap', a built-in variable
// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'
// Upgrade NOTE: replaced tex2D unity_Lightmap with UNITY_SAMPLE_TEX2D

Shader "VertexLit CG" {
Properties {
	_Color ("Main Color", Color) = (1,1,1,1)
	_Tint ("Tint", Color) = (0,0,0,0)
	_MainTex ("Base (RGB) Trans (A)", 2D) = "white" {}
}
 
SubShader {
	Tags {"Queue"="Transparent"  "IgnoreProjector"="True" "RenderType" = "Transparent" }
	LOD 100
 
	ZWrite Off
	Blend SrcAlpha OneMinusSrcAlpha
 
	Pass {
		Tags { LightMode = Vertex } 
		CGPROGRAM
		#pragma vertex vert Lambert alpha
		#pragma fragment frag Lambert alpha
 
		#include "UnityCG.cginc"
 
		fixed4 _Color;
		fixed4 _Tint;
 
		sampler2D _MainTex;
		float4 _MainTex_ST;
 
		struct v2f {
			float4 pos : SV_POSITION;
			float2 uv_MainTex : TEXCOORD0;
			fixed3 diff : COLOR;
		};
 
		v2f vert (appdata_full v)
		{
		    v2f o;
		    o.pos = UnityObjectToClipPos (v.vertex);
		    o.uv_MainTex = TRANSFORM_TEX (v.texcoord, _MainTex);
 
			o.diff = 1;
 
			o.diff = (o.diff * _Color + _Tint.rgb) * 2;
 
			return o;
		}
 
		fixed4 frag (v2f i) : COLOR {
			/*fixed4 c;
 
			fixed4 mainTex = tex2D (_MainTex, i.uv_MainTex);
 
			c.rgb = (_Tint.rgb * _Tint.a) + c.rgb; (mainTex.rgb * i.diff);
 
			c.a = 1;*/

			fixed4 c = tex2D(_MainTex, i.uv_MainTex) * _Color;
			c.rgb = (_Tint.rgb * _Tint.a) + c.rgb;
			c.a = c.a;
 
			return c;
		}
 
		ENDCG
	}
 /*
	//Lightmap pass, dLDR;
	Pass {
		Tags { "LightMode" = "VertexLM" }
 
		CGPROGRAM
		#pragma vertex vert  
		#pragma fragment frag
 
		#include "UnityCG.cginc"
 
		// float4 unity_LightmapST;
		// sampler2D unity_Lightmap;
 
		struct v2f {
			float4 pos : SV_POSITION;
			float2 lmap : TEXCOORD0;
		};
 
		v2f vert (appdata_full v)
		{
		    v2f o;
		    o.pos = UnityObjectToClipPos (v.vertex);
 
		    o.lmap.xy = v.texcoord1.xy * unity_LightmapST.xy + unity_LightmapST.zw;
 
		    return o;
		 }
 
		fixed4 frag (v2f i) : COLOR {
			fixed4 lmtex = UNITY_SAMPLE_TEX2D(unity_Lightmap, i.lmap.xy);
			fixed3 lm = (8.0 * lmtex.a) * lmtex.rgb;
			return fixed4(lm, 1);
		}
 
		ENDCG
	}
 
	//Lightmap pass, RGBM;
	Pass {
		Tags { "LightMode" = "VertexLMRGBM" }
 
		CGPROGRAM
		#pragma vertex vert  
		#pragma fragment frag
 
		#include "UnityCG.cginc"
 
		// float4 unity_LightmapST;
		// sampler2D unity_Lightmap;
 
		struct v2f {
			float4 pos : SV_POSITION;
			float2 lmap : TEXCOORD0;
		};
 
		v2f vert (appdata_full v)
		{
		    v2f o;
		    o.pos = UnityObjectToClipPos (v.vertex);
 
		    o.lmap.xy = v.texcoord1.xy * unity_LightmapST.xy + unity_LightmapST.zw;
 
		    return o;
		 }
 
		fixed4 frag (v2f i) : COLOR {
			fixed4 lmtex = UNITY_SAMPLE_TEX2D(unity_Lightmap, i.lmap.xy);
			fixed3 lm = (8.0 * lmtex.a) * lmtex.rgb;
			return fixed4(lm, 1);
		}
 
		ENDCG
	}
			*/
}
 
Fallback "VertexLit"
}