/******************************************************************************\
* Copyright (C) Leap Motion, Inc. 2011-2014.                                   *
* Leap Motion proprietary. Licensed under Apache 2.0                           *
* Available at http://www.apache.org/licenses/LICENSE-2.0.html                 *
\******************************************************************************/

using UnityEngine;
using System.Collections;
using Leap;

/** 
 * Detects pinches and grabs the closest rigidbody if it's within a given range.
 * 
 * Attach this script to the physics hand object assinged to the HandController in a scene.
 */
[RequireComponent(typeof(AudioSource))]
public class MagneticPinch : MonoBehaviour {

  public const float TRIGGER_DISTANCE_RATIO = 0.7f;

  /** The stiffness of the spring force used to move the object toward the hand. */
  public float forceSpringConstant = 100.0f;
  /** The maximum range at which an object can be picked up.*/
  public float magnetDistance = 2.0f;

	private AudioSource au_source;
	
	protected bool pinching_;
	protected Collider grabbed_;

    public static bool isHelpEnabled;
	public CreateAudibleShapes _cas;
	public LightIntensity _lt;

	public GameObject helpTitle;
	public GameObject modeButton;
	public GUIStyle header;
	public GUIStyle main;
	public GUIStyle style;
	public GUIStyle substyle;
	public Texture hand;
	public Texture cube;
	public Texture sphere;
	public Texture capsule;
	public Texture cylinder;
	public Texture help;
	public Texture garbage;
	public Texture mode;
	private float timeLeft = 2.0f;
	private bool isPinched = false;
	private GameObject prefab;
	private bool isGestureMode = false;

	public readonly double XCUBE_MIN = -1.1;
	public readonly double XCUBE_MAX = -0.4;
	public readonly double XSPHERE_MIN = -0.3;
	public readonly double XSPHERE_MAX = 0.4;
	public readonly double XCAPSULE_MIN = 0.65;
	public readonly double XCAPSULE_MAX = 1.25;
	public readonly double XCYLINDER_MIN = 1.65;
	public readonly double XCYLINDER_MAX = 2.3;
	public readonly double XHELP_MIN = 2.8;
	public readonly double XHELP_MAX = 3.3;
	public readonly double XGARBAGE_MIN = 3.65;
	public readonly double XGARBAGE_MAX = 4.2;
	public readonly double XMODE_MIN = 4.5;
	public readonly double XMODE_MAX = 5.5;


	public readonly double YCOOR_MIN = -1.0;
	public readonly double YCOOR_MAX = 0.5;
	public readonly double ZCOOR_MIN = -3;
	public readonly double ZCOOR_MAX = -1.6;

  void Start() {
    pinching_ = false;
    grabbed_ = null;
	prefab = (GameObject) Instantiate(helpTitle, new Vector3(0,0,0), Quaternion.identity);
	au_source = (AudioSource) gameObject.AddComponent<AudioSource>();
	AudioClip clip_ = (AudioClip) Resources.Load("SFX/as_grabbed");
	au_source.clip = clip_;
	au_source.loop = false;
	isHelpEnabled = false;
  }

