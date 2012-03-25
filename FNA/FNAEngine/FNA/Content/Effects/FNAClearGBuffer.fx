/*------------------------------------------------------------------------------------------------------------------------------------
File:			FNAClearGBuffer.fx
Author:			Jerad Dunn
Last Update:	10/08/11

Created with help from http://www.catalinzima.com/tutorials/deferred-rendering-in-xna/creating-the-g-buffer/
------------------------------------------------------------------------------------------------------------------------------------*/

#include "Include\FNACommon.fxh"
#include "Include\FNAPostprocessing.fxh"

struct PS_OUTPUT1
{
    float4 ColorData	: COLOR0;
    float4 NormalData	: COLOR1;
    float4 DepthData	: COLOR2;
};

struct PS_OUTPUT2
{
	float4 ColorData	: COLOR0;
    float4 NormalData	: COLOR1;
    float4 PositionData : COLOR2;
};

PS_OUTPUT1 ClearDepthBuffer_PixelShader(VS_OUTPUT IN)
{
    PS_OUTPUT1 OUT = (PS_OUTPUT1)0;
    
	OUT.ColorData = DEFAULT_COLOR_DATA;
	OUT.NormalData = DEFAULT_NORMAL_DATA;
    OUT.DepthData = DEFAULT_DEPTH_DATA;	
    
	return OUT;
}

PS_OUTPUT2 ClearColorNormalBuffers_PixelShader(VS_OUTPUT IN)
{
    PS_OUTPUT2 OUT = (PS_OUTPUT2)0;
    
	OUT.ColorData = DEFAULT_COLOR_DATA;
	OUT.NormalData = DEFAULT_NORMAL_DATA;
	OUT.PositionData = float4(IN.TexCoord.x, IN.TexCoord.y, 0, 1);
    
	return OUT;
}

technique ClearDepthBuffer
{
    pass Pass0
    {
        VertexShader = compile vs_2_0 Postprocessing_VertexShader();
		PixelShader = compile ps_2_0 ClearDepthBuffer_PixelShader();
    }
}

technique ClearColorNormalBuffers
{
    pass Pass0
    {
        VertexShader = compile vs_2_0 Postprocessing_VertexShader();
		PixelShader = compile ps_2_0 ClearColorNormalBuffers_PixelShader();
    }
}