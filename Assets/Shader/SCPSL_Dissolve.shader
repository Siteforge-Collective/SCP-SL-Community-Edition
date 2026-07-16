Shader "SCPSL/Dissolve_VertexFragment"
{
    Properties
    {
        _Cutoff ("Mask Clip Value", Float) = 0.5
        _MainTex ("Main Texture", 2D) = "white" {}
        _Color ("Color", Color) = (1,1,1,1)
        _BumpMap ("Normal Map", 2D) = "bump" {}
        _BumpScale ("Bump Scale", Float) = 1.0
        [Toggle(_USEMETALLICTEXTURE3_ON)] _UseMetallicTexture3 ("Use Metallic Texture", Float) = 0
        _MetallicGlossMap ("Metallic Gloss Map", 2D) = "white" {}
        _Metallic ("Metallic", Range(0, 1)) = 0
        _Glossiness ("Glossiness", Range(0, 1)) = 0.5
        _GlossMapScale ("Gloss Map Scale", Range(0, 1)) = 0.5
        _DisolveGuide ("Dissolve Guide", 2D) = "white" {}
        _Disintegrate ("Disintegrate", Range(0, 1)) = 0
        _BurnRamp ("Burn Ramp", 2D) = "white" {}
        [Toggle(_USEEMISSION1_ON)] _UseEmission1 ("Use Emission", Float) = 0
        [HDR] _EmissionColor ("Emission Color", Color) = (0,0,0,1)
        _EmissionMap ("Emission Map", 2D) = "white" {}
    }
    
    SubShader
    {
        Tags { "RenderType"="TransparentCutout" "Queue"="AlphaTest" "IsEmissive"="true" }
        LOD 300
        Cull Off
        
        Pass
        {
            Name "FORWARD"
            Tags { "LightMode"="ForwardBase" }
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fwdbase
            #pragma multi_compile_fog
            #pragma shader_feature _USEMETALLICTEXTURE3_ON
            #pragma shader_feature _USEEMISSION1_ON
            #pragma target 3.0
            
            #include "UnityCG.cginc"
            #include "Lighting.cginc"
            #include "AutoLight.cginc"
            #include "UnityPBSLighting.cginc"
            
            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
                float2 uv : TEXCOORD0;
            };
            
            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 worldNormal : TEXCOORD1;
                float3 worldPos : TEXCOORD2;
                float4 tangent : TEXCOORD3;
                float4 bitangent : TEXCOORD4;
                SHADOW_COORDS(5)
                UNITY_FOG_COORDS(6)
            };
            
            sampler2D _MainTex; float4 _MainTex_ST;
            sampler2D _BumpMap; float4 _BumpMap_ST;
            sampler2D _MetallicGlossMap; float4 _MetallicGlossMap_ST;
            sampler2D _DisolveGuide; float4 _DisolveGuide_ST;
            sampler2D _BurnRamp;
            sampler2D _EmissionMap; float4 _EmissionMap_ST;
            
            float4 _Color;
            float4 _EmissionColor;
            float _Cutoff;
            float _Disintegrate;
            float _Metallic;
            float _Glossiness;
            float _GlossMapScale;
            float _BumpScale;
            
            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                o.worldNormal = UnityObjectToWorldNormal(v.normal);
                o.tangent = float4(UnityObjectToWorldDir(v.tangent.xyz), v.tangent.w);
                float3 binormal = cross(v.normal, v.tangent.xyz) * v.tangent.w;
                o.bitangent = float4(UnityObjectToWorldDir(binormal), 0);
                TRANSFER_SHADOW(o);
                UNITY_TRANSFER_FOG(o, o.pos);
                return o;
            }
            
            fixed4 frag (v2f i) : SV_Target
            {
                float2 uvMain = i.uv * _MainTex_ST.xy + _MainTex_ST.zw;
                float2 uvBump = i.uv * _BumpMap_ST.xy + _BumpMap_ST.zw;
                float2 uvDissolve = i.uv * _DisolveGuide_ST.xy + _DisolveGuide_ST.zw;
                
                fixed4 mainColor = tex2D(_MainTex, uvMain) * _Color;
                
                // Normal
                fixed3 normal = UnpackNormal(tex2D(_BumpMap, uvBump));
                normal.xy *= _BumpScale;
                float3 worldNormal = normalize(
                    normal.x * i.tangent.xyz + 
                    normal.y * i.bitangent.xyz + 
                    normal.z * i.worldNormal
                );
                
                // Dissolve
                float dissolveValue = tex2D(_DisolveGuide, uvDissolve).r;
                float dissolveThreshold = (1.0 - _Disintegrate) * 1.2 - 0.6;
                float dissolveEdge = saturate((dissolveValue + dissolveThreshold) * 8.0 - 4.0);
                clip(dissolveValue + dissolveThreshold - _Cutoff);
                
                // Burn
                float3 burnColor = tex2D(_BurnRamp, float2(1.0 - dissolveEdge, 0.0)).rgb;
                float3 albedo = mainColor.rgb + burnColor * (1.0 - dissolveEdge);
                
                // Metallic/Gloss
                float metallic = _Metallic;
                float smoothness = _Glossiness;
                #ifdef _USEMETALLICTEXTURE3_ON
                    float2 uvMetallic = i.uv * _MetallicGlossMap_ST.xy + _MetallicGlossMap_ST.zw;
                    fixed4 mg = tex2D(_MetallicGlossMap, uvMetallic);
                    metallic = mg.r * _Metallic;
                    smoothness = mg.a * _GlossMapScale;
                #endif
                
                // Emission
                float3 emission = burnColor * (1.0 - dissolveEdge);
                #ifdef _USEEMISSION1_ON
                    float2 uvEmission = i.uv * _EmissionMap_ST.xy + _EmissionMap_ST.zw;
                    emission += tex2D(_EmissionMap, uvEmission).rgb * _EmissionColor.rgb;
                #endif
                
                // PBS
                float3 worldViewDir = normalize(_WorldSpaceCameraPos - i.worldPos);
                float3 worldLightDir = normalize(_WorldSpaceLightPos0.xyz);
                
                UnityGI gi;
                UNITY_INITIALIZE_OUTPUT(UnityGI, gi);
                gi.indirect.diffuse = 0;
                gi.indirect.specular = 0;
                #if !defined(LIGHTMAP_ON)
                    gi.light.color = _LightColor0.rgb;
                    gi.light.dir = worldLightDir;
                    gi.light.ndotl = LambertTerm(worldNormal, gi.light.dir);
                #endif
                
                UnityGIInput giInput;
                UNITY_INITIALIZE_OUTPUT(UnityGIInput, giInput);
                giInput.light = gi.light;
                giInput.worldPos = i.worldPos;
                giInput.worldViewDir = worldViewDir;
                giInput.atten = SHADOW_ATTENUATION(i);
                #if defined(LIGHTMAP_ON) || defined(DYNAMICLIGHTMAP_ON)
                    giInput.lightmapUV = i.uv;
                #else
                    giInput.lightmapUV = 0.0;
                #endif
                
                SurfaceOutputStandard o;
                UNITY_INITIALIZE_OUTPUT(SurfaceOutputStandard, o);
                o.Albedo = albedo;
                o.Normal = worldNormal;
                o.Emission = emission;
                o.Metallic = metallic;
                o.Smoothness = smoothness;
                o.Occlusion = 1.0;
                o.Alpha = mainColor.a;
                
                LightingStandard_GI(o, giInput, gi);
                fixed4 c = LightingStandard(o, worldViewDir, gi);
                
                UNITY_APPLY_FOG(i.fogCoord, c);
                return c;
            }
            ENDCG
        }
    }
    
    FallBack "Standard"
}