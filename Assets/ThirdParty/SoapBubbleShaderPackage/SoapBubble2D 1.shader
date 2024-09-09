Shader "Custom/SoapBubble2DEdit"
{
    Properties
    {
        _MainTex ("Main Texture", 2D) = "white" {}
        _ReflectionTex ("Reflection Map", 2D) = "white" {}
        _RefractionTex ("Refraction Map", 2D) = "white" {}
        _NormalMap ("Normal Map", 2D) = "bump" {}
        _EmissionMap ("Emission Map", 2D) = "white" {}
        _NoiseRefractionTex ("Noise Refraction Map", 2D) = "white" {}

        _MainColor ("Main Color", Color) = (1, 1, 1, 1)
        _EmissionColor ("Emission Color", Color) = (1, 1, 1, 1)
        _OutlineColor ("Outline Color", Color) = (0.5, 0.5, 1, 1)
        _NoiseRefractionColor ("Noise Refraction Color", Color) = (0.8, 0.9, 1, 1)

        _MainTex_ST ("Main Tex Tiling", Vector) = (1, 1, 0, 0)
        _ReflectionTex_ST ("Reflection Map Tiling", Vector) = (1, 1, 0, 0)
        _RefractionTex_ST ("Refraction Map Tiling", Vector) = (1, 1, 0, 0)
        _NormalMap_ST ("Normal Map Tiling", Vector) = (1, 1, 0, 0)
        _NoiseRefractionTex_ST ("Noise Refraction Map Tiling", Vector) = (1, 1, 0, 0)

        _Transparency ("Transparency", Range(0, 1)) = 0.5
        _ReflectionStrength ("Reflection Strength", Range(0, 1)) = 0.8
        _RefractionStrength ("Refraction Strength", Range(0, 1)) = 0.5
        _OutlineThickness ("Outline Thickness", Range(0, 0.05)) = 0.02
        _ToonThreshold ("Toon Threshold", Range(0, 1)) = 0.5
        _NoiseSpeed ("Noise Animation Speed", Range(0, 5)) = 1.0
        _NoiseStrength ("Noise Refraction Strength", Range(0, 1)) = 0.5
        _Shininess ("Shininess", Range(0.03, 1)) = 0.078125
        _SpecColor ("Specular Color", Color) = (1, 1, 1, 1)
    }

    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        LOD 200

        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            #include "Lighting.cginc" // Include Unity's lighting functions

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float2 uv_Main : TEXCOORD0;
                float2 uv_Reflection : TEXCOORD1;
                float2 uv_Refraction : TEXCOORD2;
                float2 uv_Normal : TEXCOORD3;
                float2 uv_NoiseRefraction : TEXCOORD4;
                float3 worldPos : TEXCOORD5;
                float3 worldNormal : TEXCOORD6;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            sampler2D _ReflectionTex;
            sampler2D _RefractionTex;
            sampler2D _NormalMap;
            sampler2D _EmissionMap;
            sampler2D _NoiseRefractionTex;

            float4 _MainTex_ST;
            float4 _ReflectionTex_ST;
            float4 _RefractionTex_ST;
            float4 _NormalMap_ST;
            float4 _NoiseRefractionTex_ST;

            float4 _MainColor;
            float4 _EmissionColor;
            float4 _OutlineColor;
            float4 _NoiseRefractionColor;
            float _Transparency;
            float _ReflectionStrength;
            float _RefractionStrength;
            float _OutlineThickness;
            float _ToonThreshold;
            float _NoiseSpeed;
            float _NoiseStrength;
            float _Shininess;
            //float4 _SpecColor;

            v2f vert (appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);

                o.uv_Main = TRANSFORM_TEX(v.uv, _MainTex);
                o.uv_Reflection = TRANSFORM_TEX(v.uv, _ReflectionTex);
                o.uv_Refraction = TRANSFORM_TEX(v.uv, _RefractionTex);
                o.uv_Normal = TRANSFORM_TEX(v.uv, _NormalMap);
                o.uv_NoiseRefraction = TRANSFORM_TEX(v.uv, _NoiseRefractionTex);

                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                o.worldNormal = mul((float3x3)unity_ObjectToWorld, v.normal);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // Normal mapping
                float3 normalMap = UnpackNormal(tex2D(_NormalMap, i.uv_Normal));
                float3 worldNormal = normalize(i.worldNormal + normalMap);

                // View direction
                float3 viewDir = normalize(_WorldSpaceCameraPos - i.worldPos);

                // Lighting calculation (Blinn-Phong using Unity's macros)
                float3 lightDir = normalize(_WorldSpaceLightPos0.xyz);
                float3 halfDir = normalize(lightDir + viewDir);
                float3 ambient = UNITY_LIGHTMODEL_AMBIENT.rgb;
                float3 diffuse = _LightColor0.rgb * max(0.0, dot(worldNormal, lightDir));
                float3 specular = _SpecColor.rgb * pow(max(0.0, dot(worldNormal, halfDir)), _Shininess * 128.0);

                // Reflection and refraction calculation
                float3 reflectionDir = reflect(-viewDir, worldNormal);
                fixed4 reflection = tex2D(_ReflectionTex, reflectionDir.xy * 0.5 + 0.5);

                float3 refractionDir = refract(-viewDir, worldNormal, 1.0 / 1.33);
                fixed4 refraction = tex2D(_RefractionTex, refractionDir.xy * 0.5 + 0.5);

                // Noisy refraction
                float2 noiseUV = i.uv_NoiseRefraction;
                noiseUV += _Time.y * _NoiseSpeed;
                fixed4 noisyRefraction = tex2D(_NoiseRefractionTex, noiseUV) * _NoiseRefractionColor;
                refraction = lerp(refraction, noisyRefraction, _NoiseStrength);

                // Main texture and emission
                fixed4 baseColor = tex2D(_MainTex, i.uv_Main) * _MainColor;
                fixed4 emission = tex2D(_EmissionMap, i.uv_Main) * _EmissionColor;

                // Combine reflection and refraction
                fixed4 combinedColor = lerp(refraction, reflection, _ReflectionStrength) * _RefractionStrength;

                // Toon shading effect
                float intensity = dot(worldNormal, viewDir);
                float toonShade = step(_ToonThreshold, intensity);
                combinedColor.rgb = lerp(combinedColor.rgb, _OutlineColor.rgb, toonShade);

                // Final color combination
                fixed4 finalColor = (combinedColor + emission) * baseColor;
                finalColor.rgb += (ambient + diffuse + specular); // Add lighting

                // Edge outline effect
                float edgeFactor = pow(saturate(1.0 - dot(viewDir, worldNormal)), _OutlineThickness * 100.0);
                finalColor.rgb = lerp(finalColor.rgb, _OutlineColor.rgb, edgeFactor);

                // Apply transparency
                finalColor.a *= _Transparency;

                return finalColor;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}
