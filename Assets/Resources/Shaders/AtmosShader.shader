// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "AtmosShader" {
    Properties
    {
        _SunPosition ("Sun Position", Vector) = (0, 0, 0)
        _Radius ("Radius", Float) = 1.5 
        _Strength ("Strength", Float) = 1.0 
        _Color ("Color", Vector) = (0.3, 0.5, 0.9)
    }
    SubShader {
        Tags {"Queue"="Transparent" "RenderType"="Transparent" }
        LOD 100

        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha
        Cull Back 

        Pass {

            CGPROGRAM

            float3 _SunPosition; 
            float _Radius; 
            float _Strength; 

            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct AppData 
            {
                float4 Position : POSITION; 
                float3 Normal : NORMAL; 
            };

            struct V2F 
            {
                float4 Position : SV_POSITION; 
                float3 ModelViewPos : TEXCOORD0; 
                // float3 ViewDir : TEXCOORD1; 
                float3 Normal : NORMAL0; 
                // float3 Color : COLOR0; 
            };

            V2F vert (AppData v)
            {
                V2F o;
                o.Position = UnityObjectToClipPos(v.Position * _Radius);
                o.ModelViewPos = mul(UNITY_MATRIX_MV, float4(v.Position.xyz * _Radius, 1)).xyz;
                // o.ViewDir = normalize(UnityWorldSpaceViewDir(o.WorldPos)); 
                o.Normal = normalize(mul(UNITY_MATRIX_IT_MV, v.Position).xyz); 
                return o;
            }

            float4 frag (V2F i) : SV_Target
            {   
                float3 lightPos = mul(UNITY_MATRIX_V, float4(_SunPosition, 1)); 
                float3 normal = normalize(i.Normal); 
                float3 lightDir = normalize(i.ModelViewPos - lightPos); 

                float diffuse = saturate(dot(-lightDir, normal) + 0.2); 
                // float3 viewDir = -normalize(mul(unity_ObjectToWorld, float4(0, 0, 0, 1)).xyz - _WorldSpaceCameraPos); 
                float3 viewDir = -normalize(i.ModelViewPos); 
                float3 twilight = float3(0.2, 0.1, 0.0); 
                float3 mainColor = float3(0.3, 0.5, 0.9); 
                float colorLerp = saturate(saturate(dot(-lightDir, normal) + 0.2) - saturate(dot(-lightDir, normal) - 0.999)); 
                float3 color = lerp(twilight, mainColor, colorLerp); 

                float viewDotNormal = saturate(dot(viewDir, normal)); 

                float angleKeep = (1 - pow(viewDotNormal - 0.2, 1)) * 1;////pow(0.1 + saturate(1 - dot(i.ViewDir, i.Normal)), 1) * 2+ 0.1; 
                float edge = 1; 
                float limit = 0.3; 
                if (viewDotNormal < limit) 
                {
                    edge = pow(viewDotNormal / limit, 2); 
                }
                // float alpha = saturate(1 * 1 * 1);
                // color = viewDir * 0.5 + 0.5; 
                // color *= diffuse; 
                // color = ; 
                float alpha = _Strength * diffuse * edge * angleKeep; 
                // - pow(1 - diffuse * min(edge, angleKeep), 1); // dot(lightDir, -viewDir); 
                //alpha *= dot(viewDir, i.normal); 

                return float4 (color, alpha);
            }
            ENDCG

        }
    }
}