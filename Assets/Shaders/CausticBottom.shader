Shader "Custom/CausticBottom"
{
    Properties
    {
        _MainTex ("Main Texture", 2D) = "white" {}
        _Voronoi1 ("Voronoi Texture 1", 2D) = "white" {}
        _Voronoi2 ("Voronoi Texture 2", 2D) = "white" {}
        _GradientCircle ("Gradient Circle Texture", 2D) = "white" {}
        _MainColor ("Main Color", Color) = (0.0, 0.5, 0.7, 1.0)
        _SecondaryColor ("Secondary Color", Color) = (0.0, 0.3, 0.5, 1.0)
        _GradientDarkColor ("Gradient Dark Color", Color) = (0.0, 0.2, 0.3, 1.0)
        _SunIntensity ("Sun Intensity", Range(0, 1)) = 0.5
        _Speed ("Animation Speed", Float) = 0.5
        _Brightness ("Brightness", Range(0, 2)) = 1.0
        _SmoothMin ("Smoothstep Min", Range(0, 1)) = 0.1
        _SmoothMax ("Smoothstep Max", Range(0, 1)) = 0.2
        _GradientAlpha ("Gradient Alpha", Range(0, 1)) = 1.0
    }

    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        LOD 100

        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows

        sampler2D _MainTex;
        sampler2D _Voronoi1;
        sampler2D _Voronoi2;
        sampler2D _GradientCircle;
        float4 _MainColor;
        float4 _SecondaryColor;
        float4 _GradientDarkColor;
        float _SunIntensity;
        float _Speed;
        float _Brightness;
        float _SmoothMin;
        float _SmoothMax;
        float _GradientAlpha;

        struct Input
        {
            float2 uv_MainTex;
            float2 uv_GradientCircle;
        };

        void surf(Input IN, inout SurfaceOutputStandard o)
        {
            // Sample the main texture
            float4 mainTex = tex2D(_MainTex, IN.uv_MainTex);

            // Animate the Voronoi textures by offsetting UVs with time
            float2 uv1 = IN.uv_MainTex + _Speed * _Time.y;
            float2 uv2 = IN.uv_MainTex - _Speed * _Time.y;

            // Sample Voronoi textures
            float4 voronoi1 = tex2D(_Voronoi1, uv1);
            float4 voronoi2 = tex2D(_Voronoi2, uv2);

            // Calculate intersection mask based on the similarity of the Voronoi textures
            float intersection = smoothstep(_SmoothMin, _SmoothMax, abs(voronoi1.r - voronoi2.r));

            // Adjust the glitter effect by masking with the intersection and applying brightness
            float glitter = intersection * _SunIntensity * _Brightness;

            // Mix colors based on the glitter effect
            float4 finalColor = lerp(_MainColor, _SecondaryColor, glitter);

            // Calculate custom UV for the gradient circle texture
            float2 gradientUV = IN.uv_GradientCircle;

            // Sample the gradient circle texture with the calculated UVs
            float4 gradientCircle = tex2D(_GradientCircle, gradientUV);

            // Blend the gradient circle dark color with the final color
            float4 gradientBlendedColor = lerp(_GradientDarkColor, finalColor, gradientCircle.r);

            // Apply the gradient alpha to control transparency
            float gradientAlpha = gradientCircle.a * _GradientAlpha;

            // Apply the gradient circle effect to the final color for depth effect
            o.Albedo = gradientBlendedColor.rgb * mainTex.rgb;
            o.Alpha = mainTex.a * gradientAlpha;
        }
        ENDCG
    }

    FallBack "Diffuse"
}
