/*------------------------------------------------------------------------------------------------------------------------------------
File:			FNALighting.fxh
Author:			Jerad Dunn
Last Update:	10/08/11

INSERT DESCRIPTION HERE
------------------------------------------------------------------------------------------------------------------------------------*/

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
// Hey, look, I actually used something from Paul's shader class!
// Thanks, Paul!
// Utility function to return the total lighting contribution of a light on a surface with the given color
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
float4 GetColorContribution(float4 lightColor, float4 surfaceColor)
{
	return float4(lightColor.rgb * lightColor.a, 1.0f) * surfaceColor;
}