  /** Finds an object to grab and grabs it. */
  void OnPinch(Vector3 pinch_position) {
    pinching_ = true;
	bool isCreated = false;
	
	//Debug.Log("pinch_position[0]" + pinch_position[0] + "\tpinch_position[1]: " + pinch_position[1] + "\tpinch_position[2]:" + pinch_position[2]);
	
		/*
	 * Checks if the pinch_position's x-coordinate is within a given range, then checks for the z-coordinate.
	 * This is necessary in creating the shapes for the soundscape environment.
	 */
	if (!isHelpEnabled) {
		if (pinch_position[0] >= XCUBE_MIN && pinch_position[0] < XCUBE_MAX) { //for creating the cube
			if (pinch_position[1] >= YCOOR_MIN && pinch_position[1] < YCOOR_MAX) {
				if (pinch_position[2] >= ZCOOR_MIN && pinch_position[2] < ZCOOR_MAX) {
					_cas.createObject(pinch_position[0], pinch_position[2], "CUBE");
					isCreated = true;
					playCreatedAudio();
				} else {
					isCreated = false;
					au_source.clip = (AudioClip) Resources.Load("SFX/as_grabbed");
				}
			} else {
				isCreated = false;
				au_source.clip = (AudioClip) Resources.Load("SFX/as_grabbed");
			}
		} else if (pinch_position[0] >= XSPHERE_MIN && pinch_position[0] < XSPHERE_MAX) { //for creating the sphere
			if (pinch_position[1] >= YCOOR_MIN && pinch_position[1] < YCOOR_MAX) {
				if (pinch_position[2] >= ZCOOR_MIN && pinch_position[2] < ZCOOR_MAX) {
					_cas.createObject(pinch_position[0], pinch_position[2], "SPHERE");
					isCreated = true;
					playCreatedAudio();
				} else {
					isCreated = false;
					au_source.clip = (AudioClip) Resources.Load("SFX/as_grabbed");
				}
			} else {
				isCreated = false;
				au_source.clip = (AudioClip) Resources.Load("SFX/as_grabbed");
			}
		} else if (pinch_position[0] >= XCAPSULE_MIN && pinch_position[0] < XCAPSULE_MAX) { //for creating the capsule
			if (pinch_position[1] >= YCOOR_MIN && pinch_position[1] < YCOOR_MAX) {
				if (pinch_position[2] >= ZCOOR_MIN && pinch_position[2] < ZCOOR_MAX) {
					_cas.createObject(pinch_position[0], pinch_position[2], "CAPSULE");
					isCreated = true;
					playCreatedAudio();
				} else {
					isCreated = false;
					au_source.clip = (AudioClip) Resources.Load("SFX/as_grabbed");
				}
			} else {
				isCreated = false;
				au_source.clip = (AudioClip) Resources.Load("SFX/as_grabbed");
			}
		} else if (pinch_position[0] >= XCYLINDER_MIN && pinch_position[0] < XCYLINDER_MAX) { //for creating the cylinder
			if (pinch_position[1] >= YCOOR_MIN && pinch_position[1] < YCOOR_MAX) {
				if (pinch_position[2] >= ZCOOR_MIN && pinch_position[2] < ZCOOR_MAX) {
					_cas.createObject(pinch_position[0], pinch_position[2], "CYLINDER");
					isCreated = true;
					playCreatedAudio();
				} else {
					isCreated = false;
					au_source.clip = (AudioClip) Resources.Load("SFX/as_grabbed");
				}
			} else {
				isCreated = false;
				au_source.clip = (AudioClip) Resources.Load("SFX/as_grabbed");
			}
		} else if (pinch_position[0] >= XHELP_MIN && pinch_position[0] < XHELP_MAX) { //for loading the Help GUI
			if (pinch_position[1] >= YCOOR_MIN && pinch_position[1] < YCOOR_MAX) {
				if (pinch_position[2] >= ZCOOR_MIN && pinch_position[2] < ZCOOR_MAX) {
					isCreated = false;
					isHelpEnabled = true;
					_lt.changeIntensityHelpOnly(0.4f);
					Debug.Log("HELP GUI ENABLED");
				}
			}
		} else if (pinch_position[0] >= XGARBAGE_MIN && pinch_position[0] < XGARBAGE_MAX) { //for deleting all shapes in scene (Garbage)
			if (pinch_position[1] >= YCOOR_MIN && pinch_position[1] < YCOOR_MAX) {
				if (pinch_position[2] >= ZCOOR_MIN && pinch_position[2] < ZCOOR_MAX) {
					isCreated = false;
					_cas.destroyAudibleShapes();
					playClearedAudio();
				} else {
					isCreated = false;
					au_source.clip = (AudioClip) Resources.Load("SFX/as_grabbed");
				}
			} else {
				au_source.clip = (AudioClip) Resources.Load("SFX/as_grabbed");
			}
		} else if (pinch_position[0] >= XMODE_MIN && pinch_position[0] < XMODE_MAX) { //for changing b/w gesture mode and pinch mode
			if (pinch_position[1] >= YCOOR_MIN && pinch_position[1] < YCOOR_MAX) {
				if (pinch_position[2] >= ZCOOR_MIN && pinch_position[2] < ZCOOR_MAX) {
						Debug.Log("HERE TO CHANGE MODES");

						if (!isGestureMode) {
							isGestureMode = true;
							modeButton.GetComponent<Renderer>().material.mainTexture = (Texture) Resources.Load("Textures/Pause");
						//	Debug.Log("TEXTURE IS PLAYED | " + modeButton.GetComponent<Renderer>().material.mainTexture);
						} else if (isGestureMode) {
							isGestureMode = false;
							modeButton.GetComponent<Renderer>().material.mainTexture = (Texture) Resources.Load("Textures/Play");
						//	Debug.Log("TEXTURE IS PAUSED | " + modeButton.GetComponent<Renderer>().material.mainTexture);
						}
				}
			}
		} else {
			isCreated = false;
			au_source.clip = (AudioClip) Resources.Load("SFX/as_grabbed");
		}

    // Check if we pinched a movable object and grab the closest one that's not part of the hand.
    Collider[] close_things = Physics.OverlapSphere(pinch_position, magnetDistance);
    Vector3 distance = new Vector3(magnetDistance, 0.0f, 0.0f);

    for (int j = 0; j < close_things.Length; ++j) {
      Vector3 new_distance = pinch_position - close_things[j].transform.position;
      if (close_things[j].GetComponent<Rigidbody>() != null && new_distance.magnitude < distance.magnitude &&
          !close_things[j].transform.IsChildOf(transform)) {
        grabbed_ = close_things[j];
        distance = new_distance;
				//using the distance magnitude, clamp value between minimum value and maximum value
				//and assign it as pitch of grabbing object based on how far the grabbed object is
				//from the hands' pinching position
				if (!isCreated) {
					au_source.pitch = Mathf.Clamp((1.0f/distance.magnitude), 2.25f, 4.0f);
					au_source.Play();
				}
      	}
    }

		} else { //user cannot interact with anything in scene when help screen is enabled except for the help button to close help screen
			if (pinch_position[0] >= XHELP_MIN && pinch_position[0] < XHELP_MAX) { //for loading the Help GUI
				if (pinch_position[1] >= YCOOR_MIN && pinch_position[1] < YCOOR_MAX) {
					if (pinch_position[2] >= ZCOOR_MIN && pinch_position[2] < ZCOOR_MAX) {
						isHelpEnabled = false;
						_lt.changeIntensityHelpOnly(1.0f);
						Debug.Log("HELP GUI DISABLED");
					}
				}
			}
		}

		//Debug.Log(isHelpEnabled + " = isHelpEnabled");
 	}

