using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CityGenerator : MonoBehaviour {

	private int counter = 0;

	// Terrain in which the city is going to be placed
	public Terrain cityLocation;

	// Queue of roads to be placed
	private Queue<Road> insertQueue = new Queue<Road> ();

	// List of roads already placed
	private List<Road> placedList = new List<Road>();

	// Side orientation (see Road.cs) of the base road from which to continue to grow
	private int initialSide = 0;

	// Use this for initialization
	void Start () {
	}

	// Update is called once per frame
	void Update () {
		if (counter == 0) {
			roadGenerator ();
			buildingGenerator ();
			counter++;
		}
	}

	public void roadGenerator(){

		bool accepted;

		//Add a highway at some limit of the cityLocation to start the algorithm
		Road ro = new Road (cityLocation);
		initialSide = ro.getOriginalSide();
		insertQueue.Enqueue (ro);

		while(insertQueue.Count > 0){
			
			ro = insertQueue.Dequeue ();
			accepted = localConstraints(ro);

			if (accepted) {
				placedList.Add (ro);

				foreach (Road s in  globalGoals (ro)) {
					insertQueue.Enqueue (s);
				}
			} else {
				ro.destroy();
			}
		}

	}

	public void buildingGenerator(){
		float randomNumber, height;
		Building buildingToPlace;
		Ray ray;
		Vector3 cityLimits = cityLocation.terrainData.size;

		foreach (Road r in placedList) {
			randomNumber = Random.Range (0.0f, 1.0f);

			if (randomNumber < 0.5f) {
				ray = new Ray (r.getTransform ().position, Quaternion.AngleAxis (r.getTransform ().localRotation.eulerAngles.y, new Vector3 (0, 1, 0)) * Vector3.right);
			} else {
				ray = new Ray (r.getTransform ().position, Quaternion.AngleAxis (r.getTransform ().localRotation.eulerAngles.y, new Vector3 (0, 1, 0)) * Vector3.left);
			}

			buildingToPlace = new Building(ray.GetPoint((r.getTransform ().localScale.x * Constant.ScaleFactor) + Constant.SidewalkWidth + Constant.BuildingBaseWidth), r.getTransform ().rotation);
			buildingToPlace.setPosition (new Vector3(buildingToPlace.getTransform().position.x,buildingToPlace.getTransform().position.y + Constant.BuildingBaseWidth,buildingToPlace.getTransform().position.z));

			if(buildingToPlace.getTransform().position.x > (cityLocation.GetPosition().x + cityLimits.x ) || buildingToPlace.getTransform().position.x < (cityLocation.GetPosition().x) || buildingToPlace.getTransform().position.z > (cityLocation.GetPosition().z + cityLimits.z ) || buildingToPlace.getTransform().position.z < (cityLocation.GetPosition().z)){
				buildingToPlace.destroy();
				buildingToPlace = null;
			}

			if (buildingToPlace != null) {
				if(buildingToPlace.detectCollisionRoad() || buildingToPlace.detectCollisionBuilding()){
						buildingToPlace.destroy ();
						buildingToPlace = null;
				}
			}

			if (buildingToPlace != null) {
				if (buildingToPlace.getNeighbourhoodDensity () == 5) {
					randomNumber = Random.Range (0.4f, 1f);
					if (randomNumber > 0.5f) {
						Vector3 undo = buildingToPlace.getTransform ().position;
						while (!buildingToPlace.detectCollisionRoad () && !buildingToPlace.detectCollisionBuilding ()) {
							undo = buildingToPlace.getTransform ().position;
							buildingToPlace.setScale (buildingToPlace.getTransform ().localScale * 2);
							Vector3 v = ray.GetPoint ((r.getTransform ().localScale.x * Constant.ScaleFactor) + Constant.SidewalkWidth + (buildingToPlace.getTransform ().localScale.x / 2));
							buildingToPlace.setPosition (new Vector3 (v.x, buildingToPlace.getTransform ().position.y + (buildingToPlace.getTransform ().localScale.y / 4), v.z));
						}
						buildingToPlace.setPosition (undo);
						buildingToPlace.setScale (buildingToPlace.getTransform ().localScale / 2);
						if (randomNumber > 0.7f) {
							height = Random.Range (48f, 96f);
							buildingToPlace.setScale (new Vector3 (buildingToPlace.getTransform ().localScale.x, buildingToPlace.getTransform ().localScale.y + height, buildingToPlace.getTransform ().localScale.z));
							buildingToPlace.setPosition (new Vector3 (buildingToPlace.getTransform ().position.x, buildingToPlace.getTransform ().position.y + ((height / 2)), buildingToPlace.getTransform ().position.z)); 
						}
					}
				} else if (buildingToPlace.getNeighbourhoodDensity () == 4) {
					randomNumber = Random.Range (0.3f, 1f);
					if (randomNumber > 0.5f) {
						Vector3 undo = buildingToPlace.getTransform ().position;
						while (!buildingToPlace.detectCollisionRoad () && !buildingToPlace.detectCollisionBuilding ()) {
							undo = buildingToPlace.getTransform ().position;
							buildingToPlace.setScale (buildingToPlace.getTransform ().localScale * 2);
							Vector3 v = ray.GetPoint ((r.getTransform ().localScale.x * Constant.ScaleFactor) + Constant.SidewalkWidth + (buildingToPlace.getTransform ().localScale.x / 2));
							buildingToPlace.setPosition (new Vector3 (v.x, buildingToPlace.getTransform ().position.y + (buildingToPlace.getTransform ().localScale.y / 4), v.z));
						}
						buildingToPlace.setPosition (undo);
						buildingToPlace.setScale (buildingToPlace.getTransform ().localScale / 2);
						if (randomNumber > 0.7f) {
							height = Random.Range (24f, 36f);
							buildingToPlace.setScale (new Vector3 (buildingToPlace.getTransform ().localScale.x, buildingToPlace.getTransform ().localScale.y + height, buildingToPlace.getTransform ().localScale.z));
							buildingToPlace.setPosition (new Vector3 (buildingToPlace.getTransform ().position.x, buildingToPlace.getTransform ().position.y + ((height / 2)), buildingToPlace.getTransform ().position.z)); 
						}
					}
				} else if (buildingToPlace.getNeighbourhoodDensity () == 3) {
					randomNumber = Random.Range (0.2f, 1f);
					if (randomNumber > 0.5f) {
						Vector3 undo = buildingToPlace.getTransform ().position;
						while (!buildingToPlace.detectCollisionRoad () && !buildingToPlace.detectCollisionBuilding ()) {
							undo = buildingToPlace.getTransform ().position;
							buildingToPlace.setScale (new Vector3 (buildingToPlace.getTransform ().localScale.x * 2, buildingToPlace.getTransform ().localScale.y, buildingToPlace.getTransform ().localScale.z * 2));
							Vector3 v = ray.GetPoint ((r.getTransform ().localScale.x * Constant.ScaleFactor) + Constant.SidewalkWidth + (buildingToPlace.getTransform ().localScale.x / 2));
							buildingToPlace.setPosition (new Vector3 (v.x, v.y + Constant.BuildingBaseWidth, v.z));
						}
						buildingToPlace.setPosition (undo);
						buildingToPlace.setScale (new Vector3 (buildingToPlace.getTransform ().localScale.x / 2, buildingToPlace.getTransform ().localScale.y, buildingToPlace.getTransform ().localScale.z / 2));
						if (randomNumber > 0.7f) {
							height = Random.Range (12f, 24f);
							buildingToPlace.setScale (new Vector3 (buildingToPlace.getTransform ().localScale.x, buildingToPlace.getTransform ().localScale.y + height, buildingToPlace.getTransform ().localScale.z));
							buildingToPlace.setPosition (new Vector3 (buildingToPlace.getTransform ().position.x, buildingToPlace.getTransform ().position.y + ((height / 2)), buildingToPlace.getTransform ().position.z)); 
						}
					}
				} else if (buildingToPlace.getNeighbourhoodDensity () == 2) {
					randomNumber = Random.Range (0.1f, 1f);
					if (randomNumber > 0.7f) {
						Vector3 undo = buildingToPlace.getTransform ().position;
						while (!buildingToPlace.detectCollisionRoad () && !buildingToPlace.detectCollisionBuilding ()) {
							undo = buildingToPlace.getTransform ().position;
							buildingToPlace.setScale (new Vector3 (buildingToPlace.getTransform ().localScale.x * 2, buildingToPlace.getTransform ().localScale.y, buildingToPlace.getTransform ().localScale.z * 2));
							Vector3 v = ray.GetPoint ((r.getTransform ().localScale.x * Constant.ScaleFactor) + Constant.SidewalkWidth + (buildingToPlace.getTransform ().localScale.x / 2));
							buildingToPlace.setPosition (new Vector3 (v.x, v.y + Constant.BuildingBaseWidth, v.z));
						}
						buildingToPlace.setPosition (undo);
						buildingToPlace.setScale (new Vector3 (buildingToPlace.getTransform ().localScale.x / 2, buildingToPlace.getTransform ().localScale.y, buildingToPlace.getTransform ().localScale.z / 2));
					}
				} else if (buildingToPlace.getNeighbourhoodDensity () == 1) {
					randomNumber = Random.Range (0f, 1f);
					if (randomNumber > 0.7f) {
						height = Random.Range (6f, 12f);
						Vector3 undo = buildingToPlace.getTransform ().position;
						buildingToPlace.setScale (new Vector3(buildingToPlace.getTransform ().localScale.x + height,buildingToPlace.getTransform ().localScale.y + height,buildingToPlace.getTransform ().localScale.z + height));
						buildingToPlace.setPosition (new Vector3 (buildingToPlace.getTransform ().position.x, buildingToPlace.getTransform ().position.y + ((height / 2)), buildingToPlace.getTransform ().position.z)); 
						if(buildingToPlace.detectCollisionRoad ()  || buildingToPlace.detectCollisionBuilding ()){
							buildingToPlace.setScale (new Vector3(buildingToPlace.getTransform ().localScale.x - height,buildingToPlace.getTransform ().localScale.y - height,buildingToPlace.getTransform ().localScale.z - height));
							buildingToPlace.setPosition (undo); 
						}
					}
				}
			}

		}

	}


	public List<Road> globalGoals(Road road){
		List<Road> returnList = new List<Road>();
		Road roadToRotate;
		float randomNumber;
		Ray ray;

		//Highways

		if (road.getTransform ().localScale.x == Constant.HighWayWidth) {
			
			randomNumber = Random.Range (0.0f, 1.0f);

			// Cast a ray in "front" of the highway to continue the road
			if (initialSide == 0) {
				ray = new Ray (road.getTransform ().position, Quaternion.AngleAxis (road.getTransform ().localRotation.eulerAngles.y, new Vector3 (0, 1, 0)) * Vector3.back);
			} else {
				ray = new Ray (road.getTransform ().position, Quaternion.AngleAxis (road.getTransform ().localRotation.eulerAngles.y - 180f, new Vector3 (0, 1, 0)) * Vector3.back);
			}

			// Chance or not to give a slight rotation to the highway
			if (randomNumber < 0.4) {					
				roadToRotate = new Road (ray.GetPoint (30f), new Vector3 (2f, 1f, 3f), road.getTransform ().rotation);
				returnList.Add (roadToRotate);
			} else if (randomNumber >= 0.4f && randomNumber < 0.7f) {
				roadToRotate = new Road (ray.GetPoint (30f), new Vector3 (2f, 1f, 3f), road.getTransform ().rotation);
				roadToRotate.getTransform ().RotateAround (ray.GetPoint (15f), Vector3.up, Random.Range (350f, 359.9f));
				returnList.Add (roadToRotate);
			} else {
				roadToRotate = new Road (ray.GetPoint (30f), new Vector3 (2f, 1f, 3f), road.getTransform ().rotation);
				roadToRotate.getTransform ().RotateAround (ray.GetPoint (15f), Vector3.up, Random.Range (0.1f, 10.0f));
				returnList.Add (roadToRotate);
			}


			// Chance to have a street branch off the main highway in either side of it
			randomNumber = Random.Range (0.0f, 1.0f);

			if (randomNumber < 0.4) {
				randomNumber = Random.Range (0.0f, 1.0f);
				if (randomNumber < 0.5) {
					ray = new Ray (road.getTransform ().position, Quaternion.AngleAxis (road.getTransform ().localRotation.eulerAngles.y, new Vector3 (0, 1, 0)) * Vector3.right);
					roadToRotate = new Road (ray.GetPoint (25f), new Vector3 (1f, 1f, 3f), road.getTransform ().rotation);
					roadToRotate.setOriginalSide (2);
				} else {
					ray = new Ray (road.getTransform ().position, Quaternion.AngleAxis (road.getTransform ().localRotation.eulerAngles.y, new Vector3 (0, 1, 0)) * Vector3.left);
					roadToRotate = new Road (ray.GetPoint (25f), new Vector3 (1f, 1f, 3f), road.getTransform ().rotation);
					roadToRotate.setOriginalSide (3);
				}

				roadToRotate.getTransform ().RotateAround (roadToRotate.getTransform().localPosition, Vector3.up, Random.Range (90.1f, 100.0f));
				returnList.Add (roadToRotate);
			}

		// Streets
		} else if (road.getTransform ().localScale.x == Constant.StreetWidth) {

			// Chance the street will continue
			randomNumber = Random.Range (0.0f, 1.0f);
			if (randomNumber < 0.95) {
				if (road.getOriginalSide () == 2) {
					ray = new Ray (road.getTransform ().position, Quaternion.AngleAxis (road.getTransform ().localRotation.eulerAngles.y, new Vector3 (0, 1, 0)) * Vector3.forward);
					roadToRotate = new Road (ray.GetPoint (30f), new Vector3 (1f, 1f, 3f), road.getTransform ().rotation);
					roadToRotate.setOriginalSide (2);
				} else {
					ray = new Ray (road.getTransform ().position, Quaternion.AngleAxis (road.getTransform ().localRotation.eulerAngles.y - 180f, new Vector3 (0, 1, 0)) * Vector3.forward);
					roadToRotate = new Road (ray.GetPoint (30f), new Vector3 (1f, 1f, 3f), road.getTransform ().rotation);
					roadToRotate.setOriginalSide (3);
				}

				// Chance the street will rotate a little bit
				randomNumber = Random.Range (0.0f, 1.0f);
				if (randomNumber < 0.4) {
					returnList.Add (roadToRotate);
				} else if (randomNumber >= 0.4f && randomNumber < 0.7f) {
					roadToRotate.getTransform ().RotateAround (ray.GetPoint (15f), Vector3.up, Random.Range (350f, 359.9f));
					returnList.Add (roadToRotate);
				} else {
					roadToRotate.getTransform ().RotateAround (ray.GetPoint (15f), Vector3.up, Random.Range (0.1f, 10.0f));
					returnList.Add (roadToRotate);
				}
			}

			// Chance that a street will branch off the current street, with a little rotation
			randomNumber = Random.Range (0.0f, 1.0f);
			if(randomNumber < 0.2){
				randomNumber = Random.Range (0.0f, 1.0f);
				if (randomNumber < 0.5) {
					ray = new Ray (road.getTransform ().position, Quaternion.AngleAxis (road.getTransform ().localRotation.eulerAngles.y, new Vector3 (0, 1, 0)) * Vector3.right);
					roadToRotate = new Road (ray.GetPoint (20f), new Vector3 (1f, 1f, 3f), road.getTransform ().rotation);
					roadToRotate.setOriginalSide (2);
				} else {
					ray = new Ray (road.getTransform ().position, Quaternion.AngleAxis (road.getTransform ().localRotation.eulerAngles.y, new Vector3 (0, 1, 0)) * Vector3.left);
					roadToRotate = new Road (ray.GetPoint (20f), new Vector3 (1f, 1f, 3f), road.getTransform ().rotation);
					roadToRotate.setOriginalSide (3);
				}

				roadToRotate.getTransform ().RotateAround (roadToRotate.getTransform().localPosition, Vector3.up, Random.Range (90.1f, 100.0f));
				returnList.Add (roadToRotate);
			}
		}

		return returnList;
	}

	public bool localConstraints(Road road){
		int intersect = 0, notReturningHighway = 0, notReturningStreet = 0;

		// Checks if road segments is out of bounds
		if(road.isOutOfBounds(cityLocation)){
			return false;
		}
			
		// Avoid too much intersection between streets and between streets and highways
		foreach(Road r in  placedList){
			if (road.getTransform ().localScale.x == Constant.StreetWidth && r.getTransform ().localScale.x == Constant.StreetWidth &&	road.getBounds ().Intersects (r.getBounds ())) {
				intersect++;
				if(intersect > 2){
					return false;
				}
			}

			if (road.getTransform ().localScale.x == Constant.StreetWidth && r.getTransform ().localScale.x == Constant.HighWayWidth &&	road.getBounds ().Intersects (r.getBounds ())) {
				if (notReturningStreet != 0) {
					return false;
				}
				notReturningHighway++;
			}

			if (road.getTransform ().localScale.x == Constant.StreetWidth && r.getTransform ().localScale.x == Constant.StreetWidth &&	road.getBounds ().Intersects (r.getBounds ())) {
				if (notReturningHighway != 0) {
					return false;
				}
				notReturningStreet++;
			}
		}

		return true;
	}

}
