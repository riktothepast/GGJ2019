Shader "Custom/SquareShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
   }
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"
            
            static const float TWO_PI =  6.28318530718;
            
            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            sampler2D _MainTex;

            fixed4 frag (float4 sp:VPOS) : SV_Target
            {
                //normalize the Coordinates min 0 max 1 in both x and y axis
                float2 st = sp.xy/_ScreenParams.xy;
                float4 color = float4(0,0,0,0);
    
    
                float red = (sin(TWO_PI*st.x*_Time[1])+1)/2;
                float alphaRed = (cos(TWO_PI*sin(TWO_PI*st.x*st.y))+1)/2;
        
        
                float blue  = (cos(TWO_PI*st.y*_Time[1])+1)/2;
                float alphaBlue = (cos(TWO_PI*sin(TWO_PI*st.x*sin(TWO_PI*st.x)))+1)/2;
        
                
                color = lerp(float4(red,0,0,alphaRed), float4(0,0,blue,alphaBlue), 0.5);
                return color;
            }
            ENDCG
        }
    }
}