	void playCreatedAudio() {
		AudioClip clip_ = (AudioClip) Resources.Load("SFX/correct");
		au_source.clip = clip_;
		au_source.pitch = 1.0f;
		au_source.Play();
	}

	void playClearedAudio() {
		AudioClip clip_ = (AudioClip) Resources.Load("SFX/as_cleared");
		au_source.clip = clip_;
		au_source.pitch = 1.0f;
		au_source.Play();
	}

  /** Clears the pinch state. */
  void OnRelease() {
    grabbed_ = null;
    pinching_ = false;
	au_source.Stop();
  }

  /**
   * Checks whether the hand is pinching and updates the position of the pinched object.
   */
  void Update() {
    bool trigger_pinch = false;
    HandModel hand_model = GetComponent<HandModel>();
    Hand leap_hand = hand_model.GetLeapHand();

    if (leap_hand == null)
      return;

    // Scale trigger distance by thumb proximal bone length.
    Vector leap_thumb_tip = leap_hand.Fingers[0].TipPosition;
    float proximal_length = leap_hand.Fingers[0].Bone(Bone.BoneType.TYPE_PROXIMAL).Length;
    float trigger_distance = proximal_length * TRIGGER_DISTANCE_RATIO;

    // Check thumb tip distance to joints on all other fingers.
    // If it's close enough, start pinching.
    for (int i = 1; i < HandModel.NUM_FINGERS && !trigger_pinch; ++i) {
      Finger finger = leap_hand.Fingers[i];

      for (int j = 0; j < FingerModel.NUM_BONES && !trigger_pinch; ++j) {
        Vector leap_joint_position = finger.Bone((Bone.BoneType)j).NextJoint;
        if (leap_joint_position.DistanceTo(leap_thumb_tip) < trigger_distance)
          trigger_pinch = true;
      }
    }

    Vector3 pinch_position = hand_model.fingers[0].GetTipPosition();

    // Only change state if it's different.
    if (trigger_pinch && !pinching_) {
      OnPinch(pinch_position);
	}
    else if (!trigger_pinch && pinching_) {
      OnRelease();
	}

    // Accelerate what we are grabbing toward the pinch.
    if (grabbed_ != null) {
      Vector3 distance = pinch_position - grabbed_.transform.position;
      grabbed_.GetComponent<Rigidbody>().AddForce(forceSpringConstant * distance);
    }

		//Debug.Log("isHelpEnabled = " + isHelpEnabled);
  }

