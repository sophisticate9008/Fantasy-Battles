Shader "Custom/UI-CircleLines"
{
    Properties
    {
        _Stencil ("Stencil ID", Float) = 0
        _StencilComp ("Stencil Comparison", Float) = 8
        _StencilOp ("Stencil Operation", Float) = 0
        _StencilWriteMask ("Stencil Write Mask", Float) = 255
        _StencilReadMask ("Stencil Read Mask", Float) = 255
        _ColorMask ("Color Mask", Float) = 15
        [HDR]_Color1("Color1", Color) = (0.2549732, 0.6528301, 0.6329373, 0.3529412)
        [HDR]_Color2("Color2", Color) = (0.8716981, 0.2878247, 0.8396771, 0.4039216)
        _Speed("Speed", Float) = 0.2
        _DisFog("DisFog", Int) = 20
        _AlaphClip("AlaphClip", Range(0, 1)) = 0
        _Thickness("Thickness", Range(0, 0.5)) = 0.2
        _R("R", Range(0, 0.5)) = 0.4
        _Center_x("Center_x", Float) = 0.5
        _Center_y("Center_y", Float) = 0.5
        [NoScaleOffset]_MainTex("MainTex", 2D) = "white" {}
        [HideInInspector]_CastShadows("_CastShadows", Float) = 0
        [HideInInspector]_Surface("_Surface", Float) = 1
        [HideInInspector]_Blend("_Blend", Float) = 0
        [HideInInspector]_AlphaClip("_AlphaClip", Float) = 1
        [HideInInspector]_SrcBlend("_SrcBlend", Float) = 1
        [HideInInspector]_DstBlend("_DstBlend", Float) = 0
        [HideInInspector][ToggleUI]_ZWrite("_ZWrite", Float) = 0
        [HideInInspector]_ZWriteControl("_ZWriteControl", Float) = 0
        [HideInInspector]_ZTest("_ZTest", Float) = 4
        [HideInInspector]_Cull("_Cull", Float) = 0
        [HideInInspector]_AlphaToMask("_AlphaToMask", Float) = 0
        [HideInInspector]_QueueOffset("_QueueOffset", Float) = 0
        [HideInInspector]_QueueControl("_QueueControl", Float) = -1
        [HideInInspector][NoScaleOffset]unity_Lightmaps("unity_Lightmaps", 2DArray) = "" {}
        [HideInInspector][NoScaleOffset]unity_LightmapsInd("unity_LightmapsInd", 2DArray) = "" {}
        [HideInInspector][NoScaleOffset]unity_ShadowMasks("unity_ShadowMasks", 2DArray) = "" {}
    }
    SubShader
    {
        Tags
        {
            "RenderPipeline"="UniversalPipeline"
            "RenderType"="Transparent"
            "UniversalMaterialType" = "Unlit"
            "Queue"="Transparent"
            "DisableBatching"="False"
            "ShaderGraphShader"="true"
            "ShaderGraphTargetId"="UniversalUnlitSubTarget"
        }
        Stencil
        {
            Ref [_Stencil]
            Comp [_StencilComp]
            Pass [_StencilOp]
            ReadMask [_StencilReadMask]
            WriteMask [_StencilWriteMask]
        }
        ColorMask [_ColorMask]
        Pass
        {
            Name "Universal Forward"
            Tags
            {
                // LightMode: <None>
            }
            
            // Render State
            Cull [_Cull]
            Blend [_SrcBlend] [_DstBlend]
            ZTest [_ZTest]
            ZWrite [_ZWrite]
            AlphaToMask [_AlphaToMask]
            
            // Debug
            // <None>
            
            // --------------------------------------------------
            // Pass
            
            HLSLPROGRAM
            
            // Pragmas
            #pragma target 2.0
            #pragma multi_compile_instancing
            #pragma multi_compile_fog
            #pragma instancing_options renderinglayer
            #pragma vertex vert
            #pragma fragment frag
            
            // Keywords
            #pragma multi_compile _ LIGHTMAP_ON
            #pragma multi_compile _ DIRLIGHTMAP_COMBINED
            #pragma shader_feature _ _SAMPLE_GI
            #pragma multi_compile_fragment _ _DBUFFER_MRT1 _DBUFFER_MRT2 _DBUFFER_MRT3
            #pragma multi_compile_fragment _ DEBUG_DISPLAY
            #pragma multi_compile_fragment _ _SCREEN_SPACE_OCCLUSION
            #pragma shader_feature_fragment _ _SURFACE_TYPE_TRANSPARENT
            #pragma shader_feature_local_fragment _ _ALPHAPREMULTIPLY_ON
            #pragma shader_feature_local_fragment _ _ALPHAMODULATE_ON
            #pragma shader_feature_local_fragment _ _ALPHATEST_ON
            // GraphKeywords: <None>
            
            // Defines
            
            #define ATTRIBUTES_NEED_NORMAL
            #define ATTRIBUTES_NEED_TANGENT
            #define ATTRIBUTES_NEED_TEXCOORD0
            #define ATTRIBUTES_NEED_COLOR
            #define VARYINGS_NEED_POSITION_WS
            #define VARYINGS_NEED_NORMAL_WS
            #define VARYINGS_NEED_TEXCOORD0
            #define VARYINGS_NEED_COLOR
            #define FEATURES_GRAPH_VERTEX
            /* WARNING: $splice Could not find named fragment 'PassInstancing' */
            #define SHADERPASS SHADERPASS_UNLIT
            #define _FOG_FRAGMENT 1
            
            
            // custom interpolator pre-include
            /* WARNING: $splice Could not find named fragment 'sgci_CustomInterpolatorPreInclude' */
            
            // Includes
            #include_with_pragmas "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DOTS.hlsl"
            #include_with_pragmas "Packages/com.unity.render-pipelines.universal/ShaderLibrary/RenderingLayers.hlsl"
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Input.hlsl"
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
            #include_with_pragmas "Packages/com.unity.render-pipelines.core/ShaderLibrary/FoveatedRenderingKeywords.hlsl"
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/FoveatedRendering.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DBuffer.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
            
            // --------------------------------------------------
            // Structs and Packing
            
            // custom interpolators pre packing
            /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPrePacking' */
            
            struct Attributes
            {
                float3 positionOS : POSITION;
                float3 normalOS : NORMAL;
                float4 tangentOS : TANGENT;
                float4 uv0 : TEXCOORD0;
                float4 color : COLOR;
                #if UNITY_ANY_INSTANCING_ENABLED
                    uint instanceID : INSTANCEID_SEMANTIC;
                #endif
            };
            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float3 positionWS;
                float3 normalWS;
                float4 texCoord0;
                float4 color;
                #if UNITY_ANY_INSTANCING_ENABLED
                    uint instanceID : CUSTOM_INSTANCE_ID;
                #endif
                #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                    uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
                #endif
                #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                    uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
                #endif
                #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                    FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
                #endif
            };
            struct SurfaceDescriptionInputs
            {
                float4 uv0;
                float4 VertexColor;
                float3 TimeParameters;
            };
            struct VertexDescriptionInputs
            {
                float3 ObjectSpaceNormal;
                float3 ObjectSpaceTangent;
                float3 ObjectSpacePosition;
            };
            struct PackedVaryings
            {
                float4 positionCS : SV_POSITION;
                float4 texCoord0 : INTERP0;
                float4 color : INTERP1;
                float3 positionWS : INTERP2;
                float3 normalWS : INTERP3;
                #if UNITY_ANY_INSTANCING_ENABLED
                    uint instanceID : CUSTOM_INSTANCE_ID;
                #endif
                #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                    uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
                #endif
                #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                    uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
                #endif
                #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                    FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
                #endif
            };
            
            PackedVaryings PackVaryings (Varyings input)
            {
                PackedVaryings output;
                ZERO_INITIALIZE(PackedVaryings, output);
                output.positionCS = input.positionCS;
                output.texCoord0.xyzw = input.texCoord0;
                output.color.xyzw = input.color;
                output.positionWS.xyz = input.positionWS;
                output.normalWS.xyz = input.normalWS;
                #if UNITY_ANY_INSTANCING_ENABLED
                    output.instanceID = input.instanceID;
                #endif
                #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                    output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
                #endif
                #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                    output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
                #endif
                #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                    output.cullFace = input.cullFace;
                #endif
                return output;
            }
            
            Varyings UnpackVaryings (PackedVaryings input)
            {
                Varyings output;
                output.positionCS = input.positionCS;
                output.texCoord0 = input.texCoord0.xyzw;
                output.color = input.color.xyzw;
                output.positionWS = input.positionWS.xyz;
                output.normalWS = input.normalWS.xyz;
                #if UNITY_ANY_INSTANCING_ENABLED
                    output.instanceID = input.instanceID;
                #endif
                #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                    output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
                #endif
                #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                    output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
                #endif
                #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                    output.cullFace = input.cullFace;
                #endif
                return output;
            }
            
            
            // --------------------------------------------------
            // Graph
            
            // Graph Properties
            CBUFFER_START(UnityPerMaterial)
                float4 _Color1;
                float4 _Color2;
                float _Speed;
                float _DisFog;
                float _AlaphClip;
                float _Thickness;
                float _R;
                float _Center_x;
                float _Center_y;
                float4 _MainTex_TexelSize;
            CBUFFER_END
            
            
            // Object and Global properties
            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);
            
            // Graph Includes
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Hashes.hlsl"
            
            // -- Property used by ScenePickingPass
            #ifdef SCENEPICKINGPASS
                float4 _SelectionID;
            #endif
            
            // -- Properties used by SceneSelectionPass
            #ifdef SCENESELECTIONPASS
                int _ObjectId;
                int _PassValue;
            #endif
            
            // Graph Functions
            
            void Unity_Power_float(float A, float B, out float Out)
            {
                Out = pow(A, B);
            }
            
            void Unity_Subtract_float(float A, float B, out float Out)
            {
                Out = A - B;
            }
            
            void Unity_Absolute_float(float In, out float Out)
            {
                Out = abs(In);
            }
            
            void Unity_Maximum_float(float A, float B, out float Out)
            {
                Out = max(A, B);
            }
            
            void Unity_SquareRoot_float(float In, out float Out)
            {
                Out = sqrt(In);
            }
            
            void Unity_Add_float(float A, float B, out float Out)
            {
                Out = A + B;
            }
            
            void Unity_Step_float(float Edge, float In, out float Out)
            {
                Out = step(Edge, In);
            }
            
            void Unity_OneMinus_float(float In, out float Out)
            {
                Out = 1 - In;
            }
            
            void Unity_Multiply_float_float(float A, float B, out float Out)
            {
                Out = A * B;
            }
            
            void Unity_TilingAndOffset_float(float2 UV, float2 Tiling, float2 Offset, out float2 Out)
            {
                Out = UV * Tiling + Offset;
            }
            
            float2 Unity_GradientNoise_Deterministic_Dir_float(float2 p)
            {
                float x; Hash_Tchou_2_1_float(p, x);
                return normalize(float2(x - floor(x + 0.5), abs(x) - 0.5));
            }
            
            void Unity_GradientNoise_Deterministic_float (float2 UV, float3 Scale, out float Out)
            {
                float2 p = UV * Scale.xy;
                float2 ip = floor(p);
                float2 fp = frac(p);
                float d00 = dot(Unity_GradientNoise_Deterministic_Dir_float(ip), fp);
                float d01 = dot(Unity_GradientNoise_Deterministic_Dir_float(ip + float2(0, 1)), fp - float2(0, 1));
                float d10 = dot(Unity_GradientNoise_Deterministic_Dir_float(ip + float2(1, 0)), fp - float2(1, 0));
                float d11 = dot(Unity_GradientNoise_Deterministic_Dir_float(ip + float2(1, 1)), fp - float2(1, 1));
                fp = fp * fp * fp * (fp * (fp * 6 - 15) + 10);
                Out = lerp(lerp(d00, d01, fp.y), lerp(d10, d11, fp.y), fp.x) + 0.5;
            }
            
            void Unity_Lerp_float4(float4 A, float4 B, float4 T, out float4 Out)
            {
                Out = lerp(A, B, T);
            }
            
            void Unity_Multiply_float4_float4(float4 A, float4 B, out float4 Out)
            {
                Out = A * B;
            }
            
            // Custom interpolators pre vertex
            /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPreVertex' */
            
            // Graph Vertex
            struct VertexDescription
            {
                float3 Position;
                float3 Normal;
                float3 Tangent;
            };
            
            VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
            {
                VertexDescription description = (VertexDescription)0;
                description.Position = IN.ObjectSpacePosition;
                description.Normal = IN.ObjectSpaceNormal;
                description.Tangent = IN.ObjectSpaceTangent;
                return description;
            }
            
            // Custom interpolators, pre surface
            #ifdef FEATURES_GRAPH_VERTEX
                Varyings CustomInterpolatorPassThroughFunc(inout Varyings output, VertexDescription input)
                {
                    return output;
                }
                #define CUSTOMINTERPOLATOR_VARYPASSTHROUGH_FUNC
            #endif
            
            // Graph Pixel
            struct SurfaceDescription
            {
                float3 BaseColor;
                float Alpha;
                float AlphaClipThreshold;
            };
            
            SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
            {
                SurfaceDescription surface = (SurfaceDescription)0;
                float _Property_0453250ce55a422989957b2a24d5cf4c_Out_0_Float = _R;
                float _Power_4dd63e2c96164c0b8781e8c33e82fdfd_Out_2_Float;
                Unity_Power_float(_Property_0453250ce55a422989957b2a24d5cf4c_Out_0_Float, float(2), _Power_4dd63e2c96164c0b8781e8c33e82fdfd_Out_2_Float);
                float4 _UV_b307cd5f728f4c9dba315825a1546ce2_Out_0_Vector4 = IN.uv0;
                float _Split_d66ef01fcbcc4601bd6a27127e943f8b_R_1_Float = _UV_b307cd5f728f4c9dba315825a1546ce2_Out_0_Vector4[0];
                float _Split_d66ef01fcbcc4601bd6a27127e943f8b_G_2_Float = _UV_b307cd5f728f4c9dba315825a1546ce2_Out_0_Vector4[1];
                float _Split_d66ef01fcbcc4601bd6a27127e943f8b_B_3_Float = _UV_b307cd5f728f4c9dba315825a1546ce2_Out_0_Vector4[2];
                float _Split_d66ef01fcbcc4601bd6a27127e943f8b_A_4_Float = _UV_b307cd5f728f4c9dba315825a1546ce2_Out_0_Vector4[3];
                float _Property_565321e1fcf845d8b9e651dd22ef9ad9_Out_0_Float = _Center_x;
                float _Subtract_08b808a289ee437d80702b9ca3b77418_Out_2_Float;
                Unity_Subtract_float(_Split_d66ef01fcbcc4601bd6a27127e943f8b_R_1_Float, _Property_565321e1fcf845d8b9e651dd22ef9ad9_Out_0_Float, _Subtract_08b808a289ee437d80702b9ca3b77418_Out_2_Float);
                float _Absolute_9a2df6aa884045a9833ddda2708a1701_Out_1_Float;
                Unity_Absolute_float(_Subtract_08b808a289ee437d80702b9ca3b77418_Out_2_Float, _Absolute_9a2df6aa884045a9833ddda2708a1701_Out_1_Float);
                float _Power_808bfe5115bb46a5bd471f75c7415b5f_Out_2_Float;
                Unity_Power_float(_Absolute_9a2df6aa884045a9833ddda2708a1701_Out_1_Float, float(2), _Power_808bfe5115bb46a5bd471f75c7415b5f_Out_2_Float);
                float _Subtract_81d05baacfe9444e9ccd775c77f260ed_Out_2_Float;
                Unity_Subtract_float(_Power_4dd63e2c96164c0b8781e8c33e82fdfd_Out_2_Float, _Power_808bfe5115bb46a5bd471f75c7415b5f_Out_2_Float, _Subtract_81d05baacfe9444e9ccd775c77f260ed_Out_2_Float);
                float _Maximum_3e60f143d40749139dd7abb3accd9c76_Out_2_Float;
                Unity_Maximum_float(_Subtract_81d05baacfe9444e9ccd775c77f260ed_Out_2_Float, float(0), _Maximum_3e60f143d40749139dd7abb3accd9c76_Out_2_Float);
                float _SquareRoot_3d30b98f941a4a448cf0ef2706d98f81_Out_1_Float;
                Unity_SquareRoot_float(_Maximum_3e60f143d40749139dd7abb3accd9c76_Out_2_Float, _SquareRoot_3d30b98f941a4a448cf0ef2706d98f81_Out_1_Float);
                float _Property_511669af191e44c8ae5464f203df2309_Out_0_Float = _Center_x;
                float _Add_dc4de7cde3c74c97b9a9f6650de483f1_Out_2_Float;
                Unity_Add_float(_SquareRoot_3d30b98f941a4a448cf0ef2706d98f81_Out_1_Float, _Property_511669af191e44c8ae5464f203df2309_Out_0_Float, _Add_dc4de7cde3c74c97b9a9f6650de483f1_Out_2_Float);
                float _Step_ac7c18bfff5d44e09cd17e8df90b3786_Out_2_Float;
                Unity_Step_float(_Add_dc4de7cde3c74c97b9a9f6650de483f1_Out_2_Float, float(0.5), _Step_ac7c18bfff5d44e09cd17e8df90b3786_Out_2_Float);
                float _OneMinus_951110f652044bb1b81f17b57b52f761_Out_1_Float;
                Unity_OneMinus_float(_Step_ac7c18bfff5d44e09cd17e8df90b3786_Out_2_Float, _OneMinus_951110f652044bb1b81f17b57b52f761_Out_1_Float);
                float _Property_1f74529ed89b461cbe39f7bfdac64f45_Out_0_Float = _Speed;
                float _Multiply_27459d20a4684b868b080474ce16050b_Out_2_Float;
                Unity_Multiply_float_float(_Property_1f74529ed89b461cbe39f7bfdac64f45_Out_0_Float, IN.TimeParameters.x, _Multiply_27459d20a4684b868b080474ce16050b_Out_2_Float);
                float2 _TilingAndOffset_53b05c866cb144499accc48e176297d7_Out_3_Vector2;
                Unity_TilingAndOffset_float(IN.uv0.xy, float2 (1, 1), (_Multiply_27459d20a4684b868b080474ce16050b_Out_2_Float.xx), _TilingAndOffset_53b05c866cb144499accc48e176297d7_Out_3_Vector2);
                float _GradientNoise_a7d81c764bc645dba81bdd25826fb3b5_Out_2_Float;
                Unity_GradientNoise_Deterministic_float(_TilingAndOffset_53b05c866cb144499accc48e176297d7_Out_3_Vector2, float(2), _GradientNoise_a7d81c764bc645dba81bdd25826fb3b5_Out_2_Float);
                float _Multiply_739d7babada24386ab7e9e90c6f9116c_Out_2_Float;
                Unity_Multiply_float_float(0.2, _GradientNoise_a7d81c764bc645dba81bdd25826fb3b5_Out_2_Float, _Multiply_739d7babada24386ab7e9e90c6f9116c_Out_2_Float);
                float _Subtract_3bc2fe63b1704521aa764183e9de979d_Out_2_Float;
                Unity_Subtract_float(_Split_d66ef01fcbcc4601bd6a27127e943f8b_G_2_Float, _Add_dc4de7cde3c74c97b9a9f6650de483f1_Out_2_Float, _Subtract_3bc2fe63b1704521aa764183e9de979d_Out_2_Float);
                float _Absolute_cd070e04fe4c45d2bd57918d06607ae2_Out_1_Float;
                Unity_Absolute_float(_Subtract_3bc2fe63b1704521aa764183e9de979d_Out_2_Float, _Absolute_cd070e04fe4c45d2bd57918d06607ae2_Out_1_Float);
                float _Multiply_ba4cc531071a468fba923c1f951173ab_Out_2_Float;
                Unity_Multiply_float_float(_SquareRoot_3d30b98f941a4a448cf0ef2706d98f81_Out_1_Float, -1, _Multiply_ba4cc531071a468fba923c1f951173ab_Out_2_Float);
                float _Add_52b0072ccce84d2880f5e3de478b3e1f_Out_2_Float;
                Unity_Add_float(_Multiply_ba4cc531071a468fba923c1f951173ab_Out_2_Float, _Property_511669af191e44c8ae5464f203df2309_Out_0_Float, _Add_52b0072ccce84d2880f5e3de478b3e1f_Out_2_Float);
                float _Subtract_34bbdfd75a8c407b85c4b5332f8078f8_Out_2_Float;
                Unity_Subtract_float(_Add_52b0072ccce84d2880f5e3de478b3e1f_Out_2_Float, _Split_d66ef01fcbcc4601bd6a27127e943f8b_G_2_Float, _Subtract_34bbdfd75a8c407b85c4b5332f8078f8_Out_2_Float);
                float _Absolute_1ca1421af2844da7b60289ec9a80008b_Out_1_Float;
                Unity_Absolute_float(_Subtract_34bbdfd75a8c407b85c4b5332f8078f8_Out_2_Float, _Absolute_1ca1421af2844da7b60289ec9a80008b_Out_1_Float);
                float _Multiply_3150f8bd849549d6bc7936e1e7837b5f_Out_2_Float;
                Unity_Multiply_float_float(_Absolute_cd070e04fe4c45d2bd57918d06607ae2_Out_1_Float, _Absolute_1ca1421af2844da7b60289ec9a80008b_Out_1_Float, _Multiply_3150f8bd849549d6bc7936e1e7837b5f_Out_2_Float);
                float _Add_c74ccb36836d4804b290acbc14dfda54_Out_2_Float;
                Unity_Add_float(_Multiply_739d7babada24386ab7e9e90c6f9116c_Out_2_Float, _Multiply_3150f8bd849549d6bc7936e1e7837b5f_Out_2_Float, _Add_c74ccb36836d4804b290acbc14dfda54_Out_2_Float);
                float _Subtract_87ed5d46ffbc4116b125de63df91c3a2_Out_2_Float;
                Unity_Subtract_float(_Add_c74ccb36836d4804b290acbc14dfda54_Out_2_Float, float(1), _Subtract_87ed5d46ffbc4116b125de63df91c3a2_Out_2_Float);
                float _Absolute_fd900b74c46a45b2853a3819f1118e9a_Out_1_Float;
                Unity_Absolute_float(_Subtract_87ed5d46ffbc4116b125de63df91c3a2_Out_2_Float, _Absolute_fd900b74c46a45b2853a3819f1118e9a_Out_1_Float);
                float _Property_1eb84fba567b4608bb3e520b204bdb40_Out_0_Float = _Thickness;
                float _Subtract_eeda4ea5821e4678abc9ea0364a4b521_Out_2_Float;
                Unity_Subtract_float(_Add_c74ccb36836d4804b290acbc14dfda54_Out_2_Float, _Property_1eb84fba567b4608bb3e520b204bdb40_Out_0_Float, _Subtract_eeda4ea5821e4678abc9ea0364a4b521_Out_2_Float);
                float _Multiply_58937333506148388992775fd4d38b55_Out_2_Float;
                Unity_Multiply_float_float(_Absolute_fd900b74c46a45b2853a3819f1118e9a_Out_1_Float, _Subtract_eeda4ea5821e4678abc9ea0364a4b521_Out_2_Float, _Multiply_58937333506148388992775fd4d38b55_Out_2_Float);
                float _OneMinus_6d21ffd74f7d4a45a92dc60dca966895_Out_1_Float;
                Unity_OneMinus_float(_Multiply_58937333506148388992775fd4d38b55_Out_2_Float, _OneMinus_6d21ffd74f7d4a45a92dc60dca966895_Out_1_Float);
                float _Property_0bfbec89b9184a92a9a2b3e4c0ca5ba2_Out_0_Float = _DisFog;
                float _Absolute_e345ff7b6b3548d8882bc79fb83623b5_Out_1_Float;
                Unity_Absolute_float(_Property_0bfbec89b9184a92a9a2b3e4c0ca5ba2_Out_0_Float, _Absolute_e345ff7b6b3548d8882bc79fb83623b5_Out_1_Float);
                float _Power_615a37574b844e0aaae25bc8656fd041_Out_2_Float;
                Unity_Power_float(_OneMinus_6d21ffd74f7d4a45a92dc60dca966895_Out_1_Float, _Absolute_e345ff7b6b3548d8882bc79fb83623b5_Out_1_Float, _Power_615a37574b844e0aaae25bc8656fd041_Out_2_Float);
                float4 _Property_461fc038600346b398b727641f1dba53_Out_0_Vector4 = IsGammaSpace() ? LinearToSRGB(_Color1) : _Color1;
                float4 _Property_645e4990f7fb4b1893f70bea993a2bb0_Out_0_Vector4 = IsGammaSpace() ? LinearToSRGB(_Color2) : _Color2;
                float _Multiply_35de62105ad4425486e2fd913a3eddcd_Out_2_Float;
                Unity_Multiply_float_float(0.5, IN.TimeParameters.x, _Multiply_35de62105ad4425486e2fd913a3eddcd_Out_2_Float);
                float2 _TilingAndOffset_80bed7331392491183a00578eb444532_Out_3_Vector2;
                Unity_TilingAndOffset_float(IN.uv0.xy, float2 (1, 1), (_Multiply_35de62105ad4425486e2fd913a3eddcd_Out_2_Float.xx), _TilingAndOffset_80bed7331392491183a00578eb444532_Out_3_Vector2);
                float _GradientNoise_64a924c4701747a8ab2e93cb59c3797f_Out_2_Float;
                Unity_GradientNoise_Deterministic_float(_TilingAndOffset_80bed7331392491183a00578eb444532_Out_3_Vector2, float(2), _GradientNoise_64a924c4701747a8ab2e93cb59c3797f_Out_2_Float);
                float4 _Lerp_cbf065a46ec7400fa7ca64e674a79130_Out_3_Vector4;
                Unity_Lerp_float4(_Property_461fc038600346b398b727641f1dba53_Out_0_Vector4, _Property_645e4990f7fb4b1893f70bea993a2bb0_Out_0_Vector4, (_GradientNoise_64a924c4701747a8ab2e93cb59c3797f_Out_2_Float.xxxx), _Lerp_cbf065a46ec7400fa7ca64e674a79130_Out_3_Vector4);
                float4 _Multiply_29620d5782444a4499580cc71dddf85a_Out_2_Vector4;
                Unity_Multiply_float4_float4((_Power_615a37574b844e0aaae25bc8656fd041_Out_2_Float.xxxx), _Lerp_cbf065a46ec7400fa7ca64e674a79130_Out_3_Vector4, _Multiply_29620d5782444a4499580cc71dddf85a_Out_2_Vector4);
                float4 _Multiply_6859e74a414249f2b278398c3e071ca3_Out_2_Vector4;
                Unity_Multiply_float4_float4((_OneMinus_951110f652044bb1b81f17b57b52f761_Out_1_Float.xxxx), _Multiply_29620d5782444a4499580cc71dddf85a_Out_2_Vector4, _Multiply_6859e74a414249f2b278398c3e071ca3_Out_2_Vector4);
                float4 _Multiply_b31d6bf966c147d5a8bb98c633a6f3fa_Out_2_Vector4;
                Unity_Multiply_float4_float4(_Multiply_6859e74a414249f2b278398c3e071ca3_Out_2_Vector4, IN.VertexColor, _Multiply_b31d6bf966c147d5a8bb98c633a6f3fa_Out_2_Vector4);
                float _Property_60b8f01c54634bcbbf2533aec45e8910_Out_0_Float = _AlaphClip;
                surface.BaseColor = (_Multiply_b31d6bf966c147d5a8bb98c633a6f3fa_Out_2_Vector4.xyz);
                surface.Alpha = (_Multiply_b31d6bf966c147d5a8bb98c633a6f3fa_Out_2_Vector4).x;
                surface.AlphaClipThreshold = _Property_60b8f01c54634bcbbf2533aec45e8910_Out_0_Float;
                return surface;
            }
            
            // --------------------------------------------------
            // Build Graph Inputs
            #ifdef HAVE_VFX_MODIFICATION
                #define VFX_SRP_ATTRIBUTES Attributes
                #define VFX_SRP_VARYINGS Varyings
                #define VFX_SRP_SURFACE_INPUTS SurfaceDescriptionInputs
            #endif
            VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
            {
                VertexDescriptionInputs output;
                ZERO_INITIALIZE(VertexDescriptionInputs, output);
                
                output.ObjectSpaceNormal =                          input.normalOS;
                output.ObjectSpaceTangent =                         input.tangentOS.xyz;
                output.ObjectSpacePosition =                        input.positionOS;
                
                return output;
            }
            SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
            {
                SurfaceDescriptionInputs output;
                ZERO_INITIALIZE(SurfaceDescriptionInputs, output);
                
                #ifdef HAVE_VFX_MODIFICATION
                    #if VFX_USE_GRAPH_VALUES
                        uint instanceActiveIndex = asuint(UNITY_ACCESS_INSTANCED_PROP(PerInstance, _InstanceActiveIndex));
                        /* WARNING: $splice Could not find named fragment 'VFXLoadGraphValues' */
                    #endif
                    /* WARNING: $splice Could not find named fragment 'VFXSetFragInputs' */
                    
                #endif
                
                
                
                
                
                
                
                
                #if UNITY_UV_STARTS_AT_TOP
                #else
                #endif
                
                
                output.uv0 = input.texCoord0;
                output.VertexColor = input.color;
                output.TimeParameters = _TimeParameters.xyz; // This is mainly for LW as HD overwrite this value
                #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                    #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
                #else
                    #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
                #endif
                #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
                
                return output;
            }
            
            // --------------------------------------------------
            // Main
            
            #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/UnlitPass.hlsl"
            
            // --------------------------------------------------
            // Visual Effect Vertex Invocations
            #ifdef HAVE_VFX_MODIFICATION
                #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/VisualEffectVertex.hlsl"
            #endif
            
            ENDHLSL
        }
    }
    CustomEditor "UnityEditor.ShaderGraph.GenericShaderGraphMaterialGUI"
    CustomEditorForRenderPipeline "UnityEditor.ShaderGraphUnlitGUI" "UnityEngine.Rendering.Universal.UniversalRenderPipelineAsset"
    FallBack "Hidden/Shader Graph/FallbackError"
}

