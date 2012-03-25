/*------------------------------------------------------------------------------------------------------------------------------------
File:			FNACreateDepthBuffer.fx
Author:			Jerad Dunn
Last Update:	10/08/11

INSERT DESCRIPTION HERE
------------------------------------------------------------------------------------------------------------------------------------*/

#include "Include\FNACommon.fxh"

struct PS_OUTPUT
{
	float4 ColorData	: COLOR0;
	float4 NormalData	: COLOR1;
	float4 DepthData	: COLOR2;
	float Depth			: DEPTH;
};

/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Pixel Shader to create the Depth Map of the scene (for sprites with provided offset maps)
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
PS_OUTPUT CreateDepthMap_PixelShader(VS_OUTPUT IN)
{
    // Skip everything if this pixel is transparent
	if (tex2D(DiffuseSampler, IN.TexCoord).a == 0)
	{
		discard;
	}

	// Set up the output
	PS_OUTPUT OUT = (PS_OUTPUT)0;

	// This is a hack so that the HLSL compiler won't yell at us...
	// we must set all of the other colors as well...so set them to the defaults
	OUT.ColorData = DEFAULT_COLOR_DATA;
	OUT.NormalData = DEFAULT_NORMAL_DATA;

	// Find and set the depth
	// The depth stencil buffer will either reject or accept this pixel operation
	OUT.Depth = GetNormalizedDepth(IN.TexCoord);	
	OUT.DepthData = float4(OUT.Depth, 0, 0, 0);

	return OUT;
}


/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Pixel Shader to create the Depth Map of the scene (for sprites with procedural offset maps)
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
PS_OUTPUT CreateDepthMapProcedural_PixelShader(VS_OUTPUT IN)
{
    // Skip everything if this pixel is transparent
	if (tex2D(DiffuseSampler, IN.TexCoord).a == 0)
	{
		discard;
	}

	// Set up the output
	PS_OUTPUT OUT = (PS_OUTPUT)0;

	// This is a hack so that the HLSL compiler won't yell at us...
	// we must set all of the other colors as well...so set them to the defaults
	OUT.ColorData = DEFAULT_COLOR_DATA;
	OUT.NormalData = DEFAULT_NORMAL_DATA;

	// Find and set the depth
	// The depth stencil buffer will either reject or accept this pixel operation
	OUT.Depth = GetNormalizedDepthProcedural(IN.TexCoord);	
	OUT.DepthData = float4(OUT.Depth, 0, 0, 0);

	return OUT;
}


/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Technique for creating the depth map for sprites with provided normal and offset maps
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
technique CreateDepthBuffer
{
    pass Pass0
    {
        CullMode = NONE;
		ZEnable = TRUE;
		ZWriteEnable = TRUE;
		AlphaBlendEnable = FALSE;
		
		VertexShader = compile vs_2_0 FNACommon_VertexShader();
		PixelShader = compile ps_2_0 CreateDepthMap_PixelShader();
    }
}


/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Technique for creating the depth map for sprites with procedural normal and offset maps
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
technique CreateDepthBufferProcedural
{
    pass Pass0
    {
        CullMode = NONE;
		ZEnable = TRUE;
		ZWriteEnable = TRUE;
		AlphaBlendEnable = FALSE;
		
		VertexShader = compile vs_2_0 FNACommon_VertexShader();
		PixelShader = compile ps_2_0 CreateDepthMapProcedural_PixelShader();
    }
}