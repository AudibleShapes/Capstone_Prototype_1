using UnityEngine;
using System.Collections;
using Leap;

public class DestroyInstructions : MonoBehaviour {

	Leap.Controller controller;
	Frame frame;
	HandList hands;
	public GameObject go;

	// Use this for initialization
	void Start () {
		controller = new Controller();
	}
	
	// Update is called once per frame
	void Update () {
		frame = controller.Frame();
		hands = frame.Hands;

		if (hands.Count < 1) {
			Destroy(go);
		}
	}
}
