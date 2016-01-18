using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ChangeMaterialTest : MonoBehaviour {

	public GameObject gridBlock;
	public Material mat_on;
	public Material mat_off;
	public Material mat_sel_move;
	public Material mat_sel_size;
	public Material mat_sel_rotate;
	private bool[] isBlockOn;
	private bool[] isMoveBlockSelect;
	private bool[] isSizeBlockSelect;
	private bool[] isRotateBlockSelect;
	private int clear = 0;
	private float Z_COORDINATE;
	private float[] timeLeftMV;
	private float[] timeLeftSZ;
	private float[] timeLeftRT;
	public static readonly float Z_MIN = -2.0f;
	public static readonly float Z_MAX = 5.0f;
	private readonly float SIZE = 1.0f;
	public static readonly float SIZE_MIN = 0.5f;
	public static readonly float SIZE_MAX = 1.5f;
	private readonly Quaternion ROTATE = new Quaternion(0,0,0,1);
	public static readonly float ROT_MIN = -1.0f;
	public static readonly float ROT_MAX = 1.0f;
	private readonly float TIME = 4.0f;

	private bool[] saved_isBlockOn;
	private float[] saved_zcoors;
	private float[] saved_sizes;
	private Quaternion[] saved_rotate;
	private bool isGridCleared;

	void Start() {
		clear = 1;
		OSCHandler.Instance.SendMessageToClient("MaxMSP", "/clear", clear);
		WithForeachLoop();
	}

	void WithForeachLoop() {
		foreach (Transform child in transform) {
			print("Foreach loop: " + child + "\tPosition: (" + child.transform.position.x + ", " + child.transform.position.y + ", " + child.transform.position.z + ")\n" +
			      "Size: " + child.transform.GetComponent<Renderer>().bounds.size + "\tRotation: " + child.transform.rotation);
			Z_COORDINATE = child.transform.position.z;
		}
		isGridCleared = true;

		int count = transform.childCount;
		isBlockOn = new bool[count];
		isMoveBlockSelect = new bool[count];
		isSizeBlockSelect = new bool[count];
		isRotateBlockSelect = new bool[count];
		timeLeftMV = new float[count];
		timeLeftSZ = new float[count];
		timeLeftRT = new float[count];

		saved_isBlockOn = new bool[count];
		saved_zcoors = new float[count];
		saved_sizes = new float[count];
		saved_rotate = new Quaternion[count];

		for (int i = 0; i < count; i++) {
			isBlockOn[i] = false;
			isMoveBlockSelect[i] = false;
			isSizeBlockSelect[i] = false;
			isRotateBlockSelect[i] = false;
			timeLeftMV[i] = TIME;
			timeLeftSZ[i] = TIME;
			timeLeftRT[i] = TIME;

			saved_isBlockOn[i] = isBlockOn[i];
			saved_zcoors[i] = Z_COORDINATE;
			saved_sizes[i] = SIZE;
			saved_rotate[i] = ROTATE;
		}
	}
	
	public void changeMaterial(float x, float y, float z) { //only used when poking any cube in the grid
		float on = 0.0f;
		int count = transform.childCount;

		//checks if any block is turned on, which switches isGridCleared
		//to its opposite value (important for resetting save states)
		for (int i = 0; i < count; i++) {
			if (!isBlockOn[i]) {
				isGridCleared = true;
				saved_isBlockOn[i] = false;
				saved_zcoors[i] = Z_COORDINATE;
				saved_sizes[i] = SIZE;
				saved_rotate[i] = ROTATE;
			} else {
				isGridCleared = false;
				break;
			}
		}

		for (int i = 0; i < count; i++) { //each coordinate of the poke is matched to each coordinate of each grid space; bounds.size gets the actual size of each grid space as to get its edges when poking
			if (x >= (transform.GetChild(i).transform.position.x - (transform.GetChild(i).GetComponent<Renderer>().bounds.size.x/2)) && x < (transform.GetChild(i).transform.position.x + (transform.GetChild(i).GetComponent<Renderer>().bounds.size.x/2))) {
				if (y >= (transform.GetChild(i).transform.position.y - (transform.GetChild(i).GetComponent<Renderer>().bounds.size.y/2)) && y < (transform.GetChild(i).transform.position.y + (transform.GetChild(i).GetComponent<Renderer>().bounds.size.y/2))) {
					if (z >= (transform.GetChild(i).transform.position.z - (transform.GetChild(i).GetComponent<Renderer>().bounds.size.z/2)) && z < (transform.GetChild(i).transform.position.z + (transform.GetChild(i).GetComponent<Renderer>().bounds.size.z/2))) {
						if (isBlockOn[i] && !isMoveBlockSelect[i]) { //if the block is active or on, change it to off
							on = 0.0f;
							string message = "/on"+transform.GetChild(i).name;
							OSCHandler.Instance.SendMessageToClient("MaxMSP", message, on);
							transform.GetChild(i).transform.GetComponent<Renderer>().material = mat_off;
							isBlockOn[i] = false;
							saved_isBlockOn[i] = isBlockOn[i];
						}
						else if (!isBlockOn[i] && !isMoveBlockSelect[i]) { //if the button is inactive or off, change it to on
							on = 1.0f;
							string message = "/on"+transform.GetChild(i).name;
							OSCHandler.Instance.SendMessageToClient("MaxMSP", message, on);
							transform.GetChild(i).transform.GetComponent<Renderer>().material = mat_on;
							isBlockOn[i] = true;
							saved_isBlockOn[i] = isBlockOn[i];
						}
					}
				}
			}
		}
	}

	public bool triggerPoke(float x, float y, float z) {
		int count = transform.childCount;
		for (int i = 0; i < count; i++) { //each coordinate of the pinch is matched to each coordinate of each grid space; bounds.size gets the actual size of each grid space as to get its edges when pinching
			if (x >= (transform.GetChild(i).transform.position.x - (transform.GetChild(i).GetComponent<Renderer>().bounds.size.x/2)) && x < (transform.GetChild(i).transform.position.x + (transform.GetChild(i).GetComponent<Renderer>().bounds.size.x/2))) {
				if (y >= (transform.GetChild(i).transform.position.y - (transform.GetChild(i).GetComponent<Renderer>().bounds.size.y/2)) && y < (transform.GetChild(i).transform.position.y + (transform.GetChild(i).GetComponent<Renderer>().bounds.size.y/2))) {
					if (z >= (transform.GetChild(i).transform.position.z - (transform.GetChild(i).GetComponent<Renderer>().bounds.size.z/2)) && z < (transform.GetChild(i).transform.position.z + (transform.GetChild(i).GetComponent<Renderer>().bounds.size.z/2))) {
						return true;
					}
				}
			}
		}

		return false;
	}

	public void selectMoveBlock(float x, float y, float z) {
		int count = transform.childCount;
		for (int i = 0; i < count; i++) { //each coordinate of the pinch is matched to each coordinate of each grid space; bounds.size gets the actual size of each grid space as to get its edges when pinching
			if (x >= (transform.GetChild(i).transform.position.x - (transform.GetChild(i).GetComponent<Renderer>().bounds.size.x/2)) && x < (transform.GetChild(i).transform.position.x + (transform.GetChild(i).GetComponent<Renderer>().bounds.size.x/2))) {
				if (y >= (transform.GetChild(i).transform.position.y - (transform.GetChild(i).GetComponent<Renderer>().bounds.size.y/2)) && y < (transform.GetChild(i).transform.position.y + (transform.GetChild(i).GetComponent<Renderer>().bounds.size.y/2))) {
					if (z >= (transform.GetChild(i).transform.position.z - (transform.GetChild(i).GetComponent<Renderer>().bounds.size.z/2)) && z < (transform.GetChild(i).transform.position.z + (transform.GetChild(i).GetComponent<Renderer>().bounds.size.z/2))) {
						if (isBlockOn[i] && !isMoveBlockSelect[i]) {
							if (isSizeBlockSelect[i]) { //deselect the cube from resize function to move function and reset its timer
								isSizeBlockSelect[i] = false;
								timeLeftSZ[i] = TIME;
							} else if (isRotateBlockSelect[i]) { //deselect cube from rotate function to move function and reset its timer
								isRotateBlockSelect[i] = false;
								timeLeftRT[i] = TIME;
							}
							transform.GetChild(i).transform.GetComponent<Renderer>().material = mat_sel_move;
							isMoveBlockSelect[i] = true;
						} else if (isBlockOn[i] && isMoveBlockSelect[i]) {
							transform.GetChild(i).transform.GetComponent<Renderer>().material = mat_on;
							isMoveBlockSelect[i] = false;
							timeLeftMV[i] = TIME;
						}
					}
				}
			}
		}
	}

	public void selectSizeBlock(float x, float y, float z) {
		int count = transform.childCount;
		for (int i = 0; i < count; i++) { //each coordinate of the pinch is matched to each coordinate of each grid space; bounds.size gets the actual size of each grid space as to get its edges when pinching
			if (x >= (transform.GetChild(i).transform.position.x - (transform.GetChild(i).GetComponent<Renderer>().bounds.size.x/2)) && x < (transform.GetChild(i).transform.position.x + (transform.GetChild(i).GetComponent<Renderer>().bounds.size.x/2))) {
				if (y >= (transform.GetChild(i).transform.position.y - (transform.GetChild(i).GetComponent<Renderer>().bounds.size.y/2)) && y < (transform.GetChild(i).transform.position.y + (transform.GetChild(i).GetComponent<Renderer>().bounds.size.y/2))) {
					if (z >= (transform.GetChild(i).transform.position.z - (transform.GetChild(i).GetComponent<Renderer>().bounds.size.z/2)) && z < (transform.GetChild(i).transform.position.z + (transform.GetChild(i).GetComponent<Renderer>().bounds.size.z/2))) {
						if (isBlockOn[i] && !isSizeBlockSelect[i]) {
							if (isMoveBlockSelect[i]) { //deselect the cube from the move function to resize function and reset its timer
								isMoveBlockSelect[i] = false;
								timeLeftMV[i] = TIME;
							} else if (isRotateBlockSelect[i]) { //deselect cube from rotate function to resize function and reset its timer
								isRotateBlockSelect[i] = false;
								timeLeftRT[i] = TIME;
							}
							transform.GetChild(i).transform.GetComponent<Renderer>().material = mat_sel_size;
							isSizeBlockSelect[i] = true;
						} else if (isBlockOn[i] && isSizeBlockSelect[i]) {
							transform.GetChild(i).transform.GetComponent<Renderer>().material = mat_on;
							isSizeBlockSelect[i] = false;
							timeLeftSZ[i] = TIME;
						}
					}
				}
			}
		}
	}

	public void selectRotateBlock(float x, float y, float z) {
		int count = transform.childCount;
		for (int i = 0; i < count; i++) { //each coordinate of the pinch is matched to each coordinate of each grid space; bounds.size gets the actual size of each grid space as to get its edges when pinching
			if (x >= (transform.GetChild(i).transform.position.x - (transform.GetChild(i).GetComponent<Renderer>().bounds.size.x/2)) && x < (transform.GetChild(i).transform.position.x + (transform.GetChild(i).GetComponent<Renderer>().bounds.size.x/2))) {
				if (y >= (transform.GetChild(i).transform.position.y - (transform.GetChild(i).GetComponent<Renderer>().bounds.size.y/2)) && y < (transform.GetChild(i).transform.position.y + (transform.GetChild(i).GetComponent<Renderer>().bounds.size.y/2))) {
					if (z >= (transform.GetChild(i).transform.position.z - (transform.GetChild(i).GetComponent<Renderer>().bounds.size.z/2)) && z < (transform.GetChild(i).transform.position.z + (transform.GetChild(i).GetComponent<Renderer>().bounds.size.z/2))) {
						if (isBlockOn[i] && !isRotateBlockSelect[i]) {
							if (isMoveBlockSelect[i]) { //deselect the cube from the move function to rotate function and reset its timer
								isMoveBlockSelect[i] = false;
								timeLeftMV[i] = TIME;
							} else if (isSizeBlockSelect[i]) { //deselect cube from resize function to rotate function and reset its timer
								isSizeBlockSelect[i] = false;
								timeLeftRT[i] = TIME;
							}
							transform.GetChild(i).transform.GetComponent<Renderer>().material = mat_sel_rotate;
							isRotateBlockSelect[i] = true;
						} else if (isBlockOn[i] && isRotateBlockSelect[i]) {
							transform.GetChild(i).transform.GetComponent<Renderer>().material = mat_on;
							isRotateBlockSelect[i] = false;
							timeLeftRT[i] = TIME;
						}
					}
				}
			}
		}
	}

	public void moveBlocks(float z) {
		int count = transform.childCount;
		for (int i = 0; i < count; i++) { //each coordinate of the pinch is matched to each coordinate of each grid space; bounds.size gets the actual size of each grid space as to get its edges when pinching

			if (!isBlockOn[i]) //deselect blocks that are turned off so as not to make them move
				isMoveBlockSelect[i] = false;

			if (isMoveBlockSelect[i]) { //move selected blocks to new z-coordinate
				if (z >= Z_MIN && z < Z_MAX) {
					transform.GetChild(i).transform.position = new Vector3(transform.GetChild(i).transform.position.x, transform.GetChild(i).transform.position.y, z);
					saved_zcoors[i] = z;
				}

				if (timeLeftMV[i] < 0) {
					transform.GetChild(i).transform.GetComponent<Renderer>().material = mat_on;
					isMoveBlockSelect[i] = false;
					timeLeftMV[i] = TIME;
				}
			}
		}
	}

	public void resizeBlocks(float z) {
		int count = transform.childCount;
		for (int i = 0; i < count; i++) {
			if (!isBlockOn[i])
				isSizeBlockSelect[i] = false;

			if (isSizeBlockSelect[i]) {

				if (z >= SIZE_MIN && z < SIZE_MAX) {
					transform.GetChild(i).GetComponent<Renderer>().transform.localScale = new Vector3(z,z,z);
					saved_sizes[i] = z;
				}

				if (timeLeftSZ[i] < 0) {
					transform.GetChild(i).transform.GetComponent<Renderer>().material = mat_on;
					isSizeBlockSelect[i] = false;
					timeLeftSZ[i] = TIME;
				}
			}
		}
	}

	public void rotateBlocks(float y) {
		int count = transform.childCount;
		for (int i = 0; i < count; i++) {
			if (!isBlockOn[i])
				isRotateBlockSelect[i] = false;
			
			if (isRotateBlockSelect[i]) {
				
				if (y >= ROT_MIN && y < ROT_MAX) {
					transform.GetChild(i).transform.Rotate(0,0,y);
					//Debug.Log(transform.GetChild(i) + "\t" + transform.GetChild(i).transform.rotation);
					saved_rotate[i] = transform.GetChild(i).transform.rotation;
				}
				
				if (timeLeftRT[i] < 0) {
					transform.GetChild(i).transform.GetComponent<Renderer>().material = mat_on;
					isRotateBlockSelect[i] = false;
					timeLeftRT[i] = TIME;
				}
			}
		}
	}

	public void clearGrid() { //clear the entire grid, change material to off, and change the blocks' bool values to false, and reset to default locations
		foreach (Transform child in transform) {
			child.transform.GetComponent<Renderer>().material = mat_off;
			child.transform.position = new Vector3(child.transform.position.x, child.transform.position.y, Z_COORDINATE);
			child.transform.GetComponent<Renderer>().transform.localScale = new Vector3(SIZE, SIZE, SIZE);
			child.transform.rotation = Quaternion.Slerp(transform.rotation, ROTATE, 1.0f);
			float on = 0.0f;
			string message = "/on"+child.name;
			OSCHandler.Instance.SendMessageToClient("MaxMSP", message, on);
		}

		int count = transform.childCount;
		for (int i = 0; i < count; i++) { //turn off all cubes and deselect them from moving
			if (isBlockOn[i])
				isBlockOn[i] = false;

			if (isMoveBlockSelect[i])
				isMoveBlockSelect[i] = false;

			if (isSizeBlockSelect[i])
				isSizeBlockSelect[i] = false;

			if (isRotateBlockSelect[i])
				isRotateBlockSelect[i] = false;
		}

		isGridCleared = true;
	}

	public void savedScene() { //using left swipe, reset last session of Audible Shapes, including all changes to cubes (z position, on states, sizes)
		if (isGridCleared) {
			int count = transform.childCount;
			for (int i = 0; i < count; i++) {
				if (saved_isBlockOn[i]) { //send OSC message to Max if cube was on in last session
					float on = 1.0f;
					string message = "/on"+transform.GetChild(i).name;
					OSCHandler.Instance.SendMessageToClient("MaxMSP", message, on);
					transform.GetChild(i).transform.GetComponent<Renderer>().material = mat_on;
				}
				isBlockOn[i] = saved_isBlockOn[i];
				transform.GetChild(i).transform.position = new Vector3(transform.GetChild(i).transform.position.x, transform.GetChild(i).transform.position.y, saved_zcoors[i]);
				transform.GetChild(i).GetComponent<Renderer>().transform.localScale = new Vector3(saved_sizes[i], saved_sizes[i], saved_sizes[i]);
				transform.GetChild(i).transform.rotation = Quaternion.Slerp(transform.rotation, saved_rotate[i], 1.0f);
			}
		} else {
			Debug.Log("DO NOTHING");
		}
	}

	void Update() { //only be used to exit the scene by manually pressing the 'esc' key
		int count = transform.childCount;
		for (int i = 0; i < count; i++) { //each coordinate of the pinch is matched to each coordinate of each grid space; bounds.size gets the actual size of each grid space as to get its edges when pinching
			if (isMoveBlockSelect[i])
				timeLeftMV[i] -= Time.deltaTime;

			if (isSizeBlockSelect[i])
				timeLeftSZ[i] -= Time.deltaTime;

			if (isRotateBlockSelect[i])
				timeLeftRT[i] -= Time.deltaTime;
		}
	}
}
