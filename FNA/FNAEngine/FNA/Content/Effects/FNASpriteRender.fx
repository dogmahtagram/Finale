/*------------------------------------------------------------------------------------------------------------------------------------
File:			FNASpriteRender.fx
Author:			Jerad Dunn
Last Update:	09/11/11

Note, for now, we are limited to 3 point lights, and we are dangerously close to our instruction count limit for Shader Model 2.0
Currently, this works for now though, so let's keep our fingers crossed.
If for some reason we need more than 3 point lights, we have two options:
	1) look for ways to optimize the shit out of this shader
	2) look into deferred rendering

I'm sure that this can be further optimzed later, but for now, this'll have to do.  At least it works.

Currently supported:
	- Ambient, Directional, and (up to 3) Point lighting on a per-pixel basis
	- Depth map generation
	- If no normal or offset maps are provided, this shader will procedurally generate normals and offsets for each pixel
		- assumes that an object is simply a plane in XZ space
------------------------------------------------------------------------------------------------------------------------------------*/

#define DEPTH_BIAS				0.001f					/// Used to prevent floating point errors that occur when the pixel of the occluder is being drawn
#define MAX_NUM_POINT_LIGHTS	2


/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// 
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
struct RECTANGLE
{
	float4 Boundaries;									/// Left bound = X				Right bound = Y				Top bound = Z				Bottom bound = W
};


