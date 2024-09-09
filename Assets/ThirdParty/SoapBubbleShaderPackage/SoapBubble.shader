Shader "Custom/SoapBubble"
{
    /* This shader creates a bubble out of any object having a material using this shader.#
    * 
    * Author: Kristyna Pavlatova
    * Linkedin: https://www.linkedin.com/in/kristyna-pavlatova/
    * This shader was created for a school course "Personal Portfolio" at Saxion University (NL). Student number 475956.
    * The author allows to use this shader only for personal use for your personal education.
    */
    Properties
    {
        _Cubemap("Cubemap", CUBE) = "" {}
        _AlphaCubemap("Alpha Cubemap", CUBE) = "" {}
        _AlphaCubemapColor("Alpha Cubemap Color", Color) = (1.0,0.8829297,0.0,0)

        _RimColor("Rim Color", Color) = (0,0.8237238,1.0,0)
        _RimPower("Rim Power", Range(0.5, 8.0)) = 1.2
    }
    SubShader
    {
        Tags
        {
            "Queue" = "Transparent"
            "RenderType" = "Transparent"
        }

        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows alpha:fade

        struct Input
        {
            float3 worldRefl;
            float3 viewDir;
        };

        samplerCUBE _Cubemap;
        samplerCUBE _AlphaCubemap;
        fixed4 _AlphaCubemapColor;

        fixed4 _RimColor;
        float _RimPower;

        void surf(Input IN, inout SurfaceOutputStandard o)
        {
            float cubemap = texCUBE(_Cubemap, IN.worldRefl);//environment reflection
            float3 aplhaCubemap = texCUBE(_AlphaCubemap, IN.worldRefl);//"environment light" reflection
            o.Albedo = cubemap * _AlphaCubemapColor;
            o.Alpha = aplhaCubemap;

            //Rim lighting
            half dotp = dot(normalize(IN.viewDir), o.Normal);
            half rim = 1 - saturate(dotp);
            o.Emission = _RimColor * pow(rim, _RimPower);
        }
        ENDCG
    }
     Fallback "Diffuse"
}