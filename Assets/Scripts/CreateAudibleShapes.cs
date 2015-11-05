using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CreateAudibleShapes : MonoBehaviour {


	public GameObject audibleShapeCube;
	public GameObject audibleShapeCapsule;
	public GameObject audibleShapeCylinder;
	public GameObject audibleShapeSphere;
	GameObject go_temp;


	public List<GameObject> audibleShapes;
	void Start() {
		audibleShapes = new List<GameObject>();
	}

	/** creates the gameobject and its various properties when hand pinches on shape menu */ 
	public void createObject(double x, double z, string go) {	
		if (go.Equals("CUBE")) {
			go_temp = (GameObject) Instantiate(audibleShapeCube, new Vector3((float) x, 0.0f, (float) z), Quaternion.identity);
		} else if (go.Equals("SPHERE")) {
			go_temp = (GameObject)  Instantiate(audibleShapeSphere, new Vector3((float) x, 0.0f, (float) z), Quaternion.identity);
		} else if (go.Equals("CAPSULE")) {
			go_temp = (GameObject)  Instantiate(audibleShapeCapsule, new Vector3((float) x, 0.0f, (float) z), Quaternion.identity);
		} else if (go.Equals("CYLINDER")) {
			go_temp = (GameObject)  Instantiate(audibleShapeCylinder, new Vector3((float) x, 0.0f, (float) z), Quaternion.identity);
		}

		Debug.Log("OBJECT CREATED: " + go);
	
		audibleShapes.Add(go_temp);
		Debug.Log("Size of AudibleShapes: " + audibleShapes.Count);
	}

	public void destroyAudibleShapes() {
		foreach (GameObject go in audibleShapes) {
			GameObject.Destroy(go);
		}

		audibleShapes.Clear();

	//	Debug.Log("Is audibleShapes.Count 0? " + audibleShapes.Count);
	}
}
