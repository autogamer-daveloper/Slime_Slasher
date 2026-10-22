
Shader "AGamesStudio/2D/Pond Animation Lit"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)

        _Frame0 ("Water Frame 1", 2D) = "white" {}
        _Frame1 ("Water Frame 2", 2D) = "white" {}
        _Frame2 ("Water Frame 3", 2D) = "white" {}
        _Frame3 ("Water Frame 4", 2D) = "white" {}

        _FrameDuration ("Frame Duration", Float) = 0.5
        _MaskTex ("Light Blend Mask", 2D) = "white" {}

        [Toggle] _UseTilemapUV ("Use Tilemap Cell UV", Float) = 0
        _TileSize ("Tile Cell Size (Local Units)", Vector) = (1,1,0,0)

        [HideInInspector] _RendererColor ("Renderer Color", Color) = (1,1,1,1)
        [HideInInspector] _Flip ("Flip", Vector) = (1,1,1,1)
        [HideInInspector] _AlphaTex ("External Alpha", 2D) = "white" {}
        [HideInInspector] _EnableExternalAlpha ("Enable External Alpha", Float) = 0
    }

    SubShader
    {
        Tags
        {
            "Queue" = "Transparent"
            "RenderType" = "Transparent"
            "RenderPipeline" = "UniversalPipeline"
            "CanUseSpriteAtlas" = "True"
        }

        Blend SrcAlpha OneMinusSrcAlpha
        Cull Off
        ZWrite Off

        Pass
        {
            Tags { "LightMode" = "Universal2D" }

            HLSLPROGRAM

            #pragma vertex Vert
            #pragma fragment Frag

            #pragma multi_compile USE_SHAPE_LIGHT_TYPE_0 __
            #pragma multi_compile USE_SHAPE_LIGHT_TYPE_1 __
            #pragma multi_compile USE_SHAPE_LIGHT_TYPE_2 __
            #pragma multi_compile USE_SHAPE_LIGHT_TYPE_3 __
            #pragma multi_compile _ DEBUG_DISPLAY
            #pragma multi_compile_instancing

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/Shaders/2D/Include/InputData2D.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/Shaders/2D/Include/SurfaceData2D.hlsl"

            TEXTURE2D(_Frame0);
            SAMPLER(sampler_Frame0);

            TEXTURE2D(_Frame1);
            SAMPLER(sampler_Frame1);

            TEXTURE2D(_Frame2);
            SAMPLER(sampler_Frame2);

            TEXTURE2D(_Frame3);
            SAMPLER(sampler_Frame3);

            TEXTURE2D(_MaskTex);
            SAMPLER(sampler_MaskTex);

            CBUFFER_START(UnityPerMaterial)
                float4 _MainTex_ST;
                half4 _Color;
                float _FrameDuration;
                float _UseTilemapUV;
                float4 _TileSize;
            CBUFFER_END

            struct Attributes
            {
                float3 positionOS : POSITION;
                half4 color : COLOR;
                float2 uv : TEXCOORD0;

                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                half4 color : COLOR;
                float2 uv : TEXCOORD0;
                half2 lightingUV : TEXCOORD1;
                float2 tileCoords : TEXCOORD2;

                UNITY_VERTEX_OUTPUT_STEREO
            };

            Varyings Vert(Attributes input)
            {
                Varyings output = (Varyings)0;

                UNITY_SETUP_INSTANCE_ID(input);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(output);

                output.positionCS =
                    TransformObjectToHClip(input.positionOS);

                output.uv = TRANSFORM_TEX(input.uv, _MainTex);
                output.color = input.color * _Color;

                output.lightingUV = ComputeScreenPos(
                    output.positionCS / output.positionCS.w
                ).xy;

                // Координаты в локальном пространстве для Tilemap.
                float2 safeTileSize = max(
                    _TileSize.xy,
                    float2(0.001, 0.001)
                );

                output.tileCoords =
                    input.positionOS.xy / safeTileSize;

                return output;
            }

            #include "Packages/com.unity.render-pipelines.universal/Shaders/2D/Include/CombinedShapeLightShared.hlsl"

            half4 Frag(Varyings input) : SV_Target
            {
                UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);

                // По умолчанию используем UV исходного спрайта.
                float2 sampleUV = input.uv;

                // Для Tilemap растягиваем текстуру на каждую клетку.
                if (_UseTilemapUV > 0.5)
                {
                    sampleUV = frac(input.tileCoords);
                }

                // Смена кадров синхронно для всех объектов.
                float duration = max(_FrameDuration, 0.001);
                uint frame = (uint)floor(_Time.y / duration) % 4u;

                half4 water;

                if (frame == 0u)
                    water = SAMPLE_TEXTURE2D(
                        _Frame0, sampler_Frame0, sampleUV);
                else if (frame == 1u)
                    water = SAMPLE_TEXTURE2D(
                        _Frame1, sampler_Frame1, sampleUV);
                else if (frame == 2u)
                    water = SAMPLE_TEXTURE2D(
                        _Frame2, sampler_Frame2, sampleUV);
                else
                    water = SAMPLE_TEXTURE2D(
                        _Frame3, sampler_Frame3, sampleUV);

                water *= input.color;

                SurfaceData2D surfaceData;
                InputData2D inputData;

                half4 mask = SAMPLE_TEXTURE2D(
                    _MaskTex,
                    sampler_MaskTex,
                    sampleUV
                );

                InitializeSurfaceData(
                    water.rgb,
                    water.a,
                    mask,
                    surfaceData
                );

                InitializeInputData(
                    sampleUV,
                    input.lightingUV,
                    inputData
                );

                return CombinedShapeLightShared(
                    surfaceData,
                    inputData
                );
            }

            ENDHLSL
        }
    }

    Fallback "Sprites/Default"
}