/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Light Structs (if these are ever updated, update these on code side as well (and vice-versa)
// [DirectionalLightComponent.cs, PointLightComponent.cs, and AmbientLightComponent.cs, respectively]
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
struct DIRECTIONAL_LIGHT
{
	float4 Color;										/// Light color = <R,G,B>		Intensity = A
	float3 Direction;
};

struct POINT_LIGHT
{
	float4 Position;									/// Light position = <X,Y,Z>	Radius = W
	float4 Color;										/// Light color = <R,G,B>		Intensity = A
};

struct AMBIENT_LIGHT
{
	float4 Color;										/// Light color = <R,G,B>		Intensity = A
};


/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Camera Stuff (will need to be more robust later)
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
float4x4 CameraTransformationMatrix;					/// Converts the sprite 'vertices' to screen space
float4 CameraInfo;										/// Position = <X,Y,Z>			Far plane = W


/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Scene Lights
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
AMBIENT_LIGHT AmbientLight;
DIRECTIONAL_LIGHT DirectionalLight;
POINT_LIGHT PointLights[MAX_NUM_POINT_LIGHTS];


/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Sprite Information
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
float3 SpriteOrigin;									/// Registration point of the sprite (in world units) [bottom-center]
float2 SpriteScreenCoord;								/// The sprite's registration point expressed in screen coordinate space

float SpriteOffsetScalar;								/// Stores the scale that the offset map should be multiplied by (in meters)
														/// NOTE: This needs to match up with the value entered for MaxOffsetCentimeters 
														//        in Maya upon object rendering. Refer to file FNAShader.cgfx

float SpriteWidthCoordScale;							/// Sprite Width / Screen Width
float SpriteHeightCoordScale;							/// Sprite Height / Screen Height

RECTANGLE Rectangle;									/// Corresponds to the C# Rect passed in (kinda...uses floating point values instead of ints)


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
// Input/Output Data Structures
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
struct VS_OUTPUT
{
    float4 Position			: POSITION0;
	float4 Color			: COLOR0;
	float2 TexCoord			: TEXCOORD0;
	float2 DepthTexCoord	: TEXCOORD1;
};

struct CREATEDEPTHMAP_PS_OUTPUT
{
	float4 PixelColor		: COLOR;
	float Depth				: DEPTH;
};

struct PS_OUTPUT
{
	float4 PixelColor		: COLOR;
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
	offset.x = (2 * (texCoord.x - Rectangle.Boundaries.x) / (Rectangle.Boundaries.y - Rectangle.Boundaries.x)) - 1;
	offset.z = 1 - (texCoord.y - Rectangle.Boundaries.z) / (Rectangle.Boundaries.w - Rectangle.Boundaries.z);

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
// Hey, look, I actually used something from Paul's shader class!
// Thanks, Paul!
// Utility function to return the total lighting contribution of a light on a surface with the given color
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
float4 GetColorContribution(float4 lightColor, float4 surfaceColor)
{
	return float4(lightColor.rgb * lightColor.a, 1.0f) * surfaceColor;
}


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
		//discard;
	}
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
VS_OUTPUT FNASprite_VertexShader(float4 Position : POSITION0, float2 TexCoord : TEXCOORD0, float4 Color : COLOR0)
{
    VS_OUTPUT OUT = (VS_OUTPUT)0;

	// Convert to screen-space
	OUT.Position = mul(Position, CameraTransformationMatrix);
	OUT.TexCoord = TexCoord;
	OUT.Color = Color;

	// Setting the depth texture coordinate
	OUT.DepthTexCoord = TexCoord * float2(SpriteWidthCoordScale, SpriteHeightCoordScale) + SpriteScreenCoord;
    
	return OUT;
}


/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Pixel Shader to create the Depth Map of the scene (for sprites with provided offset maps)
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
CREATEDEPTHMAP_PS_OUTPUT CreateDepthMap_PixelShader(VS_OUTPUT IN)
{
    // Skip everything if this pixel is transparent
	if (tex2D(DiffuseSampler, IN.TexCoord).a == 0)
	{
		discard;
	}

	// Set up the output
	CREATEDEPTHMAP_PS_OUTPUT OUT = (CREATEDEPTHMAP_PS_OUTPUT)0;

	// Find and set the depth
	// The depth stencil buffer will either reject or accept this pixel operation
	OUT.Depth = GetNormalizedDepth(IN.TexCoord);	
	OUT.PixelColor = float4(OUT.Depth, 0, 0, 0);

	return OUT;
}


/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Pixel Shader to create the Depth Map of the scene (for sprites with procedural offset maps)
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
CREATEDEPTHMAP_PS_OUTPUT CreateDepthMapProcedural_PixelShader(VS_OUTPUT IN)
{
    // Skip everything if this pixel is transparent
	if (tex2D(DiffuseSampler, IN.TexCoord).a == 0)
	{
		discard;
	}

	// Set up the output
	CREATEDEPTHMAP_PS_OUTPUT OUT = (CREATEDEPTHMAP_PS_OUTPUT)0;

	// Find and set the depth
	// The depth stencil buffer will either reject or accept this pixel operation
	OUT.Depth = GetNormalizedDepthProcedural(IN.TexCoord);	
	OUT.PixelColor = float4(OUT.Depth, 0, 0, 0);

	return OUT;
}


/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Pixel Shader for our lit sprite rendering technique (using provided normal and offset maps)
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
PS_OUTPUT LitSpriteRender_PixelShader(VS_OUTPUT IN, uniform int NUM_LIGHTS)
{
	// Skip everything if this pixel is transparent
	float4 diffuse = tex2D(DiffuseSampler, IN.TexCoord);
	if (diffuse.a == 0)
	{
		discard;
	}

	// Set up the output in the case we can draw this pixel
	PS_OUTPUT OUT = (PS_OUTPUT)0;
	OUT.PixelColor.rgb = diffuse;
	OUT.PixelColor.a = 1;

	// Check to see if the pixel passes the depth test
	// If it does, then the shader will continue
	PerformDepthTest(IN.TexCoord, IN.DepthTexCoord);	

	// Now look up normal from the normal map
	float3 normal = tex2D(NormalSampler, IN.TexCoord);	
	// converting values ranging from [0, 1] to [-1, 1]
	normal = normal * 2 - 1;

	// NOTE: This segment of code is not split out into its own function, because compiling it in the main loop
	//		 would cause the inner for() loop to unroll, giving us better performance.  This cannot happen through
	//		 a function call.
	// Compute ambient lighting contribution.
	float4 ambientLightContribution = GetColorContribution(AmbientLight.Color, diffuse);

	// Compute directional lighting contribution
	float n_dot_l = max(dot(normal, -DirectionalLight.Direction), 0);
	float4 directionalLightContribution = GetColorContribution(DirectionalLight.Color, n_dot_l * diffuse);

	// Compute light contribution from the point lights
	// NOTE: This loop will be un-rolled at shader compile time
	float3 position = GetWorldPosition(IN.TexCoord);
	float4 totalPointLightContribution = float4(0, 0, 0, 1);

	for (int i = 0; i < NUM_LIGHTS; i++)
	{
		float3 lightDirection = PointLights[i].Position.xyz - position;
		float lightDistance = length(lightDirection);
		lightDirection = normalize(lightDirection);

		n_dot_l = max(dot(normal, lightDirection), 0);
		float attenuation = saturate(1.0 - lightDistance / PointLights[i].Position.w);
		totalPointLightContribution += GetColorContribution(PointLights[i].Color, attenuation * n_dot_l * diffuse);
	}
    
	// set color		
	OUT.PixelColor.rgb = ambientLightContribution + directionalLightContribution + totalPointLightContribution;

	return OUT;
}


/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Pixel Shader for our lit sprite rendering technique (using procedural normal and offset maps)
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
PS_OUTPUT LitSpriteRenderProcedural_PixelShader(VS_OUTPUT IN, uniform int NUM_LIGHTS)
{
	// Skip everything if this pixel is transparent
	float4 diffuse = tex2D(DiffuseSampler, IN.TexCoord);
	if (diffuse.a == 0)
	{
		discard;
	}

	// Set up the output in the case we can draw this pixel
	PS_OUTPUT OUT = (PS_OUTPUT)0;
	OUT.PixelColor.rgb = diffuse;
	OUT.PixelColor.a = 1;

	// Check to see if the pixel passes the depth test
	// If it does, then the shader will continue
	PerformDepthTestProcedural(IN.TexCoord, IN.DepthTexCoord);	

	// Specifying the normal for this point. Procedural sprites are always facing in the positive-Y direction
	float3 normal = float3(0.0, 1.0, 0.0);

	// NOTE: This segment of code is not split out into its own function, because compiling it in the main loop
	//		 would cause the inner for() loop to unroll, giving us better performance.  This cannot happen through
	//		 a function call.
	// Compute ambient lighting contribution.
	float4 ambientLightContribution = GetColorContribution(AmbientLight.Color, diffuse);

	// Compute directional lighting contribution
	float n_dot_l = max(dot(normal, -DirectionalLight.Direction), 0);
	float4 directionalLightContribution = GetColorContribution(DirectionalLight.Color, n_dot_l * diffuse);

	// Compute light contribution from the point lights
	// NOTE: This loop will be un-rolled at shader compile time
	float3 position = GetWorldPositionProcedural(IN.TexCoord);
	float4 totalPointLightContribution = float4(0, 0, 0, 1);

	for (int i = 0; i < NUM_LIGHTS; i++)
	{
		float3 lightDirection = PointLights[i].Position.xyz - position;
		float lightDistance = length(lightDirection);
		lightDirection = normalize(lightDirection);

		n_dot_l = max(dot(normal, lightDirection), 0);
		float attenuation = saturate(1.0 - lightDistance / PointLights[i].Position.w);
		totalPointLightContribution += GetColorContribution(PointLights[i].Color, attenuation * n_dot_l * diffuse);
	}
    
	// set color		
	OUT.PixelColor.rgb = ambientLightContribution + directionalLightContribution + totalPointLightContribution;

	return OUT;
}



/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Pixel Shader for the normal map rendering technique (for provided normal maps)
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
PS_OUTPUT DisplayNormals_PixelShader(VS_OUTPUT IN)
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

	// We can draw this pixel, set up the output...
	PS_OUTPUT OUT = (PS_OUTPUT)0;
	OUT.PixelColor.a = 1;	

	// set color based on normal map
	OUT.PixelColor.rgb = tex2D(NormalSampler, IN.TexCoord);

	return OUT;
}



