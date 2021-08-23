// Unity built-in shader source. Copyright (c) 2016 Unity Technologies. MIT license (see license.txt)

// Unlit shader. Simplest possible textured shader.
// - no lighting
// - no lightmap support
// - no per-material color

Shader "Light2D/Unlit/Texture" {
Properties {
    _MainTex ("Base (RGB)", 2D) = "white" {}
    _Lit ("Lit", Range(0,1)) = 1
	
    // Camera 1
    [HideInInspector] _Cam1_Texture ("Camera 1 Texture", 2D) = "white" {}
    [HideInInspector] _Cam1_Rect ("Camera 1 Rect", Vector) = (0, 0, 0, 0)
    [HideInInspector] _Cam1_Rotation ("Camera 1 Rotation", Float) = 0

    // Camera 2
    [HideInInspector] _Cam2_Texture ("Camera 2 Texture", 2D) = "white" {}
    [HideInInspector] _Cam2_Rect ("Camera 2 Rect", Float) = (0, 0, 0, 0)
    [HideInInspector] _Cam2_Rotation ("Camera 2 Rotation", Float) = 0
}

SubShader {
    Tags { "RenderType"="Opaque" }
    LOD 100

    Pass {
        CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 2.0
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata_t {
                float4 vertex : POSITION;
                float2 texcoord : TEXCOORD0;
                
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f {
                float4 vertex : SV_POSITION;
                float2 texcoord : TEXCOORD0;
                float2 worldPos : TEXCOORD1;
                UNITY_FOG_COORDS(1)
                UNITY_VERTEX_OUTPUT_STEREO
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            float _Lit;

            // Cam 1
			sampler2D _Cam1_Texture;
            vector _Cam1_Rect;
            float _Cam1_Rotation;

            // Cam 2
			sampler2D _Cam2_Texture;
			vector _Cam2_Rect;
            float _Cam2_Rotation;

            bool InCamera (float2 pos, float2 rectPos, float2 rectSize) {
				rectPos -= rectSize / 2;
                return (pos.x < rectPos.x || pos.x > rectPos.x + rectSize.x || pos.y < rectPos.y || pos.y > rectPos.y + rectSize.y) == false;
            }

			float2 TransformToCamera(float2 pos, float rotation) {
				float c = cos(-rotation);
				float s = sin(-rotation);
		
				float x = pos.x;
				float y = pos.y;

				pos.x = x * c - y * s;
				pos.y = x * s + y * c;

                return(pos);
            }

			fixed4 LightColor(float id, float2 texcoord) {
				if (id < 0.5f) {
					return(tex2D (_Cam1_Texture, texcoord));
				} else if (id < 1.5f) {
					return(tex2D (_Cam2_Texture, texcoord));
				}
				
				return(fixed4(0, 0, 0, 0));
			}

			fixed4 InputColor(v2f IN) {
				return(tex2D (_MainTex, IN.texcoord));
			}

            fixed4 OutputColor(v2f IN, float2 posInCamera, float2 cameraSize, float id) {
				float2 texcoord = (posInCamera + cameraSize / 2) / cameraSize;
			
				fixed4 lightPixel = LightColor(id, texcoord);
				fixed4 spritePixel = InputColor(IN);

				lightPixel = lerp(lightPixel, fixed4(1, 1, 1, 1), 1 - _Lit);

				spritePixel = spritePixel * lightPixel;
				spritePixel.rgb *= spritePixel.a; 
				
				//spritePixel.rgb =  pow(spritePixel.rgb, 1/2.2);
				
				return spritePixel;
			}

            v2f vert (appdata_t v)
            {
                v2f o;
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);

                o.worldPos = mul (unity_ObjectToWorld, v.vertex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f IN) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, IN.texcoord);
                
                if (_Cam1_Rect.z > 0) {
					float2 camera_1_Size = float2(_Cam1_Rect.z, _Cam1_Rect.w);
					float2 posInCamera1 = TransformToCamera(IN.worldPos - float2(_Cam1_Rect.x, _Cam1_Rect.y),  _Cam1_Rotation);

					if (InCamera(posInCamera1, float2(0, 0), camera_1_Size)) {
                    	col = OutputColor(IN, posInCamera1, camera_1_Size, 0);
					}
				}

				if (_Cam2_Rect.z > 0) {
					float2 camera_2_Size = float2(_Cam2_Rect.z, _Cam2_Rect.w);
					float2 posInCamera2 = TransformToCamera(IN.worldPos - float2(_Cam2_Rect.x, _Cam2_Rect.y),  _Cam2_Rotation);
					
					if (InCamera(posInCamera2, float2(0, 0), camera_2_Size)) {
						col = OutputColor(IN, posInCamera2, camera_2_Size, 1);
					}
                }

                UNITY_APPLY_FOG(IN.fogCoord, col);
                UNITY_OPAQUE_ALPHA(col.a);
                return col;
            }
        ENDCG
    }
}

}