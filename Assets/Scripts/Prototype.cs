using UnityEngine;
using System;
using System.Collections;
using Leap;

public class Prototype : MonoBehaviour {
	
	Leap.Controller controller;
	Frame frame;
	FingerList fingers;
	HandList hands;
	#region delegates
	#endregion
	
	#region events
	#endregion
	
	// Use this for initialization
	void Start () 
	{
		controller = new Controller();
		controller.EnableGesture(Gesture.GestureType.TYPECIRCLE);
		controller.EnableGesture(Gesture.GestureType.TYPEINVALID);
		controller.EnableGesture(Gesture.GestureType.TYPEKEYTAP);
		controller.EnableGesture(Gesture.GestureType.TYPESCREENTAP);
		controller.EnableGesture(Gesture.GestureType.TYPESWIPE);
	}
	
	// Update is called once per frame
	void Update () 
	{
		Frame frame = controller.Frame();
		hands = frame.Hands;
		fingers = frame.Fingers;

		foreach (Gesture gesture in frame.Gestures())
		{
			switch(gesture.Type)
			{
			case(Gesture.GestureType.TYPECIRCLE):
			{
				CircleGesture circle = new CircleGesture (gesture);
				Debug.Log("Circle gesture recognized. Velocity: " + circle.Pointable.TipVelocity + "; Touch Distance: " + circle.Pointable.TouchDistance);
				break;
			}
			case(Gesture.GestureType.TYPEINVALID):
			{
				Debug.Log("Invalid gesture recognized.");
				break;
			}
			case(Gesture.GestureType.TYPEKEYTAP):
			{
				KeyTapGesture keyTap = new KeyTapGesture (gesture);
				Debug.Log("Key Tap gesture recognized. Velocity: " + keyTap.Pointable.TipVelocity + "; Touch Distance: " + keyTap.Pointable.TouchDistance);
				break;
			}
			case(Gesture.GestureType.TYPESCREENTAP):
			{
				ScreenTapGesture screenTap = new ScreenTapGesture (gesture);
				Debug.Log("Screen tap gesture recognized. Velocity: " + screenTap.Pointable.TipVelocity + "; Touch Distance: " + screenTap.Pointable.TouchDistance);
				break;
			}
			case(Gesture.GestureType.TYPESWIPE):
			{
				SwipeGesture swipe = new SwipeGesture (gesture);
				Debug.Log("Swipe gesture recognized. Velocity: " + swipe.Pointable.TipVelocity + "; Touch Distance: " + swipe.Pointable.TouchDistance);
				break;
			}
			default:
			{
				break;
			}
			}
		}

		Debug.Log("HANDS IN FRAME: " + hands.Count);
	}
}