/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Pixel Shader for the normal map rendering technique (for procedural normal maps)
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
PS_OUTPUT DisplayNormalsProcedural_PixelShader(VS_OUTPUT IN)
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

	// We can draw this pixel, set up the output...
	PS_OUTPUT OUT = (PS_OUTPUT)0;
	OUT.PixelColor.a = 1;	
	
	// set the color (unit vector in the positive-Y direction)
	OUT.PixelColor.rgb = float3(0.5, 1.0, 0.5);

	return OUT;
}




/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Pixel Shader for the offset map rendering technique (for provided offset maps)
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
PS_OUTPUT DisplayOffsets_PixelShader(VS_OUTPUT IN)
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

	// We can draw this pixel...set up the output
	PS_OUTPUT OUT = (PS_OUTPUT)0;
	OUT.PixelColor.a = 1;

	// set color from offset map
	OUT.PixelColor.rgb = tex2D(OffsetSampler, IN.TexCoord);

	return OUT;
}



/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Pixel Shader for the offset map rendering technique (for procedural offset maps)
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
PS_OUTPUT DisplayOffsetsProcedural_PixelShader(VS_OUTPUT IN)
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

	// We can draw this pixel...set up the output
	PS_OUTPUT OUT = (PS_OUTPUT)0;
	OUT.PixelColor.a = 1;

	// set color for offset (procedural sprites exist only in the XZ plane)
	OUT.PixelColor.r = (IN.TexCoord.x - Rectangle.Boundaries.x) / (Rectangle.Boundaries.y - Rectangle.Boundaries.x);
	OUT.PixelColor.b = 1 - (IN.TexCoord.y - Rectangle.Boundaries.z) / (Rectangle.Boundaries.w - Rectangle.Boundaries.z);

	return OUT;
}






