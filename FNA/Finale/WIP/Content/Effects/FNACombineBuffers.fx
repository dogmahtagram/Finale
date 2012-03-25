/*------------------------------------------------------------------------------------------------------------------------------------
File:			FNACombineBuffers.fx
Author:			Jerad Dunn
Last Update:	10/08/11

Created with help from http://www.catalinzima.com/tutorials/deferred-rendering-in-xna/creating-the-g-buffer/
THis is a postprocessing shader, so we don't need to use the special FNA vertex shader
------------------------------------------------------------------------------------------------------------------------------------*/

#include "Include\FNACommon.fxh"
#include "Include\FNAPostprocessing.fxh"
#include "Include\FNAGBufferData.fxh"

struct PS_OUTPUT
{
    float4 Color : COLOR0;
};

PS_OUTPUT CombineBuffers_PixelShader(VS_OUTPUT IN)
{
    PS_OUTPUT OUT = (PS_OUTPUT)0;
    
	float3 diffuse = tex2D(ColorBufferSampler, IN.TexCoord).rgb;
	float3 light = tex2D(LightBufferSampler, IN.TexCoord).rgb;
	// later do stuff with specular

	OUT.Color.rgb = diffuse * light;
	OUT.Color.a = 1;
    
	return OUT;
}

// Post-processing event...so we can use the default vertex shader
technique CombineBuffers
{
    pass Pass0
    {
        CullMode = NONE;
		ZEnable = TRUE;
		ZWriteEnable = TRUE;
		AlphaBlendEnable = FALSE;
		
		VertexShader = compile vs_2_0 Postprocessing_VertexShader();
		PixelShader = compile ps_2_0 CombineBuffers_PixelShader();
    }
}