Shader "Endless/Transparent Diffuse with Tint" {
Properties {
	_Color ("Main Color", Color) = (1,1,1,1)
	_Tint ("Tint", Color) = (1,1,1,0)
	_MainTex ("Base (RGB) Trans (A)", 2D) = "white" {}
}

// CGVertexLit http://wiki.unity3d.com/index.php/CGVertexLit

SubShader {
	Cull Off
	Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
	LOD 200

	CGPROGRAM
	//#pragma vertex vert
	#pragma surface surf Lambert alpha
	
	sampler2D _MainTex;
	fixed4 _Color;
	fixed4 _Tint;
	
	struct Input {
		float2 uv_MainTex;
	};
	
	void surf (Input IN, inout SurfaceOutput o) {
		fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
		//o.Albedo = (c.r/10) + (c.b/10) + (c.g/10);
		//o.Albedo = c.rgb + (_Tint.rgb * _Tint.a);
		o.Albedo = (_Tint.rgb * _Tint.a) + c.rgb;
		o.Alpha = c.a;
	}

	ENDCG
}

Fallback "Transparent/Diffuse"
}