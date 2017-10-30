using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Configure : MonoBehaviour {

	// Road constants
	public  float HighWayWidth = 2.0f;
	public  float StreetWidth = 1.0f;
	public  float ScaleFactor = 5.0f;

	// Building constants
	public  float BuildingBaseMeasure = 12.0f;
	public  float BuildingBaseWidth = 6.0f;
	public  float SidewalkWidth = 2.0f;

	// Layer constants
	public  int RoadLayer = 8;
	public  int BuildingLayer = 9;

	// Neighbourhood constants
	public  float Neighbourhood1MinHeight = 8f;
	public  float Neighbourhood1MaxHeight = 8f;
	public  float Neighbourhood2MinHeight = 8f;
	public  float Neighbourhood2MaxHeight = 8f;
	public  float Neighbourhood3MinHeight = 8f;
	public  float Neighbourhood3MaxHeight = 8f;
	public  float Neighbourhood4MinHeight = 8f;
	public  float Neighbourhood4MaxHeight = 8f;
	public  float Neighbourhood5MinHeight = 8f;
	public  float Neighbourhood5MaxHeight = 8f;

	// Probability constants

	public  float BuildingSideProbability = 8f;
	public  float StreetBranchProbability = 8f;

	// Use this for initialization
	void Start () {
		Constant.SidewalkWidth = this.SidewalkWidth;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
