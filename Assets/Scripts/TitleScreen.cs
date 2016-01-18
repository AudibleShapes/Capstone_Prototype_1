using UnityEngine;
using System;
using System.Collections;
using Leap;

public class TitleScreen : MonoBehaviour {

	Leap.Controller controller;
	Frame frame;
	HandList hands;

	public GameObject guiTextPrefab;
	GameObject go;

	public MagneticPinch _mp;

	public float fadeTime;
	public float duration = 2.0f;
	private Color startColor;
	private Color endColor;
	private float timeLeft = 10.0f;

	// Use this for initialization
	void Start () {
		controller = new Controller();
		go = (GameObject) Instantiate(guiTextPrefab, new Vector3(0.25f, 0.8f, 0.0f), Quaternion.identity);
		startColor = go.GetComponent<GUIText>().color;
		endColor = startColor - new Color(0,0,0,1.0f);
	}
	
	// Update is called once per frame
	void Update () {
		frame = controller.Frame();
		hands = frame.Hands;
		if (hands.Count != 0) {
			timeLeft = 10.0f;
			go.GetComponent<GUIText>().enabled = false;
			go.GetComponent<GUIText>().color = Color.Lerp(endColor, startColor, fadeTime);
			if (fadeTime < 1)
				fadeTime += Time.deltaTime/duration;
		} else if (hands.Count == 0) {
			timeLeft -= Time.deltaTime;
			if (timeLeft < 0) {
				go.GetComponent<GUIText>().enabled = true;
				go.GetComponent<GUIText>().color = Color.Lerp(endColor, startColor, fadeTime);
				if (fadeTime < 1) {
					fadeTime += Time.deltaTime/duration;
				}
			}
		}
	}
}
