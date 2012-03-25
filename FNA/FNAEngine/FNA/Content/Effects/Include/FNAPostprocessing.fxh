/*------------------------------------------------------------------------------------------------------------------------------------
File:			FNAPostprocessing.fxh
Author:			Jerad Dunn
Last Update:	10/08/11

INSERT DESCRIPTION HERE
------------------------------------------------------------------------------------------------------------------------------------*/

float2 HalfPixel;


VS_OUTPUT Postprocessing_VertexShader(VS_INPUT IN)
{
	VS_OUTPUT OUT = (VS_OUTPUT)0;

	// Convert to screen-space
	OUT.Position = IN.Position;
	OUT.TexCoord = IN.TexCoord - HalfPixel;
    
	return OUT;
}