	public void OnGUI() {
	if (isHelpEnabled) {
		timeLeft -= Time.deltaTime;
		
		GUI.skin.box = header;
		header.fontStyle = FontStyle.Italic;
		GUI.Box(new Rect(-125,0,1000,100), prefab.GetComponent<GUIText>().text);
		
		//	Debug.Log("SCREEN WIDTH: " + Screen.width + " | SCREEN HEIGHT: " + Screen.height);
		GUI.BeginGroup(new Rect(345, 146.5f, 800, 450));
		
		//testing purposes only
		GUI.skin.box = main;
		GUI.Box(new Rect(0, 0, 800, 450), "");
		
		GUI.skin.box = style;
		
		if (timeLeft < 0  && !isPinched) {
			isPinched = true;
			hand = (Texture) Resources.Load("Images/PinchedHand");
			timeLeft = 2.0f;
			timeLeft -= Time.deltaTime;
		} else if (timeLeft < 0 && isPinched){
			isPinched = false;
			hand = (Texture) Resources.Load("Images/UnpinchedHand");
			timeLeft = 2.0f;
			timeLeft -= Time.deltaTime;
		}
		
		GUILayout.BeginHorizontal("BOX");
		GUI.DrawTexture(new Rect(20, 10, 150, 150), hand, ScaleMode.ScaleToFit, true, 0);
		GUI.Box(new Rect(180, 10, 440, 150), "Use the Pinch Gesture to interact with stuff, like:");
		GUILayout.EndHorizontal();
		
		GUI.skin.box = substyle;
		
		GUILayout.BeginHorizontal("BOX");
		GUI.DrawTexture(new Rect(45, 175, 100, 100), cube, ScaleMode.ScaleToFit, true, 0);
		GUI.DrawTexture(new Rect(155, 175, 100, 100), sphere, ScaleMode.ScaleToFit, true, 0);
		GUI.DrawTexture(new Rect(245, 175, 100, 100), capsule, ScaleMode.ScaleToFit, true, 0);
		GUI.DrawTexture(new Rect(320, 175, 100, 100), cylinder, ScaleMode.ScaleToFit, true, 0);
		GUI.Box(new Rect(370, 175, 200, 100), "Create and pick up objects");
		GUILayout.EndHorizontal();
		
		GUILayout.BeginHorizontal("BOX");
		GUI.DrawTexture(new Rect(100, 280, 100, 100), help, ScaleMode.ScaleToFit, true, 0);
		GUI.Box(new Rect(150, 280, 150, 100), "Open/Close Help");
		GUI.DrawTexture(new Rect(390, 280, 100, 100), garbage, ScaleMode.ScaleToFit, true, 0);
		GUI.Box(new Rect(445, 280, 150, 100), "Clear Scene");
		GUILayout.EndHorizontal();
		
		GUILayout.BeginHorizontal("BOX");
		GUI.DrawTexture(new Rect(175, 360, 100, 100), mode, ScaleMode.ScaleToFit, true, 0);
		GUI.Box(new Rect(225, 360, 150, 100), "Change to Gesture Mode");
		GUILayout.EndHorizontal();
		GUI.EndGroup();
		}
	}
}
