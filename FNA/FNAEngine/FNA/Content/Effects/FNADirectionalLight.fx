/*------------------------------------------------------------------------------------------------------------------------------------
File:			FNADirectionalLight.fx
Author:			Jerad Dunn
Last Update:	10/08/11

Created with help from http://www.catalinzima.com/tutorials/deferred-rendering-in-xna/creating-the-g-buffer/
THis is a postprocessing shader, so we don't need to use the special FNA vertex shader
------------------------------------------------------------------------------------------------------------------------------------*/

#include "Include\FNACommon.fxh"
#include "Include\FNALighting.fxh"
#include "Include\FNAPostprocessing.fxh"
#include "Include\FNAGBufferData.fxh"

struct PS_OUTPUT
{
    float4 Color : COLOR0;
};

DIRECTIONAL_LIGHT LightSource;

PS_OUTPUT DirectionalLight_PixelShader(VS_OUTPUT IN)
{
    PS_OUTPUT OUT = (PS_OUTPUT)0;
    
	// Get normal data from the normal buffer
	float3 normal = tex2D(NormalBufferSampler, IN.TexCoord).rgb;

	// Converting values ranging from [0,1] to [-1,1]
	normal = 2 * normal - 1;

	float NdotL = max(0, dot(normal, -LightSource.Direction));

	OUT.Color.rgb = LightSource.Color.a * NdotL * LightSource.Color.rgb;
	//OUT.Color.a = LightSource.Color.a;
	OUT.Color.a = 0;
    
	return OUT;
}

// Post-processing event...so we can use the default vertex shader
technique DirectionalLight
{
    pass Pass0
    {
        //CullMode = NONE;
		//ZEnable = TRUE;
		//ZWriteEnable = TRUE;
		//AlphaBlendEnable = FALSE;
		
		VertexShader = compile vs_2_0 Postprocessing_VertexShader();
		PixelShader = compile ps_2_0 DirectionalLight_PixelShader();
    }
}