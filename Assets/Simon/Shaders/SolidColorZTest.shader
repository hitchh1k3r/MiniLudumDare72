Shader "Custom/UIColor"
{
  Properties {}

  SubShader
  {
    Tags { "RenderType"="Overlay" "Queue"="Overlay" }
    LOD 100

    Pass
    {
      Cull Off
      Lighting Off
      ZWrite On
      ZTest LEqual
      Fog { Mode Off }
      Blend SrcAlpha OneMinusSrcAlpha

      CGPROGRAM
        #pragma vertex vert
        #pragma fragment frag

        #include "UnityCG.cginc"

        struct appdata
        {
          float4 vertex : POSITION;
          float4 color  : COLOR;
        };

        struct v2f
        {
          float4 vertex : SV_POSITION;
          float4 color  : COLOR0;
        };

        v2f vert (appdata i)
        {
          v2f o;
          o.vertex = UnityObjectToClipPos(i.vertex);
          o.color = i.color;
          return o;
        }

        fixed4 frag (v2f i) : SV_Target
        {
          return i.color;
        }
      ENDCG
    }
  }
}
