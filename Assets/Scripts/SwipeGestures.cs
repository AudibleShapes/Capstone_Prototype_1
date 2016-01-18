using UnityEngine;
using System.Collections;
using Leap;

public class SwipeGestures : MonoBehaviour {

	//Leap components for gestures (to be built in the future)
	Leap.Controller controller;
	Frame frame;
	public ChangeMaterialTest _cmt;

	// Use this for initialization
	void Start () { //enable gestures you need to use here
		controller = new Controller();
		controller.EnableGesture(Gesture.GestureType.TYPESWIPE);
		//controller.EnableGesture(Gesture.GestureType.TYPECIRCLE);
	
	}
	
	// Update is called once per frame
	void Update () {
		Frame frame = controller.Frame();
		//HandModel hand_model = GetComponent<HandModel>();
		//Hand leap_hand = hand_model.GetLeapHand(); //use this to check which hand performs which gesture

		foreach (Gesture gesture in frame.Gestures())
		{
			switch(gesture.Type)
			{
				//			case(Gesture.GestureType.TYPESCREENTAP):
				//			{
				//				ScreenTapGesture screenTap = new ScreenTapGesture (gesture);
				//				Debug.Log("Circle gesture recognized. Velocity: " + circle.Pointable.TipVelocity + "; Touch Distance: " + circle.Pointable.TouchDistance);
				//				break;
				//			}
				//			case(Gesture.GestureType.TYPEINVALID):
				//			{
				//				Debug.Log("Invalid gesture recognized.");
				//				break;
				//			}
				//			case(Gesture.GestureType.TYPEKEYTAP):
				//			{
				//				KeyTapGesture keyTap = new KeyTapGesture (gesture);
				//				Debug.Log("Key Tap gesture recognized. Velocity: " + keyTap.Pointable.TipVelocity + "; Touch Distance: " + keyTap.Pointable.TouchDistance);
				//				break;
				//			}
//				case(Gesture.GestureType.TYPECIRCLE):
//				{
//					CircleGesture circle = new CircleGesture (gesture);
//					if (circle.Normal.z > 0) { //if circle's normal vector is positive
//						_cmt.moveBlocks(true);
//						Debug.Log ("CIRLCLE'S Z NORMAL (+): " + circle.Normal.z);
//					} else { //else if it's negative
//						_cmt.moveBlocks(false);
//						Debug.Log ("CIRLCLE'S Z NORMAL (-): " + circle.Normal.z);
//					}
//					Debug.Log("CIRCLE GESTURE");
//					break;
//				}
				case(Gesture.GestureType.TYPESWIPE): //when performing swipe gesture, call the clearGrid method to clear the grid in Unity and Max
				{
					SwipeGesture swipe = new SwipeGesture (gesture);

					//indicates a right swipe to reset the environment back to its regular state
					if (swipe.Direction.x >= 0.8f && swipe.Direction.x < 1.0f)
						_cmt.clearGrid();
						//Debug.Log("GRID CLEARED");

					//left swipe to reset Audible Shapes environment to last-saved state
					
					if (swipe.Direction.x <= -0.8f && swipe.Direction.x > -1.0f)
						_cmt.savedScene();
					//Debug.Log("Swipe gesture recognized. Direction: " + swipe.Direction);
					break;
				}
				default:
				{
					break;
				}
			}
		}
	}
}
