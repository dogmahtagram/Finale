/*------------------------------------------------------------------------------------------------------------------------------------
File:			FNARenderGBuffer.fx
Author:			Jerad Dunn
Last Update:	10/08/11

Created with help from http://www.catalinzima.com/tutorials/deferred-rendering-in-xna/creating-the-g-buffer/
------------------------------------------------------------------------------------------------------------------------------------*/

#include "Include\FNACommon.fxh"
#include "Include\FNADepthData.fxh"

struct PS_OUTPUT
{
    float4 ColorData		: COLOR0;
    float4 NormalData		: COLOR1;
	float4 PositionData		: COLOR2;
};


PS_OUTPUT GenerateGBuffer_PixelShader(VS_OUTPUT IN)
{
    // Skip everything if this pixel is transparent
	float4 diffuse = tex2D(DiffuseSampler, IN.TexCoord);
	if (diffuse.a == 0)
	{
		discard;
	}

	// Check to see if the pixel passes the depth test
	// If it does, then the shader will continue
	PerformDepthTest(IN.TexCoord, IN.DepthTexCoord);

	// Set up the output in the case we can draw this pixel
	PS_OUTPUT OUT = (PS_OUTPUT)0;
	OUT.ColorData.rgb = diffuse;
	OUT.ColorData.a = 1;
	OUT.NormalData.rgb = tex2D(NormalSampler, IN.TexCoord);
	OUT.NormalData.a = 1;

	float3 position = GetWorldPosition(IN.TexCoord);
	position.x = (position.x - ScreenBounds.Boundaries.x) / (ScreenBounds.Boundaries.y - ScreenBounds.Boundaries.x);
	position.y = (position.y - ScreenBounds.Boundaries.z) / (ScreenBounds.Boundaries.w - ScreenBounds.Boundaries.z);
	position.z = position.z / MaximumZValue;
	OUT.PositionData = float4(position, 1);
    
	return OUT;
}

PS_OUTPUT GenerateGBufferProcedural_PixelShader(VS_OUTPUT IN)
{
    // Skip everything if this pixel is transparent
	float4 diffuse = tex2D(DiffuseSampler, IN.TexCoord);
	if (diffuse.a == 0)
	{
		discard;
	}

	// Check to see if the pixel passes the depth test
	// If it does, then the shader will continue
	PerformDepthTestProcedural(IN.TexCoord, IN.DepthTexCoord);

	// Set up the output in the case we can draw this pixel
	PS_OUTPUT OUT = (PS_OUTPUT)0;
	OUT.ColorData.rgb = diffuse;
	OUT.ColorData.a = 1;
	OUT.NormalData.rgb = float3(0.5, 1, 0.5);
	OUT.NormalData.a = 1;

	float3 position = GetWorldPositionProcedural(IN.TexCoord);
	position.x = (position.x - ScreenBounds.Boundaries.x) / (ScreenBounds.Boundaries.y - ScreenBounds.Boundaries.x);
	position.y = (position.y - ScreenBounds.Boundaries.z) / (ScreenBounds.Boundaries.w - ScreenBounds.Boundaries.z);
	position.z = position.z / MaximumZValue;
	OUT.PositionData = float4(position, 1);
    
	return OUT;
}

technique GenerateGBuffer
{
    pass Pass0
    {
        //CullMode = NONE;
		//ZEnable = TRUE;
		//ZWriteEnable = TRUE;
		//AlphaBlendEnable = FALSE;
		//AlphaBlendEnable = TRUE;
		
		VertexShader = compile vs_2_0 FNACommon_VertexShader();
		PixelShader = compile ps_2_0 GenerateGBuffer_PixelShader();
    }
}

technique GenerateGBufferProcedural
{
    pass Pass0
    {
        //CullMode = NONE;
		//ZEnable = TRUE;
		//ZWriteEnable = TRUE;
		//AlphaBlendEnable = FALSE;
		//AlphaBlendEnable = TRUE;
		
		VertexShader = compile vs_2_0 FNACommon_VertexShader();
		PixelShader = compile ps_2_0 GenerateGBufferProcedural_PixelShader();
    }
}