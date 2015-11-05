using UnityEngine;
using System.Collections;

public class Instantiate_Grid : MonoBehaviour {

	public Transform brick;
	// Use this for initialization
	void Start () {

		for (int x = 0; x < 5; x++) {
			for (int y = 0; y < 5; y++) {
				for (int z = 0; z < 5; z++) {
					Instantiate(brick, new Vector3(x,y,z), Quaternion.identity);
				}
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