/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Technique for creating the depth map for sprites with provided normal and offset maps
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
technique CreateDepthMap
{
    pass Pass0
    {
        CullMode = NONE;
		ZEnable = TRUE;
		ZWriteEnable = TRUE;
		AlphaBlendEnable = FALSE;
		
		VertexShader = compile vs_2_0 FNASprite_VertexShader();
		PixelShader = compile ps_2_0 CreateDepthMap_PixelShader();
    }
}


/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Technique for creating the depth map for sprites with procedural normal and offset maps
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
technique CreateDepthMapProcedural
{
    pass Pass0
    {
        CullMode = NONE;
		ZEnable = TRUE;
		ZWriteEnable = TRUE;
		AlphaBlendEnable = FALSE;
		
		VertexShader = compile vs_2_0 FNASprite_VertexShader();
		PixelShader = compile ps_2_0 CreateDepthMapProcedural_PixelShader();
    }
}



/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Technique for displaying the normals for sprites with provided normal and offset maps
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
technique DisplayNormals
{
    pass Pass0
    {
        CullMode = NONE;
		ZEnable = TRUE;
		ZWriteEnable = TRUE;
		AlphaBlendEnable = FALSE;
		
		VertexShader = compile vs_2_0 FNASprite_VertexShader();
		PixelShader = compile ps_2_0 DisplayNormals_PixelShader();
    }
}


/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Technique for displaying the normals for sprites with procedural normal and offset maps
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
technique DisplayNormalsProcedural
{
    pass Pass0
    {
        CullMode = NONE;
		ZEnable = TRUE;
		ZWriteEnable = TRUE;
		AlphaBlendEnable = FALSE;
		
		VertexShader = compile vs_2_0 FNASprite_VertexShader();
		PixelShader = compile ps_2_0 DisplayNormalsProcedural_PixelShader();
    }
}



/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Technique for displaying the offsets for sprites with provided normal and offset maps
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
technique DisplayOffsets
{
    pass Pass0
    {
        CullMode = NONE;
		ZEnable = TRUE;
		ZWriteEnable = TRUE;
		AlphaBlendEnable = FALSE;
		
		VertexShader = compile vs_2_0 FNASprite_VertexShader();
		PixelShader = compile ps_2_0 DisplayOffsets_PixelShader();
    }
}


/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Technique for displaying the offsets for sprites with procedural normal and offset maps
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
technique DisplayOffsetsProcedural
{
    pass Pass0
    {
        CullMode = NONE;
		ZEnable = TRUE;
		ZWriteEnable = TRUE;
		AlphaBlendEnable = FALSE;
		
		VertexShader = compile vs_2_0 FNASprite_VertexShader();
		PixelShader = compile ps_2_0 DisplayOffsetsProcedural_PixelShader();
    }
}



