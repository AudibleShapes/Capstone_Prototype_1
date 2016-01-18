using UnityEngine;
using System.Collections;
using Leap;

public class FingerMovement : MonoBehaviour {
	//Leap components for gestures (to be built in the future)
	Leap.Controller controller;
	Frame frame;
	public ChangeMaterialTest _cmt;
	private float max_z = 0.0f; //used to establish max z between z coordinates of middle finger and palm
	private float min_z = 10.0f; //used to establish min z between z coordinates of middle finger and palm
	private float mapped = 0.0f;
	private readonly float MAX_Z_POSS = 2.0f;
	private readonly float MIN_Z_POSS = 0.1f;
	private bool poking = false;
	private bool trigger_poke;


	// Use this for initialization
	void Start () {
		controller = new Controller();
	}
	
	// Update is called once per frame
	void Update () {
		frame = controller.Frame();
		HandList hands = frame.Hands;
		trigger_poke = false;

		if (hands.Count < 1) { //reset min_z and max_z to default values once hands are out of scene
			max_z = 0.0f;
			min_z = 10.0f;
		}

		HandModel hand_model = GetComponent<HandModel>();
		Hand leap_hand = hand_model.GetLeapHand();
		
		if (leap_hand == null)
			return;

		//find tip position of middle finger and position of palm (which are both vectors)
		//calculate the difference vector's z coordinate between middle finger and palm
		//this difference is essential to moving 
		Vector3 position = hand_model.fingers[2].GetTipPosition();

		if (leap_hand.IsLeft) { //left hand pokes cube to be moved
			trigger_poke = _cmt.triggerPoke(position.x, position.y, position.z);

		if (trigger_poke && !poking)
			OnPoke(position);
		else if (!trigger_poke && poking)
			NoPoke();
		}
		else { //right hand moves cube
			Vector3 palm = hand_model.palm.position;
			float z = position.z - palm.z;

			//Debug.Log(position.z + "\t" + palm.z + "\t" + z);
				
			if (z > max_z && z < MAX_Z_POSS)
				max_z = z;

			if (z < min_z && z > MIN_Z_POSS)
				min_z = z;

			//Debug.Log(min_z + "\t" + max_z + "\t" + z);

			//map z from range of min_z - max_z to new range of Z_MIN and Z_MAX from ChangeMaterialTest script
			//then, move blocks in grid based on this mapped value
			mapped = mapZ(z, min_z, max_z, ChangeMaterialTest.Z_MIN, ChangeMaterialTest.Z_MAX);
			_cmt.moveBlocks(mapped); 
		}
	}

	void OnPoke(Vector3 position) {
		poking = true;
		_cmt.selectMoveBlock(position.x, position.y, position.z);
	}
	
	void NoPoke() {
		poking = false;
	}

	//simple formula to recalculate diff's z-coordinate from the range of min and max diff's to min and max of z-coordinate in grid
	float mapZ(float value, float from1, float to1, float from2, float to2) {
		return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
	}
}