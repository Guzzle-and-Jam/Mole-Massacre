// Creates an outline on applied meshes.
Shader "Unlit/OutlineShader"
{
    Properties
    {
        _OutlineColor ("Outline Color", Color) = (0, 0, 0, 1)
        _OutlineThickness("Outline Thickness", Range(0, 0.08)) = 0.03
        _OutlineToggle("Outline Toggle", Range(0, 1)) = 0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass {
            // Remove all pixels facing towards the camera, this was only the back pixels remain creating an illusion 
            // of an outline.
            Cull front
            
            CGPROGRAM
            // Physically based Standard lighting model, and enable shadows on all light types
            #include "UnityCG.cginc"

            // Use shader model 3.0 target, to get nicer looking lighting
            #pragma vertex vert
            #pragma fragment frag

            fixed4 _OutlineColor;
            float _OutlineThickness;
            float _OutlineToggle;

            // data put into vertex
            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };

            // data outputted by vertex shader and goes into frag.
            struct v2f
            {
                float4 position : SV_POSITION;
            };

            v2f vert(appdata v)
            {
                v2f o;
                // gets vertex positions and converts them to clip space for rendering.
                o.position = UnityObjectToClipPos(v.vertex + normalize(v.normal) * _OutlineThickness);
                return o;
            }
            
            fixed4 frag(v2f i) : SV_TARGET{
                if(_OutlineToggle == 0)
                {
                    discard;
                }
                return  _OutlineColor;
            }
            
            ENDCG
            
            }
    }
}
