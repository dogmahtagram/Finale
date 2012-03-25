/*------------------------------------------------------------------------------------------------------------------------------------
File:			FNAGBufferData.fxh
Author:			Jerad Dunn
Last Update:	10/08/11

INSERT DESCRIPTION HERE
------------------------------------------------------------------------------------------------------------------------------------*/

texture ColorBuffer;
sampler ColorBufferSampler = sampler_state
{
	Texture = <ColorBuffer>;
	MagFilter = LINEAR;
    MinFilter = LINEAR;
    Mipfilter = LINEAR;

	AddressU = Clamp;
	AddressV = Clamp;
	AddressW = Clamp;
};

texture NormalBuffer;
sampler NormalBufferSampler = sampler_state
{
	Texture = <NormalBuffer>;
	MagFilter = LINEAR;
    MinFilter = LINEAR;
    Mipfilter = LINEAR;

	AddressU = Clamp;
	AddressV = Clamp;
	AddressW = Clamp;
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

texture LightBuffer;
sampler LightBufferSampler = sampler_state
{
	Texture = <LightBuffer>;
	MagFilter = LINEAR;
    MinFilter = LINEAR;
    Mipfilter = LINEAR;

	AddressU = Clamp;
	AddressV = Clamp;
	AddressW = Clamp;
};

texture PositionBuffer;
sampler PositionBufferSampler = sampler_state
{
	Texture = <PositionBuffer>;
	MagFilter = LINEAR;
    MinFilter = LINEAR;
    Mipfilter = LINEAR;
	
	AddressU = Clamp;
	AddressV = Clamp;
	AddressW = Clamp;
};