/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Techniques for lit sprite rendering (for sprites with provided normal and offset maps) using multiple pointlights
// The Techniques are split up as such so that there will be no looping once the shader is compiled.  
// Any internal looping will be rolled out, thus making the shader perform better
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
technique LitRender0
{
	pass Pass0
	{
		VertexShader = compile vs_2_0 FNASprite_VertexShader();
		PixelShader = compile ps_2_0 LitSpriteRender_PixelShader(0);
	}
}
technique LitRender1
{
	pass Pass0
	{
		VertexShader = compile vs_2_0 FNASprite_VertexShader();
		PixelShader = compile ps_2_0 LitSpriteRender_PixelShader(1);
	}
}
technique LitRender2
{
	pass Pass0
	{
		VertexShader = compile vs_2_0 FNASprite_VertexShader();
		PixelShader = compile ps_2_0 LitSpriteRender_PixelShader(2);
	}
}
/*
technique LitRender3
{
	pass Pass0
	{
		VertexShader = compile vs_2_0 FNASprite_VertexShader();
		PixelShader = compile ps_2_0 LitSpriteRender_PixelShader(3);
	}
}
technique LitRender4
{
	pass Pass0
	{
		VertexShader = compile vs_2_0 FNASprite_VertexShader();
		PixelShader = compile ps_2_0 LitSpriteRender_PixelShader(4);
	}
}
technique LitRender5
{
	pass Pass0
	{
		VertexShader = compile vs_2_0 FNASprite_VertexShader();
		PixelShader = compile ps_2_0 LitSpriteRender_PixelShader(5);
	}
}
technique LitRender6
{
	pass Pass0
	{
		VertexShader = compile vs_2_0 FNASprite_VertexShader();
		PixelShader = compile ps_2_0 LitSpriteRender_PixelShader(6);
	}
}
technique LitRender7
{
	pass Pass0
	{
		VertexShader = compile vs_2_0 FNASprite_VertexShader();
		PixelShader = compile ps_2_0 LitSpriteRender_PixelShader(7);
	}
}
technique LitRender8
{
	pass Pass0
	{
		VertexShader = compile vs_2_0 FNASprite_VertexShader();
		PixelShader = compile ps_2_0 LitSpriteRender_PixelShader(8);
	}
}
technique LitRender9
{
	pass Pass0
	{
		VertexShader = compile vs_2_0 FNASprite_VertexShader();
		PixelShader = compile ps_2_0 LitSpriteRender_PixelShader(9);
	}
}*/




/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Techniques for lit sprite rendering (for sprites with procedural normal and offset maps) using multiple pointlights
// The Techniques are split up as such so that there will be no looping once the shader is compiled.  
// Any internal looping will be rolled out, thus making the shader perform better
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
technique LitRenderProcedural0
{
	pass Pass0
	{
		VertexShader = compile vs_2_0 FNASprite_VertexShader();
		PixelShader = compile ps_2_0 LitSpriteRenderProcedural_PixelShader(0);
	}
}
technique LitRenderProcedural1
{
	pass Pass0
	{
		VertexShader = compile vs_2_0 FNASprite_VertexShader();
		PixelShader = compile ps_2_0 LitSpriteRenderProcedural_PixelShader(1);
	}
}
technique LitRenderProcedural2
{
	pass Pass0
	{
		VertexShader = compile vs_2_0 FNASprite_VertexShader();
		PixelShader = compile ps_2_0 LitSpriteRenderProcedural_PixelShader(2);
	}
}
/*
technique LitRenderProcedural3
{
	pass Pass0
	{
		VertexShader = compile vs_2_0 FNASprite_VertexShader();
		PixelShader = compile ps_2_0 LitSpriteRenderProcedural_PixelShader(3);
	}
}
technique LitRenderProcedural4
{
	pass Pass0
	{
		VertexShader = compile vs_2_0 FNASprite_VertexShader();
		PixelShader = compile ps_2_0 LitSpriteRenderProcedural_PixelShader(4);
	}
}
technique LitRenderProcedural5
{
	pass Pass0
	{
		VertexShader = compile vs_2_0 FNASprite_VertexShader();
		PixelShader = compile ps_2_0 LitSpriteRenderProcedural_PixelShader(5);
	}
}
technique LitRenderProcedural6
{
	pass Pass0
	{
		VertexShader = compile vs_2_0 FNASprite_VertexShader();
		PixelShader = compile ps_2_0 LitSpriteRenderProcedural_PixelShader(6);
	}
}
technique LitRenderProcedural7
{
	pass Pass0
	{
		VertexShader = compile vs_2_0 FNASprite_VertexShader();
		PixelShader = compile ps_2_0 LitSpriteRenderProcedural_PixelShader(7);
	}
}
technique LitRenderProcedural8
{
	pass Pass0
	{
		VertexShader = compile vs_2_0 FNASprite_VertexShader();
		PixelShader = compile ps_2_0 LitSpriteRenderProcedural_PixelShader(8);
	}
}
technique LitRenderProcedural9
{
	pass Pass0
	{
		VertexShader = compile vs_2_0 FNASprite_VertexShader();
		PixelShader = compile ps_2_0 LitSpriteRenderProcedural_PixelShader(9);
	}
}*/