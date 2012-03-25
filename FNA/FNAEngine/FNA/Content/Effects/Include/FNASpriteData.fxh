/*------------------------------------------------------------------------------------------------------------------------------------
File:			FNASpriteData.fxh
Author:			Jerad Dunn
Last Update:	10/08/11

INSERT DESCRIPTION HERE
------------------------------------------------------------------------------------------------------------------------------------*/

/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Sprite Information
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
float3 SpriteOrigin;				/// Registration point of the sprite (in world units) [bottom-center]
float2 SpriteScreenCoord;			/// The sprite's registration point expressed in screen coordinate space

float SpriteOffsetScalar;			/// Stores the scale that the offset map should be multiplied by (in meters)
									/// NOTE: This needs to match up with the value entered for MaxOffsetCentimeters 
									///       in Maya upon object rendering. Refer to file FNAShader.cgfx

float SpriteWidthCoordScale;		/// Sprite Width / Screen Width
float SpriteHeightCoordScale;		/// Sprite Height / Screen Height

RECTANGLE SpriteRectangle;			/// Corresponds to the C# Rect passed in (kinda...uses floating point values instead of ints)

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