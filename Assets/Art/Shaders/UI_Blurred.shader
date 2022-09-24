Shader "Unlit/UI_Blurred"
{
  Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _BlurSize("BlurSize",float)=0
    }
    SubShader
    {
        Tags
        {
            "Queue"="Transparent"
            "IgnoreProjector" = "true"
            "RenderType" = "Transparent"
            "RenderPipelint" = "UniversalPipeline"
        }

        Blend SrcAlpha OneMinusSrcAlpha
        Cull Off
        ZWrite Off

        // #0
        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment fragHorizontal
            #include "./UI_Blurred_Base.hlsl"
            ENDHLSL
        }

        // #1
        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment fragVertical
            #include "./UI_Blurred_Base.hlsl"
            ENDHLSL
        }
    } 
}
