/*------------------------------------------------------------------------------------------------------------------------------------
File:			FNAAmbientLight.fx
Author:			Jerad Dunn
Last Update:	10/08/11

Created with help from http://www.catalinzima.com/tutorials/deferred-rendering-in-xna/creating-the-g-buffer/
THis is a postprocessing shader, so we don't need to use the special FNA vertex shader
------------------------------------------------------------------------------------------------------------------------------------*/

#include "Include\FNACommon.fxh"
#include "Include\FNALighting.fxh"
#include "Include\FNAPostprocessing.fxh"

struct PS_OUTPUT
{
    float4 Color : COLOR0;
};

AMBIENT_LIGHT LightSource;

PS_OUTPUT AmbientLight_PixelShader(VS_OUTPUT IN)
{
    PS_OUTPUT OUT = (PS_OUTPUT)0;
    
	OUT.Color.rgb = LightSource.Color.a * LightSource.Color.rgb;
	//OUT.Color.a = LightSource.Color.a;
	OUT.Color.a = 0;
    
	return OUT;
}

// Post-processing event...so we can use the default vertex shader
technique AmbientLight
{
    pass Pass0
    {
        //CullMode = NONE;
		//ZEnable = TRUE;
		//ZWriteEnable = TRUE;
		//AlphaBlendEnable = FALSE;
		
		VertexShader = compile vs_2_0 Postprocessing_VertexShader();
		PixelShader = compile ps_2_0 AmbientLight_PixelShader();
    }
}