/*------------------------------------------------------------------------------------------------------------------------------------
File:			FNACameraData.fxh
Author:			Jerad Dunn
Last Update:	10/08/11

INSERT DESCRIPTION HERE
------------------------------------------------------------------------------------------------------------------------------------*/

/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Camera Stuff (will need to be more robust later)
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
float4x4 CameraTransformationMatrix;					/// Converts the sprite 'vertices' to screen space
float4 CameraInfo;										/// Position = <X,Y,Z>			Far plane = W