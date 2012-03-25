/*------------------------------------------------------------------------------------------------------------------------------------
File:			FNACommon.fxh
Author:			Jerad Dunn
Last Update:	10/08/11

INSERT DESCRIPTION HERE
------------------------------------------------------------------------------------------------------------------------------------*/

#define DEFAULT_COLOR_DATA			float4(0, 0, 0, 1)				/// Black with no specular intensity
#define DEFAULT_NORMAL_DATA			float4(0.5, 0.5, 0.5, 1)		/// Normals of <0,0,0> with no specular power
#define DEFAULT_DEPTH_DATA			1								/// Maximum depth

/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// A structre that represents a Rectangle within FNA Shaders 
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
struct RECTANGLE
{
	float4 Boundaries;									/// Left bound = X				Right bound = Y				Top bound = Z				Bottom bound = W
};


/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Vertex Shader Input/Output Data Structures
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
struct VS_INPUT
{
	float4 Position			: POSITION0;
	float2 TexCoord			: TEXCOORD0;
	float4 Color			: COLOR0;
};

struct VS_OUTPUT
{
    float4 Position			: POSITION0;
	float4 Color			: COLOR0;
	float2 TexCoord			: TEXCOORD0;
	float2 DepthTexCoord	: TEXCOORD1;
};




/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Camera Stuff (will need to be more robust later)
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
float4x4 CameraTransformationMatrix;			/// Converts the sprite 'vertices' to screen space
float4 CameraInfo;								/// Position = <X,Y,Z>			Far plane = W
RECTANGLE ScreenBounds;							/// The current boundary of the screen
float MaximumZValue;




/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Sprite Information
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
float3 SpriteOrigin;							/// Registration point of the sprite (in world units) [bottom-center]
float2 SpriteScreenCoord;						/// The sprite's registration point expressed in screen coordinate space

float SpriteOffsetScalar;						/// Stores the scale that the offset map should be multiplied by (in meters)
												/// NOTE: This needs to match up with the value entered for MaxOffsetCentimeters 
												///       in Maya upon object rendering. Refer to file FNAShader.cgfx

float SpriteWidthCoordScale;					/// Sprite Width / Screen Width
float SpriteHeightCoordScale;					/// Sprite Height / Screen Height

RECTANGLE SpriteRectangle;						/// Corresponds to the C# Rect passed in (kinda...uses floating point values instead of ints)

/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Sprite Textures
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
texture DiffuseTexture;
sampler DiffuseSampler = sampler_state
{
	Texture = <DiffuseTexture>;
};

texture NormalTexture;
sampler NormalSampler = sampler_state
{
	Texture = <NormalTexture>;
};

texture OffsetTexture;
sampler OffsetSampler = sampler_state
{
	Texture = <OffsetTexture>;
};





/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Utility function to get the position of a specific pixel in world units for sprites with provided offset maps
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
float3 GetWorldPosition(float2 texCoord)
{
	float3 position;
	
	// Getting the offset
	// Converting values from <R,G,B> to proper <X,Y,Z> offset values
	float3 offset = tex2D(OffsetSampler, texCoord);
	offset *= float3(2, -1, 1);
	offset.x -= 1;

	// Scaling to get the correct dimensions
	offset *= SpriteOffsetScalar; 

	// Computing the world space coordinate of this pixel
	position = SpriteOrigin + offset;

	return position;
}


/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Utility function to get the position of a specific pixel in world units for sprites with procedural offset maps
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
float3 GetWorldPositionProcedural(float2 texCoord)
{
	float3 position;
	
	// Procedurally specifying the offset (assuming the object exists in the XZ plane)
	//float3 offset = float3(2 * (texCoord.x - Rectangle.Boundaries.x) - 1, 0, 1 - (texCoord.y - Rectangle.Boundaries.z));
	float3 offset = (float3)0;	
	offset.x = (2 * (texCoord.x - SpriteRectangle.Boundaries.x) / (SpriteRectangle.Boundaries.y - SpriteRectangle.Boundaries.x)) - 1;
	offset.z = 1 - (texCoord.y - SpriteRectangle.Boundaries.z) / (SpriteRectangle.Boundaries.w - SpriteRectangle.Boundaries.z);

	// Scaling to get the correct dimensions
	offset *= SpriteOffsetScalar; 

	// Computing the world space coordinate of this pixel
	position = SpriteOrigin + offset;

	return position;
}





/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Utility function to compute the normalized distance (against the Camera's far plane) between a specific pixel and the camera [for sprites with provided offset maps]
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
float GetNormalizedDepth(float2 texCoord)
{
	// Get the world position of this pixel
	float3 position = GetWorldPosition(texCoord);
    
    // Computing distance to Camera
	float3 vectorToCamera = position - CameraInfo.xyz;
	float distance = length(vectorToCamera);

	// Scaling the distance
	distance /= CameraInfo.w;
	
	return distance;
}


/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Utility function to compute the normalized distance (against the Camera's far plane) between a specific pixel and the camera [for sprites with procedural offset maps]
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
float GetNormalizedDepthProcedural(float2 texCoord)
{
	// Get the world position of this pixel
	float3 position = GetWorldPositionProcedural(texCoord);
    
    // Computing distance to Camera
	float3 vectorToCamera = position - CameraInfo.xyz;
	float distance = length(vectorToCamera);

	// Scaling the distance
	distance /= CameraInfo.w;
	
	return distance;
}





/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Vertex Shader override for FNA sprites
//
// Based on the method presented in:
//		http://blogs.msdn.com/b/shawnhar/archive/2010/04/05/spritebatch-and-custom-shaders-in-xna-game-studio-4-0.aspx
//
// Basically mimics the default vertex shader in the XNA sprite rendering pipeline, but with the addition of
// computing the depth texture coordinate.  This is done in the vertex shader to prevent additional calculations on
// the pixel shader and to avoid creating too complex of a texture-lookup dependency chain for Shader Model 2.0
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
VS_OUTPUT FNACommon_VertexShader(VS_INPUT IN)
{
    VS_OUTPUT OUT = (VS_OUTPUT)0;

	// Convert to screen-space
	OUT.Position = mul(IN.Position, CameraTransformationMatrix);
	OUT.TexCoord = IN.TexCoord;
	OUT.Color = IN.Color;

	// Setting the depth texture coordinate
	OUT.DepthTexCoord = IN.TexCoord * float2(SpriteWidthCoordScale, SpriteHeightCoordScale) + SpriteScreenCoord;
    
	return OUT;
}