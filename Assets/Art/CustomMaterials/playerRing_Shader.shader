Shader "Custom/playerRing_Shader" {
	Properties {
		_Tint ("IGNORE: DON'T USE", Color) = (0,0,0,1)
		_MainTex ("Toylight Texture HERE (RGBA)", 2D) = "white" {}
	}
	SubShader {
		Tags 
		{ 
			"RenderType" = "Opaque"
		}
		Pass{
			cull off
			AlphaTest Less 0.5
		}
		
		CGPROGRAM
		#pragma surface surf Lambert alpha

		sampler2D _MainTex;
		float4 _Tint;

		struct Input {
			float2 uv_MainTex;
		};

		void surf (Input IN, inout SurfaceOutput o) {
			half4 c = tex2D (_MainTex, IN.uv_MainTex);
			
			if (c.a > 0.95) 
			{
				o.Emission = (1,1,1);
				o.Alpha = c.a;
			}
			else 
			{
				o.Emission = _Tint.rgb;
				o.Alpha = c.a * _Tint.a;
			}
		}
		ENDCG
	} 
	FallBack "Diffuse"
}
