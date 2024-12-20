Shader "Custom/Haloing"
{
    Properties
    {
        _Stencil ("Stencil ID", Float) = 0
        _StencilComp ("Stencil Comparison", Float) = 8
        _StencilOp ("Stencil Operation", Float) = 0
        _StencilWriteMask ("Stencil Write Mask", Float) = 255
        _StencilReadMask ("Stencil Read Mask", Float) = 255
        _ColorMask ("Color Mask", Float) = 15
        [NoScaleOffset]_MainTex("MainTex", 2D) = "white" {}
        _Length("Length", Float) = 10
        _Radius("Radius", Float) = 0.2
        _BlurnessSelf("BlurnessSelf", Range(0, 0.015)) = 0
        _BlurnessHalo("BlurnessHalo", Range(0, 0.015)) = 0.015
        _MaxFade("MaxFade", Float) = 0.05
        _Speed("Speed", Float) = 1
        _FadeLength("FadeLength", Float) = 3
        [HideInInspector]_CastShadows("_CastShadows", Float) = 1
        [HideInInspector]_Surface("_Surface", Float) = 1
        [HideInInspector]_Blend("_Blend", Float) = 0
        [HideInInspector]_AlphaClip("_AlphaClip", Float) = 0
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
        Stencil
        {
            Ref [_Stencil]
            Comp [_StencilComp]
            Pass [_StencilOp]
            ReadMask [_StencilReadMask]
            WriteMask [_StencilWriteMask]
        }
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
            #define VARYINGS_NEED_POSITION_WS
            #define VARYINGS_NEED_NORMAL_WS
            #define VARYINGS_NEED_TEXCOORD0
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
                float3 positionWS : INTERP1;
                float3 normalWS : INTERP2;
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
                float4 _MainTex_TexelSize;
                float _Length;
                float _Radius;
                float _BlurnessHalo;
                float _BlurnessSelf;
                float _MaxFade;
                float _Speed;
                float _FadeLength;
            CBUFFER_END
            
            
            // Object and Global properties
            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);
            
            // Graph Includes
            // GraphIncludes: <None>
            
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
            
            void GaussianBlur_float(UnityTexture2D _MainTex, float2 uv, float offset, out float4 color){
                color = tex2D(_MainTex,float2(uv.x, uv.y)) * 0.147761f;
                color += tex2D(_MainTex, float2(uv.x - offset, uv.y - offset)) * 0.0947416f;
                color += tex2D(_MainTex,float2(uv.x,uv.y - offset)) * 0.118318f;
                color += tex2D(_MainTex,float2(uv.x + offset, uv.y + offset)) * 0.0947416f;
                color += tex2D(_MainTex,float2(uv.x - offset, uv.y)) * 0.118318f;
                color += tex2D(_MainTex,float2(uv.x + offset, uv.y)) * 0.118318f;
                color += tex2D (_MainTex, float2(uv.x - offset, uv.y + offset)) * 0.0947416f;
                
                
                color += tex2D(_MainTex,float2(uv.x, uv.y + offset)) * 0.118318f;
                
                
                color += tex2D(_MainTex,float2(uv.x + offset, uv.y - offset)) * 0.0947416f;
            }
            
            void Unity_Multiply_float4_float4(float4 A, float4 B, out float4 Out)
            {
                Out = A * B;
            }
            
            void Unity_Subtract_float2(float2 A, float2 B, out float2 Out)
            {
                Out = A - B;
            }
            
            void Unity_Multiply_float_float(float A, float B, out float Out)
            {
                Out = A * B;
            }
            
            void Unity_Sine_float(float In, out float Out)
            {
                Out = sin(In);
            }
            
            void Unity_Absolute_float(float In, out float Out)
            {
                Out = abs(In);
            }
            
            void Unity_OneMinus_float(float In, out float Out)
            {
                Out = 1 - In;
            }
            
            void Unity_Multiply_float2_float2(float2 A, float2 B, out float2 Out)
            {
                Out = A * B;
            }
            
            void Unity_Add_float2(float2 A, float2 B, out float2 Out)
            {
                Out = A + B;
            }
            
            void Unity_Cosine_float(float In, out float Out)
            {
                Out = cos(In);
            }
            
            void Unity_Length_float2(float2 In, out float Out)
            {
                Out = length(In);
            }
            
            void Unity_Divide_float(float A, float B, out float Out)
            {
                Out = A / B;
            }
            
            void Unity_Smoothstep_float(float Edge1, float Edge2, float In, out float Out)
            {
                Out = smoothstep(Edge1, Edge2, In);
            }
            
            void Unity_Add_float4(float4 A, float4 B, out float4 Out)
            {
                Out = A + B;
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
                UnityTexture2D _Property_02041029848f4508820972c3e41bc3a6_Out_0_Texture2D = UnityBuildTexture2DStructNoScale(_MainTex);
                float4 _UV_fff6a839c1f9436d9ca374e9429629e6_Out_0_Vector4 = IN.uv0;
                float _Property_c45ebf7be19d43e18a818cf7fcbe0834_Out_0_Float = _BlurnessSelf;
                float4 _GaussianBlurCustomFunction_a82e59e7ca3742c8b0ba13cdf6a49a07_color_0_Vector4;
                GaussianBlur_float(_Property_02041029848f4508820972c3e41bc3a6_Out_0_Texture2D, (_UV_fff6a839c1f9436d9ca374e9429629e6_Out_0_Vector4.xy), _Property_c45ebf7be19d43e18a818cf7fcbe0834_Out_0_Float, _GaussianBlurCustomFunction_a82e59e7ca3742c8b0ba13cdf6a49a07_color_0_Vector4);
                float _Split_f4b8543a821a4754ae20a3af3f477158_R_1_Float = _GaussianBlurCustomFunction_a82e59e7ca3742c8b0ba13cdf6a49a07_color_0_Vector4[0];
                float _Split_f4b8543a821a4754ae20a3af3f477158_G_2_Float = _GaussianBlurCustomFunction_a82e59e7ca3742c8b0ba13cdf6a49a07_color_0_Vector4[1];
                float _Split_f4b8543a821a4754ae20a3af3f477158_B_3_Float = _GaussianBlurCustomFunction_a82e59e7ca3742c8b0ba13cdf6a49a07_color_0_Vector4[2];
                float _Split_f4b8543a821a4754ae20a3af3f477158_A_4_Float = _GaussianBlurCustomFunction_a82e59e7ca3742c8b0ba13cdf6a49a07_color_0_Vector4[3];
                float4 _Multiply_26927c7203054bd2bf63d2f4e793e6b8_Out_2_Vector4;
                Unity_Multiply_float4_float4(_GaussianBlurCustomFunction_a82e59e7ca3742c8b0ba13cdf6a49a07_color_0_Vector4, (_Split_f4b8543a821a4754ae20a3af3f477158_A_4_Float.xxxx), _Multiply_26927c7203054bd2bf63d2f4e793e6b8_Out_2_Vector4);
                UnityTexture2D _Property_a0e7f7b30ac94e5ba3b8347150dab390_Out_0_Texture2D = UnityBuildTexture2DStructNoScale(_MainTex);
                float2 _Vector2_4d7ea5ff7a8f4dd491f262ec11f34836_Out_0_Vector2 = float2(float(0.5), float(0.5));
                float4 _UV_934bacd2da304a2f91557668a1da3668_Out_0_Vector4 = IN.uv0;
                float2 _Subtract_7fad1d1c12214db2aa3811721580575e_Out_2_Vector2;
                Unity_Subtract_float2((_UV_934bacd2da304a2f91557668a1da3668_Out_0_Vector4.xy), _Vector2_4d7ea5ff7a8f4dd491f262ec11f34836_Out_0_Vector2, _Subtract_7fad1d1c12214db2aa3811721580575e_Out_2_Vector2);
                float _Property_21069413f35b47978ea866f0e5887340_Out_0_Float = _Radius;
                float _Property_a339d99cf2f34735a5578b4084a16c70_Out_0_Float = _Speed;
                float _Multiply_8ceabfb19cfe47d3a9a7ff52c27040e7_Out_2_Float;
                Unity_Multiply_float_float(IN.TimeParameters.x, _Property_a339d99cf2f34735a5578b4084a16c70_Out_0_Float, _Multiply_8ceabfb19cfe47d3a9a7ff52c27040e7_Out_2_Float);
                float _Sine_605fd4bddb274e328485b635eb0be1f6_Out_1_Float;
                Unity_Sine_float(_Multiply_8ceabfb19cfe47d3a9a7ff52c27040e7_Out_2_Float, _Sine_605fd4bddb274e328485b635eb0be1f6_Out_1_Float);
                float _Absolute_410776b94a0643019b03c5ef26816cec_Out_1_Float;
                Unity_Absolute_float(_Sine_605fd4bddb274e328485b635eb0be1f6_Out_1_Float, _Absolute_410776b94a0643019b03c5ef26816cec_Out_1_Float);
                float _Multiply_09f936dddebb4e6fa59b2da8bca63073_Out_2_Float;
                Unity_Multiply_float_float(_Property_21069413f35b47978ea866f0e5887340_Out_0_Float, _Absolute_410776b94a0643019b03c5ef26816cec_Out_1_Float, _Multiply_09f936dddebb4e6fa59b2da8bca63073_Out_2_Float);
                float _OneMinus_554ef05645cb48cf8787da5e00eda506_Out_1_Float;
                Unity_OneMinus_float(_Multiply_09f936dddebb4e6fa59b2da8bca63073_Out_2_Float, _OneMinus_554ef05645cb48cf8787da5e00eda506_Out_1_Float);
                float2 _Multiply_8f56fd3fe92d42da8f9cc8a7904a9894_Out_2_Vector2;
                Unity_Multiply_float2_float2(_Subtract_7fad1d1c12214db2aa3811721580575e_Out_2_Vector2, (_OneMinus_554ef05645cb48cf8787da5e00eda506_Out_1_Float.xx), _Multiply_8f56fd3fe92d42da8f9cc8a7904a9894_Out_2_Vector2);
                float2 _Add_f6e5de34d3d44721a560377890819d15_Out_2_Vector2;
                Unity_Add_float2(_Vector2_4d7ea5ff7a8f4dd491f262ec11f34836_Out_0_Vector2, _Multiply_8f56fd3fe92d42da8f9cc8a7904a9894_Out_2_Vector2, _Add_f6e5de34d3d44721a560377890819d15_Out_2_Vector2);
                float _Property_5d66e61e7fa240ea9533912d1fd4a834_Out_0_Float = _BlurnessHalo;
                float4 _GaussianBlurCustomFunction_f039085039f0467c96299622ab45db3f_color_0_Vector4;
                GaussianBlur_float(_Property_a0e7f7b30ac94e5ba3b8347150dab390_Out_0_Texture2D, _Add_f6e5de34d3d44721a560377890819d15_Out_2_Vector2, _Property_5d66e61e7fa240ea9533912d1fd4a834_Out_0_Float, _GaussianBlurCustomFunction_f039085039f0467c96299622ab45db3f_color_0_Vector4);
                float _Split_0852f8649c8a44c7b74b58d294a7341b_R_1_Float = _GaussianBlurCustomFunction_f039085039f0467c96299622ab45db3f_color_0_Vector4[0];
                float _Split_0852f8649c8a44c7b74b58d294a7341b_G_2_Float = _GaussianBlurCustomFunction_f039085039f0467c96299622ab45db3f_color_0_Vector4[1];
                float _Split_0852f8649c8a44c7b74b58d294a7341b_B_3_Float = _GaussianBlurCustomFunction_f039085039f0467c96299622ab45db3f_color_0_Vector4[2];
                float _Split_0852f8649c8a44c7b74b58d294a7341b_A_4_Float = _GaussianBlurCustomFunction_f039085039f0467c96299622ab45db3f_color_0_Vector4[3];
                float4 _Multiply_10055bff298646b384e193471f8a27f0_Out_2_Vector4;
                Unity_Multiply_float4_float4(_GaussianBlurCustomFunction_f039085039f0467c96299622ab45db3f_color_0_Vector4, (_Split_0852f8649c8a44c7b74b58d294a7341b_A_4_Float.xxxx), _Multiply_10055bff298646b384e193471f8a27f0_Out_2_Vector4);
                float _Property_59378cddbf554d469f5ea4f6b5a99305_Out_0_Float = _Length;
                float4 _Multiply_352dc6ba77034ac3bee260667a97daff_Out_2_Vector4;
                Unity_Multiply_float4_float4(_Multiply_10055bff298646b384e193471f8a27f0_Out_2_Vector4, (_Property_59378cddbf554d469f5ea4f6b5a99305_Out_0_Float.xxxx), _Multiply_352dc6ba77034ac3bee260667a97daff_Out_2_Vector4);
                float _OneMinus_8aa36af6c1a549bdab189f92933b4374_Out_1_Float;
                Unity_OneMinus_float(_Split_f4b8543a821a4754ae20a3af3f477158_A_4_Float, _OneMinus_8aa36af6c1a549bdab189f92933b4374_Out_1_Float);
                float4 _Multiply_e95b8e00117a47c6a49a1e0e02ba1175_Out_2_Vector4;
                Unity_Multiply_float4_float4(_Multiply_352dc6ba77034ac3bee260667a97daff_Out_2_Vector4, (_OneMinus_8aa36af6c1a549bdab189f92933b4374_Out_1_Float.xxxx), _Multiply_e95b8e00117a47c6a49a1e0e02ba1175_Out_2_Vector4);
                float _Property_d62f7a1d803145e4958be54ffaa6593b_Out_0_Float = _FadeLength;
                float2 _Vector2_9283fe9d37a047b5a13ffa5ef0a447a3_Out_0_Vector2 = float2(float(0.5), float(0.5));
                float4 _UV_061ddc7fba34409c9f4e2c5b3c1e89d6_Out_0_Vector4 = IN.uv0;
                float2 _Subtract_66412a2663314257a21003044c46eaa0_Out_2_Vector2;
                Unity_Subtract_float2((_UV_061ddc7fba34409c9f4e2c5b3c1e89d6_Out_0_Vector4.xy), _Vector2_9283fe9d37a047b5a13ffa5ef0a447a3_Out_0_Vector2, _Subtract_66412a2663314257a21003044c46eaa0_Out_2_Vector2);
                float _Property_2804681380964e7b9c6d299ed5b3a837_Out_0_Float = _Radius;
                float _Property_74a00496e3d14599b19d54a20a88b77a_Out_0_Float = _Speed;
                float _Multiply_afe2d2b5457149978814acf4c4401e60_Out_2_Float;
                Unity_Multiply_float_float(IN.TimeParameters.x, _Property_74a00496e3d14599b19d54a20a88b77a_Out_0_Float, _Multiply_afe2d2b5457149978814acf4c4401e60_Out_2_Float);
                float _Cosine_8c8f436012444fb2bdf1f2020176efaf_Out_1_Float;
                Unity_Cosine_float(_Multiply_afe2d2b5457149978814acf4c4401e60_Out_2_Float, _Cosine_8c8f436012444fb2bdf1f2020176efaf_Out_1_Float);
                float _Absolute_4528284e1809498b844f4136668bd169_Out_1_Float;
                Unity_Absolute_float(_Cosine_8c8f436012444fb2bdf1f2020176efaf_Out_1_Float, _Absolute_4528284e1809498b844f4136668bd169_Out_1_Float);
                float _Multiply_f8db578a527f4c7ea811322c39e2444d_Out_2_Float;
                Unity_Multiply_float_float(_Property_2804681380964e7b9c6d299ed5b3a837_Out_0_Float, _Absolute_4528284e1809498b844f4136668bd169_Out_1_Float, _Multiply_f8db578a527f4c7ea811322c39e2444d_Out_2_Float);
                float _OneMinus_2a28c2cd535c43e693bba9a3ea0e05ae_Out_1_Float;
                Unity_OneMinus_float(_Multiply_f8db578a527f4c7ea811322c39e2444d_Out_2_Float, _OneMinus_2a28c2cd535c43e693bba9a3ea0e05ae_Out_1_Float);
                float2 _Multiply_268b7a1ada804b1e8390bcb98edf9f59_Out_2_Vector2;
                Unity_Multiply_float2_float2(_Subtract_66412a2663314257a21003044c46eaa0_Out_2_Vector2, (_OneMinus_2a28c2cd535c43e693bba9a3ea0e05ae_Out_1_Float.xx), _Multiply_268b7a1ada804b1e8390bcb98edf9f59_Out_2_Vector2);
                float2 _Add_1949119a7c584785b990f253bf7ae497_Out_2_Vector2;
                Unity_Add_float2(_Vector2_9283fe9d37a047b5a13ffa5ef0a447a3_Out_0_Vector2, _Multiply_268b7a1ada804b1e8390bcb98edf9f59_Out_2_Vector2, _Add_1949119a7c584785b990f253bf7ae497_Out_2_Vector2);
                float4 _UV_9a2fff31139f4c95ac6c44dd01fd7bea_Out_0_Vector4 = IN.uv0;
                float2 _Subtract_b3eb954b07124635a118776b29e09ca5_Out_2_Vector2;
                Unity_Subtract_float2(_Add_1949119a7c584785b990f253bf7ae497_Out_2_Vector2, (_UV_9a2fff31139f4c95ac6c44dd01fd7bea_Out_0_Vector4.xy), _Subtract_b3eb954b07124635a118776b29e09ca5_Out_2_Vector2);
                float _Length_6a0b997a7042475da8f6425e35c6ff9b_Out_1_Float;
                Unity_Length_float2(_Subtract_b3eb954b07124635a118776b29e09ca5_Out_2_Vector2, _Length_6a0b997a7042475da8f6425e35c6ff9b_Out_1_Float);
                float _Property_d72ba63042534b0e970e0d84f4bc6d22_Out_0_Float = _MaxFade;
                float _Divide_018cce6c4c1f4e8281ded03c51e32fb3_Out_2_Float;
                Unity_Divide_float(_Length_6a0b997a7042475da8f6425e35c6ff9b_Out_1_Float, _Property_d72ba63042534b0e970e0d84f4bc6d22_Out_0_Float, _Divide_018cce6c4c1f4e8281ded03c51e32fb3_Out_2_Float);
                float _OneMinus_79875b9a312e4dd7a899bdf4cd44b1d8_Out_1_Float;
                Unity_OneMinus_float(_Divide_018cce6c4c1f4e8281ded03c51e32fb3_Out_2_Float, _OneMinus_79875b9a312e4dd7a899bdf4cd44b1d8_Out_1_Float);
                float _Smoothstep_b85d7673ee7e403a9073267bdb6502af_Out_3_Float;
                Unity_Smoothstep_float(float(0), _Property_d62f7a1d803145e4958be54ffaa6593b_Out_0_Float, _OneMinus_79875b9a312e4dd7a899bdf4cd44b1d8_Out_1_Float, _Smoothstep_b85d7673ee7e403a9073267bdb6502af_Out_3_Float);
                float4 _Multiply_b00dbb9038e64554b3cc58803641d526_Out_2_Vector4;
                Unity_Multiply_float4_float4(_Multiply_e95b8e00117a47c6a49a1e0e02ba1175_Out_2_Vector4, (_Smoothstep_b85d7673ee7e403a9073267bdb6502af_Out_3_Float.xxxx), _Multiply_b00dbb9038e64554b3cc58803641d526_Out_2_Vector4);
                float4 _Add_cf80906c6b794fb1a0a5519c7327e08b_Out_2_Vector4;
                Unity_Add_float4(_Multiply_26927c7203054bd2bf63d2f4e793e6b8_Out_2_Vector4, _Multiply_b00dbb9038e64554b3cc58803641d526_Out_2_Vector4, _Add_cf80906c6b794fb1a0a5519c7327e08b_Out_2_Vector4);
                surface.BaseColor = (_Add_cf80906c6b794fb1a0a5519c7327e08b_Out_2_Vector4.xyz);
                surface.Alpha = (_Add_cf80906c6b794fb1a0a5519c7327e08b_Out_2_Vector4).x;
                surface.AlphaClipThreshold = float(1);
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