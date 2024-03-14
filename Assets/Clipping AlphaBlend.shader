Shader "Stencil/Clipping AlphaBlend"
{
    Properties
    {
        _Color("2X Color (RGBA Mul)", Color) = (0.5, 0.5, 0.5, 1.0)
        _MainTex("Main Texture (RGBA)", 2D) = "white" {}
    }

    SubShader
    {
        Tags { "RenderType" = "Transparent" "Queue" = "Transparent" "PreviewType" = "Plane" }
        Blend SrcAlpha OneMinusSrcAlpha
        LOD 200
    
        /*Stencil
        {
        Ref 1
        Comp LEqual
        }*/

        CGPROGRAM

        #pragma surface surf SimpleColor alpha
        #pragma target 3.0
        half4 LightingSimpleColor(SurfaceOutput s, half3 lightDir, half atten)
        {
            half4 c;
            c.rgb = s.Albedo;
            c.a = s.Alpha;
            return c;
        }

        half4 _Color;
        sampler2D _MainTex;

        struct Input
        {
            float2 uv_MainTex;
            float4 color : COLOR;
        };

        void surf(Input IN, inout SurfaceOutput o)
        {
        half4 c = tex2D(_MainTex, IN.uv_MainTex);

        c.rgb *= _Color.rgb * 2.0f;
        o.Alpha = c.a * _Color.a;
        o.Albedo = c.rgb;
        }
        ENDCG
    }
}