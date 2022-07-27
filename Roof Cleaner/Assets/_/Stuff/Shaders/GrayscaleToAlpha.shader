Shader "Endless/GrayscaleToAlpha" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
	}
	SubShader {
		Tags{ "Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent" }
		LOD 200
		
		CGPROGRAM
		#pragma surface surf Lambert alpha

		sampler2D _MainTex;

		struct Input {
			float2 uv_MainTex;
		};

		void surf (Input IN, inout SurfaceOutput o) {
			half4 c = tex2D (_MainTex, IN.uv_MainTex);
			//c.r = .5;
			//c.g = 1;
			//c.b = 1;
			//c.a = 0;
			o.Albedo = 0.2;// c.rgb;
			o.Alpha = 1-c.g;// 1 - c.r;
		}
		ENDCG
	} 
	FallBack "Diffuse"
}
