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
	public static int count;

	public CreateAudibleShapes _cas;
	public LightIntensity _lt;
	public SPOTLIGHTINTENSITY spt_cube;
	public SPOTLIGHTINTENSITY spt_sphere;
	public SPOTLIGHTINTENSITY spt_capsule;
	public SPOTLIGHTINTENSITY spt_cylinder;
	public SPOTLIGHTINTENSITY spt_tutorial;
	public SPOTLIGHTINTENSITY spt_garbage;
	public SPOTLIGHTINTENSITY spt_mode;
	public SPOTLIGHTINTENSITY spt_grid;

	public GameObject helpTitle;
	public GameObject modeButton;
	public GUIStyle header;
	public GUIStyle main;
	public GUIStyle style;
	public GUIStyle enjoy;
	public Texture hand;
	public Texture cube;
	public Texture sphere;
	public Texture capsule;
	public Texture cylinder;
	public Texture help;
	public Texture garbage;
	public Texture mode;
	public float timeLeft = 2.0f;
	public bool isPinched = false;
	private GameObject prefab;
	private bool isGestureMode = false;

	public static readonly int COUNT_CREATE = 0;
	public static readonly int COUNT_PLACE = 1;
	public static readonly int COUNT_DELETE = 2;
	public static readonly int COUNT_MODE = 3;
	public static readonly int COUNT_HELP = 4;

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
	count = 0;
  }

  /** Finds an object to grab and grabs it. */
  void OnPinch(Vector3 pinch_position) {
    pinching_ = true;
	bool isCreated = false;
	
	//Debug.Log("pinch_position[0]" + pinch_position[0] + "\tpinch_position[1]: " + pinch_position[1] + "\tpinch_position[2]:" + pinch_position[2]);
	
		/*
	 * Checks if the pinch_position's x-coordinate is within a given range, then checks for the z-coordinate.
	 * This is necessary in creating the shapes for the soundscape environment.
	 * 
	 * First, we check that help is currently disabled, or we are currently in the game itself.
	 * Basic in-game functions using pinch all derive from the shape menu in the game, such as creating objects,
	 * deleting them, and changing modes.
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
						} else if (isGestureMode) {
							isGestureMode = false;
							modeButton.GetComponent<Renderer>().material.mainTexture = (Texture) Resources.Load("Textures/Play");
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
			/*
			 * If help is enabled, then we proceed through a quick tutorial on the mechanics of the game./
			 */
  } else if (isHelpEnabled) {

			if (count == COUNT_CREATE) {

				if (pinch_position[0] >= XCUBE_MIN && pinch_position[0] < XCUBE_MAX) { //for creating the cube
					if (pinch_position[1] >= YCOOR_MIN && pinch_position[1] < YCOOR_MAX) {
						if (pinch_position[2] >= ZCOOR_MIN && pinch_position[2] < ZCOOR_MAX) {
							_cas.createObject(pinch_position[0], pinch_position[2], "CUBE");
							isCreated = true;
							playCreatedAudio();
							count++;
						}
					}
				} else if (pinch_position[0] >= XSPHERE_MIN && pinch_position[0] < XSPHERE_MAX) { //for creating the sphere
					if (pinch_position[1] >= YCOOR_MIN && pinch_position[1] < YCOOR_MAX) {
						if (pinch_position[2] >= ZCOOR_MIN && pinch_position[2] < ZCOOR_MAX) {
							_cas.createObject(pinch_position[0], pinch_position[2], "SPHERE");
							isCreated = true;
							playCreatedAudio();
							count++;
						}
					}
				} else if (pinch_position[0] >= XCAPSULE_MIN && pinch_position[0] < XCAPSULE_MAX) { //for creating the capsule
					if (pinch_position[1] >= YCOOR_MIN && pinch_position[1] < YCOOR_MAX) {
						if (pinch_position[2] >= ZCOOR_MIN && pinch_position[2] < ZCOOR_MAX) {
							_cas.createObject(pinch_position[0], pinch_position[2], "CAPSULE");
							isCreated = true;
							playCreatedAudio();
							count++;
						}
					}
				} else if (pinch_position[0] >= XCYLINDER_MIN && pinch_position[0] < XCYLINDER_MAX) { //for creating the cylinder
					if (pinch_position[1] >= YCOOR_MIN && pinch_position[1] < YCOOR_MAX) {
						if (pinch_position[2] >= ZCOOR_MIN && pinch_position[2] < ZCOOR_MAX) {
							_cas.createObject(pinch_position[0], pinch_position[2], "CYLINDER");
							isCreated = true;
							playCreatedAudio();
							count++;
						}
					}
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
					}
				}
			} else if (count == COUNT_DELETE) {
				if (pinch_position[0] >= XGARBAGE_MIN && pinch_position[0] < XGARBAGE_MAX) { //for deleting all shapes in scene (Garbage)
					if (pinch_position[1] >= YCOOR_MIN && pinch_position[1] < YCOOR_MAX) {
						if (pinch_position[2] >= ZCOOR_MIN && pinch_position[2] < ZCOOR_MAX) {
							isCreated = false;
							_cas.destroyAudibleShapes();
							playClearedAudio();
							count++;
						}
					}
				}
			} else if (count == COUNT_MODE) {
				if (pinch_position[0] >= XMODE_MIN && pinch_position[0] < XMODE_MAX) { //for changing b/w gesture mode and pinch mode
					if (pinch_position[1] >= YCOOR_MIN && pinch_position[1] < YCOOR_MAX) {
						if (pinch_position[2] >= ZCOOR_MIN && pinch_position[2] < ZCOOR_MAX) {
							count++;
							//							if (!isGestureMode) {
							//								isGestureMode = true;
							//								modeButton.GetComponent<Renderer>().material.mainTexture = (Texture) Resources.Load("Textures/Pause");
							//								//	Debug.Log("TEXTURE IS PLAYED | " + modeButton.GetComponent<Renderer>().material.mainTexture);
							//							} else if (isGestureMode) {
							//								isGestureMode = false;
							//								modeButton.GetComponent<Renderer>().material.mainTexture = (Texture) Resources.Load("Textures/Play");
							//								//	Debug.Log("TEXTURE IS PAUSED | " + modeButton.GetComponent<Renderer>().material.mainTexture);
						}
					}
				}
			} else if (count == COUNT_HELP) {
				if (pinch_position[0] >= XHELP_MIN && pinch_position[0] < XHELP_MAX) { //for loading the Help GUI
					if (pinch_position[1] >= YCOOR_MIN && pinch_position[1] < YCOOR_MAX) {
						if (pinch_position[2] >= ZCOOR_MIN && pinch_position[2] < ZCOOR_MAX) {
							count = 0;
							isHelpEnabled = false;
							Debug.Log("TUTORIAL IS COMPLETE");
						}
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

		if (count == COUNT_PLACE)
			count++;
  }

  /**
   * Checks whether the hand is pinching and updates the position of the pinched object.
   */
  void Update() {

	if (isHelpEnabled) {
		if (count == COUNT_CREATE) {
			spt_cube.changeSpotlightIntensity(7);
			spt_sphere.changeSpotlightIntensity(7);
			spt_capsule.changeSpotlightIntensity(7);
			spt_cylinder.changeSpotlightIntensity(7);
		} else if (count == COUNT_PLACE) {
			spt_cube.changeSpotlightIntensity(0);
			spt_sphere.changeSpotlightIntensity(0);
			spt_capsule.changeSpotlightIntensity(0);
			spt_cylinder.changeSpotlightIntensity(0);
			spt_grid.changeSpotlightIntensity(4);
		} else if (count == COUNT_DELETE) {
			spt_grid.changeSpotlightIntensity(0);
			spt_garbage.changeSpotlightIntensity(7);
		} else if (count == COUNT_MODE) {
			spt_garbage.changeSpotlightIntensity(0);
			spt_mode.changeSpotlightIntensity(7);
		} else if (count == COUNT_HELP) {
			spt_mode.changeSpotlightIntensity(0);
			spt_tutorial.changeSpotlightIntensity(7);
		}
	} else {
			spt_tutorial.changeSpotlightIntensity(0);	
	}

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
		GUI.Box(new Rect(112,0,900,100), prefab.GetComponent<GUIText>().text);
		
		//	Debug.Log("SCREEN WIDTH: " + Screen.width + " | SCREEN HEIGHT: " + Screen.height);
		GUI.BeginGroup(new Rect(295, 146.5f, 900, 150));
		
		//testing purposes only
		GUI.skin.box = main;
		GUI.Box(new Rect(0, 0, 1000, 150), "");
		
		GUI.skin.box = style;
		
		if (timeLeft < 0  && !isPinched) {
			isPinched = true;
				if (count == COUNT_PLACE)
					hand = (Texture) Resources.Load("Images/Unpinching_1");
				else
					hand = (Texture) Resources.Load("Images/Pinching_2");
			timeLeft = 2.0f;
			timeLeft = 2.0f;
			timeLeft -= Time.deltaTime;
		} else if (timeLeft < 0 && isPinched){
			isPinched = false;
				if (count == COUNT_PLACE)
					hand = (Texture) Resources.Load ("Images/Unpinching_2");
				else
					hand = (Texture) Resources.Load("Images/Pinching_1");
			timeLeft = 2.0f;
			timeLeft -= Time.deltaTime;
		}

			if (MagneticPinch.count == MagneticPinch.COUNT_CREATE) {
				GUILayout.BeginHorizontal();
				GUI.DrawTexture(new Rect(35,5,140,140), hand, ScaleMode.ScaleToFit, true, 0);
				GUI.Box(new Rect(190,0,25,150), "ON");
				GUI.DrawTexture(new Rect(225, 25, 100, 100), cube, ScaleMode.ScaleToFit, true, 0);
				GUI.DrawTexture(new Rect(335, 25, 100, 100), sphere, ScaleMode.ScaleToFit, true, 0);
				GUI.DrawTexture(new Rect(425, 25, 100, 100), capsule, ScaleMode.ScaleToFit, true, 0);
				GUI.DrawTexture(new Rect(500, 25, 100, 100), cylinder, ScaleMode.ScaleToFit, true, 0);
				GUI.Box(new Rect(600,0,300,150), "TO CREATE AND GRAB");
				GUILayout.EndHorizontal();
			} else if (MagneticPinch.count == MagneticPinch.COUNT_PLACE) {
				GUILayout.BeginHorizontal();
				GUI.Box(new Rect(55, 0, 45, 150), "Next, ");
				GUI.DrawTexture(new Rect(125,5,140,140), hand, ScaleMode.ScaleToFit, true, 0);
				GUI.DrawTexture(new Rect(285, 25, 100, 100), cube, ScaleMode.ScaleToFit, true, 0);
				GUI.DrawTexture(new Rect(395, 25, 100, 100), sphere, ScaleMode.ScaleToFit, true, 0);
				GUI.DrawTexture(new Rect(485, 25, 100, 100), capsule, ScaleMode.ScaleToFit, true, 0);
				GUI.DrawTexture(new Rect(560, 25, 100, 100), cylinder, ScaleMode.ScaleToFit, true, 0);
				GUI.Box(new Rect(660,0,25,150), "to Release it");
				GUILayout.EndHorizontal();
			} else if (MagneticPinch.count == MagneticPinch.COUNT_DELETE) {
				GUILayout.BeginHorizontal();
				GUI.Box(new Rect(200, 0, 45, 150), "Next, ");
				GUI.DrawTexture(new Rect(270,5,140,140), hand, ScaleMode.ScaleToFit, true, 0);
				GUI.Box(new Rect(430,0,25,150), "ON");
				GUI.DrawTexture(new Rect(465, 25, 100, 100), garbage, ScaleMode.ScaleToFit, true, 0);
				GUI.Box (new Rect(580, 0, 150, 150), "TO CLEAR SCENE");
				GUILayout.EndHorizontal();
			} else if (MagneticPinch.count == MagneticPinch.COUNT_MODE) {
				GUILayout.BeginHorizontal();
				GUI.Box(new Rect(175, 0, 45, 150), "Next, ");
				GUI.DrawTexture(new Rect(245,5,140,140), hand, ScaleMode.ScaleToFit, true, 0);
				GUI.Box(new Rect(405,0,25,150), "ON");
				GUI.DrawTexture(new Rect(460, 37, 75, 75), mode, ScaleMode.ScaleToFit, true, 0);
				GUI.Box (new Rect(555, 0, 150, 150), "TO CHANGE MODES");
				GUILayout.EndHorizontal();
			} else if (MagneticPinch.count == MagneticPinch.COUNT_HELP) {
				GUILayout.BeginHorizontal();
				GUI.Box(new Rect(75, 0, 45, 150), "Finally, ");
				GUI.DrawTexture(new Rect(175,5,140,140), hand, ScaleMode.ScaleToFit, true, 0);
				GUI.Box(new Rect(335,0,25,150), "ON");
				GUI.DrawTexture(new Rect(380, 37, 75, 75), help, ScaleMode.ScaleToFit, true, 0);
				GUI.Box (new Rect(475, 0, 150, 150), "TO OPEN AND CLOSE TUTORIAL");
				GUILayout.EndHorizontal();
			}
			GUI.EndGroup();

			if (count == COUNT_HELP) {
				GUI.skin.box = enjoy;
				GUI.Box(new Rect(625, 310, 800, 150), "ENJOY!");
			}
		}
	}
}
