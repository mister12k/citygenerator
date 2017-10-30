using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building {

	// GamObject which contains the building created
	private GameObject building;

	//Renderer of the building
	private Renderer rend;

	// Texture applied to buildings
	//private Material buildingMaterial = Resources.Load("M_Concreate_01", typeof(Material)) as Material;

	// Number to identify the orientation side of the building (0 - grows in x axis, 1 - grows in z axis)
	private int originSide;

	public Building(Vector3 pos, Quaternion rotation){
		building = GameObject.CreatePrimitive(PrimitiveType.Cube);
		building.name = "Building";
		building.layer = Constant.BuildingLayer;
		rend = building.GetComponent<Renderer> ();
		pos.y -= 0.001f ;
		building.transform.position = pos;
		building.transform.localScale = new Vector3(Constant.BuildingBaseMeasure,Constant.BuildingBaseMeasure,Constant.BuildingBaseMeasure);
		building.transform.rotation = rotation;
	}

	public bool detectCollisionRoad(){
		return Physics.CheckBox (this.getTransform().position,this.getTransform().localScale/2, this.getTransform().rotation,Constant.RoadLayerMask);
	}

	public bool detectCollisionBuilding(){
		building.layer = Constant.RoadLayer;
		bool result = Physics.CheckBox (this.getTransform().position,this.getTransform().localScale, this.getTransform().rotation,Constant.BuildingLayerMask);
		building.layer = Constant.BuildingLayer;
		return result;
	}

	public int getNeighbourhoodDensity (){
		Collider[] collisions = Physics.OverlapBox(this.getTransform().position, new Vector3(250,15,250), Quaternion.Euler(Vector3.zero),Constant.RoadLayerMask);
		int size = collisions.Length;

		string name = "buildings";
		string option;

		float rand = Random.Range (0.0f, 1.0f);

		if (rand < 0.33f) {
			option = "1";
		} else if (rand > 0.33 && rand < 0.66) {
			option = "2";
		} else {
			option = "3";
		}


		if (size > 209) {
			rend.material.color = Color.red;
			//rend.material.mainTexture = Resources.Load(name+"5-"+option) as Texture;
			return 5;
		} else if (size > 169 && size < 210) {
			rend.material.color = new Color(1f, 0.4f, 0f);
			//rend.material.mainTexture = Resources.Load(name+"4-"+option) as Texture;
			return 4;
		} else if (size > 129 && size < 170) {
			rend.material.color = Color.yellow;
			//rend.material.mainTexture = Resources.Load(name+"3-"+option) as Texture;
			return 3;
		} 		else if (size > 89 && size < 130) {
			rend.material.color = Color.green ;
			//rend.material.mainTexture = Resources.Load(name+"2-"+option) as Texture;
			return 2;
		} else if (size > 49 && size < 90) {
			rend.material.color = Color.blue ;
			//rend.material.mainTexture = Resources.Load(name+"1-"+option) as Texture;
			return 1;
		} else {
			return 0;
		}

	}

	public Transform getTransform(){
		return building.GetComponent<Transform> ();
	}

	public Bounds getBounds(){
		return this.rend.bounds;
	}

	public void setScale(Vector3 scale){
		this.building.transform.localScale = scale;
	}

	public void setPosition(Vector3 pos){
		this.building.transform.position = pos;
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

	public void destroy(){
		UnityEngine.Object.Destroy(this.building);
	}
}
