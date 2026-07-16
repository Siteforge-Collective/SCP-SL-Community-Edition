Shader "Rollthered/Fire"
{
    Properties
    {
        _FresnelPower ("Fresnel Power", Range(0, 5)) = 2
        [HideInInspector] _FresnelScale ("Fresnel Scale", Range(0, 0.3)) = 1.5
        _EdgeLength ("Edge length", Range(2, 50)) = 13.5
        _FresnelBias ("Fresnel Bias", Range(0, 0.2)) = 0.2364706
        [HDR] _Flamecolor2 ("Flame color 2", Color) = (1,0,0,0)
        [HDR] _FlameColor ("Flame Color", Color) = (1,0.8068966,0,0)
        _Y_Mask ("Y_Mask", Range(0, 5)) = 0
        _FlameHeight ("Flame Height", Range(0, 1)) = 0
        _Flamenoise ("Flame noise", 2D) = "white" {}
        _FlameWave ("Flame Wave", 2D) = "white" {}
        _v ("v", Range(-1, 1)) = 0
        _u ("u", Range(-1, 1)) = 0
        _Alpha ("Alpha", Range(0, 1)) = 0
        [HideInInspector] __dirty ("", Float) = 1
    }

    SubShader
    {
        Tags 
        { 
            "IgnoreProjector" = "true" 
            "IsEmissive" = "true" 
            "Queue" = "Transparent+100" 
            "RenderType" = "Transparent" 
        }

        Pass
        {
            Name "FORWARD"
            Tags { "LightMode" = "ForwardBase" }

            Blend SrcAlpha OneMinusSrcAlpha
            ColorMask RGB
            ZWrite Off
            Cull Front

            CGPROGRAM
            #pragma vertex tessvert
            #pragma fragment frag
            #pragma hull hull
            #pragma domain domain
            #pragma target 5.0
            
            #pragma multi_compile_fwdbase
            #pragma multi_compile_fog

            #include "UnityCG.cginc"
            #include "UnityLightingCommon.cginc"
            #include "AutoLight.cginc"

            float _FresnelPower;
            float _FresnelScale;
            float _FresnelBias;
            float _Y_Mask;
            float _Alpha;
            float4 _Flamecolor2;
            float4 _FlameColor;
            float _EdgeLength;

            struct TessVertex 
            {
                float4 vertex : INTERNALTESSPOS;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
                float4 texcoord : TEXCOORD0;
                float4 texcoord1 : TEXCOORD1;
                float4 texcoord2 : TEXCOORD2;
                float4 color : COLOR;
            };

            struct OutputPatchConstant 
            {
                float edge[3] : SV_TessFactor;
                float inside : SV_InsideTessFactor;
            };

            struct v2f 
            {
                float4 pos : SV_POSITION;
                float3 worldNormal : NORMAL;
                float3 worldPos : TEXCOORD0;
                float3 normal : TEXCOORD1;
                float2 uv : TEXCOORD2;
                float4 color : TEXCOORD3;
                UNITY_FOG_COORDS(4)
            };

            float Tessellation(TessVertex v0, TessVertex v1, float edgeLength)
            {
                float3 v0world = mul(unity_ObjectToWorld, v0.vertex).xyz;
                float3 v1world = mul(unity_ObjectToWorld, v1.vertex).xyz;
                float edgeLen = distance(v0world, v1world);
                return max(edgeLen / edgeLength, 1.0);
            }

            OutputPatchConstant hullconst(InputPatch<TessVertex, 3> v) 
            {
                OutputPatchConstant o;
                o.edge[0] = Tessellation(v[1], v[2], _EdgeLength);
                o.edge[1] = Tessellation(v[2], v[0], _EdgeLength);
                o.edge[2] = Tessellation(v[0], v[1], _EdgeLength);
                o.inside = (o.edge[0] + o.edge[1] + o.edge[2]) / 3.0;
                return o;
            }

            [domain("tri")]
            [partitioning("fractional_odd")]
            [outputtopology("triangle_cw")]
            [outputcontrolpoints(3)]
            [patchconstantfunc("hullconst")]
            TessVertex hull(InputPatch<TessVertex, 3> v, uint id : SV_OutputControlPointID) 
            {
                return v[id];
            }

            v2f vert(appdata_full v) 
            {
                v2f o = (v2f)0;         
                o.pos = UnityObjectToClipPos(v.vertex);
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                o.worldNormal = UnityObjectToWorldNormal(v.normal);
                o.normal = v.normal;
                o.uv = v.texcoord;
                o.color = v.color;
                UNITY_TRANSFER_FOG(o, o.pos);
                return o;
            }

            TessVertex tessvert(appdata_full v) 
            {
                TessVertex o;
                o.vertex = v.vertex;
                o.normal = v.normal;
                o.tangent = v.tangent;
                o.texcoord = v.texcoord;
                o.texcoord1 = v.texcoord1;
                o.texcoord2 = v.texcoord2;
                o.color = v.color;
                return o;
            }

            [domain("tri")]
            v2f domain(OutputPatchConstant tessFactors, const OutputPatch<TessVertex, 3> vi, float3 bary : SV_DomainLocation) 
            {
                appdata_full v = (appdata_full)0;
                v.vertex = vi[0].vertex * bary.x + vi[1].vertex * bary.y + vi[2].vertex * bary.z;
                v.normal = vi[0].normal * bary.x + vi[1].normal * bary.y + vi[2].normal * bary.z;
                v.tangent = vi[0].tangent * bary.x + vi[1].tangent * bary.y + vi[2].tangent * bary.z;
                v.texcoord = vi[0].texcoord * bary.x + vi[1].texcoord * bary.y + vi[2].texcoord * bary.z;
                v.texcoord1 = vi[0].texcoord1 * bary.x + vi[1].texcoord1 * bary.y + vi[2].texcoord1 * bary.z;
                v.texcoord2 = vi[0].texcoord2 * bary.x + vi[1].texcoord2 * bary.y + vi[2].texcoord2 * bary.z;
                v.color = vi[0].color * bary.x + vi[1].color * bary.y + vi[2].color * bary.z;
                return vert(v);
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float3 viewDir = normalize(_WorldSpaceCameraPos - i.worldPos);
                float3 worldNormal = normalize(i.worldNormal);
                
                float fresnel = 1.0 - saturate(dot(worldNormal, viewDir));
                fresnel = pow(fresnel, _FresnelPower);
                fresnel = _FresnelScale * fresnel + _FresnelBias;
                
                float yMask = abs(i.normal.y - _Y_Mask) - _Y_Mask;
                yMask = saturate(yMask);
                yMask = 1.0 - yMask;
                fresnel *= yMask;
                
                float3 flameColor = lerp(_Flamecolor2.rgb, _FlameColor.rgb, fresnel);
                flameColor = max(flameColor, 0.0);
                flameColor = min(flameColor, 5.0);
                
                float3 reflectDir = reflect(-viewDir, worldNormal);
                float4 envColor = UNITY_SAMPLE_TEXCUBE(unity_SpecCube0, reflectDir);
                envColor.rgb = DecodeHDR(envColor, unity_SpecCube0_HDR);
                
                float3 lightDir = normalize(_WorldSpaceLightPos0.xyz);
                float NdotL = saturate(dot(worldNormal, lightDir));
                
                float3 halfDir = normalize(lightDir + viewDir);
                float NdotH = saturate(dot(worldNormal, halfDir));
                
                float specAngle = max(NdotH, 0.0);
                float specular = pow(specAngle, 1.0) * NdotL;
                
                float3 lightColor = _LightColor0.rgb * NdotL;
                
                float4 finalColor;
                finalColor.rgb = flameColor + (lightColor * 0.5) + (envColor.rgb * 0.5 * fresnel);
                finalColor.a = fresnel * _Alpha;
                
                UNITY_APPLY_FOG(i.fogCoord, finalColor);
                
                return finalColor;
            }
            ENDCG
        }

        Pass
        {
            Name "FORWARD_ADD"
            Tags { "LightMode" = "ForwardAdd" }

            Blend SrcAlpha One
            ColorMask RGB
            ZWrite Off
            Cull Front
            ZTest LEqual

            CGPROGRAM
            #pragma vertex tessvert_add
            #pragma fragment frag_add
            #pragma hull hull_add
            #pragma domain domain_add
            #pragma target 5.0
            
            #pragma multi_compile_fwdadd
            #pragma multi_compile_fog

            #include "UnityCG.cginc"
            #include "UnityLightingCommon.cginc"
            #include "AutoLight.cginc"

            float _FresnelPower;
            float _FresnelScale;
            float _FresnelBias;
            float _Y_Mask;
            float _Alpha;
            float _EdgeLength;

            struct TessVertex 
            {
                float4 vertex : INTERNALTESSPOS;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
                float4 texcoord : TEXCOORD0;
                float4 texcoord1 : TEXCOORD1;
                float4 texcoord2 : TEXCOORD2;
                float4 color : COLOR;
            };

            struct OutputPatchConstant 
            {
                float edge[3] : SV_TessFactor;
                float inside : SV_InsideTessFactor;
            };

            struct v2f_add 
            {
                float4 pos : SV_POSITION;
                float3 worldNormal : NORMAL;
                float3 worldPos : TEXCOORD0;
                float3 normal : TEXCOORD1;
                float4 color : TEXCOORD2;
                UNITY_FOG_COORDS(3)
                UNITY_LIGHTING_COORDS(4, 5)
            };

            float Tessellation(TessVertex v0, TessVertex v1, float edgeLength)
            {
                float3 v0world = mul(unity_ObjectToWorld, v0.vertex).xyz;
                float3 v1world = mul(unity_ObjectToWorld, v1.vertex).xyz;
                float edgeLen = distance(v0world, v1world);
                return max(edgeLen / edgeLength, 1.0);
            }

            OutputPatchConstant hullconst_add(InputPatch<TessVertex, 3> v) 
            {
                OutputPatchConstant o;
                o.edge[0] = Tessellation(v[1], v[2], _EdgeLength);
                o.edge[1] = Tessellation(v[2], v[0], _EdgeLength);
                o.edge[2] = Tessellation(v[0], v[1], _EdgeLength);
                o.inside = (o.edge[0] + o.edge[1] + o.edge[2]) / 3.0;
                return o;
            }

            [domain("tri")]
            [partitioning("fractional_odd")]
            [outputtopology("triangle_cw")]
            [outputcontrolpoints(3)]
            [patchconstantfunc("hullconst_add")]
            TessVertex hull_add(InputPatch<TessVertex, 3> v, uint id : SV_OutputControlPointID) 
            {
                return v[id];
            }

            v2f_add vert_add(appdata_full v) 
            {
                v2f_add o = (v2f_add)0;   
                o.pos = UnityObjectToClipPos(v.vertex);
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                o.worldNormal = UnityObjectToWorldNormal(v.normal);
                o.normal = v.normal;
                o.color = v.color;
                UNITY_TRANSFER_FOG(o, o.pos);
                TRANSFER_VERTEX_TO_FRAGMENT(o);
                return o;
            }

            TessVertex tessvert_add(appdata_full v) 
            {
                TessVertex o;
                o.vertex = v.vertex;
                o.normal = v.normal;
                o.tangent = v.tangent;
                o.texcoord = v.texcoord;
                o.texcoord1 = v.texcoord1;
                o.texcoord2 = v.texcoord2;
                o.color = v.color;
                return o;
            }

            [domain("tri")]
            v2f_add domain_add(OutputPatchConstant tessFactors, const OutputPatch<TessVertex, 3> vi, float3 bary : SV_DomainLocation) 
            {
                appdata_full v = (appdata_full)0;  
                v.vertex = vi[0].vertex * bary.x + vi[1].vertex * bary.y + vi[2].vertex * bary.z;
                v.normal = vi[0].normal * bary.x + vi[1].normal * bary.y + vi[2].normal * bary.z;
                v.tangent = vi[0].tangent * bary.x + vi[1].tangent * bary.y + vi[2].tangent * bary.z;
                v.texcoord = vi[0].texcoord * bary.x + vi[1].texcoord * bary.y + vi[2].texcoord * bary.z;
                v.texcoord1 = vi[0].texcoord1 * bary.x + vi[1].texcoord1 * bary.y + vi[2].texcoord1 * bary.z;
                v.texcoord2 = vi[0].texcoord2 * bary.x + vi[1].texcoord2 * bary.y + vi[2].texcoord2 * bary.z;
                v.color = vi[0].color * bary.x + vi[1].color * bary.y + vi[2].color * bary.z;
                return vert_add(v);
            }

            fixed4 frag_add(v2f_add i) : SV_Target
            {
                float3 viewDir = normalize(_WorldSpaceCameraPos - i.worldPos);
                float3 worldNormal = normalize(i.worldNormal);
                
                float fresnel = 1.0 - saturate(dot(worldNormal, viewDir));
                fresnel = pow(fresnel, _FresnelPower);
                fresnel = _FresnelScale * fresnel + _FresnelBias;
                
                float yMask = abs(i.normal.y - _Y_Mask) - _Y_Mask;
                yMask = saturate(yMask);
                yMask = 1.0 - yMask;
                fresnel *= yMask;
                
                UNITY_LIGHT_ATTENUATION(atten, i, i.worldPos)
                
                #if defined(POINT) || defined(SPOT)
                    float3 lightDir = normalize(_WorldSpaceLightPos0.xyz - i.worldPos);
                #else
                    float3 lightDir = _WorldSpaceLightPos0.xyz;
                #endif
                
                float NdotL = saturate(dot(worldNormal, lightDir));
                
                float3 halfDir = normalize(lightDir + viewDir);
                float NdotH = saturate(dot(worldNormal, halfDir));
                
                float specAngle = max(NdotH, 0.0);
                float specular = pow(specAngle, 1.0);
                
                float3 lightColor = _LightColor0.rgb * atten * NdotL;
                
                float4 finalColor;
                finalColor.rgb = lightColor * specular;
                finalColor.a = fresnel * _Alpha;
                
                UNITY_APPLY_FOG(i.fogCoord, finalColor);
                
                return finalColor;
            }
            ENDCG
        }

        Pass
        {
            Name "ShadowCaster"
            Tags { "LightMode" = "ShadowCaster" }

            ZWrite On
            ZTest LEqual
            Cull Front

            CGPROGRAM
            #pragma vertex vertShadow
            #pragma fragment fragShadow
            #pragma target 3.0
            #pragma multi_compile_shadowcaster

            #include "UnityCG.cginc"

            struct v2fShadow 
            {
                V2F_SHADOW_CASTER;
            };

            v2fShadow vertShadow(appdata_base v) 
            {
                v2fShadow o;
                TRANSFER_SHADOW_CASTER_NORMALOFFSET(o)
                return o;
            }

            float4 fragShadow(v2fShadow i) : SV_Target
            {
                SHADOW_CASTER_FRAGMENT(i)
            }
            ENDCG
        }
    }

    FallBack "Transparent/VertexLit"
}