using UnityEngine;
using System.Collections;
using Leap;

public class Poke : MonoBehaviour {
	//Leap components for gestures (to be built in the future)
	//Leap.Controller controller;
	//Frame frame;
	private bool poking = false;
	private bool trigger_poke;
	public ChangeMaterialTest _cmt;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		trigger_poke = false;

		//frame = controller.Frame();
		HandModel hand_model = GetComponent<HandModel>();
		Hand leap_hand = hand_model.GetLeapHand();

		if (leap_hand == null)
			return;

		//get the position of index finger from either hand
		Vector3 position = hand_model.fingers[1].GetTipPosition();

		//if the tip position's z coordinate is within a certain limit, trigger poke
		trigger_poke = _cmt.triggerPoke(position[0], position[1], position[2]);

		//once triggered poke, poke to call OnPoke and change grid square colour
		if (trigger_poke && !poking)
			OnPoke(position);
		else if (!trigger_poke && poking)
			NoPoke();
	}

	void OnPoke(Vector3 position) {
		poking = true;
		_cmt.changeMaterial(position[0], position[1], position[2]);
	}

	void NoPoke() {
		poking = false;
	}
}
