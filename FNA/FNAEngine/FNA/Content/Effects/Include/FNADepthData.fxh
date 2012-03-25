/*------------------------------------------------------------------------------------------------------------------------------------
File:			FNADepthData.fxh
Author:			Jerad Dunn
Last Update:	10/08/11

INSERT DESCRIPTION HERE
------------------------------------------------------------------------------------------------------------------------------------*/

#define DEPTH_BIAS				0.001f					/// Used to prevent floating point errors that occur when the pixel of the occluder is being drawn

texture DepthTexture;									/// Generated and read by this shader
sampler DepthSampler = sampler_state
{
	Texture = <DepthTexture>;
	Filter = POINT;

	AddressU = Clamp;
	AddressV = Clamp;
	AddressW = Clamp;
};

/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Accepts/Rejects a pixel of a sprite (with provided offset map) based on the depth map generated for the scene
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
void PerformDepthTest(float2 texCoord, float2 depthTexCoord)
{
	// Get the biased depth
	float depth = GetNormalizedDepth(texCoord);
	// The bias is used to prevent folating point errors that occur when the pixel of the occluder is being drawn
	depth -= DEPTH_BIAS;

	// If depth is greater than is what in map, discard it
	float nearestDepth = tex2D(DepthSampler, depthTexCoord).r;	
	if (depth > nearestDepth)
	{
		discard;
	}
}


/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Accepts/Rejects a pixel of a sprite (with procedural offset map) based on the depth map generated for the scene
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
void PerformDepthTestProcedural(float2 texCoord, float2 depthTexCoord)
{
	// Get the biased depth
	float depth = GetNormalizedDepthProcedural(texCoord);
	// The bias is used to prevent folating point errors that occur when the pixel of the occluder is being drawn
	depth -= DEPTH_BIAS;

	// If depth is greater than is what in map, discard it
	float nearestDepth = tex2D(DepthSampler, depthTexCoord).r;	
	if (depth > nearestDepth)
	{
		discard;
	}
}