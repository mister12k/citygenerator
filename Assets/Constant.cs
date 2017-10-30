using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Constant{

	// Road constants
	public static float HighWayWidth = 2.0f;
	public static float StreetWidth = 1.0f;
	public static float ScaleFactor = 5.0f;

	// Building constants
	public static float BuildingBaseMeasure = 12.0f;
	public static float BuildingBaseWidth = 6.0f;
	public static float SidewalkWidth = 2.0f;

	// Layer constants
	public static int RoadLayer = 8;
	public static int BuildingLayer = 9;
	public static int RoadLayerMask = 1 << RoadLayer;
	public static int BuildingLayerMask = 1 << BuildingLayer;

	// Neighbourhood constants
	public static float Neighbourhood1MinHeight = 8f;
	public static float Neighbourhood1MaxHeight = 8f;
	public static float Neighbourhood2MinHeight = 8f;
	public static float Neighbourhood2MaxHeight = 8f;
	public static float Neighbourhood3MinHeight = 8f;
	public static float Neighbourhood3MaxHeight = 8f;
	public static float Neighbourhood4MinHeight = 8f;
	public static float Neighbourhood4MaxHeight = 8f;
	public static float Neighbourhood5MinHeight = 8f;
	public static float Neighbourhood5MaxHeight = 8f;

	// Probability constants

	public static float BuildingSideProbability = 8f;
	public static float StreetBranchProbability = 8f;
}
