using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Road {

	// GamObject which contains the road created
	private GameObject road;

	//Renderer of the road
	private Renderer rend;

	// Texture applied to roads
	private Material roadMaterial = Resources.Load("M_Concreate_01", typeof(Material)) as Material;

	// Number to identify the orientation side of the road (0 - grows in x axis, 1 - grows in z axis, 2 - branching street right, 3 - branching street left)
	private int originSide;

	public Road (Terrain terr){
		Vector3 cityLimits = terr.terrainData.size;

		road = GameObject.CreatePrimitive(PrimitiveType.Plane);
		road.name = "Highway";
		road.layer = Constant.RoadLayer;
		rend = road.GetComponent<Renderer> ();

		// 50% chace to either start on one side of the cityLimit or the other, always in the 2 middle quarters of the side
		if (Random.Range (0.0f, 1.0f) < 0.5) {
			road.transform.position = new Vector3 (((terr.GetPosition().x + cityLimits.x) - 15), 0.01f,Random.Range(((terr.GetPosition().z + cityLimits.z)/2)/2, ((((terr.GetPosition().z + cityLimits.z)/2)/2)*3)));
			road.transform.rotation = Quaternion.Euler (0, Random.Range(80f,100f), 0);
			originSide = 0;
		} else {
			road.transform.position = new Vector3 (Random.Range(((terr.GetPosition().x +cityLimits.x)/2)/2, (((terr.GetPosition().x +cityLimits.x)/2)/2)*3), 0.01f,((terr.GetPosition().z +cityLimits.z) - 15));
			road.transform.rotation = Quaternion.Euler (0, Random.Range(170f,190f), 0);
			originSide = 1;
		}

		road.transform.localScale = new Vector3 (2f, 1f, 3f);
		rend.material = roadMaterial;
	}

	public Road(Vector3 pos,Vector3 scale, Quaternion rotation){
		
		road = GameObject.CreatePrimitive(PrimitiveType.Plane);

		if (scale.x == Constant.StreetWidth) {
			road.name = "Street";
		} else {
			road.name = "Highway";
		}

		road.layer = Constant.RoadLayer;
		rend = road.GetComponent<Renderer> ();
		road.transform.position = pos;
		road.transform.localScale = scale;
		road.transform.rotation = rotation;
		rend.material = roadMaterial;
	}

	public Transform getTransform(){
		return road.GetComponent<Transform> ();
	}

	public Bounds getBounds(){
		return this.rend.bounds;
	}

	public void setRenderer(bool state){
		this.rend.enabled = state;
	}

	public void setOriginalSide(int side){
		this.originSide = side;
	}

	public int getOriginalSide(){
		return this.originSide;
	}

	public bool isOutOfBounds(Terrain cityLocation){
		Vector3 cityLimits = cityLocation.terrainData.size;

		if (this.getTransform ().position.x > (cityLocation.GetPosition ().x + cityLimits.x) || this.getTransform ().position.x < (cityLocation.GetPosition ().x) || 
			this.getTransform ().position.z > (cityLocation.GetPosition ().z + cityLimits.z) || this.getTransform ().position.z < (cityLocation.GetPosition ().z) ) {
			return true;
		} else {
			return false;
		}
	}

	public void destroy(){
		UnityEngine.Object.Destroy(this.road);
	}
		
}
